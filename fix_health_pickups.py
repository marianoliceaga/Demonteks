import re

file_path = "d:/Demonteks/Scenes/Levels/Level1_Jungle.tscn"
with open(file_path, "r") as f:
    content = f.read()

# 1. Add ext_resource 15_healthpickup
if "15_healthpickup" not in content:
    content = content.replace(
        '[ext_resource type="Script" uid="uid://cd6wpk82k038j" path="res://Scripts/Environment/AmbientSoundController.cs" id="ambient_audio_script"]',
        '[ext_resource type="PackedScene" path="res://Scenes/Items/HealthPickup.tscn" id="15_healthpickup"]\n[ext_resource type="Script" uid="uid://cd6wpk82k038j" path="res://Scripts/Environment/AmbientSoundController.cs" id="ambient_audio_script"]'
    )

# 2. Remove sub_resources
content = re.sub(
    r'\[sub_resource type="CircleShape2D" id="CircleShape2D_health1"\]\nradius = 12\.0\n\n',
    '',
    content
)
content = re.sub(
    r'\[sub_resource type="CircleShape2D" id="CircleShape2D_health2"\]\nradius = 12\.0\n\n',
    '',
    content
)

# 3. Replace HealthPickup1 and its children
pickup1_pattern = r'\[node name="HealthPickup1" type="Area2D".*?(?=\[node name="HealthPickup2"|$)'
pickup1_replacement = """[node name="HealthPickup1" parent="Pickups" unique_id=1020187895 instance=ExtResource("15_healthpickup")]
position = Vector2(2000, 410)

"""
content = re.sub(pickup1_pattern, pickup1_replacement, content, flags=re.DOTALL)

# 4. Replace HealthPickup2 and its children
pickup2_pattern = r'\[node name="HealthPickup2" type="Area2D".*?(?=\[node name="Door"|$)'
pickup2_replacement = """[node name="HealthPickup2" parent="Pickups" unique_id=1648355685 instance=ExtResource("15_healthpickup")]
position = Vector2(2700, 410)

"""
content = re.sub(pickup2_pattern, pickup2_replacement, content, flags=re.DOTALL)


with open(file_path, "w") as f:
    f.write(content)

print("Replaced raw HealthPickups with instances.")
