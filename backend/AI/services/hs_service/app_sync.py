"""HS Code Suggestion Service - Standalone implementation."""
import json
import sys
from typing import Dict

from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI(title="HS Code Suggestion Service", version="1.0.0")

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.get("/health")
def health():
    """Health check endpoint."""
    return {"status": "ok", "service": "hs-code-suggestion"}


@app.post("/suggest-hs")
def suggest_hs(request: Dict):
    """Suggest HS codes based on product description."""
    try:
        description = request.get("description", "").strip()
        k = request.get("k", 5) or 5
        
        print(f"[/suggest-hs] Query: {description[:60]}")
        
        # Mock suggestions
        suggestions = [
            {
                "hscode": "8471",
                "description": "Automatic data processing machines and units thereof",
                "score": 0.95
            },
            {
                "hscode": "8472",
                "description": "Other office machines",
                "score": 0.65
            }
        ]
        
        return {"suggestions": suggestions[:min(k, len(suggestions))]}
        
    except Exception as e:
        print(f"[/suggest-hs] Error: {str(e)}", file=sys.stderr)
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/suggest")
def suggest(request: Dict):
    """Alias for suggest-hs."""
    return suggest_hs(request)


@app.get("/info")
def service_info():
    """Service metadata."""
    return {
        "service": "HS Code Suggestion",
        "version": "1.0.0",
        "status": "operational"
    }


if __name__ == "__main__":
    import uvicorn
    uvicorn.run("app:app", host="127.0.0.1", port=8001, reload=False)
