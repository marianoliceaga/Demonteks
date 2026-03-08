# 🎮 PixelLab MCP Complete Reference for Demonteks
## Compatible with Antigravity/OpenCode and Claude

---

## 🎯 What You Need

Generate **2D horizontal platformer levels** with:
- **Characters:** 128x128px 
- **Level viewport:** 1920px wide (levels can be wider)
- **Parallax scrolling:** Using Parallax2D + Sprite2D in Godot C#
- **Tool:** PixelLab MCP via Antigravity/OpenCode

---

## 🔧 MCP Tool Syntax

**All PixelLab MCP tools use this format in Antigravity/OpenCode:**

```
Tool Format:
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_sidescroller_tileset</tool_name>
<arguments>
{
  "lower_description": "your description",
  "transition_description": "your description",
  "tile_size": {"width": 16, "height": 16}
}
</arguments>
</use_mcp_tool>

Or in shorthand (if your environment supports):
pixellab:create_sidescroller_tileset({
  lower_description: "your description",
  transition_description: "your description",
  tile_size: { width: 16, height: 16 }
})
```

---

## ✅ Complete Asset Generation Workflow

### PHASE 1: CHARACTER SPRITES (6 Characters × 7-8 Animations Each)

#### 1. Explorer Character (Player)

**Generate Base Character:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_character</tool_name>
<arguments>
{
  "description": "rugged male explorer adventurer, muscular build, short dark hair with stubble, torn khaki pants, tank top, combat boots, holds combat knife, determined expression, Rambo style",
  "name": "Explorer",
  "body_type": "humanoid",
  "n_directions": 8,
  "size": 128,
  "proportions": "{\"type\": \"preset\", \"name\": \"realistic_male\"}",
  "detail": "medium detail",
  "outline": "single color black outline",
  "shading": "medium shading",
  "view": "low top-down"
}
</arguments>
</use_mcp_tool>
```

**Then Get Character ID and Animate:**
```xml
<!-- After creation, get character_id from response -->
<!-- Generate ALL animations: -->

<!-- Idle Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "breathing-idle",
  "animation_name": "idle"
}
</arguments>
</use_mcp_tool>

<!-- Walk Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "walking-8-frames",
  "animation_name": "walk"
}
</arguments>
</use_mcp_tool>

<!-- Run Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "running-6-frames",
  "animation_name": "run"
}
</arguments>
</use_mcp_tool>

<!-- Jump Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "jumping-2",
  "animation_name": "jump"
}
</arguments>
</use_mcp_tool>

<!-- Attack Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "cross-punch",
  "animation_name": "attack_knife",
  "action_description": "slashing with combat knife, quick melee strike"
}
</arguments>
</use_mcp_tool>

<!-- Hurt Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "taking-punch",
  "animation_name": "hurt"
}
</arguments>
</use_mcp_tool>

<!-- Death Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "falling-back-death",
  "animation_name": "death"
}
</arguments>
</use_mcp_tool>

<!-- Crouch Animation -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "template_animation_id": "crouching",
  "animation_name": "crouch"
}
</arguments>
</use_mcp_tool>
```

**Retrieve All Animations:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>get_character</tool_name>
<arguments>
{
  "character_id": "YOUR_CHARACTER_ID_HERE",
  "include_preview": true
}
</arguments>
</use_mcp_tool>
```

#### 2. Normal Demontek (Enemy)

**Generate Character:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_character</tool_name>
<arguments>
{
  "description": "small demonic alien creature, reddish skin color #D32F2F, small horns on head, big yellow-orange eyes, sharp teeth and small fangs, clawed hands and feet, short tail with arrow tip, slightly hunched posture, menacing but small, cute demon alien",
  "name": "Demontek",
  "body_type": "humanoid",
  "n_directions": 8,
  "size": 128,
  "proportions": "{\"type\": \"preset\", \"name\": \"chibi\"}",
  "detail": "medium detail",
  "outline": "single color black outline",
  "shading": "medium shading",
  "view": "low top-down"
}
</arguments>
</use_mcp_tool>
```

**Then animate with same animation templates as Explorer**

#### 3. Guard Demontek

**Generate Character:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_character</tool_name>
<arguments>
{
  "description": "militarized demon alien guard, same as Demontek but wearing military uniform, guard cap or helmet, chest armor plate, belt with pouches, official badge, stands more upright, carries baton weapon, disciplined posture",
  "name": "Guard_Demontek",
  "body_type": "humanoid",
  "n_directions": 8,
  "size": 128,
  "proportions": "{\"type\": \"preset\", \"name\": \"chibi\"}",
  "detail": "medium detail",
  "outline": "single color black outline",
  "shading": "medium shading",
  "view": "low top-down"
}
</arguments>
</use_mcp_tool>
```

