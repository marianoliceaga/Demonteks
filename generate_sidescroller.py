import os
import json
import urllib.request
import base64

_raw_key = os.environ.get("PIXELLAB_API_KEY", "704a1bcc-104f-4bee-a729-20de458a039d")
API_KEY = _raw_key.removeprefix("Bearer ").strip()

def create_image(prompt, out_path):
    url = "https://api.pixellab.ai/v2/create-image-pixflux"
    headers = {
        "Authorization": f"Bearer {API_KEY}",
        "Content-Type": "application/json"
    }
    
    data = {
        "description": prompt,
        "image_size": { "width": 48, "height": 48 },
        "no_background": True
    }
    
    print(f"Generating image for: {prompt[:30]}...")
    req = urllib.request.Request(url, headers=headers, data=json.dumps(data).encode('utf-8'))
    try:
        with urllib.request.urlopen(req) as response:
            result = json.loads(response.read().decode('utf-8'))
            if 'data' in result:
                d = result['data']
                if 'image' in d and 'base64' in d['image']:
                    b64 = d['image']['base64']
                elif 'base64' in d:
                    b64 = d['base64']
                elif 'images' in d and len(d['images']) > 0 and 'base64' in d['images'][0]:
                    b64 = d['images'][0]['base64']
                else:
                    print("Could not find base64 in response:", list(d.keys()))
                    return False
                    
                with open(out_path, "wb") as f:
                    f.write(base64.b64decode(b64))
                print(f"Saved to {out_path}")
                return True
            else:
                print("Unexpected response:", result)
                return False
    except urllib.error.HTTPError as e:
        print(f"Error creating image: {e.code} - {e.read().decode('utf-8')}")
        return False

if __name__ == "__main__":
    dungeon_prompt = "sidescroller tileset, dark stone blocks, mossy, dungeon. 3x3 grid of 16x16 tiles for 2d platformer game, perfectly tiled, pixel art"
    dungeon_out = r"d:\Demonteks\Assets\tilesets\dungeon_tileset_16x16.png"
    create_image(dungeon_prompt, dungeon_out)
    
    castle_prompt = "sidescroller tileset, castle brick wall, well-lit red carpet elegant floor. 3x3 grid of 16x16 tiles for 2d platformer game, perfectly tiled, pixel art"
    castle_out = r"d:\Demonteks\Assets\tilesets\castle_tileset_16x16.png"
    create_image(castle_prompt, castle_out)
