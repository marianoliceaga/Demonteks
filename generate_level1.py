import sys
import os

def generate_scene():
    # 1. Generate TileMap Data
    # Godot 4 TileMap data format:
    # layer_0/tile_data = PackedInt32Array(...)
    # Format per tile: [ (x << 16) | (y & 0xFFFF), source_id, (atlas_x << 16) | (atlas_y & 0xFFFF) ]
    # Tile size 16x16.
    
    tiles = set()
    def rect(x_start, x_end, y_start, y_end):
        for x in range(x_start, x_end + 1):
            for y in range(y_start, y_end + 1):
                tiles.add((x, y))
                
    # --- LEVEL LAYOUT ---
    # Total width: 3200px -> 200 tiles. Safe bottom Y for safety floors is Row 37 (600px).
    # Camera bounds are y=-600 to y=600.
    
    # 1. Start Platform (ground level left side)
    # 400px Y is Row 25. Large trunk base.
    rect(0, 10, 25, 30)
    
    # 2. Ascending branch path (Y varies from 25 to 10. X from 12 to 180)
    # Small/long platforms
    rect(14, 21, 23, 23) # long
    rect(24, 25, 20, 20) # short
    rect(29, 36, 18, 18) # long
    rect(40, 42, 19, 19) # short
    rect(45, 50, 15, 15) # medium
    rect(54, 56, 17, 17) # medium
    
    # Climb up tall tree trunk at X=65
    rect(63, 67, 10, 37) # Tall trunk from row 10 to bottom
    rect(60, 62, 22, 22) # step
    rect(68, 70, 18, 18) # step right
    rect(60, 62, 14, 14) # step left
    rect(68, 72, 10, 10) # step right top
    
    # Continue path
    rect(76, 82, 12, 12)
    rect(86, 92, 14, 14)
    rect(95, 97, 16, 16)
    rect(102, 110, 14, 14)
    rect(115, 117, 12, 12)
    rect(122, 130, 15, 15)
    rect(134, 142, 13, 13)
    rect(146, 148, 11, 11)
    rect(152, 158, 14, 14)
    rect(162, 168, 12, 12)
    rect(172, 180, 10, 10)

    # 3. Safe endpoint platform
    rect(185, 198, 8, 30) # large endpoint base from Row 8 down
    
    # 4. Safety floors along bottom (Row 36-37)
    rect(12, 60, 36, 37)
    rect(70, 180, 36, 37)

    array = []
    for (x, y) in sorted(tiles):
        coord = (x << 16) | (y & 0xFFFF)
        source_id = 0
        atlas_coord = 0 # (0,0) in atlas. Real atlas coordinates would depend on the tilset.
        # Assuming (0,0) is a valid block in jungle_tileset_16x16.tres
        array.extend([coord, source_id, atlas_coord])
        
    tilemap_data = ", ".join(map(str, array))

    scene_content = f"""[gd_scene load_steps=18 format=3 uid="uid://b73do0w2oan5i"]

[ext_resource type="PackedScene" uid="uid://x5pm7vvasqky" path="res://Scenes/Characters/Player.tscn" id="1_player"]
[ext_resource type="PackedScene" path="res://Scenes/Characters/Demontek.tscn" id="2_demontek"]
[ext_resource type="TileSet" uid="uid://b76hyvacdj2yg" path="res://Assets/tilesets/jungle_tileset_16x16.tres" id="3_tileset"]
[ext_resource type="Texture2D" uid="uid://bg_layer_1" path="res://Assets/backgrounds/fgtiles-alpha.png" id="4_bg1"]
[ext_resource type="Texture2D" uid="uid://bg_layer_2" path="res://Assets/backgrounds/bg-layer-2-full-alpha.png" id="5_bg2"]
[ext_resource type="Texture2D" uid="uid://bg_layer_3" path="res://Assets/backgrounds/bg-layer-1-full-alpha.png" id="6_bg3"]
[ext_resource type="PackedScene" uid="uid://hud_scene" path="res://Scenes/UI/HUD.tscn" id="7_hud"]
[ext_resource type="PackedScene" uid="uid://pause_scene" path="res://Scenes/UI/PauseMenu.tscn" id="8_pause"]
[ext_resource type="Script" uid="uid://7twiyd1bo8c6" path="res://Scripts/Utils/LevelEndTrigger.cs" id="10_levelend"]
[ext_resource type="Script" uid="uid://ctoegptmn0ent" path="res://Scripts/Utils/CameraFollow.cs" id="11_camera"]
[ext_resource type="Script" uid="uid://bf8h6jfb721ho" path="res://Scripts/Utils/KillZone.cs" id="12_killzone"]
[ext_resource type="Script" path="res://Scripts/Environment/FloorTrap.cs" id="13_floortrap"]
[ext_resource type="PackedScene" path="res://Scenes/Items/HealthPickup.tscn" id="14_health"]
[ext_resource type="AudioStream" path="res://Assets/audio/music/Level1 (The Jungle).mp3" id="15_bgm"]
[ext_resource type="Script" path="res://Scripts/Environment/AmbientSoundController.cs" id="16_ambient_ctrl"]
[ext_resource type="AudioStream" path="res://Assets/audio/sfx/Level1_Jungle/Birds_Chirping.mp3" id="17_ambient_1"]
[ext_resource type="AudioStream" path="res://Assets/audio/sfx/Level1_Jungle/Bush_Rustle.mp3" id="18_ambient_2"]

[sub_resource type="RectangleShape2D" id="RectangleShape2D_levelend"]
size = Vector2(32, 64)

[sub_resource type="RectangleShape2D" id="RectangleShape2D_killzone"]
size = Vector2(8000, 64)

[sub_resource type="RectangleShape2D" id="RectangleShape2D_trap"]
size = Vector2(32, 16)

[node name="Level1_Jungley" type="Node2D"]

[node name="ParallaxBackground" type="ParallaxBackground" parent="."]

[node name="Layer1" type="ParallaxLayer" parent="ParallaxBackground"]
motion_scale = Vector2(0.05, 0.05)
motion_mirroring = Vector2(1024, 0)

[node name="Sprite2D" type="Sprite2D" parent="ParallaxBackground/Layer1"]
texture = ExtResource("6_bg3")
centered = false

[node name="Layer2" type="ParallaxLayer" parent="ParallaxBackground"]
motion_scale = Vector2(0.35, 0.35)
motion_mirroring = Vector2(1024, 0)

[node name="Sprite2D" type="Sprite2D" parent="ParallaxBackground/Layer2"]
texture = ExtResource("5_bg2")
centered = false

[node name="Layer3" type="ParallaxLayer" parent="ParallaxBackground"]
motion_scale = Vector2(0.9, 0.9)
motion_mirroring = Vector2(1024, 0)

[node name="Sprite2D" type="Sprite2D" parent="ParallaxBackground/Layer3"]
texture = ExtResource("4_bg1")
centered = false

[node name="TileMap" type="TileMap" parent="."]
tile_set = ExtResource("3_tileset")
format = 2
layer_0/tile_data = PackedInt32Array({tilemap_data})

[node name="Player" parent="." instance=ExtResource("1_player")]
position = Vector2(96, 380)

[node name="Enemies" type="Node2D" parent="."]

[node name="Demontek1" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(300, 340)

[node name="Demontek2" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(500, 260)

[node name="Demontek3" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(800, 240)

[node name="Demontek4" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(1300, 160)

[node name="Demontek5" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(1450, 200)

[node name="Demontek6" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(1600, 200)

[node name="Demontek7" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(1900, 170)

[node name="Demontek8" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(2100, 180)

[node name="Demontek9" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(2500, 200)

[node name="Demontek10" parent="Enemies" instance=ExtResource("2_demontek")]
position = Vector2(2700, 170)

[node name="Traps" type="Node2D" parent="."]

[node name="Trap1" type="Area2D" parent="Traps"]
position = Vector2(700, 280)
collision_layer = 0
collision_mask = 1
script = ExtResource("13_floortrap")
EnemySpawnCount = 2
EnemyPrefabPath = "res://Scenes/Characters/Demontek.tscn"

[node name="CollisionShape2D" type="CollisionShape2D" parent="Traps/Trap1"]
shape = SubResource("RectangleShape2D_trap")

[node name="Trap2" type="Area2D" parent="Traps"]
position = Vector2(1700, 200)
collision_layer = 0
collision_mask = 1
script = ExtResource("13_floortrap")
EnemySpawnCount = 2
EnemyPrefabPath = "res://Scenes/Characters/Demontek.tscn"

[node name="CollisionShape2D" type="CollisionShape2D" parent="Traps/Trap2"]
shape = SubResource("RectangleShape2D_trap")

[node name="Trap3" type="Area2D" parent="Traps"]
position = Vector2(2300, 230)
collision_layer = 0
collision_mask = 1
script = ExtResource("13_floortrap")
EnemySpawnCount = 2
EnemyPrefabPath = "res://Scenes/Characters/Demontek.tscn"

[node name="CollisionShape2D" type="CollisionShape2D" parent="Traps/Trap3"]
shape = SubResource("RectangleShape2D_trap")

[node name="Pickups" type="Node2D" parent="."]

[node name="Health1" parent="Pickups" instance=ExtResource("14_health")]
position = Vector2(1000, 130)

[node name="Health2" parent="Pickups" instance=ExtResource("14_health")]
position = Vector2(2000, 150)

[node name="Health3" parent="Pickups" instance=ExtResource("14_health")]
position = Vector2(2600, 150)

[node name="LevelEnd" type="Area2D" parent="."]
position = Vector2(3050, 96)
collision_layer = 0
collision_mask = 1
script = ExtResource("10_levelend")

[node name="CollisionShape2D" type="CollisionShape2D" parent="LevelEnd"]
shape = SubResource("RectangleShape2D_levelend")

[node name="Marker" type="ColorRect" parent="LevelEnd"]
offset_left = -16.0
offset_top = -32.0
offset_right = 16.0
offset_bottom = 32.0
color = Color(1, 0.843137, 0, 0.6)

[node name="HUD" parent="." instance=ExtResource("7_hud")]

[node name="PauseMenu" parent="." instance=ExtResource("8_pause")]

[node name="Camera" type="Camera2D" parent="."]
script = ExtResource("11_camera")
TargetPath = NodePath("../Player")
SmoothSpeed = 5.0
LimitLeftValue = 0
LimitRightValue = 3200
LimitTopValue = -600
LimitBottomValue = 600

[node name="KillZone" type="Area2D" parent="."]
position = Vector2(1600, 800)
collision_layer = 0
collision_mask = 1
script = ExtResource("12_killzone")

[node name="CollisionShape2D" type="CollisionShape2D" parent="KillZone"]
shape = SubResource("RectangleShape2D_killzone")

[node name="AmbientSoundController" type="Node2D" parent="."]
script = ExtResource("16_ambient_ctrl")
SoundClips = Array[AudioStream]([ExtResource("17_ambient_1"), ExtResource("18_ambient_2")])
MinWaitTime = 4.0
MaxWaitTime = 12.0

[node name="BackgroundMusic" type="AudioStreamPlayer" parent="."]
stream = ExtResource("15_bgm")
autoplay = true
"""

    with open('Scenes/Levels/Level1_Jungley.tscn', 'w', encoding='utf-8') as f:
        f.write(scene_content)
    print("Level1_Jungley.tscn generated successfully!")

if __name__ == "__main__":
    generate_scene()
