import urllib.request
import os

req = urllib.request.Request(
    'https://api.pixellab.ai/mcp/messages',
    headers={
        'Authorization': 'Bearer 704a1bcc-104f-4bee-a729-20de458a039d',
        'Content-Type': 'application/json'
    },
    data=b'{"jsonrpc": "2.0", "id": 1, "method": "tools/call", "params": {"name": "create_sidescroller_tileset", "arguments": {"lower_description": "dungeon", "tile_size": {"width": 16, "height": 16}}}}'
)
try:
    with urllib.request.urlopen(req) as res:
        print(res.read().decode())
except Exception as e:
    print(e)
