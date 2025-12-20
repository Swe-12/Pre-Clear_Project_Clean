"""
Document-Form Consistency Validator API
FastAPI service for validating document content against shipment form data.
"""
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Dict, List, Optional
import os

from ocr import extract_text
from validator import DocumentValidator

app = FastAPI(title='Document-Form Consistency Validator')


class ShipmentData(BaseModel):
    origin_country: str = ''
    destination_country: str = ''
    hs_code: str = ''
    product_description: str = ''
    quantity: str = ''
    weight: str = ''
    package_type: str = ''
    mode_of_transport: str = ''


class ValidateRequest(BaseModel):
    shipment: ShipmentData
    document_name: str
    file_path: str


class ValidateResponse(BaseModel):
    documentName: str
    status: str  # PASS | WARNING | FAIL
    issues: List[Dict]  # Detailed field-level issues


@app.post('/validate-document', response_model=ValidateResponse)
def validate_document(request: ValidateRequest):
    """
    Validate whether uploaded document content matches shipment form data.
    
    Does NOT check:
    - Legal import/export eligibility
    - Country-specific trade rules
    - Prohibited/restricted products
    - Compliance thresholds
    
    Only checks document-form consistency.
    """
    try:
        # Extract text from document
        document_text = extract_text(request.file_path)
        
    except FileNotFoundError:
        raise HTTPException(status_code=404, detail=f"Document file not found: {request.file_path}")
    
    except ValueError as e:
        # Unreadable or empty document
        return ValidateResponse(
            documentName=request.document_name,
            status='FAIL',
            issues=[f"Document unreadable: {str(e)}"]
        )
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Text extraction failed: {str(e)}")
    
    # Validate consistency
    validator = DocumentValidator()
    shipment_dict = request.shipment.dict()
    
    result = validator.validate(
        shipment_data=shipment_dict,
        document_text=document_text,
        document_name=request.document_name
    )
    
    return ValidateResponse(**result)


@app.get('/health')
def health():
    """Health check endpoint."""
    return {'status': 'ok', 'service': 'document-validator'}


@app.get('/info')
def info():
    """Service information."""
    return {
        'service': 'Document-Form Consistency Validator',
        'version': '1.0.0',
        'description': 'Validates document content against shipment form data',
        'scope': 'Consistency validation only - NO compliance rules'
    }


if __name__ == '__main__':
    import uvicorn
    uvicorn.run(app, host='0.0.0.0', port=8003)
