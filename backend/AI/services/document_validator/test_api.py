#!/usr/bin/env python3
"""
Test document validator API
"""
import json
import requests
import tempfile
import os
from reportlab.pdfgen import canvas
from reportlab.lib.pagesizes import letter

def create_test_pdf(content):
    """Create a simple test PDF with given content"""
    with tempfile.NamedTemporaryFile(mode='w', suffix='.pdf', delete=False) as f:
        pdf_path = f.name
    
    c = canvas.Canvas(pdf_path, pagesize=letter)
    y = 750
    for line in content.split('\n'):
        if line.strip():
            c.drawString(50, y, line[:100])
            y -= 20
    c.save()
    return pdf_path

# Test Case 1: HS Code Mismatch
print("Test 1: HS Code Mismatch (Expected FAIL)")
pdf_path = create_test_pdf("""
COMMERCIAL INVOICE
Date: December 16, 2024
Product Description: Electronic Device
HS Code: 851712
Quantity: 100
Gross Weight: 50 kg
Country of Origin: USA
Country of Destination: Canada
Package Type: Box
""")

payload = {
    "shipment": {
        "hs_code": "847130",  # Mismatch!
        "product_description": "Electronic Device",
        "quantity": "100",
        "weight": "50",
        "origin_country": "USA",
        "destination_country": "Canada",
        "package_type": "Box"
    },
    "document_name": "invoice.pdf",
    "file_path": pdf_path
}

response = requests.post('http://localhost:8003/validate-document', json=payload)
result = response.json()
print(f"  Status: {result['status']}")
print(f"  Issues: {len(result['issues'])}")
if result['issues']:
    for issue in result['issues']:
        print(f"    - {issue['field']}: {issue['severity']} - {issue['message'][:60]}")
print()

os.unlink(pdf_path)

# Test Case 2: All Match
print("Test 2: All Fields Match (Expected PASS)")
pdf_path = create_test_pdf("""
COMMERCIAL INVOICE
Date: December 16, 2024
Product Description: Widget Equipment
HS Code: 847130
Quantity: 50
Gross Weight: 25 kg
Country of Origin: China
Country of Destination: USA
Package Type: Carton
""")

payload = {
    "shipment": {
        "hs_code": "847130",
        "product_description": "Widget Equipment",
        "quantity": "50",
        "weight": "25",
        "origin_country": "China",
        "destination_country": "USA",
        "package_type": "Carton"
    },
    "document_name": "invoice.pdf",
    "file_path": pdf_path
}

response = requests.post('http://localhost:8003/validate-document', json=payload)
result = response.json()
print(f"  Status: {result['status']}")
print(f"  Issues: {len(result['issues'])}")
if result['issues']:
    for issue in result['issues']:
        print(f"    - {issue['field']}: {issue['severity']} - {issue['message'][:60]}")
print()

os.unlink(pdf_path)

print("✓ API Tests Complete")