#### 4. Mutant Demontek

**Generate Character:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_character</tool_name>
<arguments>
{
  "description": "mutant demon alien with HUGE exaggerated muscles, bulging muscular arms legs and chest, visible veins on muscles, MUCH larger glowing yellow eyes with crazed look, wider mouth with more teeth, hunched aggressive stance, bigger claws, thicker tail, ripped torn clothing scraps, intimidating and powerful",
  "name": "Mutant_Demontek",
  "body_type": "humanoid",
  "n_directions": 8,
  "size": 128,
  "proportions": "{\"type\": \"custom\", \"head_size\": 1.2, \"arms_length\": 1.3, \"legs_length\": 1.2, \"shoulder_width\": 1.8, \"hip_width\": 1.4}",
  "detail": "highly detailed",
  "outline": "single color black outline",
  "shading": "detailed shading",
  "view": "low top-down"
}
</arguments>
</use_mcp_tool>
```

#### 5. Goliath Boss

**Generate Character:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_character</tool_name>
<arguments>
{
  "description": "massive Demontek-Monster fusion boss, FILLS ENTIRE TILE, giant size, Demontek head with monstrous body, multiple arms (4 total), thick powerful legs, armored plates on body, spikes on shoulders and back, torn flesh showing muscle, stitches and scars from experiments, hunched but huge, glowing bright red eyes, sharp teeth drooling, intimidating boss creature",
  "name": "Goliath",
  "body_type": "humanoid",
  "n_directions": 8,
  "size": 128,
  "proportions": "{\"type\": \"custom\", \"head_size\": 1.5, \"arms_length\": 1.4, \"legs_length\": 1.3, \"shoulder_width\": 2.0, \"hip_width\": 1.8}",
  "detail": "highly detailed",
  "outline": "single color black outline",
  "shading": "detailed shading",
  "view": "low top-down"
}
</arguments>
</use_mcp_tool>
```

**Boss-Specific Animations:**
```xml
<!-- Goliath gets additional boss animations -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "GOLIATH_CHARACTER_ID",
  "template_animation_id": "leg-sweep",
  "animation_name": "ground_pound",
  "action_description": "massive ground pound attack creating shockwave"
}
</arguments>
</use_mcp_tool>

<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "GOLIATH_CHARACTER_ID",
  "template_animation_id": "roundhouse-kick",
  "animation_name": "multi_arm_swipe",
  "action_description": "sweeping attack with multiple arms"
}
</arguments>
</use_mcp_tool>
```

#### 6. King Demontek (Final Boss)

**Generate Character:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_character</tool_name>
<arguments>
{
  "description": "giant majestic King Demontek final boss, FILLS ENTIRE TILE tallest character, giant Demontek 2-3x normal size, long flowing red-grey beard, large ornate golden crown with red gems, regal torn battle-worn robes, ornate armor pieces, holds massive sword or scepter, flowing cape behind, larger horns, wise but fierce expression, battle scars, royal and terrifying, ultimate final boss",
  "name": "King_Demontek",
  "body_type": "humanoid",
  "n_directions": 8,
  "size": 128,
  "proportions": "{\"type\": \"custom\", \"head_size\": 1.3, \"arms_length\": 1.5, \"legs_length\": 1.5, \"shoulder_width\": 1.9, \"hip_width\": 1.6}",
  "detail": "highly detailed",
  "outline": "single color black outline",
  "shading": "detailed shading",
  "view": "low top-down"
}
</arguments>
</use_mcp_tool>
```

**King-Specific Animations:**
```xml
<!-- Massive sword swing -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "KING_CHARACTER_ID",
  "template_animation_id": "roundhouse-kick",
  "animation_name": "massive_sword_swing",
  "action_description": "huge horizontal sword slash with giant blade"
}
</arguments>
</use_mcp_tool>

