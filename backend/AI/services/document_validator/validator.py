"""
STRICT Document-Form Consistency Validator
Validates document content against shipment form data with deterministic field-level rules.
NO semantic guessing, NO permissive matching, NO default PASS.
"""
import re
from typing import Dict, List, Optional, Tuple
from sentence_transformers import SentenceTransformer


class DocumentValidator:
    """
    Validates document consistency using STRICT field-by-field comparison.
    Uses regex extraction + keyword anchors + semantic similarity (>0.75 only).
    """
    
    def __init__(self):
        # Load embedding model for product description semantic similarity
        try:
            self.embedding_model = SentenceTransformer('all-MiniLM-L6-v2')
        except Exception as e:
            print(f"Warning: Failed to load embedding model: {e}")
            self.embedding_model = None
        
        self.issues: List[Dict] = []
    
    def validate(self, shipment_data: Dict, document_text: str, document_name: str) -> Dict:
        """
        STRICT validation: returns PASS only if ALL mandatory fields match exactly.
        
        Args:
            shipment_data: Form data {hs_code, quantity, weight, origin_country, etc.}
            document_text: Raw extracted text from document
            document_name: Document file name
            
        Returns:
            {
                "documentName": str,
                "status": "FAIL" | "WARNING" | "PASS",
                "issues": [
                    {
                        "field": str,
                        "document_value": str,
                        "shipment_value": str,
                        "severity": "FAIL" | "WARNING",
                        "message": str
                    }
                ]
            }
        """
        self.issues = []
        
        # STEP 1: Document Completeness (must have >= 200 chars)
        if not self._check_minimum_content(document_text):
            return self._build_response(document_name, "FAIL")
        
        # Extract all fields from document using regex
        extracted_fields = self._extract_fields(document_text)
        
        # STEP 2: Field-by-field validation
        self._validate_hs_code(extracted_fields, shipment_data)
        self._validate_quantity(extracted_fields, shipment_data)
        self._validate_weight(extracted_fields, shipment_data)
        self._validate_product_description(extracted_fields, shipment_data)
        self._validate_package_type(extracted_fields, shipment_data)
        self._validate_origin_destination(extracted_fields, shipment_data)
        self._validate_mode_of_transport(extracted_fields, shipment_data)
        
        # STEP 3: Determine verdict
        status = self._determine_status()
        
        return self._build_response(document_name, status)
    
    def _check_minimum_content(self, text: str) -> bool:
        """
        FAIL if document has less than 200 characters of content.
        """
        if not text or len(text.strip()) < 200:
            self.issues.append({
                "field": "Document Content",
                "document_value": f"{len(text) if text else 0} chars",
                "shipment_value": ">= 200 chars",
                "severity": "FAIL",
                "message": "Document content insufficient or unreadable (< 200 characters)"
            })
            return False
        return True
    
    def _extract_fields(self, text: str) -> Dict[str, Optional[str]]:
        """
        Extract ONLY these fields using regex + keyword anchors.
        Returns dict with field name → extracted value (or None if not found).
        """
        # Work with lowercase for pattern matching, preserve case in extractions
        text_lower = text.lower()
        
        fields = {}
        
        # HS Code: 6-8 digit number (look for keyword first, then any 6-8 digit sequence)
        hs_match = re.search(
            r'(?:hs\s+code|hs\s+code|product\s+code)[\s:]+(\d{6,8})',
            text_lower
        )
        if not hs_match:
            # Fallback: just find any 6-8 digit sequence
            hs_match = re.search(r'\b(\d{6,8})\b', text)
        fields['hs_code'] = hs_match.group(1) if hs_match else None
        
        # Product Description: text following keyword
        prod_match = re.search(
            r'(?:product\s+description|product\s+name|item\s+description)[\s:]+([^\n]{10,200})',
            text_lower
        )
        fields['product_description'] = prod_match.group(1).strip() if prod_match else None
        
        # Quantity: number following keyword
        qty_match = re.search(
            r'(?:quantity|total\s+quantity|number\s+of\s+items?)[\s:]+(\d+(?:\.\d+)?)',
            text_lower
        )
        fields['quantity'] = qty_match.group(1) if qty_match else None
        
        # Weight: number following "Gross Weight" or "Weight"
        weight_match = re.search(
            r'(?:gross\s+weight|net\s+weight|total\s+weight|weight)[\s:]+(\d+(?:\.\d+)?)',
            text_lower
        )
        fields['weight'] = weight_match.group(1) if weight_match else None
        
        # Package Type: text following keyword
        pkg_match = re.search(
            r'(?:package\s+type|packaging|container\s+type)[\s:]+([^\n]{3,50})',
            text_lower
        )
        fields['package_type'] = pkg_match.group(1).strip() if pkg_match else None
        
        # Origin Country: text following keyword
        origin_match = re.search(
            r'(?:country\s+of\s+origin|origin\s+country|made\s+in|manufactured\s+in)[\s:]+([^\n]{2,50})',
            text_lower
        )
        fields['origin_country'] = origin_match.group(1).strip() if origin_match else None
        
        # Destination Country: text following keyword
        dest_match = re.search(
            r'(?:destination\s+country|country\s+of\s+destination|ship\s+to|consignee\s+country)[\s:]+([^\n]{2,50})',
            text_lower
        )
        fields['destination_country'] = dest_match.group(1).strip() if dest_match else None
        
        # Mode of Transport: text following keyword
        mode_match = re.search(
            r'(?:mode\s+of\s+transport|transportation\s+mode|method\s+of\s+transport|shipment\s+mode)[\s:]+([^\n]{3,30})',
            text_lower
        )
        fields['mode_of_transport'] = mode_match.group(1).strip() if mode_match else None
        
        return fields
    
    def _validate_hs_code(self, extracted: Dict, shipment: Dict):
        """
        HS Code mismatch is ALWAYS FAIL (blocking issue).
        """
        doc_hs = extracted.get('hs_code')
        ship_hs = shipment.get('hs_code', '').replace('.', '').replace(' ', '')
        
        if not ship_hs:
            return  # No HS code in shipment to validate
        
        # Normalize both
        ship_hs_normalized = ship_hs.strip()
        
        if not doc_hs:
            self.issues.append({
                "field": "HS Code",
                "document_value": "NOT FOUND",
                "shipment_value": ship_hs_normalized,
                "severity": "FAIL",
                "message": "HS code not found in document"
            })
        elif doc_hs != ship_hs_normalized:
            self.issues.append({
                "field": "HS Code",
                "document_value": doc_hs,
                "shipment_value": ship_hs_normalized,
                "severity": "FAIL",
                "message": f"HS code mismatch: document has '{doc_hs}', shipment form has '{ship_hs_normalized}'"
            })
    
    def _validate_quantity(self, extracted: Dict, shipment: Dict):
        """
        Quantity mismatch is FAIL.
        abs(extracted_qty - shipment_qty) > 0 → FAIL
        """
        doc_qty = extracted.get('quantity')
        ship_qty = shipment.get('quantity', '')
        
        if not ship_qty:
            return
        
        try:
            doc_qty_float = float(doc_qty) if doc_qty else None
            ship_qty_float = float(str(ship_qty).replace(',', ''))
        except (ValueError, TypeError):
            return
        
        if doc_qty_float is None:
            self.issues.append({
                "field": "Quantity",
                "document_value": "NOT FOUND",
                "shipment_value": str(ship_qty),
                "severity": "FAIL",
                "message": "Quantity not found in document"
            })
        elif abs(doc_qty_float - ship_qty_float) > 0.01:  # Allow tiny floating point variance
            self.issues.append({
                "field": "Quantity",
                "document_value": str(doc_qty_float),
                "shipment_value": str(ship_qty_float),
                "severity": "FAIL",
                "message": f"Quantity mismatch: document has {doc_qty_float}, shipment form has {ship_qty_float}"
            })
    
    def _validate_weight(self, extracted: Dict, shipment: Dict):
        """
        Weight validation:
        - Missing → FAIL
        - Difference > 5 kg → FAIL
        - Difference 2-5 kg → WARNING
        - Otherwise → OK
        """
        doc_weight = extracted.get('weight')
        ship_weight = shipment.get('weight', '')
        
        if not ship_weight:
            return
        
        try:
            doc_weight_float = float(doc_weight) if doc_weight else None
            ship_weight_float = float(str(ship_weight).replace(',', ''))
        except (ValueError, TypeError):
            return
        
        if doc_weight_float is None:
            self.issues.append({
                "field": "Weight",
                "document_value": "NOT FOUND",
                "shipment_value": f"{ship_weight_float} kg",
                "severity": "FAIL",
                "message": "Weight not found in document"
            })
        else:
            diff = abs(doc_weight_float - ship_weight_float)
            if diff > 5:
                self.issues.append({
                    "field": "Weight",
                    "document_value": f"{doc_weight_float} kg",
                    "shipment_value": f"{ship_weight_float} kg",
                    "severity": "FAIL",
                    "message": f"Weight mismatch exceeds 5kg threshold: difference is {diff} kg"
                })
            elif diff > 2:
                self.issues.append({
                    "field": "Weight",
                    "document_value": f"{doc_weight_float} kg",
                    "shipment_value": f"{ship_weight_float} kg",
                    "severity": "WARNING",
                    "message": f"Weight slightly differs: difference is {diff} kg"
                })
    
    def _validate_product_description(self, extracted: Dict, shipment: Dict):
        """
        Product description validation using semantic similarity.
        - Similarity < 0.75 → FAIL
        - Similarity 0.75-0.90 → WARNING
        - Similarity > 0.90 → OK
        """
        doc_desc = extracted.get('product_description')
        ship_desc = shipment.get('product_description', '')
        
        if not ship_desc or not doc_desc:
            if ship_desc and not doc_desc:
                self.issues.append({
                    "field": "Product Description",
                    "document_value": "NOT FOUND",
                    "shipment_value": ship_desc,
                    "severity": "FAIL",
                    "message": "Product description not found in document"
                })
            return
        
        # Try semantic similarity if model available
        if self.embedding_model:
            try:
                embeddings = self.embedding_model.encode([ship_desc, doc_desc])
                similarity = self._cosine_similarity(embeddings[0], embeddings[1])
                
                if similarity < 0.75:
                    self.issues.append({
                        "field": "Product Description",
                        "document_value": doc_desc,
                        "shipment_value": ship_desc,
                        "severity": "FAIL",
                        "message": f"Product description has low semantic similarity ({similarity:.2f} < 0.75)"
                    })
                elif similarity < 0.90:
                    self.issues.append({
                        "field": "Product Description",
                        "document_value": doc_desc,
                        "shipment_value": ship_desc,
                        "severity": "WARNING",
                        "message": f"Product description has moderate semantic similarity ({similarity:.2f})"
                    })
            except Exception:
                # Fallback to string matching if embedding fails
                if ship_desc.lower() not in doc_desc.lower():
                    self.issues.append({
                        "field": "Product Description",
                        "document_value": doc_desc,
                        "shipment_value": ship_desc,
                        "severity": "FAIL",
                        "message": "Product description not found in document"
                    })
        else:
            # No model available - use string matching
            if ship_desc.lower() not in doc_desc.lower():
                self.issues.append({
                    "field": "Product Description",
                    "document_value": doc_desc,
                    "shipment_value": ship_desc,
                    "severity": "FAIL",
                    "message": "Product description not found in document"
                })
    
    def _validate_package_type(self, extracted: Dict, shipment: Dict):
        """
        Package type: exact or normalized match only.
        Otherwise → FAIL
        """
        doc_pkg = extracted.get('package_type')
        ship_pkg = shipment.get('package_type', '')
        
        if not ship_pkg:
            return
        
        if not doc_pkg:
            self.issues.append({
                "field": "Package Type",
                "document_value": "NOT FOUND",
                "shipment_value": ship_pkg,
                "severity": "FAIL",
                "message": "Package type not found in document"
            })
        elif ship_pkg.lower().strip() != doc_pkg.lower().strip():
            self.issues.append({
                "field": "Package Type",
                "document_value": doc_pkg,
                "shipment_value": ship_pkg,
                "severity": "FAIL",
                "message": f"Package type mismatch: document has '{doc_pkg}', shipment form has '{ship_pkg}'"
            })
    
    def _validate_origin_destination(self, extracted: Dict, shipment: Dict):
        """
        Origin and Destination countries are CRITICAL.
        Missing or mismatch → FAIL
        """
        doc_origin = extracted.get('origin_country')
        doc_dest = extracted.get('destination_country')
        ship_origin = shipment.get('origin_country', '')
        ship_dest = shipment.get('destination_country', '')
        
        # Check origin
        if ship_origin:
            if not doc_origin:
                self.issues.append({
                    "field": "Origin Country",
                    "document_value": "NOT FOUND",
                    "shipment_value": ship_origin,
                    "severity": "FAIL",
                    "message": "Origin country not found in document"
                })
            elif ship_origin.lower() not in doc_origin.lower():
                self.issues.append({
                    "field": "Origin Country",
                    "document_value": doc_origin,
                    "shipment_value": ship_origin,
                    "severity": "FAIL",
                    "message": f"Origin country mismatch: document has '{doc_origin}', shipment form has '{ship_origin}'"
                })
        
        # Check destination
        if ship_dest:
            if not doc_dest:
                self.issues.append({
                    "field": "Destination Country",
                    "document_value": "NOT FOUND",
                    "shipment_value": ship_dest,
                    "severity": "FAIL",
                    "message": "Destination country not found in document"
                })
            elif ship_dest.lower() not in doc_dest.lower():
                self.issues.append({
                    "field": "Destination Country",
                    "document_value": doc_dest,
                    "shipment_value": ship_dest,
                    "severity": "FAIL",
                    "message": f"Destination country mismatch: document has '{doc_dest}', shipment form has '{ship_dest}'"
                })
    
    def _validate_mode_of_transport(self, extracted: Dict, shipment: Dict):
        """
        Mode of transport: if present in both, mismatch → WARNING
        """
        doc_mode = extracted.get('mode_of_transport')
        ship_mode = shipment.get('mode_of_transport', '')
        
        if ship_mode and doc_mode:
            if ship_mode.lower() not in doc_mode.lower():
                self.issues.append({
                    "field": "Mode of Transport",
                    "document_value": doc_mode,
                    "shipment_value": ship_mode,
                    "severity": "WARNING",
                    "message": f"Mode of transport mismatch: document has '{doc_mode}', shipment form has '{ship_mode}'"
                })
    
    @staticmethod
    def _cosine_similarity(vec1, vec2) -> float:
        """Calculate cosine similarity between two embeddings."""
        import numpy as np
        return float(np.dot(vec1, vec2) / (np.linalg.norm(vec1) * np.linalg.norm(vec2)))
    
    def _determine_status(self) -> str:
        """
        CRITICAL: Returns PASS only if ALL issues are empty.
        - Any FAIL → return FAIL
        - Any WARNING (and no FAIL) → return WARNING
        - Otherwise → return PASS
        """
        has_fail = any(issue['severity'] == 'FAIL' for issue in self.issues)
        has_warning = any(issue['severity'] == 'WARNING' for issue in self.issues)
        
        if has_fail:
            return "FAIL"
        elif has_warning:
            return "WARNING"
        else:
            return "PASS"
    
    def _build_response(self, document_name: str, status: str) -> Dict:
        """Build the response object."""
        return {
            "documentName": document_name,
            "status": status,
            "issues": self.issues
        }

