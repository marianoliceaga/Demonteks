# 🎨 Demonteks - Asset Generation Guide

> Complete guide for generating all game assets using PixelLab MCP and proper sprite sheet workflows

## 📑 Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Asset Requirements](#asset-requirements)
- [Character Sprites](#character-sprites)
- [Background Layers](#background-layers)
- [Animation Specifications](#animation-specifications)
- [PixelLab Workflow](#pixellab-workflow)
- [Export Guidelines](#export-guidelines)
- [Integration Checklist](#integration-checklist)
- [Troubleshooting](#troubleshooting)

---

## 🎯 Overview

This guide covers the complete asset pipeline for Demonteks, from generation with PixelLab to integration in Godot. All assets use a **consistent pixel art style** at **128x128 tile size** for characters and **1920x1080** for backgrounds.

### Style Guidelines

- **Pixel Art Style**: Retro 16-bit aesthetic
- **Color Palette**: Rich, saturated colors
- **Demontek Theme**: Reddish aliens, demon-like features
- **Jungle Theme**: Purple sky, giant alien plants
- **Castle Theme**: Reddish bricks, hellish atmosphere

---

## 📋 Prerequisites

### Required Tools

1. **PixelLab MCP** (for asset generation)
   ```bash
   # Ensure PixelLab MCP is configured in your environment
   # See: https://pixellab-mcp.docs
   ```

2. **Aseprite** (recommended for sprite editing)
   - Download from: https://www.aseprite.org
   - Alternative: Piskel, GraphicsGale, or GIMP

3. **Godot 4.3+** (for integration testing)

### Color Palette

```
// Demonteks Palette
Primary Red:    #D32F2F (demons)
Dark Red:       #8B1A1A (shadows)
Accent Orange:  #FF6F00 (highlights)

// Jungle Palette
Violet Sky:     #6A1B9A (background)
Plant Green:    #2E7D32 (foliage)
Alien Purple:   #9C27B0 (flowers)

// Castle Palette
Brick Red:      #B71C1C (walls)
Dark Stone:     #424242 (floor)
Fire Orange:    #FF6F00 (torches)
```

---

## 🎭 Character Sprites

### Specifications

- **Tile Size**: 128x128 pixels
- **Format**: PNG with transparency
- **Frame Rate**: 10 FPS (100ms per frame)
- **Padding**: 2px between frames in sprite sheets

---

### 1. Explorer Character

**Theme**: Rambo/Indiana Jones inspired adventurer
**Size**: Normal (fills ~70% of 128x128 tile)

#### PixelLab Prompt

```
Generate pixel art character, 128x128 tile size:

Character: Rugged male explorer/adventurer
Style: 16-bit pixel art, detailed
Appearance:
- Muscular build, athletic
- Short dark hair, stubble
- Torn khaki pants
- Tank top or torn shirt
- Combat boots
- NO hat, NO whip
- Holds combat knife
- Determined expression

Colors:
- Skin: tan/brown tones
- Clothes: earth tones (khaki, brown, green)
- Hair: dark brown/black

Facing: Right side
Pose: Idle stance, knife at side
Background: Transparent
Pixel density: Detailed, not minimalist
Export: PNG, 128x128, clean edges
```

#### Animation Frames Needed

1. **idle.png** (4 frames, 100ms each)
   - Standing ready
   - Slight breathing motion
   - Knife hand at side

2. **walk.png** (8 frames, 80ms each)
   - Smooth walking cycle
   - Knife stays at side
   - Natural arm swing

3. **run.png** (6 frames, 60ms each)
   - Faster motion
   - Forward lean
   - Aggressive stride

4. **jump.png** (4 frames, static)
   - Frame 1: Crouch prepare
   - Frame 2: Push off
   - Frame 3: Peak height
   - Frame 4: Start descend

5. **attack_knife.png** (6 frames, 50ms each)
   - Wind up
   - Slash motion
   - Follow through
   - Recovery

6. **hurt.png** (3 frames, 100ms each)
   - Impact reaction
   - Stagger back
   - Recover

7. **death.png** (5 frames, 120ms each)
   - Initial hit
   - Fall to knees
   - Collapse forward
   - Static dead pose
   - Final frame holds

#### File Structure

```
assets/characters/explorer/
├── explorer_base.png          (single sprite, reference)
├── explorer_idle_sheet.png    (4 frames horizontal)
├── explorer_walk_sheet.png    (8 frames horizontal)
├── explorer_run_sheet.png     (6 frames horizontal)
├── explorer_jump_sheet.png    (4 frames horizontal)
├── explorer_attack_knife_sheet.png (6 frames)
├── explorer_hurt_sheet.png    (3 frames horizontal)
└── explorer_death_sheet.png   (5 frames horizontal)
```

---

### 2. Normal Demontek

**Theme**: Small demonic alien, reddish, says "tek"
**Size**: Normal (fills ~60% of tile, smaller than Explorer)

#### PixelLab Prompt

```
Generate pixel art enemy character, 128x128 tile size:

Character: Demontek (small demon alien)
Style: 16-bit pixel art, cute but menacing
Appearance:
- Small body (60% of tile height)
- Red/reddish skin color
- Small horns on head
- Big eyes (yellow/orange)
- Sharp teeth, small fangs
- Clawed hands and feet
- Short tail with arrow tip
- Slightly hunched posture

Colors:
- Skin: #D32F2F (primary red)
- Shadows: #8B1A1A (dark red)
- Eyes: #FFC107 (amber)
- Teeth: white
- Claws: dark grey

Facing: Right side
Pose: Idle stance, menacing
Background: Transparent
Style: Slightly cartoony but threatening
Export: PNG, 128x128
```

#### Animation Frames Needed

Same structure as Explorer:
- idle.png (4 frames)
- walk.png (8 frames)
- run.png (6 frames) - aggressive chase
- jump.png (4 frames)
- attack.png (6 frames) - claw swipe
- hurt.png (3 frames)
- death.png (5 frames)

#### File Structure

```
assets/characters/demontek/
├── demontek_base.png
├── demontek_idle_sheet.png
├── demontek_walk_sheet.png
├── demontek_run_sheet.png
├── demontek_jump_sheet.png
├── demontek_attack_sheet.png
├── demontek_hurt_sheet.png
└── demontek_death_sheet.png
```

---

### 3. Guard Demontek

**Theme**: Military-style Demontek with uniform
**Size**: Same as Normal Demontek

#### PixelLab Prompt

```
Generate pixel art enemy character, 128x128 tile size:

Character: Guard Demontek (militarized version)
Style: 16-bit pixel art
Appearance:
- Same base as normal Demontek
- Wearing guard uniform:
  - Military cap/helmet
  - Chest armor plate
  - Belt with pouches
  - Official badge/insignia
- Stands more upright
- Carries baton/club weapon

Colors:
- Skin: #D32F2F (red demon)
- Uniform: dark blue/grey
- Armor: metallic grey
- Badge: gold accent

Facing: Right side
Pose: Alert guard stance
Background: Transparent
Export: PNG, 128x128
```

#### Animation Frames

Same as Normal Demontek but with uniform visible in all frames.

---

### 4. Mutant Demontek

**Theme**: Muscular, crazed version with exaggerated features
**Size**: Normal (but appears bigger due to muscles)

#### PixelLab Prompt

```
Generate pixel art enemy character, 128x128 tile size:

Character: Mutant Demontek (pumped up variant)
Style: 16-bit pixel art, aggressive
Appearance:
- HUGE exaggerated muscles
- Bulging arms, legs, chest
- Veins visible on muscles
- MUCH larger eyes (crazed look)
- Wider mouth, more teeth
- Hunched aggressive stance
- Bigger claws
- Thicker tail
- Ripped, torn clothing scraps

Colors:
- Skin: #D32F2F (red) with darker muscle definition
- Muscles highlighted: #FF6F00 (orange)
- Eyes: glowing yellow (crazy look)
- Veins: darker red/purple

Facing: Right side
Pose: Aggressive, ready to charge
Background: Transparent
Style: More intimidating than normal Demontek
Export: PNG, 128x128
```

#### Animation Frames

Same structure but with more aggressive, powerful movements.

---

### 5. Goliath (Boss)

**Theme**: Demontek-Monster fusion experiment
**Size**: FILLS ENTIRE 128x128 TILE

#### PixelLab Prompt

```
Generate pixel art boss character, 128x128 tile size:

Character: Goliath - Demontek Monster Fusion
Style: 16-bit pixel art, imposing boss
Appearance:
- FILLS ENTIRE TILE (massive)
- Demontek head/features
- Monstrous body:
  - Multiple arms (4 total)
  - Thick legs
  - Armored plates on body
  - Spikes on shoulders/back
  - Torn flesh showing muscle
  - Stitches/scars (experiment marks)
- Hunched but still huge
- Glowing red eyes
- Sharp teeth, drooling

Colors:
- Skin: mix of red (#D32F2F) and grey (monster)
- Armor: dark metallic
- Eyes: glowing bright red
- Scars: darker red/black
- Stitches: black thread

Facing: Right side
Pose: Menacing idle
Background: Transparent
Detail: High detail, this is a boss!
Export: PNG, 128x128
```

#### Animation Frames

- idle.png (4 frames, heavy breathing)
- walk.png (8 frames, slow lumbering)
- attack1.png (8 frames, ground pound)
- attack2.png (8 frames, multi-arm swipe)
- hurt.png (3 frames)
- roar.png (6 frames, phase transition)
- death.png (10 frames, epic death)

---

### 6. King Demontek (Final Boss)

**Theme**: Giant regal Demontek with crown and beard
**Size**: FILLS ENTIRE 128x128 TILE

#### PixelLab Prompt

```
Generate pixel art final boss character, 128x128 tile size:

Character: King Demontek - Final Boss
Style: 16-bit pixel art, majestic and terrifying
Appearance:
- FILLS ENTIRE TILE (tallest character)
- Giant Demontek (2-3x normal size)
- Long flowing beard (red/grey)
- Large golden crown with gems
- Regal robes (torn, battle-worn)
- Ornate armor pieces
- Holds massive sword/scepter
- Cape flowing behind
- Larger horns
- Wise but fierce expression
- Battle scars

Colors:
- Skin: #D32F2F (deeper red)
- Beard: #8B1A1A with grey streaks
- Crown: gold (#FFD700) with red gems
- Robes: royal purple/red
- Armor: ornate gold/silver
- Sword: mystical blue glow

Facing: Right side
Pose: Regal standing, weapon ready
Background: Transparent
Detail: HIGHEST detail, this is THE boss!
Export: PNG, 128x128
```

#### Animation Frames

- idle.png (4 frames, breathing, cape flows)
- walk.png (8 frames, slow powerful stride)
- attack_melee.png (10 frames, massive sword swing)
- attack_magic.png (8 frames, fire projectiles)
- attack_combo.png (12 frames, 3-hit combo)
- hurt.png (4 frames)
- phase_transition.png (10 frames, power up)
- death.png (15 frames, dramatic death)

---

## 🌄 Background Layers

### Specifications

- **Resolution**: 1920x1080 (Full HD)
- **Format**: PNG
- **Parallax**: Multiple layers at different scroll speeds
- **Seamless**: Tile horizontally for infinite scrolling

---

### Level 1: Jungle Backgrounds

#### Layer 1: Sky (Farthest, slowest scroll)

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Alien jungle sky
Style: 16-bit pixel art background
Elements:
- Violet/purple gradient sky (#6A1B9A to #9C27B0)
- Multiple moons/planets visible
- Distant nebula clouds
- Alien constellations
- No sun, purple ambient light
- Smooth gradient top to bottom

Atmosphere: Otherworldly, alien
Tile: Seamless horizontal repeat
Detail: Low detail (background layer)
Export: PNG, 1920x1080
```

#### Layer 2: Distant Trees (Middle parallax)

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Distant giant alien trees
Style: 16-bit pixel art, silhouetted
Elements:
- Massive tree trunks in background
- Giant leaves (silhouettes)
- Purple/dark green (#2E7D32)
- Atmospheric perspective (faded)
- Various heights
- Organic alien shapes

Position: Middle distance
Tile: Seamless horizontal repeat
Detail: Medium detail
Transparency: Alpha channel for compositing
Export: PNG, 1920x1080
```

#### Layer 3: Foreground Foliage (Closest, fastest scroll)

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Giant jungle foliage foreground
Style: 16-bit pixel art, detailed
Elements:
- GIANT leaves (20x human size)
- Alien flowers (glowing purple)
- Thick vines hanging
- Strange fungi
- Bright greens (#2E7D32, #4CAF50)
- Purple accent flowers (#9C27B0)
- Some foreground blur

Position: Closest to camera
Tile: Seamless horizontal repeat
Detail: High detail, rich colors
Export: PNG, 1920x1080 with alpha
```

---

### Level 2: Dungeon Backgrounds

#### Layer 1: Back Wall

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Ancient dungeon back wall
Style: 16-bit pixel art, dark
Elements:
- Stone brick wall texture
- Old, weathered blocks
- Cracks and moss
- Torch sconces (unlit)
- Ancient carvings/symbols
- Dark grey/brown tones
- Water damage stains

Atmosphere: Ancient, abandoned
Tile: Seamless horizontal repeat
Detail: Medium detail
Export: PNG, 1920x1080
```

#### Layer 2: Foreground Pillars

**PixelLab Prompt:**

```
Generate pixel art foreground layer, 1920x1080:

Scene: Dungeon pillars and chains
Style: 16-bit pixel art
Elements:
- Stone pillars (partial view)
- Hanging chains
- Broken cage bars
- Flickering torch light
- Darker than background
- Frame edges of screen

Position: Foreground
Detail: High detail on pillars
Transparency: Alpha for compositing
Export: PNG, 1920x1080 with alpha
```

---

### Level 3: Castle Backgrounds

#### Layer 1: Distant Castle

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Red brick castle exterior/interior
Style: 16-bit pixel art
Elements:
- Red brick walls (#B71C1C)
- Gothic architecture
- Tall windows (hellish glow from within)
- Towers in distance
- Fire/lava glow
- Ominous atmosphere
- Dark stone accents

Atmosphere: Hellish, imposing
Tile: Seamless horizontal repeat
Detail: Medium detail
Export: PNG, 1920x1080
```

#### Layer 2: Castle Interior

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Castle interior hallway
Style: 16-bit pixel art, detailed
Elements:
- Red brick walls with torch brackets
- Suits of armor (background)
- Hanging banners (torn, Demontek symbols)
- Weapon racks
- Staircase elements
- Fire braziers
- Rich red/orange lighting

Position: Mid-ground
Detail: High detail
Tile: Seamless horizontal
Export: PNG, 1920x1080
```

#### Layer 3: Terrace (Final Boss Arena)

**PixelLab Prompt:**

```
Generate pixel art background layer, 1920x1080:

Scene: Castle terrace/throne room
Style: 16-bit pixel art, epic
Elements:
- Open air terrace
- Red/purple sky behind (alien world)
- Ornate throne in background
- Pillars flanking sides
- Portal gateway (inactive, stone)
- Fire braziers
- Royal tapestries
- Dramatic lighting

Atmosphere: Final boss arena, epic
Detail: Very high detail
Export: PNG, 1920x1080
Note: This is where King Demontek fight happens!
```

---

## 🎬 Animation Specifications

### Frame Timing Standards

| Animation | Frames | Duration (ms) | FPS | Loop |
|-----------|--------|---------------|-----|------|
| idle | 4 | 400 | 10 | Yes |
| walk | 8 | 640 | 12.5 | Yes |
| run | 6 | 360 | 16.7 | Yes |
| jump | 4 | Static | N/A | No |
| attack | 6 | 300 | 20 | No |
| hurt | 3 | 300 | 10 | No |
| death | 5-15 | Varies | 8.3 | No |

### Sprite Sheet Layout

```
Standard Horizontal Layout:
┌─────┬─────┬─────┬─────┬─────┬─────┐
│ F1  │ F2  │ F3  │ F4  │ F5  │ F6  │
│128px│128px│128px│128px│128px│128px│
└─────┴─────┴─────┴─────┴─────┴─────┘
Total width = frames × 128px
Height = 128px
Padding = 2px between frames
```

### Godot AnimatedSprite2D Configuration

```gdscript
# Example configuration in Godot
[node name="AnimatedSprite2D" type="AnimatedSprite2D"]

[animation name="idle"]
frames = 4
fps = 10
loop = true
texture = preload("res://assets/characters/explorer/explorer_idle_sheet.png")

[animation name="walk"]
frames = 8
fps = 12.5
loop = true
texture = preload("res://assets/characters/explorer/explorer_walk_sheet.png")
```

---

## 🔧 PixelLab Workflow

### Step-by-Step Asset Generation

#### 1. Generate Base Sprite

```bash
# Using PixelLab MCP
pixellab generate character \
  --prompt "[INSERT PROMPT FROM ABOVE]" \
  --size 128x128 \
  --style "pixel-art" \
  --output "assets/characters/[name]/[name]_base.png"
```

#### 2. Generate Animation Frames

```bash
# Generate animation sprite sheet
pixellab animate \
  --input "assets/characters/[name]/[name]_base.png" \
  --animations "idle:4,walk:8,run:6,jump:4,attack:6,hurt:3,death:5" \
  --output "assets/characters/[name]/" \
  --format "horizontal-sheet"
```

#### 3. Manual Refinement (Optional)

If needed, open in Aseprite for touch-ups:

1. Open sprite sheet in Aseprite
2. Edit individual frames
3. Add details, fix artifacts
4. Export back to PNG

#### 4. Generate Backgrounds

```bash
pixellab generate background \
  --prompt "[INSERT BACKGROUND PROMPT]" \
  --size 1920x1080 \
  --parallax-layer [1-3] \
  --seamless-horizontal true \
  --output "assets/backgrounds/[level]/[layer_name].png"
```

---

## 📦 Export Guidelines

### Character Exports

1. **Base Reference Sprite**
   - Single 128x128 PNG
   - Clean, centered
   - Transparent background
   - Use as reference

2. **Animation Sprite Sheets**
   - Horizontal layout
   - 128px per frame
   - 2px padding between frames
   - No extra border space
   - Transparent background

3. **Naming Convention**
   ```
   [character]_[animation]_sheet.png
   
   Examples:
   explorer_idle_sheet.png
   demontek_attack_sheet.png
   king_death_sheet.png
   ```

### Background Exports

1. **Full Resolution**
   - 1920x1080 PNG
   - Full color depth
   - Seamless horizontal tiling

2. **Layer Naming**
   ```
   bg_[level]_[description]_layer[1-3].png
   
   Examples:
   bg_jungle_sky_layer1.png
   bg_castle_interior_layer2.png
   ```

### Compression Settings

For Godot import:

```
Texture Import Settings:
- Compress: Lossless (for pixel art)
- Filter: Nearest (critical for pixel art!)
- Mipmaps: Disabled
- Repeat: Disabled (characters) / Enabled (backgrounds)
```

---

## ✅ Integration Checklist

### Pre-Import Checks

- [ ] All character base sprites generated (6 total)
- [ ] All animation sprite sheets exported
- [ ] All background layers created
- [ ] Files named correctly
- [ ] Transparent backgrounds where needed
- [ ] Seamless tiling for backgrounds tested

### Godot Import Steps

1. **Copy Assets to Project**
   ```bash
   cp -r generated_assets/ demonteks/Assets/
   ```

2. **Select All Assets in Godot**
   - Navigate to Assets folder
   - Select all sprite files

3. **Configure Import Settings**
   ```
   Import Tab:
   - Compress: Lossless
   - Filter: Nearest (CRITICAL!)
   - Mipmaps: Off
   - Repeat: Off (characters) / On (backgrounds)
   ```

4. **Click "Reimport"**

5. **Test in Scene**
   - Create test scene
   - Add AnimatedSprite2D
   - Load sprite sheets
   - Configure animations
   - Test playback

### Animation Configuration

For each character:

1. **Open Character Scene**
   ```
   Scenes/Characters/[Character].tscn
   ```

2. **Add AnimatedSprite2D Node**

3. **Create SpriteFrames Resource**
   - Right-click AnimatedSprite2D
   - New SpriteFrames

4. **Add Animations**
   For each animation:
   - Click "New Animation"
   - Name it (idle, walk, etc.)
   - Set FPS (see timing table)
   - Load sprite sheet
   - Configure frame count
   - Set loop property

5. **Test Animations**
   - Play scene
   - Verify all animations
   - Check transitions
   - Adjust timing if needed

---

## 🔍 Troubleshooting

### Blurry Sprites

**Problem**: Sprites look blurry or smooth
**Solution**: 
```
1. Select sprite in FileSystem
2. Import tab > Filter: Nearest
3. Click Reimport
4. Project Settings > Rendering > Textures > Canvas Textures > Default Texture Filter: Nearest
```

### Animation Not Playing

**Problem**: Animation frames don't animate
**Solution**:
```
1. Check SpriteFrames resource is set
2. Verify animation name in code matches SpriteFrames
3. Confirm FPS is not 0
4. Check animation is set to loop (if needed)
5. Use $AnimatedSprite2D.play("animation_name")
```

### Background Seams

**Problem**: Visible line where background tiles repeat
**Solution**:
```
1. Ensure image width is exact multiple of tile size
2. Check for edge artifacts in source PNG
3. Use "Repeat" import mode
4. Verify seamless generation in PixelLab
```

### Missing Frames

**Problem**: Sprite sheet frames not loading correctly
**Solution**:
```
1. Verify frame count in SpriteFrames matches sheet
2. Check sprite sheet has no extra padding
3. Confirm frame size is exactly 128x128
4. Use "Grid" mode in SpriteFrames with:
   - Columns: number of frames
   - Rows: 1
   - Frame size: 128x128
```

### Wrong Animation Playing

**Problem**: Character plays wrong animation
**Solution**:
```csharp
// In code, ensure animation names match SpriteFrames:
_animatedSprite.Play("idle");  // Lowercase, exact match

// Not:
_animatedSprite.Play("Idle");  // Wrong case
```

---

## 📊 Asset Checklist

### Characters (6 total)

#### Explorer
- [ ] Base sprite
- [ ] Idle animation (4 frames)
- [ ] Walk animation (8 frames)
- [ ] Run animation (6 frames)
- [ ] Jump animation (4 frames)
- [ ] Attack knife animation (6 frames)
- [ ] Hurt animation (3 frames)
- [ ] Death animation (5 frames)

#### Normal Demontek
- [ ] Base sprite
- [ ] Idle animation (4 frames)
- [ ] Walk animation (8 frames)
- [ ] Run animation (6 frames)
- [ ] Jump animation (4 frames)
- [ ] Attack animation (6 frames)
- [ ] Hurt animation (3 frames)
- [ ] Death animation (5 frames)

#### Guard Demontek
- [ ] Base sprite
- [ ] All 8 animations (same as Normal)

#### Mutant Demontek
- [ ] Base sprite
- [ ] All 8 animations

#### Goliath Boss
- [ ] Base sprite
- [ ] Idle animation (4 frames)
- [ ] Walk animation (8 frames)
- [ ] Attack1 animation (8 frames)
- [ ] Attack2 animation (8 frames)
- [ ] Hurt animation (3 frames)
- [ ] Roar animation (6 frames)
- [ ] Death animation (10 frames)

#### King Demontek Boss
- [ ] Base sprite
- [ ] Idle animation (4 frames)
- [ ] Walk animation (8 frames)
- [ ] Attack melee animation (10 frames)
- [ ] Attack magic animation (8 frames)
- [ ] Attack combo animation (12 frames)
- [ ] Hurt animation (4 frames)
- [ ] Phase transition animation (10 frames)
- [ ] Death animation (15 frames)

### Backgrounds (8 layers)

#### Jungle (Level 1)
- [ ] Sky layer (1920x1080)
- [ ] Distant trees layer (1920x1080)
- [ ] Foreground foliage layer (1920x1080)

#### Dungeon (Level 2)
- [ ] Back wall layer (1920x1080)
- [ ] Foreground pillars layer (1920x1080)

#### Castle (Level 3)
- [ ] Distant castle layer (1920x1080)
- [ ] Interior hallway layer (1920x1080)
- [ ] Terrace/throne layer (1920x1080)

---

## 🎯 Next Steps

After completing asset generation:

1. **Test All Assets**
   - Create test scenes for each character
   - Verify all animations play correctly
   - Check background parallax

2. **Build First Level**
   - Use Jungle backgrounds
   - Place Explorer character
   - Add Demontek enemies
   - Test gameplay

3. **Iterate**
   - Adjust animations if needed
   - Tweak colors/details
   - Get feedback
   - Refine

4. **Document**
   - Screenshot examples
   - Note any issues
   - Share with team

---

## 📚 Additional Resources

- [Lospec Palette List](https://lospec.com/palette-list) - Color palettes
- [Aseprite Tutorials](https://www.aseprite.org/docs/) - Sprite editing
- [Godot Animation Docs](https://docs.godotengine.org/en/stable/tutorials/animation/index.html)
- [PixelLab MCP Docs](https://pixellab-mcp.docs) - Asset generation

---

**Ready to generate assets? Start with the Explorer character and work through the checklist!**

*Last updated: 2026-02-13*
