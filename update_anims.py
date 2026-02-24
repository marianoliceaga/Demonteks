import os
import re
import hashlib

def process_character(char_name, tscn_path, anim_root):
    if not os.path.exists(tscn_path):
        print(f"File not found: {tscn_path}")
        return
        
    print(f"Processing {char_name} in {tscn_path}")
    prefix_map = {
        "breathing-idle": "idle",
        "taking-punch": "hurt",
        "falling-back-death": "death",
        "walk": "walk",
        "walking-8-frames": "walk",
        "run": "run",
        "running-6-frames": "run",
        "cross-punch": "attack",
        "fireball": "fireball",
        "jump": "jump",
        "jumping-1": "jump"
    }
    
    speed_map = {
        "idle": 8.0,
        "walk": 10.0,
        "run": 12.0,
        "attack": 10.0,
        "hurt": 10.0,
        "death": 8.0,
        "fireball": 10.0,
        "jump": 10.0
    }
    
    loop_map = {
        "idle": True,
        "walk": True,
        "run": True,
        "attack": False,
        "hurt": False,
        "death": False,
        "fireball": False,
        "jump": False
    }

    frames = []
    anim_dirs = os.listdir(anim_root)
    for adir in anim_dirs:
        adir_path = os.path.join(anim_root, adir)
        if not os.path.isdir(adir_path): continue
        prefix = prefix_map.get(adir, adir)
        
        for ddir in os.listdir(adir_path):
            ddir_path = os.path.join(adir_path, ddir)
            if not os.path.isdir(ddir_path): continue
            
            for f in sorted(os.listdir(ddir_path)):
                if f.endswith('.png'):
                    rel_path = f"res://Assets/characters/{char_name}/animations/{adir}/{ddir}/{f}"
                    frames.append((prefix, ddir, rel_path))
                    
    animations = {}
    for prefix, direction, rel_path in frames:
        anim_name = f"{prefix}_{direction}"
        if anim_name not in animations:
            animations[anim_name] = []
        animations[anim_name].append(rel_path)
    
    def add_alias(base, fallback_dirs=['east', 'south', 'north', 'west']):
        for d in fallback_dirs:
             src = f"{base}_{d}"
             if src in animations:
                 animations[base] = list(animations[src])
                 return
                 
    add_alias('idle')
    add_alias('walk')
    add_alias('run')
    if 'run' not in animations and 'walk' in animations:
        animations['run'] = list(animations['walk'])
    add_alias('attack')
    add_alias('hurt')
    add_alias('death')
    
    # Generate all directional fallbacks if missing (e.g. if run_east is missing, try walk_east)
    directions = ['east', 'south', 'north', 'west']
    if 'run' in animations and 'walk' in animations:
        for d in directions:
            run_d = f"run_{d}"
            walk_d = f"walk_{d}"
            if run_d not in animations and walk_d in animations:
                animations[run_d] = list(animations[walk_d])
                
    # Read original
    with open(tscn_path, 'r', encoding='utf-8') as f:
        content = f.read()
        
    # Remove Texture2D ext_resources
    content = re.sub(r'\[ext_resource type="Texture2D" [^\]]+\]\n?', '', content)
    
    def get_id(path):
        return "tex_" + hashlib.md5(path.encode()).hexdigest()[:8]
        
    ext_resources_str = ""
    all_paths = set()
    for paths in animations.values():
        for p in paths:
            all_paths.add(p)
            
    for p in sorted(all_paths):
        ext_resources_str += f'[ext_resource type="Texture2D" path="{p}" id="{get_id(p)}"]\n'
        
    insert_pos = 0
    match = list(re.finditer(r'\[ext_resource .*\]', content))
    if match:
        insert_pos = match[-1].end() + 1
        
    content = content[:insert_pos] + ext_resources_str + content[insert_pos:]
    
    sprite_frames_str = '[sub_resource type="SpriteFrames" id="SpriteFrames_enemy"]\nanimations = ['
    anim_blocks = []
    
    for anim_name, paths in sorted(animations.items()):
        base_name = anim_name.split('_')[0]
        speed = speed_map.get(base_name, 10.0)
        loop = str(loop_map.get(base_name, False)).lower()
        
        frames_str = '"frames": ['
        frame_blocks = []
        for p in paths:
            frame_blocks.append('{\n"duration": 1.0,\n"texture": ExtResource("' + get_id(p) + '")\n}')
        frames_str += ', '.join(frame_blocks) + ']'
        
        block = '{\n' + frames_str + ',\n"loop": ' + loop + ',\n"name": &"' + anim_name + '",\n"speed": ' + str(speed) + '\n}'
        anim_blocks.append(block)
        
    sprite_frames_str += ', '.join(anim_blocks) + ']\n'
    
    start_match = re.search(r'\[sub_resource type="SpriteFrames" id="[^"]*"\]', content)
        
    if start_match:
        start_idx = start_match.start()
        end_match = re.search(r'\n\[(?:sub_resource|node) ', content[start_match.end():])
        if end_match:
            end_idx = start_match.end() + end_match.start()
            content = content[:start_idx] + sprite_frames_str + content[end_idx:]
        else:
            content = content[:start_idx] + sprite_frames_str
            
    with open(tscn_path, 'w', encoding='utf-8') as f:
        f.write(content)

process_character("goliath", "d:/Demonteks/Scenes/Characters/Goliath.tscn", "d:/Demonteks/assets/characters/goliath/animations")
process_character("mutant_demontek", "d:/Demonteks/Scenes/Characters/Mutant.tscn", "d:/Demonteks/assets/characters/mutant_demontek/animations")
process_character("king_demontek", "d:/Demonteks/Scenes/Characters/King.tscn", "d:/Demonteks/assets/characters/king_demontek/animations")
print("Done")
