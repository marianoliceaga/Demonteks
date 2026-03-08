import re

file_path = "d:/Demonteks/Scenes/Levels/Level1_Jungle.tscn"
with open(file_path, "r") as f:
    lines = f.readlines()

new_lines = []
skip = False
for line in lines:
    if '[node name="HealthPickup1" type="Area2D"' in line:
        skip = True
        new_lines.append('[node name="HealthPickup1" parent="Pickups" unique_id=1020187895 instance=ExtResource("15_healthpickup")]\n')
        new_lines.append('position = Vector2(2000, 410)\n\n')
        continue
    elif '[node name="HealthPickup2" type="Area2D"' in line:
        skip = True
        new_lines.append('[node name="HealthPickup2" parent="Pickups" unique_id=1648355685 instance=ExtResource("15_healthpickup")]\n')
        new_lines.append('position = Vector2(2700, 410)\n\n')
        continue
    elif '[node name="LevelEnd" type="Area2D"' in line:
        skip = False

    if not skip:
        # Also remove the two sub_resources
        if '[sub_resource type="CircleShape2D" id="CircleShape2D_health' in line:
            # We skip this and the next two lines
            skip = "subresource"
            continue
        if skip == "subresource" and line.strip() == "":
            skip = False
            continue
        if skip == "subresource":
            continue

        new_lines.append(line)

content = "".join(new_lines)
if "15_healthpickup" not in content and 'id="ambient_audio_script"]' in content:
    content = content.replace(
        '[ext_resource type="Script" uid="uid://cd6wpk82k038j" path="res://Scripts/Environment/AmbientSoundController.cs" id="ambient_audio_script"]',
        '[ext_resource type="PackedScene" path="res://Scenes/Items/HealthPickup.tscn" id="15_healthpickup"]\n[ext_resource type="Script" uid="uid://cd6wpk82k038j" path="res://Scripts/Environment/AmbientSoundController.cs" id="ambient_audio_script"]'
    )

with open(file_path, "w") as f:
    f.write(content)
print("Safe replacement complete")
