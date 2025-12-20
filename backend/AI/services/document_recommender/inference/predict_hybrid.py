"""
Hybrid inference pipeline for document recommendation.

Merges deterministic rules engine with ML predictions:
1. Apply rules engine → mandatory documents (always included)
2. Run ML model → ranked documents with probabilities
3. Merge results with explanations
4. Return final document list with confidence scores
"""

import os
import numpy as np
import pandas as pd
import joblib
from typing import List, Dict, Tuple, Optional
import warnings
warnings.filterwarnings('ignore')

from engine.rules import get_rules_engine
from training.preprocess_enhanced import create_feature_matrix


DEFAULT_MODEL_PATH = os.path.join(
    os.path.dirname(__file__), "model", "document_model_enhanced.pkl"
)

# Confidence threshold for ML predictions
ML_CONFIDENCE_THRESHOLD = 0.3

# Model cache
_MODEL_CACHE = {}


def _load_model_artifact(model_path: str = None) -> Dict:
    """Load model artifacts with caching."""
    path = model_path or DEFAULT_MODEL_PATH
    path = os.path.abspath(path)
    
    if path in _MODEL_CACHE:
        return _MODEL_CACHE[path]
    
    if not os.path.exists(path):
        raise FileNotFoundError(f"Model artifact not found: {path}")
    
    artifact = joblib.load(path)
    
    required_keys = {'tfidf_vectorizer', 'cat_encoders', 'mlb', 'labels'}
    missing = required_keys - set(artifact.keys())
    if missing:
        raise ValueError(f"Model artifact missing keys: {missing}")
    
    _MODEL_CACHE[path] = artifact
    return artifact


def _prepare_single_sample_features(
    origin_country: str,
    destination_country: str,
    hs_code: str,
    hts_flag: bool,
    product_category: str,
    product_description: str,
    package_type_weight: str,
    mode_of_transport: str
) -> pd.DataFrame:
    """Prepare single sample in the same format as training data."""
    from training.preprocess_enhanced import (
        _normalize_text,
        _extract_hs_prefix,
        _extract_package_type,
        _extract_weight_range
    )
    
    # Normalize inputs
    origin = _normalize_text(origin_country)
    destination = _normalize_text(destination_country)
    hs_prefix = _extract_hs_prefix(hs_code)
    hts = 'yes' if hts_flag else 'no'
    category = _normalize_text(product_category)
    description = _normalize_text(product_description)
    package_type = _extract_package_type(package_type_weight)
    weight_range = _extract_weight_range(package_type_weight)
    mode = _normalize_text(mode_of_transport)
    
    # Create DataFrame with same structure as training
    features_df = pd.DataFrame([{
        'origin_country': origin,
        'destination_country': destination,
        'hs_prefix': hs_prefix,
        'hts_flag': hts,
        'product_category': category,
        'product_description': description,
        'package_type': package_type,
        'weight_range': weight_range,
        'mode_of_transport': mode,
    }])
    
    return features_df


def _predict_ml_documents(
    features_df: pd.DataFrame,
    artifact: Dict,
    threshold: float = ML_CONFIDENCE_THRESHOLD
) -> Dict[str, float]:
    """
    Run ML model to predict documents with confidence scores.
    
    Returns:
        Dict mapping document name to confidence score
    """
    # Extract components
    tfidf_vectorizer = artifact['tfidf_vectorizer']
    cat_encoders = artifact['cat_encoders']
    mlb = artifact['mlb']
    model_type = artifact.get('model_type', 'sklearn')
    
    # Create feature matrix
    X, _, _ = create_feature_matrix(
        features_df,
        tfidf_vectorizer=tfidf_vectorizer,
        cat_encoders=cat_encoders,
        fit=False
    )
    
    # Predict
    if model_type == 'keras':
        from tensorflow import keras
        keras_model_path = artifact['keras_model_path']
        model = keras.models.load_model(keras_model_path)
        X_dense = X.toarray()
        probabilities = model.predict(X_dense, verbose=0)[0]
    else:
        # Sklearn model
        model = artifact['model']
        # For MultiOutputClassifier, use predict_proba if available
        if hasattr(model, 'predict_proba'):
            # Returns list of arrays, one per output
            proba_list = model.predict_proba(X)
            # Extract positive class probabilities
            probabilities = np.array([p[0][1] if p.shape[1] > 1 else p[0][0] for p in proba_list])
        else:
            # Fallback to binary predictions
            predictions = model.predict(X)[0]
            probabilities = predictions.astype(float)
    
    # Map to document names with scores above threshold
    ml_docs = {}
    for idx, score in enumerate(probabilities):
        if score >= threshold:
            doc_name = mlb.classes_[idx]
            ml_docs[doc_name] = float(score)
    
    return ml_docs


