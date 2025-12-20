"""
Enhanced multi-label document classification model training.

Implements:
- Proper feature engineering with categorical encoding and TF-IDF
- Neural network with sigmoid activation for multi-label classification
- Binary cross-entropy loss
- Class weighting to handle imbalance
- Optimization for recall of mandatory documents
"""

import os
import numpy as np
import joblib
from typing import Tuple
from sklearn.preprocessing import MultiLabelBinarizer
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, hamming_loss, f1_score
import warnings
warnings.filterwarnings('ignore')

try:
    from .preprocess_enhanced import load_and_preprocess_enhanced, create_feature_matrix
except:
    from preprocess_enhanced import load_and_preprocess_enhanced, create_feature_matrix


def compute_class_weights(Y: np.ndarray) -> np.ndarray:
    """
    Compute class weights to handle imbalance.
    
    Args:
        Y: Multi-hot encoded label matrix (n_samples, n_classes)
    
    Returns:
        Class weights array (n_classes,)
    """
    n_samples = Y.shape[0]
    pos_counts = Y.sum(axis=0)
    neg_counts = n_samples - pos_counts
    
    # Inverse frequency weighting
    weights = np.zeros(Y.shape[1])
    for i in range(Y.shape[1]):
        if pos_counts[i] > 0:
            # Weight = total_samples / (n_classes * positive_samples)
            weights[i] = n_samples / (Y.shape[1] * pos_counts[i])
        else:
            weights[i] = 1.0
    
    # Normalize weights to reasonable range
    weights = weights / weights.mean()
    weights = np.clip(weights, 0.5, 10.0)  # Prevent extreme weights
    
    return weights


def build_neural_network(input_dim: int, output_dim: int):
    """
    Build neural network for multi-label classification.
    
    Uses sigmoid activation for independent binary predictions per label.
    """
    try:
        from tensorflow import keras
        from tensorflow.keras import layers
        
        model = keras.Sequential([
            layers.Input(shape=(input_dim,)),
            layers.Dense(512, activation='relu'),
            layers.Dropout(0.3),
            layers.Dense(256, activation='relu'),
            layers.Dropout(0.3),
            layers.Dense(128, activation='relu'),
            layers.Dropout(0.2),
            layers.Dense(output_dim, activation='sigmoid')  # Sigmoid for multi-label
        ])
        
        # Binary cross-entropy for multi-label classification
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.001),
            loss='binary_crossentropy',
            metrics=['binary_accuracy', keras.metrics.Recall(name='recall')]
        )
        
        return model
    
    except ImportError:
        # Fallback to sklearn if TensorFlow not available
        from sklearn.neural_network import MLPClassifier
        from sklearn.multioutput import MultiOutputClassifier
        
        base_model = MLPClassifier(
            hidden_layer_sizes=(512, 256, 128),
            activation='relu',
            solver='adam',
            alpha=0.0001,
            batch_size=32,
            learning_rate='adaptive',
            learning_rate_init=0.001,
            max_iter=100,
            random_state=42,
            early_stopping=True,
            validation_fraction=0.1,
            n_iter_no_change=10,
            verbose=True
        )
        
        model = MultiOutputClassifier(base_model, n_jobs=-1)
        return model


