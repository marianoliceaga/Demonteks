from PIL import Image
import os
import glob


def combine_frames_horizontal(input_dir, output_path, padding=2):
    """
    Combine PNG frames from input_dir into a horizontal sprite sheet
    """
    frames = sorted(glob.glob(os.path.join(input_dir, "*.png")))

    if not frames:
        print(f"No frames found in {input_dir}")
        return False

    # Load all frames
    images = [Image.open(f) for f in frames]

    # Get dimensions
    width, height = images[0].size
    total_width = len(images) * width + (len(images) - 1) * padding

    # Create new sprite sheet
    sprite_sheet = Image.new("RGBA", (total_width, height), (0, 0, 0, 0))

    # Paste each frame
    x_offset = 0
    for img in images:
        sprite_sheet.paste(img, (x_offset, 0))
        x_offset += width + padding

    # Save
    sprite_sheet.save(output_path, "PNG")
    print(f"Created sprite sheet: {output_path}")
    print(f"  Frames: {len(images)}, Size: {total_width}x{height}")
    return True


if __name__ == "__main__":
    import sys

    if len(sys.argv) < 3:
        print(
            "Usage: python combine_anim_frames.py <input_dir> <output_path> [padding]"
        )
        sys.exit(1)

    input_dir = sys.argv[1]
    output_path = sys.argv[2]
    padding = int(sys.argv[3]) if len(sys.argv) > 3 else 2

    combine_frames_horizontal(input_dir, output_path, padding)
