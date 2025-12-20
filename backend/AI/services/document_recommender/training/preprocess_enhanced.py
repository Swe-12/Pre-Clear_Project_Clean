"""
Enhanced preprocessing pipeline for multi-label document classification.

Implements proper feature engineering:
- Categorical encoding (one-hot for low-cardinality, target encoding for high-cardinality)
- HS Code treated as categorical with prefix extraction
- TF-IDF for product descriptions
- Normalized package weight extraction
- Multi-hot label encoding
"""

import re
from typing import List, Tuple, Dict
import pandas as pd
import numpy as np
import os


REQUIRED_COLUMNS = [
    "Origin Country",
    "Destination Country",
    "HS Code",
    "HTS / Regional Tariff Flag",
    "Product Category",
    "Product Description",
    "Package Type & Weight Range",
    "Mode of Transport",
    "Required Documents",
]


def _normalize_text(value: object) -> str:
    """Normalize text field for consistency."""
    if pd.isna(value):
        return ""
    s = str(value).strip()
    s = re.sub(r"\s+", " ", s)
    return s.lower()


def _extract_hs_prefix(hs_code: str, prefix_length: int = 4) -> str:
    """Extract HS code prefix for categorical grouping."""
    if not hs_code or pd.isna(hs_code):
        return "unknown"
    
    hs_clean = str(hs_code).strip()
    if len(hs_clean) >= prefix_length:
        return hs_clean[:prefix_length]
    return hs_clean if hs_clean else "unknown"


def _extract_weight_range(package_str: str) -> str:
    """Extract weight range category from package string."""
    if not package_str or pd.isna(package_str):
        return "unknown"
    
    s = str(package_str).lower()
    
    # Weight range patterns from dataset
    if "5–25 kg" in s or "5-25" in s:
        return "light"
    elif "50–200 kg" in s or "50-200" in s:
        return "medium"
    elif "200–800 kg" in s or "200-800" in s:
        return "heavy"
    elif "1–20 mt" in s or "1-20" in s:
        return "very_heavy"
    else:
        return "unknown"


def _extract_package_type(package_str: str) -> str:
    """Extract package type from package string."""
    if not package_str or pd.isna(package_str):
        return "unknown"
    
    s = str(package_str).lower()
    
    if "pallet" in s:
        return "pallets"
    elif "container" in s:
        return "containers"
    elif "carton" in s:
        return "cartons"
    elif "drum" in s:
        return "drums"
    else:
        return "unknown"


def _split_documents(raw: object) -> List[str]:
    """Split semicolon-delimited document string into list."""
    if pd.isna(raw):
        return []
    
    s = str(raw)
    parts = [p.strip() for p in s.split(";")]
    parts = [p for p in parts if p]
    
    # Remove duplicates while preserving order
    seen = set()
    result = []
    for p in parts:
        if p not in seen:
            seen.add(p)
            result.append(p)
    
    return result


def load_and_preprocess_enhanced(csv_path: str = None) -> Tuple[pd.DataFrame, List[List[str]]]:
    """
    Load dataset and create enhanced feature engineering.
    
    Returns:
        features_df: DataFrame with engineered features
        labels: List of document label lists (multi-label)
    """
    if csv_path is None:
        csv_path = os.path.join(
            os.path.dirname(__file__), "..", "dataset", "required_documents_dataset.csv"
        )
    
    df = pd.read_csv(csv_path, dtype=object)
    
    # Validate columns
    missing = [c for c in REQUIRED_COLUMNS if c not in df.columns]
    if missing:
        raise ValueError(f"Dataset missing required columns: {missing}")
    
    # Feature engineering
    features = {
        'origin_country': [],
        'destination_country': [],
        'hs_prefix': [],
        'hts_flag': [],
        'product_category': [],
        'product_description': [],
        'package_type': [],
        'weight_range': [],
        'mode_of_transport': [],
    }
    
    labels = []
    
    for idx, row in df.iterrows():
        # Extract and normalize features
        origin = _normalize_text(row.get("Origin Country"))
        destination = _normalize_text(row.get("Destination Country"))
        hs_code = row.get("HS Code", "")
        hts_flag = _normalize_text(row.get("HTS / Regional Tariff Flag"))
        category = _normalize_text(row.get("Product Category"))
        description = _normalize_text(row.get("Product Description"))
        package_raw = row.get("Package Type & Weight Range", "")
        mode = _normalize_text(row.get("Mode of Transport"))
        
        # Extract structured features
        hs_prefix = _extract_hs_prefix(hs_code)
        package_type = _extract_package_type(package_raw)
        weight_range = _extract_weight_range(package_raw)
        
        # Process labels
        docs = _split_documents(row.get("Required Documents"))
        
        # Skip completely empty rows
        if not any([origin, destination, hs_prefix != "unknown", category, description]) and not docs:
            continue
        
        # Add to features
        features['origin_country'].append(origin)
        features['destination_country'].append(destination)
        features['hs_prefix'].append(hs_prefix)
        features['hts_flag'].append(hts_flag)
        features['product_category'].append(category)
        features['product_description'].append(description)
        features['package_type'].append(package_type)
        features['weight_range'].append(weight_range)
        features['mode_of_transport'].append(mode)
        
        labels.append(docs)
    
    features_df = pd.DataFrame(features)
    
    return features_df, labels