def predict_documents_hybrid(
    origin_country: str = "",
    destination_country: str = "",
    hs_code: str = "",
    hts_flag: bool = False,
    product_category: str = "",
    product_description: str = "",
    package_type_weight: str = "",
    mode_of_transport: str = "",
    model_path: str = None,
    include_explanations: bool = True
) -> Dict:
    """
    Hybrid document recommendation combining rules and ML.
    
    Args:
        origin_country: Origin country name
        destination_country: Destination country name
        hs_code: HS/HTS code
        hts_flag: Whether regional tariff applies
        product_category: Product category
        product_description: Product description text
        package_type_weight: Package type and weight range
        mode_of_transport: Transportation mode
        model_path: Path to ML model artifact (optional)
        include_explanations: Include explanation for each document
    
    Returns:
        Dict with:
            - required_documents: List of document names
            - documents_with_scores: Dict of doc -> confidence score
            - explanations: Dict of doc -> explanation (if include_explanations=True)
    """
    # Step 1: Apply deterministic rules
    rules_engine = get_rules_engine()
    mandatory_docs = rules_engine.get_mandatory_documents(
        origin_country=origin_country,
        destination_country=destination_country,
        hs_code=hs_code,
        hts_flag=hts_flag,
        product_category=product_category,
        product_description=product_description,
        package_type_weight=package_type_weight,
        mode_of_transport=mode_of_transport
    )
    
    # Step 2: Run ML model
    ml_docs = {}
    try:
        artifact = _load_model_artifact(model_path)
        features_df = _prepare_single_sample_features(
            origin_country,
            destination_country,
            hs_code,
            hts_flag,
            product_category,
            product_description,
            package_type_weight,
            mode_of_transport
        )
        ml_docs = _predict_ml_documents(features_df, artifact)
    except FileNotFoundError:
        # Model not trained yet, only use rules
        pass
    except Exception as e:
        # Log error but don't fail
        print(f"Warning: ML prediction failed: {e}")
    
    # Step 3: Merge results
    # Rules always included with confidence 1.0
    documents_with_scores = {doc: 1.0 for doc in mandatory_docs.keys()}
    
    # Add ML predictions (if not already in mandatory)
    for doc, score in ml_docs.items():
        if doc not in documents_with_scores:
            documents_with_scores[doc] = score
    
    # Step 4: Create explanations
    explanations = {}
    if include_explanations:
        # Add rule-based explanations
        for doc, explanation in mandatory_docs.items():
            explanations[doc] = f"[MANDATORY] {explanation}"
        
        # Add ML-based explanations
        for doc, score in ml_docs.items():
            if doc not in mandatory_docs:
                explanations[doc] = f"[ML PREDICTED] Confidence: {score:.2%} based on similar shipment patterns"
    
    # Final document list (sorted by confidence descending)
    sorted_docs = sorted(documents_with_scores.items(), key=lambda x: x[1], reverse=True)
    required_documents = [doc for doc, _ in sorted_docs]
    
    result = {
        'required_documents': required_documents,
        'documents_with_scores': documents_with_scores,
    }
    
    if include_explanations:
        result['explanations'] = explanations
    
    return result