<!-- Fire projectile magic -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "KING_CHARACTER_ID",
  "template_animation_id": "fireball",
  "animation_name": "fire_magic",
  "action_description": "casting fire projectiles, magical attack"
}
</arguments>
</use_mcp_tool>

<!-- Power up phase transition -->
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>animate_character</tool_name>
<arguments>
{
  "character_id": "KING_CHARACTER_ID",
  "template_animation_id": "surprise-uppercut",
  "animation_name": "phase_power_up",
  "action_description": "powering up for next phase, energy aura"
}
</arguments>
</use_mcp_tool>
```

---

### PHASE 2: LEVEL TILESETS (3 Tilesets)

#### Jungle Tileset (Level 1)

```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_sidescroller_tileset</tool_name>
<arguments>
{
  "lower_description": "dark brown earth and rich dirt ground, natural soil texture, organic ground material",
  "transition_description": "vibrant green grass top layer with alien purple flowering plants, hanging vine details, moss patches, exotic alien vegetation",
  "transition_size": 0.5,
  "tile_size": {"width": 16, "height": 16},
  "detail": "medium detail",
  "outline": "selective outline",
  "shading": "medium shading",
  "view": "high top-down",
  "text_guidance_scale": 8,
  "tile_strength": 1.0,
  "tileset_adherence": 100,
  "tileset_adherence_freedom": 500
}
</arguments>
</use_mcp_tool>
```

**Retrieve Tileset:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>get_sidescroller_tileset</tool_name>
<arguments>
{
  "tileset_id": "YOUR_TILESET_ID_HERE",
  "include_example_map": true
}
</arguments>
</use_mcp_tool>
```

#### Dungeon Tileset (Level 2)

```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_sidescroller_tileset</tool_name>
<arguments>
{
  "lower_description": "dark weathered stone brick walls, ancient medieval dungeon blocks, worn stone texture with cracks",
  "transition_description": "mossy dark green top surface with hanging iron chains details, prison bar elements, torch bracket details",
  "transition_size": 0.25,
  "tile_size": {"width": 16, "height": 16},
  "detail": "medium detail",
  "outline": "single color outline",
  "shading": "detailed shading",
  "view": "high top-down",
  "text_guidance_scale": 8,
  "tile_strength": 1.2,
  "tileset_adherence": 150,
  "tileset_adherence_freedom": 400
}
</arguments>
</use_mcp_tool>
```

#### Castle Tileset (Level 3)

```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_sidescroller_tileset</tool_name>
<arguments>
{
  "lower_description": "crimson red brick castle walls, gothic stone blocks, hellish medieval architecture, demonic fortress material",
  "transition_description": "dark charcoal stone floor top surface with fire brazier glow details, torn demon banner elements, gothic ornate carvings",
  "transition_size": 0.3,
  "tile_size": {"width": 16, "height": 16},
  "detail": "highly detailed",
  "outline": "selective outline",
  "shading": "detailed shading",
  "view": "high top-down",
  "text_guidance_scale": 9,
  "tile_strength": 1.1,
  "tileset_adherence": 120,
  "tileset_adherence_freedom": 450
}
</arguments>
</use_mcp_tool>
```

---

### PHASE 3: DECORATIVE ELEMENTS (Props)

#### Jungle Decorations

**Large Background Trees:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "giant alien tree with glowing purple bioluminescent leaves, mystical jungle background element, large trunk",
  "size": 128,
  "detail": "highly detailed",
  "outline": "selective outline",
  "shading": "detailed shading",
  "tile_shape": "block",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

