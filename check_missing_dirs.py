import os
import glob

def check_missing_directions(character_dir):
    animations = {
        'breathing-idle': 4,
        'walking-8-frames': 4,
        'running-6-frames': 4,
        'jumping-1': 4,
        'cross-punch': 4,
        'taking-punch': 4,
        'falling-back-death': 4
    }
    
    all_directions = ['south', 'east', 'north', 'west']
    
    for anim_name, expected_dirs in animations.items():
        anim_dir = os.path.join(character_dir, 'animations', anim_name)
        if os.path.exists(anim_dir):
            existing_dirs = os.listdir(anim_dir)
            missing = [d for d in all_directions if d not in existing_dirs]
            if missing:
                print(f"{anim_name:20} missing: {', '.join(missing)}")

for char in ['explorer', 'demontek', 'goliath', 'king_demontek']:
    char_dir = f'Assets/characters/{char}'
    print(f"\n=== {char.upper()} ===")
    if os.path.exists(os.path.join(char_dir, 'animations')):
        check_missing_directions(char_dir)
