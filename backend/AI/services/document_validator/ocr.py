"""
OCR & Text Extraction Module
Extracts text from PDFs and images for document validation.
"""
import os
from typing import Optional

def extract_text(file_path: str) -> str:
    """
    Extract text from PDF or image file.
    
    Args:
        file_path: Absolute path to document file
        
    Returns:
        Normalized extracted text (at least 200 characters)
        
    Raises:
        FileNotFoundError: If file doesn't exist
        ValueError: If file is unreadable, empty, or has insufficient content
    """
    if not os.path.exists(file_path):
        raise FileNotFoundError(f"File not found: {file_path}")
    
    ext = os.path.splitext(file_path)[1].lower()
    
    if ext == '.pdf':
        text = _extract_from_pdf(file_path)
    elif ext in ['.jpg', '.jpeg', '.png', '.bmp', '.tiff']:
        text = _extract_from_image(file_path)
    else:
        raise ValueError(f"Unsupported file type: {ext}")
    
    # Verify minimum content requirement
    if len(text.strip()) < 200:
        raise ValueError(f"Document content insufficient: {len(text)} characters (minimum 200 required)")
    
    return text


def _extract_from_pdf(file_path: str) -> str:
    """Extract text from PDF using pdfplumber."""
    try:
        import pdfplumber
    except ImportError:
        raise ImportError("pdfplumber not installed. Run: pip install pdfplumber")
    
    try:
        text_parts = []
        with pdfplumber.open(file_path) as pdf:
            for page in pdf.pages:
                page_text = page.extract_text()
                if page_text:
                    text_parts.append(page_text)
        
        raw_text = '\n'.join(text_parts)
        
        if not raw_text or not raw_text.strip():
            raise ValueError("PDF contains no extractable text")
        
        return _normalize_text(raw_text)
    
    except Exception as e:
        if isinstance(e, ValueError):
            raise
        raise ValueError(f"Failed to extract text from PDF: {str(e)}")


def _extract_from_image(file_path: str) -> str:
    """Extract text from image using pytesseract."""
    try:
        import pytesseract
        from PIL import Image
    except ImportError:
        raise ImportError("pytesseract or PIL not installed. Run: pip install pytesseract pillow")
    
    try:
        img = Image.open(file_path)
        raw_text = pytesseract.image_to_string(img)
        
        if not raw_text or not raw_text.strip():
            raise ValueError("Image contains no extractable text")
        
        return _normalize_text(raw_text)
    
    except Exception as e:
        if isinstance(e, ValueError):
            raise
        raise ValueError(f"Failed to extract text from image: {str(e)}")


def _normalize_text(text: str) -> str:
    """
    Normalize extracted text.
    - Lowercase
    - Trim whitespace
    - Collapse multiple spaces
    """
    normalized = text.lower().strip()
    # Collapse multiple spaces/newlines to single space
    normalized = ' '.join(normalized.split())
    return normalized