def train_and_persist_enhanced(model_path: str, csv_path: str = None) -> None:
    """
    Train enhanced multi-label document classification model.
    
    Steps:
    1. Load and preprocess data with enhanced feature engineering
    2. Create feature matrix with TF-IDF and categorical encoding
    3. Train neural network with class weighting
    4. Evaluate on validation set
    5. Persist model artifacts
    """
    print("=" * 60)
    print("Enhanced Multi-Label Document Classification Training")
    print("=" * 60)
    
    # Load and preprocess data
    print("\n[1/6] Loading and preprocessing data...")
    features_df, labels = load_and_preprocess_enhanced(csv_path)
    print(f"   Loaded {len(features_df)} samples")
    
    # Create feature matrix
    print("\n[2/6] Creating feature matrix with TF-IDF and categorical encoding...")
    X, tfidf_vectorizer, cat_encoders = create_feature_matrix(features_df, fit=True)
    print(f"   Feature matrix shape: {X.shape}")
    
    # Encode labels
    print("\n[3/6] Encoding labels with MultiLabelBinarizer...")
    mlb = MultiLabelBinarizer()
    Y = mlb.fit_transform(labels)
    print(f"   Label matrix shape: {Y.shape}")
    print(f"   Number of unique documents: {len(mlb.classes_)}")
    
    # Compute class weights
    print("\n[4/6] Computing class weights for imbalanced data...")
    class_weights = compute_class_weights(Y)
    print(f"   Class weights range: {class_weights.min():.2f} - {class_weights.max():.2f}")
    
    # Split data
    print("\n[5/6] Splitting data into train/validation sets...")
    X_train, X_val, Y_train, Y_val = train_test_split(
        X, Y, test_size=0.2, random_state=42
    )
    print(f"   Training samples: {X_train.shape[0]}")
    print(f"   Validation samples: {X_val.shape[0]}")
    
    # Train model
    print("\n[6/6] Training neural network model...")
    print("   Architecture: Dense(512) -> Dense(256) -> Dense(128) -> Sigmoid")
    print("   Loss: Binary Cross-Entropy")
    print("   Optimizer: Adam")
    
    try:
        # Try TensorFlow/Keras
        from tensorflow import keras
        
        model = build_neural_network(X_train.shape[1], Y_train.shape[1])
        
        # Convert sparse to dense for Keras
        X_train_dense = X_train.toarray()
        X_val_dense = X_val.toarray()
        
        # Sample weights (apply class weights to each sample)
        sample_weights = np.zeros((Y_train.shape[0], Y_train.shape[1]))
        for i in range(Y_train.shape[1]):
            sample_weights[:, i] = class_weights[i]
        # Average weights for each sample
        sample_weights = (sample_weights * Y_train).sum(axis=1) / (Y_train.sum(axis=1) + 1e-10)
        
        history = model.fit(
            X_train_dense, Y_train,
            validation_data=(X_val_dense, Y_val),
            epochs=50,
            batch_size=32,
            sample_weight=sample_weights,
            verbose=1,
            callbacks=[
                keras.callbacks.EarlyStopping(
                    monitor='val_loss',
                    patience=10,
                    restore_best_weights=True
                ),
                keras.callbacks.ReduceLROnPlateau(
                    monitor='val_loss',
                    factor=0.5,
                    patience=5,
                    min_lr=1e-6
                )
            ]
        )
        
        # Evaluate
        print("\n" + "=" * 60)
        print("Validation Results:")
        print("=" * 60)
        
        Y_pred_proba = model.predict(X_val_dense)
        Y_pred = (Y_pred_proba >= 0.5).astype(int)
        
        # Metrics
        hamming = hamming_loss(Y_val, Y_pred)
        f1_macro = f1_score(Y_val, Y_pred, average='macro', zero_division=0)
        f1_micro = f1_score(Y_val, Y_pred, average='micro', zero_division=0)
        
        print(f"Hamming Loss: {hamming:.4f}")
        print(f"F1 Score (Macro): {f1_macro:.4f}")
        print(f"F1 Score (Micro): {f1_micro:.4f}")
        
        # Save model using Keras format
        model_dir = os.path.dirname(model_path)
        keras_model_path = os.path.join(model_dir, "keras_model.h5")
        model.save(keras_model_path)
        print(f"\nKeras model saved to: {keras_model_path}")
        
        # Save artifacts
        artifact = {
            'model_type': 'keras',
            'keras_model_path': keras_model_path,
            'tfidf_vectorizer': tfidf_vectorizer,
            'cat_encoders': cat_encoders,
            'mlb': mlb,
            'labels': list(mlb.classes_),
            'class_weights': class_weights,
            'feature_columns': list(features_df.columns),
        }
        
    except ImportError:
        # Fallback to sklearn
        print("   TensorFlow not available, using scikit-learn MLPClassifier")
        
        model = build_neural_network(X_train.shape[1], Y_train.shape[1])
        model.fit(X_train, Y_train)
        
        # Evaluate
        print("\n" + "=" * 60)
        print("Validation Results:")
        print("=" * 60)
        
        Y_pred = model.predict(X_val)
        
        hamming = hamming_loss(Y_val, Y_pred)
        f1_macro = f1_score(Y_val, Y_pred, average='macro', zero_division=0)
        f1_micro = f1_score(Y_val, Y_pred, average='micro', zero_division=0)
        
        print(f"Hamming Loss: {hamming:.4f}")
        print(f"F1 Score (Macro): {f1_macro:.4f}")
        print(f"F1 Score (Micro): {f1_micro:.4f}")
        
        artifact = {
            'model_type': 'sklearn',
            'model': model,
            'tfidf_vectorizer': tfidf_vectorizer,
            'cat_encoders': cat_encoders,
            'mlb': mlb,
            'labels': list(mlb.classes_),
            'class_weights': class_weights,
            'feature_columns': list(features_df.columns),
        }
    
    # Persist artifacts
    os.makedirs(os.path.dirname(model_path), exist_ok=True)
    joblib.dump(artifact, model_path)
    
    print(f"\nModel artifacts saved to: {model_path}")
    print("=" * 60)
    print("Training completed successfully!")
    print("=" * 60)


if __name__ == "__main__":
    import argparse
    
    default_model_path = os.path.join(
        os.path.dirname(__file__), "..", "model", "document_model_enhanced.pkl"
    )
    
    parser = argparse.ArgumentParser(description="Train enhanced document recommendation model")
    parser.add_argument("--csv", help="Path to dataset CSV", default=None)
    parser.add_argument("--out", help="Output model path", default=default_model_path)
    args = parser.parse_args()
    
    train_and_persist_enhanced(args.out, args.csv)
