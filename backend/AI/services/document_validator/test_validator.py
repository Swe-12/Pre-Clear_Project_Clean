#!/usr/bin/env python3
"""
Test cases for STRICT Document Validator
"""
from validator import DocumentValidator

def test_hs_code_mismatch():
    """Test Case 1: HS Code Mismatch - Should FAIL"""
    validator = DocumentValidator()
    
    doc_text = """
    COMMERCIAL INVOICE
    Invoice Number: INV-2024-001
    Date: December 16, 2024
    Seller: Electronics Manufacturing LLC
    Product: Electronic Device
    Product Description: Industrial Control Module
    HS Code: 851712
    Quantity: 100
    Total Quantity: 100 units
    Gross Weight: 50 kg
    Net Weight: 45 kg
    Country of Origin: USA
    Country of Destination: Canada
    Package Type: Box
    Shipping Method: Air Freight
    Mode of Transport: Air
    Terms: FOB
    Total Value: $50,000 USD
    This is a detailed commercial invoice with all required information
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
    
    result = validator.validate(shipment_data, doc_text, "test_hs_mismatch.pdf")
    print("✓ Test 1 - HS Code Mismatch")
    print(f"  Expected: FAIL | Got: {result['status']}")
    assert result['status'] == 'FAIL', f"Expected FAIL, got {result['status']}"
    
    fail_issues = [i for i in result['issues'] if i['severity'] == 'FAIL']
    assert len(fail_issues) > 0, "Expected FAIL issues"
    assert any('HS Code' in i['field'] for i in fail_issues), "Expected HS Code FAIL"
    print("  ✓ Correctly identified HS code mismatch")
    print()


def test_all_fields_match():
    """Test Case 2: All Fields Match - Should PASS"""
    validator = DocumentValidator()
    
    doc_text = """
    COMMERCIAL INVOICE
    Invoice Number: INV-2024-002
    Date: December 16, 2024
    Seller: Industrial Equipment Ltd
    Product: Widget Manufacturing Equipment
    Product Description: Widget Manufacturing Equipment
    HS Code: 847130
    Quantity: 50
    Total Quantity: 50 units
    Gross Weight: 25 kg
    Net Weight: 22 kg
    Country of Origin: China
    Country of Destination: USA
    Package Type: Carton
    Shipping Method: Sea Freight
    Mode of Transport: Sea
    Terms: CIF
    Total Value: $75,000 USD
    This is a detailed commercial invoice with all required information included
    """
    
    shipment_data = {
        "hs_code": "847130",
        "product_description": "Widget Manufacturing Equipment",
        "quantity": "50",
        "weight": "25",
        "origin_country": "China",
        "destination_country": "USA",
        "package_type": "Carton"
    }
    
    result = validator.validate(shipment_data, doc_text, "test_all_match.pdf")
    print("✓ Test 2 - All Fields Match")
    print(f"  Expected: PASS | Got: {result['status']}")
    assert result['status'] == 'PASS', f"Expected PASS, got {result['status']} with issues: {result['issues']}"
    print("  ✓ Correctly returned PASS")
    print()


def test_quantity_mismatch():
    """Test Case 3: Quantity Mismatch - Should FAIL"""
    validator = DocumentValidator()
    
    doc_text = """
    COMMERCIAL INVOICE
    Invoice Number: INV-2024-003
    Date: December 16, 2024
    Seller: Computer Technology Inc
    Product: Laptop Computers
    Product Description: Professional Laptop Computers
    HS Code: 847130
    Quantity: 100
    Total Quantity: 100 units
    Gross Weight: 50 kg
    Net Weight: 48 kg
    Country of Origin: Taiwan
    Country of Destination: USA
    Package Type: Box
    Shipping Method: Air Freight
    Mode of Transport: Air
    Terms: FOB
    Total Value: $100,000 USD
    This is a detailed commercial invoice with all required information included
    """
    
    shipment_data = {
        "hs_code": "847130",
        "product_description": "Laptop Computers",
        "quantity": "50",  # Different quantity - should FAIL
        "weight": "50",
        "origin_country": "Taiwan",
        "destination_country": "USA",
        "package_type": "Box"
    }
    
    result = validator.validate(shipment_data, doc_text, "test_qty_mismatch.pdf")
    print("✓ Test 3 - Quantity Mismatch")
    print(f"  Expected: FAIL | Got: {result['status']}")
    assert result['status'] == 'FAIL', f"Expected FAIL, got {result['status']}"
    
    fail_issues = [i for i in result['issues'] if i['severity'] == 'FAIL']
    assert any('Quantity' in i['field'] for i in fail_issues), "Expected Quantity FAIL"
    print("  ✓ Correctly identified quantity mismatch")
    print()


def test_weight_warning():
    """Test Case 4: Weight Slightly Different - Should WARNING"""
    validator = DocumentValidator()
    
    doc_text = """
    COMMERCIAL INVOICE
    Invoice Number: INV-2024-004
    Date: December 16, 2024
    Seller: Steel Manufacturing Company
    Product: Steel Parts
    Product Description: Steel Manufacturing Parts
    HS Code: 730890
    Quantity: 100
    Total Quantity: 100 units
    Gross Weight: 28 kg
    Net Weight: 26 kg
    Country of Origin: India
    Country of Destination: Brazil
    Package Type: Pallet
    Shipping Method: Sea Freight
    Mode of Transport: Sea
    Terms: CIF
    Total Value: $25,000 USD
    This is a detailed commercial invoice with all required information included
    """
    
    shipment_data = {
        "hs_code": "730890",
        "product_description": "Steel Parts",
        "quantity": "100",
        "weight": "25",  # 3 kg difference - should WARNING
        "origin_country": "India",
        "destination_country": "Brazil",
        "package_type": "Pallet"
    }
    
    result = validator.validate(shipment_data, doc_text, "test_weight_warn.pdf")
    print("✓ Test 4 - Weight Slight Difference")
    print(f"  Expected: WARNING | Got: {result['status']}")
    assert result['status'] == 'WARNING', f"Expected WARNING, got {result['status']}"
    
    warn_issues = [i for i in result['issues'] if i['severity'] == 'WARNING']
    assert any('Weight' in i['field'] for i in warn_issues), "Expected Weight WARNING"
    print("  ✓ Correctly identified weight difference as WARNING")
    print()


def test_missing_destination():
    """Test Case 5: Missing Destination Country - Should FAIL"""
    validator = DocumentValidator()
    
    doc_text = """
    COMMERCIAL INVOICE
    Invoice Number: INV-2024-005
    Date: December 16, 2024
    Seller: Textile Manufacturing Corporation
    Product: Textiles
    Product Description: Fine Quality Textiles
    HS Code: 620530
    Quantity: 200
    Total Quantity: 200 units
    Gross Weight: 100 kg
    Net Weight: 95 kg
    Country of Origin: India
    Package Type: Box
    Shipping Method: Sea Freight
    Mode of Transport: Sea
    Terms: FOB
    Total Value: $30,000 USD
    This is a detailed commercial invoice with all required information included
    """
    
    shipment_data = {
        "hs_code": "620530",
        "product_description": "Textiles",
        "quantity": "200",
        "weight": "100",
        "origin_country": "India",
        "destination_country": "USA",  # Missing in doc
        "package_type": "Box"
    }
    
    result = validator.validate(shipment_data, doc_text, "test_missing_dest.pdf")
    print("✓ Test 5 - Missing Destination Country")
    print(f"  Expected: FAIL | Got: {result['status']}")
    assert result['status'] == 'FAIL', f"Expected FAIL, got {result['status']}"
    
    fail_issues = [i for i in result['issues'] if i['severity'] == 'FAIL']
    assert any('Destination' in i['field'] for i in fail_issues), "Expected Destination FAIL"
    print("  ✓ Correctly identified missing destination")
    print()


def test_insufficient_content():
    """Test Case 6: Document Content < 200 chars - Should FAIL"""
    validator = DocumentValidator()
    
    doc_text = "Short text."  # Way too short
    
    shipment_data = {
        "hs_code": "847130",
        "product_description": "Test",
        "quantity": "10",
        "weight": "5",
        "origin_country": "USA",
        "destination_country": "Canada",
        "package_type": "Box"
    }
    
    result = validator.validate(shipment_data, doc_text, "test_short.pdf")
    print("✓ Test 6 - Insufficient Document Content")
    print(f"  Expected: FAIL | Got: {result['status']}")
    assert result['status'] == 'FAIL', f"Expected FAIL, got {result['status']}"
    print("  ✓ Correctly rejected insufficient content")
    print()


if __name__ == '__main__':
    print("=" * 60)
    print("STRICT DOCUMENT VALIDATOR TEST SUITE")
    print("=" * 60)
    print()
    
    try:
        test_hs_code_mismatch()
        test_all_fields_match()
        test_quantity_mismatch()
        test_weight_warning()
        test_missing_destination()
        test_insufficient_content()
        
        print("=" * 60)
        print("✓ ALL TESTS PASSED - STRICT VALIDATOR WORKING CORRECTLY")
        print("=" * 60)
    except AssertionError as e:
        print(f"✗ TEST FAILED: {e}")
        exit(1)
