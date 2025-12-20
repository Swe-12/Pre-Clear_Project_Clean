import json

# Test the /suggest-hs endpoint
payload = {
    "name": "Test Product",
    "category": "Electronics",
    "description": "Computer Equipment",
    "k": 5
}

print(f"Sending request: {json.dumps(payload)}")
response = requests.post("http://127.0.0.1:8001/suggest-hs", json=payload)
print(f"Status: {response.status_code}")
print(f"Response: {response.text}")
