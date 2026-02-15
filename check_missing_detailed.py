import os

characters = {
    'demontek': 'f71e0f48-806d-4205-91b1-3aed25ee833a',
    'guard_demontek': 'ce43ef06-34a9-4da8-89a9-86eaf046bb29',
    'mutant_demontek': '778a4341-4581-499f-a1eb-79c1c0542480',
    'goliath': 'fc4331a5-5250-4791-9ffa-36ff3227f485',
    'king_demontek': 'fdde59ab-f799-4d60-9ae4-886672073500'
}

animations = {
    'breathing-idle': 4,
    'walking-8-frames': 4,
    'running-6-frames': 4,
    'jumping-1': 4,
    'cross-punch': 4,
    'fireball': 4,
    'taking-punch': 4,
    'falling-back-death': 4
}

all_directions = ['south', 'east', 'north', 'west']

for char_name, char_id in characters.items():
    char_dir = f'Assets/characters/{char_name}'
    if not os.path.exists(os.path.join(char_dir, 'animations')):
        continue
    
    print(f"\n=== {char_name.upper()} ===")
    
    for anim_name, expected_dirs in animations.items():
        anim_dir = os.path.join(char_dir, 'animations', anim_name)
        if not os.path.exists(anim_dir):
            continue
        
        existing_dirs = sorted(os.listdir(anim_dir)) if os.path.isdir(anim_dir) else []
        missing = [d for d in all_directions if d not in existing_dirs]
        
        if missing:
            print(f"  {anim_name:20} {len(existing_dirs)}/4 - missing: {', '.join(missing)}")
