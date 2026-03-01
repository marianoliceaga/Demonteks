import glob, re

for file in glob.glob('d:/Demonteks/Scenes/Characters/*.tscn'):
    if "Player.tscn" in file:
        continue

    with open(file, 'r') as f:
        content = f.read()

    modified = False

    if '[node name="FloorRay"' not in content:
        content += '\n\n[node name="FloorRay" type="RayCast2D" parent="."]\nposition = Vector2(25, 0)\ntarget_position = Vector2(0, 50)\ncollision_mask = 1\n'
        modified = True

    if 'FloorRayPath = NodePath("FloorRay")' not in content:
        lines = content.split('\n')
        out_lines = []
        for line in lines:
            out_lines.append(line)
            if line.startswith('script = ExtResource') and ('1_ai' in line or '1_scr' in line or 'DemontekAI.cs' in content):
                # Add the property right after the script is attached
                out_lines.append('FloorRayPath = NodePath("FloorRay")')
        content = '\n'.join(out_lines)
        modified = True

    if modified:
        with open(file, 'w') as f:
            f.write(content)
        print(f"Updated {file}")
