"""
download_weapon_anims_v2.py
Downloads weapon animation frames from PixelLab character API frame URLs.
Works without waiting for ZIP (downloads each frame individually).

Usage:  python download_weapon_anims_v2.py
        python download_weapon_anims_v2.py --wait   (polls until all done)
"""

import os
import sys
import json
import time
import urllib.request

CHARACTER_ID = "66ff5bce-5a4e-4981-9c2f-7f5acd6bb5ee"
_raw_key = os.environ.get("PIXELLAB_API_KEY", "704a1bcc-104f-4bee-a729-20de458a039d")
API_KEY = _raw_key.removeprefix("Bearer ").strip()
ANIMATIONS_DIR = os.path.join(os.path.dirname(__file__), "Assets", "characters", "explorer", "animations")

# Maps PixelLab template -> game animation name
WEAPON_ANIM_MAP = {
    "roundhouse-kick":   "roundhouse-kick",   # Sword
    "flying-kick":       "flying-kick",       # DemontekSaber
    "surprise-uppercut": "surprise-uppercut", # Sawtooth
    "lead-jab":          "lead-jab",          # AlienSword
}

def get_character():
    req = urllib.request.Request(
        f"https://api.pixellab.ai/v1/animate-character/characters/{CHARACTER_ID}",
        headers={"Authorization": f"Bearer {API_KEY}"}
    )
    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read())

def download_frame(url, dest_path):
    req = urllib.request.Request(url, headers={"Authorization": f"Bearer {API_KEY}"})
    with urllib.request.urlopen(req) as resp, open(dest_path, "wb") as f:
        f.write(resp.read())

def download_anim_frames(char_data, template_name):
    """Download all east frames for a given animation template from the API response."""
    animations = char_data.get("animations", [])
    
    # Group by animation name
    dirs_found = {}
    for anim in animations:
        if anim.get("name") == template_name:
            direction = anim.get("direction", "east")
            frames = anim.get("frames", [])
            dirs_found[direction] = frames

    if not dirs_found:
        return False, 0

    # Prefer east; fall back to west
    for preferred_dir in ["east", "west", "south", "north"]:
        if preferred_dir in dirs_found:
            frames = dirs_found[preferred_dir]
            dst_dir = os.path.join(ANIMATIONS_DIR, template_name, "east")
            os.makedirs(dst_dir, exist_ok=True)
            
            for i, frame in enumerate(frames):
                url = frame.get("image_url") or frame.get("url") or frame.get("sprite_url")
                if not url:
                    print(f"  WARNING: No URL for frame {i} of {template_name}/{preferred_dir}")
                    continue
                dest = os.path.join(dst_dir, f"frame_{i:03d}.png")
                if not os.path.exists(dest):
                    download_frame(url, dest)
            
            actual_count = len([f for f in os.listdir(dst_dir) if f.endswith(".png")])
            if preferred_dir != "east":
                print(f"  Used {preferred_dir}->east fallback for {template_name}")
            return True, actual_count

    return False, 0

def wait_for_animations(needed):
    """Poll until all needed animations are complete."""
    poll_interval = 30
    while True:
        char = get_character()
        anim_names = {a["name"] for a in char.get("animations", [])}
        pending_jobs = char.get("pending_jobs", []) or char.get("jobs", [])
        
        still_pending = [n for n in needed if n not in anim_names]
        # Also check pending jobs
        job_names = set()
        for job in pending_jobs:
            job_name = job.get("animation_name") or job.get("name", "")
            if job_name:
                job_names.add(job_name)
        
        still_processing = [n for n in needed if n in job_names]
        
        if not still_pending and not still_processing:
            print("All animations complete!")
            return char
        
        todo = set(still_pending) | set(still_processing)
        print(f"Still processing: {list(todo)}. Waiting {poll_interval}s...")
        time.sleep(poll_interval)

if __name__ == "__main__":
    wait_mode = "--wait" in sys.argv
    
    print("Checking character status...")
    char = get_character()
    
    # Debug: show raw animation structure
    sample = char.get("animations", [{}])[0] if char.get("animations") else {}
    print(f"  Sample animation keys: {list(sample.keys())}")
    
    anim_names = {a["name"] for a in char.get("animations", [])}
    pending = [name for name in WEAPON_ANIM_MAP if name not in anim_names]
    complete = [name for name in WEAPON_ANIM_MAP if name in anim_names]
    
    print(f"Complete: {complete}")
    print(f"Pending/Missing: {pending}")
    
    if pending:
        if wait_mode:
            print(f"Waiting for {len(pending)} animations to complete...")
            char = wait_for_animations(pending)
        else:
            print(f"Still waiting for: {pending}")
            print("Run with --wait to poll automatically, or re-run when complete.")
            if not complete:
                sys.exit(1)
            print(f"Downloading what's available: {complete}")
    
    print("\nDownloading animation frames...")
    results = {}
    for template_name in WEAPON_ANIM_MAP:
        if template_name not in anim_names and not wait_mode:
            print(f"  SKIP: {template_name} (not ready)")
            continue
        
        # Check if already downloaded
        dst_dir = os.path.join(ANIMATIONS_DIR, template_name, "east")
        existing = [f for f in os.listdir(dst_dir) if f.endswith(".png")] if os.path.exists(dst_dir) else []
        if existing:
            print(f"  ALREADY LOCAL: {template_name} ({len(existing)} frames)")
            results[template_name] = len(existing)
            continue
        
        ok, count = download_anim_frames(char, template_name)
        if ok:
            print(f"  OK: {template_name} -> {count} frames")
            results[template_name] = count
        else:
            print(f"  FAILED: {template_name}")
    
    print("\nSummary:")
    for name, count in results.items():
        game_anim = WEAPON_ANIM_MAP[name]
        print(f"  {name}: {count} frames -> animation 'attack_{game_anim.replace('-', '_')}'")
