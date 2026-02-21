import sys
import os

def generate_scene():
    with open('tilemap_data.txt', 'r') as f:
        tilemap_data = f.read().strip()

    scene_content = f"""[gd_scene load_steps=22 format=3]

[ext_resource type="PackedScene" uid="uid://x5pm7vvasqky" path="res://Scenes/Characters/Player.tscn" id="1_player"]
[ext_resource type="PackedScene" uid="uid://c6s75q5i6n123" path="res://Scenes/Characters/Guard.tscn" id="2_guard"]
[ext_resource type="PackedScene" uid="uid://dmdsgwqrbtwfg" path="res://Scenes/Prefabs/Checkpoint.tscn" id="3_checkpoint"]
[ext_resource type="TileSet" path="res://Assets/tilesets/dungeon_tileset_16x16.tres" id="4_tileset"]
[ext_resource type="Texture2D" path="res://Assets/backgrounds/dungeon/sky_1920.png" id="5_bg_sky"]
[ext_resource type="Texture2D" path="res://Assets/backgrounds/dungeon/mid_3840.png" id="6_bg_mid"]
[ext_resource type="Texture2D" path="res://Assets/backgrounds/dungeon/foreground_5760.png" id="7_bg_fg"]
[ext_resource type="PackedScene" uid="uid://hud_scene_001" path="res://Scenes/UI/HUD.tscn" id="8_hud"]
[ext_resource type="PackedScene" uid="uid://dttb8i783ht3m" path="res://Scenes/UI/PauseMenu.tscn" id="9_pause"]
[ext_resource type="Script" path="res://Scripts/Utils/LevelEndTrigger.cs" id="10_levelend"]
[ext_resource type="Script" path="res://Scripts/Utils/CameraFollow.cs" id="11_camera"]
[ext_resource type="Script" path="res://Scripts/Utils/KillZone.cs" id="12_killzone"]
[ext_resource type="PackedScene" uid="uid://cy84w8nx" path="res://Scenes/Prefabs/StoneButton.tscn" id="13_button"]
[ext_resource type="PackedScene" uid="uid://b8h344xweqqj" path="res://Scenes/Prefabs/WeaponPickup.tscn" id="14_weapon"]
[ext_resource type="Texture2D" path="res://Assets/tilesets/dungeon_tileset_16x16.png" id="15_dungeon_tex"]

[sub_resource type="RectangleShape2D" id="RectangleShape2D_door"]
size = Vector2(32, 48)

[sub_resource type="RectangleShape2D" id="RectangleShape2D_levelend"]
size = Vector2(16, 96)

[sub_resource type="RectangleShape2D" id="RectangleShape2D_killzone"]
size = Vector2(4000, 32)

[node name="Level2_Dungeon" type="Node2D"]

[node name="ParallaxBackground" type="ParallaxBackground" parent="."]

[node name="SkyLayer" type="ParallaxLayer" parent="ParallaxBackground"]
motion_scale = Vector2(0.1, 0.1)
motion_mirroring = Vector2(1920, 0)

[node name="Sprite2D" type="Sprite2D" parent="ParallaxBackground/SkyLayer"]
texture = ExtResource("5_bg_sky")
centered = false

[node name="MidLayer" type="ParallaxLayer" parent="ParallaxBackground"]
motion_scale = Vector2(0.4, 0.3)
motion_mirroring = Vector2(3840, 0)

[node name="Sprite2D" type="Sprite2D" parent="ParallaxBackground/MidLayer"]
texture = ExtResource("6_bg_mid")
centered = false

[node name="ForegroundLayer" type="ParallaxLayer" parent="ParallaxBackground"]
motion_scale = Vector2(0.7, 0.5)
motion_mirroring = Vector2(5760, 0)

[node name="Sprite2D" type="Sprite2D" parent="ParallaxBackground/ForegroundLayer"]
texture = ExtResource("7_bg_fg")
centered = false

[node name="TileMap" type="TileMap" parent="."]
tile_set = ExtResource("4_tileset")
format = 2
layer_0/tile_data = PackedInt32Array({tilemap_data})

[node name="Player" parent="." instance=ExtResource("1_player")]
position = Vector2(64, 240)

[node name="Checkpoints" type="Node2D" parent="."]

[node name="Checkpoint1" parent="Checkpoints" instance=ExtResource("3_checkpoint")]
position = Vector2(64, 240)

[node name="Checkpoint2_" parent="Checkpoints" instance=ExtResource("3_checkpoint")]
position = Vector2(1500, 240)

[node name="Enemies" type="Node2D" parent="."]

# Cell guards
[node name="Guard1" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(400, 240)

[node name="Guard2" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(500, 240)

# Tunnel guards
[node name="Guard3" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(800, 240)

[node name="Guard4" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(1200, 240)

[node name="Guard5" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(1800, 240)

[node name="Guard6" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(2100, 240)

# Ruins guards
[node name="Guard7" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(2600, 272)

[node name="Guard8" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(3000, 272)

[node name="Guard9" parent="Enemies" instance=ExtResource("2_guard")]
position = Vector2(3500, 272)

[node name="Interactables" type="Node2D" parent="."]

[node name="StoneButton" parent="Interactables" instance=ExtResource("13_button")]
position = Vector2(3100, 280)
TargetDoorPath = NodePath("../HiddenChamberDoor")

[node name="HiddenChamberDoor" type="StaticBody2D" parent="Interactables"]
position = Vector2(3216, 264)
collision_layer = 4
collision_mask = 0

[node name="CollisionShape2D" type="CollisionShape2D" parent="Interactables/HiddenChamberDoor"]
shape = SubResource("RectangleShape2D_door")

[node name="Sprite2D" type="Sprite2D" parent="Interactables/HiddenChamberDoor"]
texture = ExtResource("15_dungeon_tex")
region_enabled = true
region_rect = Rect2(0, 0, 32, 48)

[node name="Pickups" type="Node2D" parent="."]

[node name="WeaponPickupMachete" parent="Pickups" instance=ExtResource("14_weapon")]
position = Vector2(3350, 280)
WeaponType = "Machete"

[node name="WeaponPickupSword" parent="Pickups" instance=ExtResource("14_weapon")]
position = Vector2(3450, 280)
WeaponType = "Sword"

[node name="LevelEnd" type="Area2D" parent="."]
position = Vector2(3900, 272)
collision_layer = 0
collision_mask = 1
script = ExtResource("10_levelend")

[node name="CollisionShape2D" type="CollisionShape2D" parent="LevelEnd"]
shape = SubResource("RectangleShape2D_levelend")

[node name="Marker" type="ColorRect" parent="LevelEnd"]
offset_left = -8.0
offset_top = -48.0
offset_right = 8.0
offset_bottom = 48.0
color = Color(1, 0.843137, 0, 0.6)

[node name="HUD" parent="." instance=ExtResource("8_hud")]

[node name="PauseMenu" parent="." instance=ExtResource("9_pause")]

[node name="Camera" type="Camera2D" parent="."]
script = ExtResource("11_camera")
LimitRightValue = 4000
LimitTopValue = -600

[node name="KillZone" type="Area2D" parent="."]
position = Vector2(2000, 600)
collision_layer = 0
collision_mask = 1
script = ExtResource("12_killzone")

[node name="CollisionShape2D" type="CollisionShape2D" parent="KillZone"]
shape = SubResource("RectangleShape2D_killzone")
"""
    with open('Scenes/Levels/Level2_Dungeon.tscn', 'w', encoding='utf-8') as f:
        f.write(scene_content)
    print("Level2_Dungeon.tscn generated successfully!")

if __name__ == "__main__":
    generate_scene()