**Hanging Vines:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "hanging vine with glowing purple alien flowers, draping jungle plant, organic curves",
  "size": 64,
  "detail": "medium detail",
  "outline": "selective outline",
  "shading": "basic shading",
  "tile_shape": "thin tile",
  "text_guidance_scale": 7
}
</arguments>
</use_mcp_tool>
```

**Alien Flowers:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "large alien flower with purple petals and bioluminescent glow, exotic jungle plant",
  "size": 48,
  "detail": "medium detail",
  "outline": "selective outline",
  "shading": "medium shading",
  "tile_shape": "thin tile",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

**Moss-Covered Rocks:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "moss covered stone rock decoration, alien green moss texture, jungle ground prop",
  "size": 32,
  "detail": "medium detail",
  "outline": "selective outline",
  "shading": "medium shading",
  "tile_shape": "block",
  "text_guidance_scale": 7
}
</arguments>
</use_mcp_tool>
```

#### Dungeon Decorations

**Large Chains:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "large rusty iron chain links hanging, medieval dungeon prison chains, corroded metal",
  "size": 64,
  "detail": "highly detailed",
  "outline": "single color outline",
  "shading": "detailed shading",
  "tile_shape": "thin tile",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

**Prison Bars:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "vertical iron prison bars, medieval dungeon cell bars, rusty metal",
  "size": 48,
  "detail": "medium detail",
  "outline": "single color outline",
  "shading": "medium shading",
  "tile_shape": "thin tile",
  "text_guidance_scale": 7
}
</arguments>
</use_mcp_tool>
```

**Torch Bracket:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "wall-mounted torch bracket with flickering flame, medieval dungeon lighting, iron holder",
  "size": 32,
  "detail": "medium detail",
  "outline": "selective outline",
  "shading": "medium shading",
  "tile_shape": "block",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

#### Castle Decorations

**Gothic Pillars:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "ornate gothic stone pillar with demon carvings, castle architecture column, intricate detail",
  "size": 128,
  "detail": "highly detailed",
  "outline": "selective outline",
  "shading": "detailed shading",
  "tile_shape": "block",
  "text_guidance_scale": 9
}
</arguments>
</use_mcp_tool>
```

**Fire Braziers:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "large fire brazier with roaring flames, hellish castle lighting, ornate metal bowl",
  "size": 64,
  "detail": "highly detailed",
  "outline": "selective outline",
  "shading": "detailed shading",
  "tile_shape": "block",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

**Demon Banners:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "torn hanging banner with demon symbol, tattered red fabric, castle decoration",
  "size": 48,
  "detail": "medium detail",
  "outline": "selective outline",
  "shading": "medium shading",
  "tile_shape": "thin tile",
  "text_guidance_scale": 7
}
</arguments>
</use_mcp_tool>
```

**Suits of Armor:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "medieval suit of armor standing, full plate armor on display, castle guard decoration",
  "size": 64,
  "detail": "highly detailed",
  "outline": "selective outline",
  "shading": "detailed shading",
  "tile_shape": "block",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

---

## 🌄 PHASE 4: BACKGROUNDS (3 Options)

### OPTION 1: External AI Tools (RECOMMENDED - Best Quality) ⭐

**You prefer this option!** Use the prompts from the separate document "AI_BACKGROUND_PROMPTS.md"

**Tools to use:**
- DALL-E 3 via ChatGPT Plus
- Midjourney with `--tile --ar 16:9`
- Scenario.com
- Leonardo.ai

See complete prompts in the AI Background Prompts document provided separately.

---

### OPTION 2: Quick Godot Solution (Fastest - 2-3 hours)

**No PixelLab MCP needed - use Godot C# only!**

Create in your Level scenes:

```csharp
// In Level1_Jungle.tscn - Add this via C# script or in editor

// Sky Layer (Far)
var skyLayer = new ParallaxLayer();
skyLayer.MotionScale = new Vector2(0.2f, 0.2f);
skyLayer.MotionMirroring = new Vector2(1920, 0);

var skyRect = new ColorRect();
skyRect.Size = new Vector2(1920, 1080);

// Create gradient
var gradient = new Gradient();
gradient.AddPoint(0.0f, new Color("#6A1B9A")); // Top violet
gradient.AddPoint(1.0f, new Color("#9C27B0")); // Bottom purple