def create_feature_matrix(
    features_df: pd.DataFrame,
    tfidf_vectorizer=None,
    cat_encoders: Dict = None,
    fit: bool = True
):
    """
    Create feature matrix from engineered features.
    
    Args:
        features_df: DataFrame with engineered features
        tfidf_vectorizer: TF-IDF vectorizer for descriptions (provide if fit=False)
        cat_encoders: Dictionary of categorical encoders (provide if fit=False)
        fit: Whether to fit encoders (True for training, False for inference)
    
    Returns:
        feature_matrix: Sparse matrix ready for ML
        tfidf_vectorizer: Fitted TF-IDF vectorizer
        cat_encoders: Dictionary of fitted encoders
    """
    from sklearn.feature_extraction.text import TfidfVectorizer
    from sklearn.preprocessing import LabelEncoder
    from scipy.sparse import hstack, csr_matrix
    
    if fit:
        # Initialize encoders
        tfidf_vectorizer = TfidfVectorizer(
            max_features=1000,
            ngram_range=(1, 2),
            min_df=2,
            max_df=0.95
        )
        
        cat_encoders = {}
        
        # Fit TF-IDF on product descriptions
        desc_features = tfidf_vectorizer.fit_transform(features_df['product_description'])
        
        # Encode categorical features
        categorical_features = [
            'origin_country',
            'destination_country',
            'hs_prefix',
            'product_category',
            'package_type',
            'weight_range',
            'mode_of_transport'
        ]
        
        encoded_cats = []
        for cat_col in categorical_features:
            encoder = LabelEncoder()
            encoded = encoder.fit_transform(features_df[cat_col].fillna('unknown'))
            cat_encoders[cat_col] = encoder
            # Convert to sparse matrix
            encoded_cats.append(csr_matrix(encoded.reshape(-1, 1)))
        
        # Encode HTS flag (binary)
        hts_binary = (features_df['hts_flag'] == 'yes').astype(int).values.reshape(-1, 1)
        encoded_cats.append(csr_matrix(hts_binary))
        
        # Combine all features
        feature_matrix = hstack([desc_features] + encoded_cats)
        
    else:
        # Transform using fitted encoders
        desc_features = tfidf_vectorizer.transform(features_df['product_description'])
        
        categorical_features = [
            'origin_country',
            'destination_country',
            'hs_prefix',
            'product_category',
            'package_type',
            'weight_range',
            'mode_of_transport'
        ]
        
        encoded_cats = []
        for cat_col in categorical_features:
            encoder = cat_encoders[cat_col]
            # Handle unseen categories
            values = features_df[cat_col].fillna('unknown').values
            encoded = []
            for val in values:
                if val in encoder.classes_:
                    encoded.append(encoder.transform([val])[0])
                else:
                    # Assign to first class for unseen categories
                    encoded.append(0)
            encoded = np.array(encoded)
            encoded_cats.append(csr_matrix(encoded.reshape(-1, 1)))
        
        # Encode HTS flag
        hts_binary = (features_df['hts_flag'] == 'yes').astype(int).values.reshape(-1, 1)
        encoded_cats.append(csr_matrix(hts_binary))
        
        feature_matrix = hstack([desc_features] + encoded_cats)
    
    return feature_matrix, tfidf_vectorizer, cat_encoders


if __name__ == "__main__":
    import argparse
    
    parser = argparse.ArgumentParser(description="Test enhanced preprocessing")
    parser.add_argument(
        "--csv",
        default=None,
        help="Path to dataset CSV"
    )
    args = parser.parse_args()
    
    features_df, labels = load_and_preprocess_enhanced(args.csv)
    print(f"Processed {len(features_df)} rows")
    print(f"Features shape: {features_df.shape}")
    print(f"Sample features:\n{features_df.head()}")
    
    # Count unique documents
    all_docs = set()
    for doc_list in labels:
        all_docs.update(doc_list)
    print(f"\nUnique documents: {len(all_docs)}")
    print(f"Sample documents: {sorted(list(all_docs))[:10]}")
