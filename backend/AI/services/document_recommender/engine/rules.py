"""
Deterministic Rules Engine for Mandatory Trade Compliance Documents

This module encodes mandatory regulatory requirements derived from dataset patterns.
Rules are triggered based on HS Code, Product Category, Mode of Transport, and Country pairs.
Documents returned by this engine MUST always be included in final recommendations.
"""

from typing import Set, Dict, List
import re


class ComplianceRulesEngine:
    """Rule-based engine for mandatory compliance documents."""
    
    def __init__(self):
        """Initialize the rules engine with regulatory patterns."""
        self._initialize_rules()
    
    def _initialize_rules(self):
        """Define all deterministic rules based on dataset analysis."""
        
        # HS Code prefix patterns for product classification
        self.hs_patterns = {
            'pharmaceuticals': ['3004'],  # Medicinal products
            'medical_devices': ['9018'],  # Medical instruments
            'chemicals': ['2807', '2809', '2810', '2811', '2812', '2813', '2814', '2815', '2816', '2817', '2818', '2819', '2820'],
            'food_agriculture': ['1006', '0201', '0202', '0203', '0204', '0401', '0402'],  # Food items
            'electronics': ['8504', '8517', '8518', '8519', '8520', '8521', '8522'],  # Electronic equipment
            'vehicles': ['8703', '8704', '8705'],  # Motor vehicles
            'textiles': ['6203', '6204', '6205', '6206'],  # Clothing
        }
        
        # Country-specific requirements
        self.us_destinations = ['united states', 'usa', 'us']
        self.eu_countries = ['germany', 'france', 'united kingdom', 'italy', 'spain', 'netherlands', 'belgium']
        self.canada = ['canada']
        
    def get_mandatory_documents(
        self,
        origin_country: str = "",
        destination_country: str = "",
        hs_code: str = "",
        hts_flag: bool = False,
        product_category: str = "",
        product_description: str = "",
        package_type_weight: str = "",
        mode_of_transport: str = ""
    ) -> Dict[str, str]:
        """
        Apply deterministic rules to determine mandatory documents.
        
        Returns:
            Dict mapping document name to explanation (why it's required)
        """
        mandatory_docs = {}
        
        # Normalize inputs
        origin = origin_country.lower().strip()
        destination = destination_country.lower().strip()
        hs = hs_code.strip()
        category = product_category.lower().strip()
        description = product_description.lower().strip()
        mode = mode_of_transport.lower().strip()
        
        # Determine product type from HS code
        product_type = self._classify_product_by_hs(hs)
        
        # Rule 1: US FDA Requirements (Pharmaceuticals)
        if self._is_us_destination(destination) and product_type == 'pharmaceuticals':
            mandatory_docs['FDA Compliance'] = 'Required for pharmaceutical products entering the United States'
            mandatory_docs['Certificate of Analysis'] = 'Quality assurance document required for pharmaceuticals'
            mandatory_docs['Drug License'] = 'Authorization to manufacture/distribute pharmaceutical products'
        
        # Rule 2: US FDA Requirements (Food & Agriculture)
        if self._is_us_destination(destination) and product_type == 'food_agriculture':
            mandatory_docs['FDA Prior Notice'] = 'Advance notification required for food products entering US'
            mandatory_docs['FSSAI Compliance'] = 'Food safety standard compliance documentation'
            mandatory_docs['Phytosanitary Certificate'] = 'Plant health certification for agricultural products'
        
        # Rule 3: Canada Import Requirements (Pharmaceuticals)
        if self._is_canada_destination(destination) and product_type == 'pharmaceuticals':
            mandatory_docs['FDA Compliance'] = 'Health Canada requires FDA-equivalent compliance for pharmaceuticals'
            mandatory_docs['Certificate of Analysis'] = 'Quality control documentation required by Health Canada'
            mandatory_docs['Drug License'] = 'Canadian pharmaceutical import authorization'
        
        # Rule 4: Canada Import Requirements (Medical Devices)
        if self._is_canada_destination(destination) and product_type == 'medical_devices':
            mandatory_docs['CE Compliance'] = 'Medical device certification required for Canadian imports'
            mandatory_docs['FDA Compliance'] = 'Health Canada medical device authorization'
        
        # Rule 5: Canada Import Requirements (Electronics)
        if self._is_canada_destination(destination) and product_type == 'electronics':
            mandatory_docs['BIS Compliance'] = 'Electronic safety standards certification'
            mandatory_docs['CE Compliance'] = 'Electromagnetic compatibility certification'
            mandatory_docs['FCC Compliance'] = 'Wireless device authorization for Canadian market'
        
        # Rule 6: EU CE Marking (Pharmaceuticals)
        if self._is_eu_destination(destination) and product_type == 'pharmaceuticals':
            mandatory_docs['CE Compliance'] = 'EU conformity marking required for pharmaceutical products'
            mandatory_docs['Certificate of Analysis'] = 'EU pharmaceutical quality standards documentation'
            mandatory_docs['Drug License'] = 'EU pharmaceutical manufacturing/distribution authorization'
        
        # Rule 7: EU CE Marking (Medical Devices)
        if self._is_eu_destination(destination) and product_type == 'medical_devices':
            mandatory_docs['CE Compliance'] = 'EU medical device directive compliance marking'
        
        # Rule 8: Dangerous Goods (Chemicals)
        if product_type == 'chemicals':
            mandatory_docs['Dangerous Goods Declaration'] = 'IATA/IMDG hazardous materials declaration'
            mandatory_docs['MSDS'] = 'Material Safety Data Sheet required for all chemical shipments'
        
        # Rule 9: Phytosanitary Certificate (Food & Agriculture)
        if product_type == 'food_agriculture':
            mandatory_docs['Phytosanitary Certificate'] = 'Plant health certification required for agricultural products'
            mandatory_docs['Certificate of Origin'] = 'Agricultural product origin verification'
        
        # Rule 10: Type Approval Certificate (Vehicles)
        if product_type == 'vehicles':
            mandatory_docs['Type Approval Certificate'] = 'Vehicle safety and emissions standards certification'
        
        # Rule 11: India Import Export Code
        if self._is_india_destination(destination):
            mandatory_docs['Import Export Code (IEC)'] = 'Indian customs clearance authorization code'
        
        # Rule 12: HTS/Regional Tariff Flag Requirements
        if hts_flag:
            # Additional scrutiny for tariff-sensitive products
            if product_type == 'textiles':
                mandatory_docs['Certificate of Origin'] = 'Origin verification for tariff classification'
        
        # Rule 13: Mode-specific requirements
        if mode in ['air']:
            if product_type == 'chemicals':
                mandatory_docs['Dangerous Goods Declaration'] = 'IATA air transport of dangerous goods authorization'
        
        # Rule 14: Category-based fallback rules
        if 'pharma' in category or 'medicine' in category or 'drug' in description:
            if 'Certificate of Analysis' not in mandatory_docs:
                mandatory_docs['Certificate of Analysis'] = 'Pharmaceutical quality assurance documentation'
            if 'Drug License' not in mandatory_docs:
                mandatory_docs['Drug License'] = 'Pharmaceutical product authorization'
        
        if 'chemical' in category or 'acid' in description or 'hazard' in description:
            if 'MSDS' not in mandatory_docs:
                mandatory_docs['MSDS'] = 'Chemical safety information requirement'
        
        if 'food' in category or 'agricult' in category or 'rice' in description or 'grain' in description:
            if 'Phytosanitary Certificate' not in mandatory_docs:
                mandatory_docs['Phytosanitary Certificate'] = 'Agricultural product health certification'
        
        if 'vehicle' in category or 'automotive' in category or 'car' in description:
            if 'Type Approval Certificate' not in mandatory_docs:
                mandatory_docs['Type Approval Certificate'] = 'Vehicle regulatory compliance certification'
        
        # Rule 15: Universal documents (always required for commercial shipments)
        mandatory_docs['Commercial Invoice'] = 'Universal customs documentation required for all commercial shipments'
        mandatory_docs['Packing List'] = 'Detailed shipment contents listing required for customs clearance'
        
        return mandatory_docs
    
    def _classify_product_by_hs(self, hs_code: str) -> str:
        """Classify product type based on HS code prefix."""
        if not hs_code:
            return 'unknown'
        
        # Extract first 4 digits
        hs_prefix = hs_code[:4] if len(hs_code) >= 4 else hs_code
        
        for product_type, prefixes in self.hs_patterns.items():
            if hs_prefix in prefixes:
                return product_type
        
        return 'unknown'
    
    def _is_us_destination(self, destination: str) -> bool:
        """Check if destination is United States."""
        return any(us in destination for us in self.us_destinations)
    
    def _is_eu_destination(self, destination: str) -> bool:
        """Check if destination is in European Union."""
        return any(eu in destination for eu in self.eu_countries)
    
    def _is_canada_destination(self, destination: str) -> bool:
        """Check if destination is Canada."""
        return any(ca in destination for ca in self.canada)
    
    def _is_india_destination(self, destination: str) -> bool:
        """Check if destination is India."""
        return 'india' in destination


# Global instance for reuse
_rules_engine_instance = None


def get_rules_engine() -> ComplianceRulesEngine:
    """Get or create the global rules engine instance."""
    global _rules_engine_instance
    if _rules_engine_instance is None:
        _rules_engine_instance = ComplianceRulesEngine()
    return _rules_engine_instance
