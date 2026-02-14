import os

def check_all_characters():
    characters = ['explorer', 'demontek', 'guard_demontek', 'mutant_demontek', 'goliath', 'king_demontek']
    
    for char in characters:
        char_dir = f'Assets/characters/{char}'
        if not os.path.exists(os.path.join(char_dir, 'animations')):
            continue
            
        print(f"\n=== {char.upper()} ===")
        
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
            anim_dir = os.path.join(char_dir, 'animations', anim_name)
            if os.path.exists(anim_dir):
                existing_dirs = sorted(os.listdir(anim_dir))
                count = len(existing_dirs)
                if count < expected_dirs:
                    missing = [d for d in all_directions if d not in existing_dirs]
                    print(f"  {anim_name:20} {count}/4 - missing: {', '.join(missing)}")

check_all_characters()
