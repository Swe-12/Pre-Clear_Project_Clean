#!/usr/bin/env python3
from validator import DocumentValidator

validator = DocumentValidator()

doc_text = """
Commercial Invoice
Product: Electronic Device
HS Code: 851712
Quantity: 100
Weight: 50 kg
Country of Origin: USA
Country of Destination: Canada
Package Type: Box
"""

shipment_data = {
    "hs_code": "847130",  # Different HS code - should FAIL
    "product_description": "Electronic Device",
    "quantity": "100",
    "weight": "50",
    "origin_country": "USA",
    "destination_country": "Canada",
    "package_type": "Box"
}

result = validator.validate(shipment_data, doc_text, "test.pdf")
print("Status:", result['status'])
print("\nAll issues:")
for issue in result['issues']:
    print(f"  Field: {issue['field']}")
    print(f"  Severity: {issue['severity']}")
    print(f"  Message: {issue['message']}")
    print()