# Simplified interface for backward compatibility
def predict(
    origin_country: str = "",
    destination_country: str = "",
    hs_code: str = "",
    hts_flag: str = "",
    product_category: str = "",
    product_description: str = "",
    package_type: str = "",
    mode_of_transport: str = "",
    model_path: Optional[str] = None
) -> List[str]:
    """
    Simplified prediction interface for backward compatibility.
    
    Returns only document list without scores/explanations.
    """
    # Convert hts_flag string to bool
    hts_bool = hts_flag.lower() in ['true', 'yes', '1'] if hts_flag else False
    
    result = predict_documents_hybrid(
        origin_country=origin_country,
        destination_country=destination_country,
        hs_code=hs_code,
        hts_flag=hts_bool,
        product_category=product_category,
        product_description=product_description,
        package_type_weight=package_type,
        mode_of_transport=mode_of_transport,
        model_path=model_path,
        include_explanations=False
    )
    
    return result['required_documents']


if __name__ == "__main__":
    # Test with sample shipments
    test_cases = [
        {
            'name': 'Pharmaceuticals to US',
            'origin_country': 'India',
            'destination_country': 'United States',
            'hs_code': '300490',
            'hts_flag': True,
            'product_category': 'Pharmaceuticals',
            'product_description': 'Finished formulation medicinal tablets',
            'package_type_weight': 'Pallets (200–800 kg)',
            'mode_of_transport': 'Air',
        },
        {
            'name': 'Food to US',
            'origin_country': 'Japan',
            'destination_country': 'United States',
            'hs_code': '100630',
            'hts_flag': True,
            'product_category': 'Food & Agriculture',
            'product_description': 'Semi-milled rice for human consumption',
            'package_type_weight': 'Containers (1–20 MT)',
            'mode_of_transport': 'Air',
        },
        {
            'name': 'Chemicals',
            'origin_country': 'Germany',
            'destination_country': 'Canada',
            'hs_code': '280700',
            'hts_flag': True,
            'product_category': 'Chemicals',
            'product_description': 'Sulfuric acid industrial grade',
            'package_type_weight': 'Cartons (5–25 kg)',
            'mode_of_transport': 'Sea',
        },
        {
            'name': 'Vehicles',
            'origin_country': 'Japan',
            'destination_country': 'United States',
            'hs_code': '870323',
            'hts_flag': True,
            'product_category': 'Automotive',
            'product_description': 'Petrol engine passenger vehicles',
            'package_type_weight': 'Pallets (200–800 kg)',
            'mode_of_transport': 'Air',
        }
    ]
    
    print("=" * 80)
    print("HYBRID DOCUMENT RECOMMENDATION - TEST CASES")
    print("=" * 80)
    
    for test in test_cases:
        print(f"\n{'='*80}")
        print(f"Test Case: {test['name']}")
        print(f"{'='*80}")
        print(f"Origin: {test['origin_country']} → Destination: {test['destination_country']}")
        print(f"HS Code: {test['hs_code']} | Category: {test['product_category']}")
        print(f"Mode: {test['mode_of_transport']} | HTS Flag: {test['hts_flag']}")
        print(f"\nProduct: {test['product_description']}")
        print(f"Package: {test['package_type_weight']}")
        
        result = predict_documents_hybrid(**test, include_explanations=True)
        
        print(f"\n{'─'*80}")
        print("Required Documents:")
        print(f"{'─'*80}")
        
        for doc in result['required_documents']:
            score = result['documents_with_scores'][doc]
            explanation = result.get('explanations', {}).get(doc, '')
            
            confidence = "MANDATORY" if score == 1.0 else f"{score:.0%}"
            print(f"  ✓ {doc}")
            print(f"    Confidence: {confidence}")
            if explanation:
                print(f"    Reason: {explanation}")
            print()
