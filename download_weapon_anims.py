"""
download_weapon_anims.py
Run this script after PixelLab finishes generating the weapon attack animations
to automatically download the frames and print the Player.tscn additions needed.

Usage:  python download_weapon_anims.py
Requires: PIXELLAB_API_KEY env var set
"""

import os
import json
import urllib.request
import zipfile
import shutil

CHARACTER_ID = "66ff5bce-5a4e-4981-9c2f-7f5acd6bb5ee"
_raw_key = os.environ.get("PIXELLAB_API_KEY", "704a1bcc-104f-4bee-a729-20de458a039d")
API_KEY = _raw_key.removeprefix("Bearer ").strip()
ANIMATIONS_DIR = os.path.join(os.path.dirname(__file__), "Assets", "characters", "explorer", "animations")
ZIP_PATH = os.path.join(os.path.dirname(__file__), "explorer_weapon_anims.zip")

# Maps PixelLab template -> game animation name
WEAPON_ANIM_MAP = {
    "roundhouse-kick": "roundhouse-kick",   # Sword
    "flying-kick":     "flying-kick",       # DemontekSaber
    "surprise-uppercut": "surprise-uppercut", # Sawtooth
    "lead-jab":        "lead-jab",          # AlienSword
}

def get_character():
    req = urllib.request.Request(
        f"https://api.pixellab.ai/v1/animate-character/characters/{CHARACTER_ID}",
        headers={"Authorization": f"Bearer {API_KEY}"}
    )
    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read())

def download_zip():
    """Downloads all character animations as ZIP"""
    req = urllib.request.Request(
        f"https://api.pixellab.ai/mcp/characters/{CHARACTER_ID}/download",
        headers={"Authorization": f"Bearer {API_KEY}"}
    )
    with urllib.request.urlopen(req) as resp, open(ZIP_PATH, "wb") as f:
        shutil.copyfileobj(resp, f)
    return os.path.getsize(ZIP_PATH)

def extract_and_organize():
    """Extract ZIP and copy animation frames for new weapon animations"""
    extract_dir = ZIP_PATH.replace(".zip", "_extract")
    os.makedirs(extract_dir, exist_ok=True)
    with zipfile.ZipFile(ZIP_PATH, "r") as z:
        z.extractall(extract_dir)

    for template_name, anim_name in WEAPON_ANIM_MAP.items():
        src_anim = os.path.join(extract_dir, "animations", template_name)
        dst_anim = os.path.join(ANIMATIONS_DIR, template_name)

        if not os.path.exists(src_anim):
            print(f"MISSING in ZIP: {template_name}")
            continue

        for direction in ["east", "west", "north", "south"]:
            src_dir = os.path.join(src_anim, direction)
            dst_dir = os.path.join(dst_anim, direction)
            os.makedirs(dst_dir, exist_ok=True)

            if not os.path.exists(src_dir):
                # Fallback: copy west frames as east
                fallback = os.path.join(src_anim, "west")
                if os.path.exists(fallback) and direction == "east":
                    for f in os.listdir(fallback):
                        if f.endswith(".png"):
                            shutil.copy2(os.path.join(fallback, f), os.path.join(dst_dir, f))
                    print(f"  Copied west->east fallback for {template_name}")
                continue

            for filename in os.listdir(src_dir):
                if filename.endswith(".png"):
                    shutil.copy2(os.path.join(src_dir, filename), os.path.join(dst_dir, filename))

        frames = [f for f in os.listdir(os.path.join(dst_anim, "east")) if f.endswith(".png")]
        frames.sort()
        print(f"OK: {template_name} -> {len(frames)} frames/dir")
        print(f"    Add to Player.tscn: animation name 'attack_{anim_name.replace('-', '_')}'")

if __name__ == "__main__":
    print("Checking character status...")
    char = get_character()
    anims = {a["name"]: a for a in char.get("animations", [])}
    
    pending = [name for name in WEAPON_ANIM_MAP if name not in anims]
    complete = [name for name in WEAPON_ANIM_MAP if name in anims]
    
    print(f"Complete: {complete}")
    print(f"Pending: {pending}")
    
    if pending:
        print(f"Still waiting for {len(pending)} animations. Run again when they complete.")
    else:
        print("All animations complete! Downloading ZIP...")
        size = download_zip()
        print(f"Downloaded ZIP: {size} bytes")
        extract_and_organize()
        print("Done! Open Player.tscn in Godot and verify the new attack animations.")
