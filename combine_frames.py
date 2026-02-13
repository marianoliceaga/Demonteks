#!/usr/bin/env python
"""
Combine animation frames into a horizontal sprite sheet
"""

import os
import sys
from PIL import Image


def combine_frames_to_sprite_sheet(frames_dir, output_path, frame_count=0):
    """
    Combine PNG frames from frames_dir into a horizontal sprite sheet

    Args:
        frames_dir: Directory containing frame_000.png, frame_001.png, etc.
        output_path: Path where sprite sheet will be saved
        frame_count: Number of frames (0 = auto-detect)
    """
    # Get all frame files
    frame_files = []
    if frame_count > 0:
        frame_files = [f"frame_{i:03d}.png" for i in range(frame_count)]
    else:
        # Auto-detect frames
        for f in os.listdir(frames_dir):
            if f.startswith("frame_") and f.endswith(".png"):
                frame_files.append(f)
        frame_files.sort()

    if not frame_files:
        print(f"No frames found in {frames_dir}")
        return False

    # Open all frames
    frames = []
    for frame_file in frame_files:
        frame_path = os.path.join(frames_dir, frame_file)
        frame = Image.open(frame_path)
        frames.append(frame)

    # Calculate dimensions
    width = frames[0].width
    height = frames[0].height
    total_width = width * len(frames)

    # Create new image
    sprite_sheet = Image.new("RGBA", (total_width, height))

    # Paste each frame
    for i, frame in enumerate(frames):
        sprite_sheet.paste(frame, (i * width, 0))

    # Save
    sprite_sheet.save(output_path)
    print(f"[OK] Sprite sheet created: {output_path}")
    print(f"  Size: {sprite_sheet.size} ({len(frames)} frames × {width}×{height})")
    return True


def verify_png(filepath):
    """Verify file is a valid PNG"""
    try:
        with open(filepath, "rb") as f:
            header = f.read(8)
            if header == b"\x89PNG\r\n\x1a\n":
                print(f"[OK] PNG valid: {filepath}")
                return True
            else:
                print(f"[FAIL] Invalid PNG: {filepath}")
                return False
    except Exception as e:
        print(f"[ERROR] Error verifying PNG: {e}")
        return False
    except Exception as e:
        print(f"✗ Error verifying PNG: {e}")
        return False


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print(
            "Usage: python combine_frames.py <frames_dir> <output_path> [frame_count]"
        )
        sys.exit(1)

    frames_dir = sys.argv[1]
    output_path = sys.argv[2]
    frame_count = int(sys.argv[3]) if len(sys.argv) > 3 else 0

    if combine_frames_to_sprite_sheet(frames_dir, output_path, frame_count):
        verify_png(output_path)
