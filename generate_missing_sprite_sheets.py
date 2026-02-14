import os
import glob

# Define characters and their directories
characters = {
    'explorer': 'Assets/characters/explorer',
    'demontek': 'Assets/characters/demontek',
    'guard_demontek': 'Assets/characters/guard_demontek',
    'mutant_demontek': 'Assets/characters/mutant_demontek',
    'goliath': 'Assets/characters/goliath',
    'king_demontek': 'Assets/characters/king_demontek'
}

def generate_sprite_sheet(char_dir, char_name, anim_dir, output_name):
    """Generate a sprite sheet from animation frames"""
    anim_path = os.path.join(char_dir, 'animations', anim_dir)
    
    # Check if directory exists and has frames
    if not os.path.exists(anim_path):
        return False
    
    frames = sorted(glob.glob(os.path.join(anim_path, "*.png")))
    if not frames:
        return False
    
    # Generate sprite sheet using combine_anim_frames.py
    output_path = os.path.join(char_dir, output_name)
    cmd = f"cd {char_dir} && python ../../../combine_anim_frames.py animations/{anim_dir} {output_name}"
    
    result = os.system(cmd)
    return result == 0

# Generate sprite sheets for existing animation directories
for char_name, char_dir in characters.items():
    if not os.path.exists(char_dir):
        continue
    
    animations_path = os.path.join(char_dir, 'animations')
    if not os.path.exists(animations_path):
        continue
    
    # List all animation directories
    for anim_name in os.listdir(animations_path):
        anim_dir = os.path.join(animations_path, anim_name)
        if os.path.isdir(anim_dir):
            # Check for each direction
            for direction in os.listdir(anim_dir):
                direction_dir = os.path.join(anim_dir, direction)
                if os.path.isdir(direction_dir):
                    # Create sprite sheet for this direction
                    output_name = f"{char_name}_{anim_name}_{direction}_sheet.png"
                    
                    # Check if sprite sheet already exists
                    if os.path.exists(os.path.join(char_dir, output_name)):
                        continue
                    
                    print(f"Generating: {char_name}/{output_name}")
                    generate_sprite_sheet(char_dir, char_name, f"{anim_name}/{direction}", output_name)