var gradientTexture = new GradientTexture2D();
gradientTexture.Gradient = gradient;
gradientTexture.Width = 1920;
gradientTexture.Height = 1080;

skyRect.Texture = gradientTexture;
skyLayer.AddChild(skyRect);
parallaxBackground.AddChild(skyLayer);
```

**Then add PixelLab decorative elements** (generated above) arranged as Sprite2D nodes in mid/foreground layers.

---

### OPTION 3: Hybrid PixelLab + Godot (Good Balance)

**Step 1: Generate large decorative background pieces with PixelLab**

**Jungle Background Piece:**
```xml
<use_mcp_tool>
<server_name>pixellab</server_name>
<tool_name>create_isometric_tile</tool_name>
<arguments>
{
  "description": "wide alien jungle background vista, purple sky, distant tree silhouettes, atmospheric depth, panoramic view",
  "size": 128,
  "detail": "highly detailed",
  "outline": "lineless",
  "shading": "detailed shading",
  "tile_shape": "block",
  "text_guidance_scale": 8
}
</arguments>
</use_mcp_tool>
```

**Step 2: In Godot C#, arrange these tiles in a pattern:**

```csharp
public partial class BackgroundBuilder : Node2D
{
    [Export] public Texture2D BackgroundTile;
    [Export] public int TileRepeatCount = 20;
    
    public override void _Ready()
    {
        // Create repeating background pattern
        for (int i = 0; i < TileRepeatCount; i++)
        {
            var sprite = new Sprite2D();
            sprite.Texture = BackgroundTile;
            sprite.Position = new Vector2(i * 128, 0);
            
            // Vary tint for depth
            float tintVariation = 0.8f + (i % 3) * 0.1f;
            sprite.Modulate = new Color(tintVariation, tintVariation, tintVariation);
            
            AddChild(sprite);
        }
    }
}
```

---

## 🎮 Godot C# Integration

### Setting Up ParallaxBackground in C#

```csharp
using Godot;

public partial class LevelBackgroundSetup : Node
{
    [Export] public Texture2D SkyTexture;
    [Export] public Texture2D MidTexture;
    [Export] public Texture2D ForeTexture;
    
    private ParallaxBackground _parallaxBg;
    
    public override void _Ready()
    {
        _parallaxBg = new ParallaxBackground();
        AddChild(_parallaxBg);
        
        // Layer 1: Sky (slowest)
        CreateParallaxLayer(SkyTexture, new Vector2(0.2f, 0.2f), 1920, 0);
        
        // Layer 2: Mid-ground
        CreateParallaxLayer(MidTexture, new Vector2(0.5f, 0.5f), 3840, 1);
        
        // Layer 3: Foreground (fastest)
        CreateParallaxLayer(ForeTexture, new Vector2(1.2f, 1.0f), 5760, 2);
    }
    
    private void CreateParallaxLayer(Texture2D texture, Vector2 motionScale, 
                                      int mirrorWidth, int zIndex)
    {
        var layer = new ParallaxLayer();
        layer.MotionScale = motionScale;
        layer.MotionMirroring = new Vector2(mirrorWidth, 0);
        layer.ZIndex = zIndex;
        
        var sprite = new Sprite2D();
        sprite.Texture = texture;
        sprite.Centered = false;
        sprite.TextureFilter = TextureFilterEnum.Nearest; // CRITICAL for pixel art!
        
        layer.AddChild(sprite);
        _parallaxBg.AddChild(layer);
    }
}
```

### Setting Up TileMap from PixelLab Tileset in C#

```csharp
using Godot;

public partial class LevelTileMapSetup : Node2D
{
    [Export] public TileSet TileSetResource;
    
    private TileMap _tileMap;
    
