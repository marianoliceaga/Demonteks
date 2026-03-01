import glob
import os

for file in glob.glob('d:/Demonteks/Scenes/Characters/*.tscn'):
    if "Player.tscn" in file:
        continue

    with open(file, 'r') as f:
        content = f.read()

    # Remove the FloorRayPath assignment
    lines = content.split('\n')
    new_lines = []
    for line in lines:
        if 'FloorRayPath = NodePath("FloorRay")' not in line:
            new_lines.append(line)
    
    content = '\n'.join(new_lines)

    # Remove the node block
    block_to_remove = """

[node name="FloorRay" type="RayCast2D" parent="."]
position = Vector2(25, 0)
target_position = Vector2(0, 50)
collision_mask = 1
"""
    content = content.replace(block_to_remove, '')

    with open(file, 'w') as f:
        f.write(content)
    print(f"Reverted {file}")
