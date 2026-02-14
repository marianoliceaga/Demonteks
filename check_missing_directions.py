import os
import glob

def check_character_animations(character_dir, character_name):
    """Check which directions exist for each animation type"""
    
    # List all sprite sheets
    sprite_sheets = glob.glob(os.path.join(character_dir, f"{character_name}_*.png"))
    sprite_sheets = [os.path.basename(f) for f in sprite_sheets]
    
    # Animation types we need
    animations = ['idle', 'walk', 'run', 'jump', 'attack', 'attack_melee', 'attack_magic', 'hurt', 'death']
    directions = ['south', 'east', 'north', 'west']
    
    print(f"\n=== {character_name.upper()} ===")
    print(f"Total sprite sheets: {len(sprite_sheets)}")
    
    for anim in animations:
        # Check which directions exist for this animation
        existing_dirs = []
        for direction in directions:
            # Check if sprite sheet exists for this animation and direction
            pattern = f"{character_name}_{anim}_{direction}_sheet.png"
            if pattern in sprite_sheets:
                existing_dirs.append(direction)
        
        if existing_dirs:
            print(f"{anim:15} {len(existing_dirs)}/4 directions: {', '.join(existing_dirs)}")
        else:
            print(f"{anim:15} 0/4 directions: MISSING")
    
    return sprite_sheets

# Check all characters
characters = {
    'explorer': 'Assets/characters/explorer',
    'demontek': 'Assets/characters/demontek',
    'guard_demontek': 'Assets/characters/guard_demontek',
    'mutant_demontek': 'Assets/characters/mutant_demontek',
    'goliath': 'Assets/characters/goliath',
    'king_demontek': 'Assets/characters/king_demontek'
}

for char_name, char_dir in characters.items():
    if os.path.exists(char_dir):
        check_character_animations(char_dir, char_name)
    else:
        print(f"\n=== {char_name.upper()} ===")
        print("Directory not found!")