    public override void _Ready()
    {
        _tileMap = new TileMap();
        _tileMap.TileSet = TileSetResource;
        AddChild(_tileMap);
        
        // Example: Paint some platforms
        // Layer 0, coords (x, y), source_id 0, atlas_coords (tile_x, tile_y)
        _tileMap.SetCell(0, new Vector2I(5, 10), 0, new Vector2I(0, 0));
        _tileMap.SetCell(0, new Vector2I(6, 10), 0, new Vector2I(1, 0));
        _tileMap.SetCell(0, new Vector2I(7, 10), 0, new Vector2I(2, 0));
    }
}
```

### Importing PixelLab Assets Programmatically

```csharp
using Godot;
using System.IO;

public partial class AssetImporter : Node
{
    public void ImportPixelLabAsset(string pngPath, string godotPath)
    {
        // Copy PNG to Godot project
        File.Copy(pngPath, godotPath, true);
        
        // Load as texture
        var texture = GD.Load<Texture2D>(godotPath);
        
        // CRITICAL: Set import settings
        // In Godot 4.x, you'd set these in the .import file or via code:
        // texture.Filter = Texture2D.FilterEnum.Nearest;
        
        GD.Print($"Imported: {godotPath}");
    }
}
```

---

## 📋 Complete Asset Checklist with MCP Calls

### Characters (6 total)
- [ ] Explorer - `create_character` → 8 `animate_character` calls
- [ ] Normal Demontek - `create_character` → 8 `animate_character` calls
- [ ] Guard Demontek - `create_character` → 8 `animate_character` calls
- [ ] Mutant Demontek - `create_character` → 8 `animate_character` calls
- [ ] Goliath Boss - `create_character` → 10 `animate_character` calls
- [ ] King Boss - `create_character` → 11 `animate_character` calls

**Total MCP calls for characters: 6 + (6 × 8-11) = ~59 calls**

### Tilesets (3 total)
- [ ] Jungle - `create_sidescroller_tileset` + `get_sidescroller_tileset`
- [ ] Dungeon - `create_sidescroller_tileset` + `get_sidescroller_tileset`
- [ ] Castle - `create_sidescroller_tileset` + `get_sidescroller_tileset`

**Total MCP calls for tilesets: 6 calls**

### Decorations (~15-20 elements)
- [ ] Jungle decorations (4-5 items) - `create_isometric_tile` each
- [ ] Dungeon decorations (3-4 items) - `create_isometric_tile` each
- [ ] Castle decorations (4-5 items) - `create_isometric_tile` each

**Total MCP calls for decorations: ~15 calls**

### Backgrounds
- [ ] **Option 1:** External AI tools (0 MCP calls)
- [ ] **Option 2:** Godot C# only (0 MCP calls)
- [ ] **Option 3:** Hybrid (3-9 `create_isometric_tile` calls)

---

## ⏱️ Time Estimates

| Phase | MCP Calls | Time |
|-------|-----------|------|
| Characters (all 6 + animations) | ~59 | 2-3 days |
| Tilesets (all 3) | 6 | 1 day |
| Decorations (15 items) | 15 | 1 day |
| Backgrounds Option 1 (External) | 0 | 2-3 days |
| Import & Setup in Godot | 0 | 2-3 days |
| **TOTAL** | **~80 MCP calls** | **8-11 days** |

---

## 🚀 Quick Start Sequence

**Day 1:** Explorer character (1 create + 8 animate)
**Day 2:** Demontek + Guard (2 create + 16 animate)  
**Day 3:** Mutant + Goliath (2 create + 18 animate)
**Day 4:** King + Jungle tileset (1 create + 11 animate + 2 tileset)
**Day 5:** Dungeon + Castle tilesets + decorations (4 tileset + 15 decor)
**Day 6-8:** Generate backgrounds with external AI
**Day 9-11:** Import everything to Godot, setup, test

---

## 💡 Pro Tips

1. **Generate base characters first** before animations
2. **Save all character_ids** - you'll need them for animations
3. **Test tilesets in Godot immediately** after generation
4. **Use get_character with include_preview: true** to see all animations
5. **For backgrounds, Option 1 (External AI) gives best quality**
6. **Keep all generated PNGs organized** by type

---

**You now have every MCP call needed to generate all assets! 🎨✨**

Start with the Explorer character and work through the checklist systematically!
