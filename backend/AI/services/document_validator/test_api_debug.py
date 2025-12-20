#!/usr/bin/env python3
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
        "hs_code": "847130",
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

print(f"Request payload file path: {pdf_path}")
print(f"File exists: {os.path.exists(pdf_path)}")

response = requests.post('http://localhost:8003/validate-document', json=payload)
print(f"Response status: {response.status_code}")
print(f"Response text: {response.text}")

try:
    result = response.json()
    print(json.dumps(result, indent=2))
except Exception as e:
    print(f"Error parsing JSON: {e}")

os.unlink(pdf_path)
