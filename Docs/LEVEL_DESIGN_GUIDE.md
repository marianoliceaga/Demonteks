# 🗺️ Demonteks - Complete Level Design Guide

## 📐 Scale & Dimensions Reference

### Character Scale
- **Player/Enemies:** 128x128px per frame
- **Recommended Tile Size:** 16x16px (8 tiles = character height)
- **Alternative:** 32x32px (4 tiles = character height)
- **Level Viewport:** 1920×1080px visible area
- **Total Level Width:** Can extend to 5760px+ (3-6× screen width)

### Why 16×16 Tiles?

```
128px character ÷ 16px tiles = 8 tiles tall

Perfect for platformer level design!

Example jump calculations:
- 1 tile jump (16px) = easy hop
- 2 tile jump (32px) = normal jump
- 3 tile jump (48px) = challenging leap
- 4+ tile jump (64px+) = requires running start
```

---

## 🎨 TILESET GENERATION WITH MCP

### Method 1: Sidescroller Tilesets (RECOMMENDED)

Perfect for 2D platformer games!

#### Jungle Tileset (Level 1)

```javascript
// MCP Call:
create_sidescroller_tileset({
  lower_description: "dark rich brown earth and fertile soil ground base, dirt texture with embedded small rocks, underground dirt layers visible in cross-section",
  transition_description: "vibrant lush green grass top surface layer with alien purple glowing flowering plants scattered throughout, thin hanging vines with heart-shaped leaves, moss patches growing on platform edges, exotic jungle foliage details with ferns",
  transition_size: 0.5,
  tile_size: { width: 16, height: 16 },
  detail: "medium detail",
  outline: "selective outline",
  shading: "medium shading",
  view: "high top-down",
  text_guidance_scale: 8.0,
  tile_strength: 1.0,
  tileset_adherence: 100,
  tileset_adherence_freedom: 500
})

// Wait ~100 seconds for processing

// Retrieve completed tileset:
get_sidescroller_tileset({
  tileset_id: "your-jungle-tileset-id",
  include_example_map: true
})

// Download PNG to: Assets/tilesets/jungle_tileset_16x16.png
// Also download metadata JSON for reference
```

**Generated Output:**
- ✅ Grass-topped platform tiles
- ✅ Dirt middle/fill blocks
- ✅ Left/right edge pieces
- ✅ Corner tiles (4 types)
- ✅ Transition variants
- ✅ Seamless horizontal tiling
- ✅ Ready for Godot TileMap

---

#### Dungeon Tileset (Level 2)

```javascript
create_sidescroller_tileset({
  lower_description: "dark weathered grey stone brick walls, ancient cracked medieval dungeon masonry blocks with visible mortar, worn eroded stone texture with deep cracks and chips",
  transition_description: "moss covered dark green top surface with orange rust stains and water damage streaks, hanging iron chain link details draped across, rusted prison bar texture overlay embedded, torch scorch burn marks on edges creating soot",
  transition_size: 0.25,
  tile_size: { width: 16, height: 16 },
  detail: "medium detail",
  outline: "single color outline",
  shading: "detailed shading",
  view: "high top-down",
  text_guidance_scale: 8.0,
  tile_strength: 1.0,
  tileset_adherence: 100,
  tileset_adherence_freedom: 500
})

// Download to: Assets/tilesets/dungeon_tileset_16x16.png
```

---

#### Castle Tileset (Level 3)

```javascript
create_sidescroller_tileset({
  lower_description: "crimson red brick castle walls, gothic medieval stone blocks with dark mortar, dark red hellish aged masonry with cracks, imposing fortress architecture blocks with weathering",
  transition_description: "dark charred stone floor surface with fire brazier scorch marks and black soot stains, torn demon banner fabric details hanging down from edges, ornate gothic carved patterns in stone surface, flame texture burns on platform edges",
  transition_size: 0.3,
  tile_size: { width: 16, height: 16 },
  detail: "highly detailed",
  outline: "selective outline",
  shading: "detailed shading",
  view: "high top-down",
  text_guidance_scale: 8.0,
  tile_strength: 1.0,
  tileset_adherence: 100,
  tileset_adherence_freedom: 500
})

// Download to: Assets/tilesets/castle_tileset_16x16.png
```

---

### Method 2: Decorative Elements (Optional)

Generate individual props with isometric tiles:

```javascript
// Large decorative tree
create_isometric_tile({
  description: "giant alien tree with glowing purple bioluminescent leaves, thick gnarled trunk, mystical magical jungle background element, ethereal purple glow effect emanating from leaves",
  size: 128,
  detail: "highly detailed",
  outline: "selective outline",
  shading: "detailed shading",
  tile_shape: "block",
  text_guidance_scale: 8.0
})

// Hanging vine decoration
create_isometric_tile({
  description: "long hanging vine with glowing purple flowers draping down vertically, alien jungle plant with bioluminescent blooms, thin flexible vine",
  size: 64,
  detail: "medium detail",
  outline: "selective outline",
  shading: "basic shading",
  tile_shape: "thin tile",
  text_guidance_scale: 8.0
})

// Rock/boulder decoration
create_isometric_tile({
  description: "moss covered round stone rock, alien glowing moss texture, jungle ground decoration with small lichens growing",
  size: 32,
  detail: "medium detail",
  outline: "selective outline",
  shading: "medium shading",
  tile_shape: "block",
  text_guidance_scale: 8.0
})
```

---

## 🌄 BACKGROUND GENERATION

### 🥇 OPTION 1: External AI Tools (Your Choice - Best Quality)

Use DALL-E 3, Midjourney, or Scenario.com with these optimized prompts.

---

#### JUNGLE BACKGROUNDS (4 Layers)

**Layer 1: Far Sky Background**
- **Dimensions:** 1920×1080px
- **Scroll Speed:** 0.2× (slowest, 5× slower than player)
- **Purpose:** Static-feeling deep background

```
DALL-E 3 / Midjourney Prompt:

"Pixel art alien jungle sky background layer, smooth violet to purple 
vertical gradient transitioning from light lavender (#9C27B0) at top 
to deep dark purple (#6A1B9A) at bottom creating dusk atmosphere, 
multiple large distant planets with rings visible in upper portion of 
sky, three moons of varying sizes (one large, two small), ethereal 
glowing nebula clouds with twinkling stars scattered throughout, mystical 
otherworldly alien atmosphere, NO ground NO trees NO plants NO foreground 
elements whatsoever, pure sky only, 16-bit retro game art style, seamless 
horizontal tiling repeating pattern for parallax scrolling, vibrant rich 
purple color scheme, sci-fi space landscape aesthetic, clean pixel art 
with dithering"

For Midjourney add: --ar 16:9 --tile --style raw --v 6 --q 2
For DALL-E 3: Request 1792×1024 (closest available to 1920×1080)
Scenario.com: Use "seamless tile" mode enabled
```

---

**Layer 2: Distant Mountains/Trees**
- **Dimensions:** 3840×1080px (2× screen width for smooth scroll)
- **Scroll Speed:** 0.5× (mid-distance)
- **Purpose:** Far background depth element

```
Prompt:

"Pixel art giant alien tree silhouettes forming mountain-like shapes in 
far distant background, massive thick tree trunks reaching hundreds of 
feet tall, sprawling branch canopy creating horizon line, dark purple 
(#4A148C) and deep forest green (#1B5E20) silhouette colors creating 
depth through color, atmospheric depth haze effect softening edges, 
very low detail appropriate for distant background layer, NO foreground 
elements NO close-up details NO texture detail, distant background 
silhouette layer only, 3840×1080 ultra-wide resolution, seamless 
horizontal tileable repeating pattern with no visible seam, 16-bit 
retro pixel art style, mysterious alien jungle atmosphere, low contrast 
for depth perception, side-scrolling game parallax layer aesthetic"

Note: May need to generate 1920×1080 twice and tile horizontally
Alternative: Generate at maximum size and crop/extend to 3840
```

---

**Layer 3: Mid-ground Foliage**
- **Dimensions:** 3840×1080px
- **Scroll Speed:** 0.7× (slightly slower than player)
- **Purpose:** Main atmospheric layer with plant detail

```
Prompt:

"Pixel art alien jungle mid-ground foliage layer with depth, large exotic 
fern leaves and broad tropical plants with unique alien shapes, thick 
hanging vines with bioluminescent glowing purple flowers creating light 
sources, strange unique alien plant life with spiraling tendrils and 
unusual leaf patterns, vibrant bright greens (#2E7D32 #4CAF50) with 
glowing purple light accents (#9C27B0 #CE93D8), medium level detail 
appropriate for mid-ground distance, semi-transparent elements with 
alpha channel transparency for layering over background, 3840×1080 
resolution, seamless horizontal tiling repeating pattern, 16-bit pixel 
art style, mystical glowing plant aesthetic with ambient lighting, 
parallax scrolling game layer with depth perception, atmospheric alien 
jungle environment with fog effects"
```

---

**Layer 4: Foreground Details**
- **Dimensions:** 5760×1080px (3× screen width)
- **Scroll Speed:** 1.2× (faster than player, creates depth)
- **Purpose:** Cinematic foreground frame

```
Prompt:

"Pixel art alien jungle foreground extreme close-up layer creating 
cinematic frame, massive detailed leaves filling frame edges in extreme 
closeup perspective, thick hanging vines draping down from top edge of 
frame creating natural framing, glowing bioluminescent purple alien 
flowers with visible particle light effects and bloom glow, dramatic 
bokeh depth of field blur effect on outer edges, rich fine detail and 
intricate texture on leaves showing veins, semi-transparent foreground 
with alpha channel transparency for layering, 5760×1080 ultra-wide 
resolution, seamless horizontal repeating tiling pattern, 16-bit highly 
detailed pixel art style, vibrant saturated colors with high contrast, 
cinematic depth of field effect, foreground parallax scrolling layer, 
immersive atmospheric framing"

Important: Generate at maximum resolution, may need multiple generations
```

---

#### DUNGEON BACKGROUNDS (3 Layers)

**Layer 1: Stone Wall Background**
- **Dimensions:** 1920×1080px
- **Scroll Speed:** 0.3×

```
Prompt:

"Pixel art medieval dungeon back wall background with detailed masonry, 
dark weathered stone bricks (#3E2723 #4E342E brown-grey tones) with 
visible mortar lines between each individual brick creating texture, 
deep cracks spiderwebbing through stone with dark green moss (#2E7D32) 
growing in deep crevices and cracks, ancient worn eroded texture showing 
centuries of age and weathering, empty rusted corroded metal torch 
sconces mounted on wall at regular intervals, dark water damage stains 
running down walls creating streaks, oppressive heavy dark prison 
atmosphere, dark grey brown color palette creating gloom, NO chains 
NO bars NO pillars NO foreground elements whatsoever, flat wall surface 
only showing stonework, 1920×1080 resolution, seamless horizontal tiling 
repeating pattern, 16-bit retro game pixel art style, gloomy medieval 
dungeon prison aesthetic, dim atmospheric lighting from distant torches 
casting shadows, highly detailed ancient stonework texture"
```

---

**Layer 2: Architectural Mid-ground**
- **Dimensions:** 3840×1080px
- **Scroll Speed:** 0.6×

```
Prompt:

"Pixel art dungeon mid-ground architectural layer creating depth, thick 
stone support pillars and columns at intervals creating spatial depth, 
heavy iron chains hanging down from vaulted ceiling in foreground with 
weight, sections of rusted prison bars creating dungeon cells between 
pillars, medieval dungeon fortress architecture with gothic arches, 
flickering orange warm torch light casting dramatic moving shadows 
across all surfaces, worn cracked stone pillar texture with age, dark 
atmospheric dungeon colors with orange torch glow, semi-transparent 
layer for parallax layering effect, 3840×1080 resolution, seamless 
horizontal repeating pattern, 16-bit pixel art style, ominous oppressive 
prison atmosphere with heavy shadows, depth and dimensional space between 
pillars, gothic medieval aesthetic with imposing architecture"
```

---

**Layer 3: Chains Foreground**
- **Dimensions:** 3840×1080px
- **Scroll Speed:** 1.3× (fastest)

```
Prompt:

"Pixel art extreme closeup foreground of heavy hanging chains creating 
frame, large rusty corroded metal chain links in sharp focus with fine 
detail visible, heavily worn iron texture with rust (#5D4037 #6D4C41 
brown rust colors) and deep corrosion pitting visible, dramatic high 
contrast lighting with deep black shadows creating mood, oppressive 
prison dungeon atmosphere weighing down, very close camera perspective 
creating cinematic depth of field, semi-transparent foreground with 
alpha channel transparency for layering, 3840×1080 resolution, seamless 
horizontal tiling pattern, 16-bit highly detailed pixel art style, 
depth of field blur effect on distant chains, dark moody medieval 
aesthetic, chains appearing to sway slightly suggesting movement"
```

---

#### CASTLE BACKGROUNDS (3 Layers)

**Layer 1: Distant Castle Vista**
- **Dimensions:** 1920×1080px
- **Scroll Speed:** 0.2×

```
Prompt:

"Pixel art red brick castle fortress walls in far distance creating 
imposing silhouette, gothic medieval architecture with tall towers and 
crenellated ramparts visible on horizon, hellish crimson red sky 
background creating ominous atmosphere (#8B0000 to #B71C1C gradient 
from dark to light), fire glow and floating embers rising from below 
creating orange-red ambient lighting (#FF6F00), distant castle 
silhouette looming on horizon line, oppressive demonic evil atmosphere 
with smoke, dark red aged weathered bricks with shadow detail, orange 
flame ambient glow illuminating entire scene from below, NO foreground 
detail NO close elements NO interior, background vista panorama only 
showing exterior, 1920×1080 resolution, seamless horizontal tiling 
repeating pattern, 16-bit retro pixel art style, evil demon lord castle 
aesthetic, infernal hellish atmosphere with fire and brimstone, imposing 
throne room approach aesthetic"
```

---

**Layer 2: Interior Architecture**
- **Dimensions:** 3840×1080px
- **Scroll Speed:** 0.5×

```
Prompt:

"Pixel art castle interior mid-ground hallway layer with gothic details, 
red brick walls (#B71C1C #C62828 crimson red tones) with tall gothic 
pointed arches creating dramatic vertical lines, medieval full plate 
suits of armor with demon horn motifs standing at attention along walls 
at intervals, torn hanging banners with demon skull symbols and faded 
red fabric draped between pillars, wall-mounted torch brackets with 
bright flickering orange flames casting dancing shadows on walls, 
ornate gothic castle architecture with carved stone details and reliefs, 
hellish red and orange fire lighting atmosphere creating warmth, dark 
ominous gothic environment with heavy shadows, semi-transparent layer 
for parallax effect, 3840×1080 resolution, seamless horizontal repeating 
pattern, 16-bit pixel art style, demon lord castle interior with grandeur, 
menacing imposing grand scale, throne hall corridor aesthetic leading 
to throne room"
```

---

**Layer 3: Ornate Foreground**
- **Dimensions:** 3840×1080px
- **Scroll Speed:** 1.1× (slightly faster than player)

```
Prompt:

"Pixel art gothic castle ornate pillars extreme closeup foreground 
creating frame, carved stone columns with demon gargoyle faces and bat 
wing motifs in sharp detail, large fire braziers with roaring bright 
orange and yellow flames creating intense light, intricate architectural 
carved detail and weathered stone texture visible, dramatic fire lighting 
casting dynamic moving shadows across carved surfaces, hellish demonic 
atmosphere with heat distortion effects, rich fine texture and relief 
detail work showing craftsmanship, semi-transparent foreground with 
alpha channel for layering, 3840×1080 resolution, seamless horizontal 
tiling pattern, 16-bit highly detailed pixel art style, cinematic depth 
of field blur effect, demonic castle throne terrace aesthetic, epic 
imposing scale creating grandeur, infernal vibe with flames and shadows"
```

---

### 🚀 OPTION 2: Quick Godot + MCP Hybrid (Fastest Implementation)

Use Godot's built-in tools combined with MCP decorative elements.

#### Sky Gradient: C# Implementation

```csharp
using Godot;

namespace Demonteks.Environment
{
    /// <summary>
    /// Creates gradient sky backgrounds using shaders.
    /// </summary>
    public partial class GradientSkyBackground : ParallaxLayer
    {
        [Export] public Color TopColor { get; set; } = new Color(0.61f, 0.15f, 0.69f); // #9C27B0
        [Export] public Color BottomColor { get; set; } = new Color(0.42f, 0.11f, 0.60f); // #6A1B9A
        [Export] public Vector2 SkySize { get; set; } = new Vector2(1920, 1080);

        public override void _Ready()
        {
            // Set parallax properties
            MotionScale = new Vector2(0.2f, 0.2f);
            MotionMirroring = new Vector2(1920, 0);

            CreateGradientSky();
        }

        private void CreateGradientSky()
        {
            var colorRect = new ColorRect();
            colorRect.Size = SkySize;
            
            // Create gradient shader
            var shader = new Shader();
            shader.Code = @"
shader_type canvas_item;

uniform vec4 top_color : source_color = vec4(0.61, 0.15, 0.69, 1.0);
uniform vec4 bottom_color : source_color = vec4(0.42, 0.11, 0.60, 1.0);

void fragment() {
    vec4 gradient = mix(top_color, bottom_color, UV.y);
    COLOR = gradient;
}
";

            var material = new ShaderMaterial();
            material.Shader = shader;
            material.SetShaderParameter("top_color", TopColor);
            material.SetShaderParameter("bottom_color", BottomColor);
            
            colorRect.Material = material;
            AddChild(colorRect);
        }
    }
}
```

**Usage in scene:**
```csharp
// In your level script:
var jungleSky = new GradientSkyBackground();
jungleSky.TopColor = new Color(0.61f, 0.15f, 0.69f); // #9C27B0
jungleSky.BottomColor = new Color(0.42f, 0.11f, 0.60f); // #6A1B9A
parallaxBackground.AddChild(jungleSky);
```

---

#### Decorative Elements: MCP Generation

```javascript
// Large alien tree (128px for background)
create_isometric_tile({
  description: "giant alien tree with glowing purple bioluminescent leaves emanating soft light, thick massive gnarled trunk with bark texture, mystical magical jungle background element towering high, ethereal purple glow effect surrounding leaves creating ambient light",
  size: 128,
  detail: "highly detailed",
  outline: "selective outline",
  shading: "detailed shading",
  tile_shape: "block",
  text_guidance_scale: 8.0
})

// Medium foliage (64px for mid-ground)
create_isometric_tile({
  description: "large exotic alien leaf with purple veins, broad tropical foliage, vibrant green with glowing purple accent lines, jungle decoration element",
  size: 64,
  detail: "medium detail",
  outline: "selective outline",
  shading: "medium shading",
  tile_shape: "thin tile",
  text_guidance_scale: 8.0
})

// Hanging vine (64px tall)
create_isometric_tile({
  description: "long hanging vine with glowing purple flowers draping down vertically, alien jungle plant with small bioluminescent blooms scattered along length, thin flexible vine swaying",
  size: 64,
  detail: "medium detail",
  outline: "selective outline",
  shading: "basic shading",
  tile_shape: "thin tile",
  text_guidance_scale: 8.0
})

// Ground rock (32px for foreground)
create_isometric_tile({
  description: "moss covered round boulder stone, alien glowing moss texture with bioluminescence, jungle ground decoration with small lichens and fungi growing on surface",
  size: 32,
  detail: "medium detail",
  outline: "selective outline",
  shading: "medium shading",
  tile_shape: "block",
  text_guidance_scale: 8.0
})

// Flower cluster (32px accent)
create_isometric_tile({
  description: "cluster of glowing purple alien flowers, bioluminescent blooms emitting soft light, small decorative plant element for foreground",
  size: 32,
  detail: "medium detail",
  outline: "selective outline",
  shading: "basic shading",
  tile_shape: "thin tile",
  text_guidance_scale: 8.0
})
```

**Then arrange in Godot:**

```csharp
using Godot;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    /// <summary>
    /// Arranges decorative sprites in parallax layers.
    /// </summary>
    public partial class DecorativeLayerManager : ParallaxLayer
    {
        [Export] public string[] DecorationPaths { get; set; }
        [Export] public int SpacingBetween { get; set; } = 200;
        [Export] public Vector2 LayerMotionScale { get; set; } = new Vector2(0.5f, 0.5f);
        [Export] public Color TintColor { get; set; } = Colors.White;

        public override void _Ready()
        {
            MotionScale = LayerMotionScale;
            MotionMirroring = new Vector2(SpacingBetween * DecorationPaths.Length, 0);
            
            ArrangeDecorations();
        }

        private void ArrangeDecorations()
        {
            int xPosition = 0;
            
            foreach (string path in DecorationPaths)
            {
                var sprite = new Sprite2D();
                sprite.Texture = GD.Load<Texture2D>(path);
                sprite.Position = new Vector2(xPosition, 540); // Center vertically
                sprite.Modulate = TintColor; // Apply tint for depth
                sprite.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
                
                AddChild(sprite);
                xPosition += SpacingBetween;
            }
        }
    }
}
```

**Result:** Professional-looking backgrounds in 1-2 hours!

---

### 🛠️ OPTION 3: Aseprite Manual Creation ($20)

For complete artistic control:

**Workflow:**
1. **Purchase Aseprite:** https://www.aseprite.org ($19.99)
2. **Create new sprite** at exact dimensions (1920×1080, 3840×1080, etc.)
3. **Enable Tile Mode:**
   - View → Grid → Grid Settings
   - Enable "Tilemap Mode"
   - Check both horizontal and vertical axes
4. **Paint seamless patterns** (what you paint on one edge appears on opposite edge)
5. **Use layers** for complex compositions
6. **Export as PNG** at exact sizes
7. **Import into Godot**

**Advantages:**
- ✅ Perfect style matching
- ✅ Complete artistic control
- ✅ Exact color palettes
- ✅ Custom dimensions
- ✅ Guaranteed seamless tiling
- ✅ Professional workflow

---

## 🛠️ GODOT IMPLEMENTATION (C# Code)

### Complete Parallax Background System

```csharp
using Godot;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    /// <summary>
    /// Complete parallax background management system for all levels.
    /// Supports dynamic level switching and layer configuration.
    /// </summary>
    public partial class ParallaxBackgroundSystem : Node2D
    {
        [Export] public string CurrentLevel { get; set; } = "Jungle";
        [Export] public bool AutoSetup { get; set; } = true;
        
        private ParallaxBackground _parallaxBg;
        private Dictionary<string, LevelBackgroundConfig> _levelConfigs;

        public override void _Ready()
        {
            InitializeLevelConfigs();
            
            if (AutoSetup)
            {
                SetupParallaxBackground(CurrentLevel);
            }
        }

        private void InitializeLevelConfigs()
        {
            _levelConfigs = new Dictionary<string, LevelBackgroundConfig>
            {
                ["Jungle"] = new LevelBackgroundConfig
                {
                    Layers = new List<BackgroundLayer>
                    {
                        new BackgroundLayer("sky", "res://Assets/backgrounds/jungle/sky_1920.png", 
                            new Vector2(0.2f, 0.2f), new Vector2(1920, 0)),
                        new BackgroundLayer("trees", "res://Assets/backgrounds/jungle/trees_3840.png", 
                            new Vector2(0.5f, 0.5f), new Vector2(3840, 0)),
                        new BackgroundLayer("foliage", "res://Assets/backgrounds/jungle/foliage_3840.png", 
                            new Vector2(0.7f, 0.7f), new Vector2(3840, 0)),
                        new BackgroundLayer("foreground", "res://Assets/backgrounds/jungle/foreground_5760.png", 
                            new Vector2(1.2f, 1.0f), new Vector2(5760, 0))
                    }
                },
                
                ["Dungeon"] = new LevelBackgroundConfig
                {
                    Layers = new List<BackgroundLayer>
                    {
                        new BackgroundLayer("wall", "res://Assets/backgrounds/dungeon/wall_1920.png", 
                            new Vector2(0.3f, 0.3f), new Vector2(1920, 0)),
                        new BackgroundLayer("pillars", "res://Assets/backgrounds/dungeon/pillars_3840.png", 
                            new Vector2(0.6f, 0.6f), new Vector2(3840, 0)),
                        new BackgroundLayer("chains", "res://Assets/backgrounds/dungeon/chains_3840.png", 
                            new Vector2(1.3f, 1.0f), new Vector2(3840, 0))
                    }
                },
                
                ["Castle"] = new LevelBackgroundConfig
                {
                    Layers = new List<BackgroundLayer>
                    {
                        new BackgroundLayer("walls", "res://Assets/backgrounds/castle/walls_1920.png", 
                            new Vector2(0.2f, 0.2f), new Vector2(1920, 0)),
                        new BackgroundLayer("interior", "res://Assets/backgrounds/castle/interior_3840.png", 
                            new Vector2(0.5f, 0.5f), new Vector2(3840, 0)),
                        new BackgroundLayer("pillars", "res://Assets/backgrounds/castle/pillars_3840.png", 
                            new Vector2(1.1f, 1.0f), new Vector2(3840, 0))
                    }
                }
            };
        }

        public void SetupParallaxBackground(string levelName)
        {
            // Clear existing background if present
            if (_parallaxBg != null)
            {
                _parallaxBg.QueueFree();
            }

            // Create new parallax background
            _parallaxBg = new ParallaxBackground();
            _parallaxBg.Name = "ParallaxBackground";
            AddChild(_parallaxBg);

            // Get level configuration
            if (!_levelConfigs.TryGetValue(levelName, out var config))
            {
                GD.PrintErr($"No background configuration found for level: {levelName}");
                return;
            }

            // Create all layers
            foreach (var layerConfig in config.Layers)
            {
                var layer = CreateParallaxLayer(layerConfig);
                if (layer != null)
                {
                    _parallaxBg.AddChild(layer);
                }
            }

            GD.Print($"Parallax background setup complete for {levelName}");
        }

        private ParallaxLayer CreateParallaxLayer(BackgroundLayer config)
        {
            var layer = new ParallaxLayer();
            layer.Name = $"{config.Name}Layer";
            layer.MotionScale = config.MotionScale;
            layer.MotionMirroring = config.MotionMirroring;

            // Load texture
            var texture = GD.Load<Texture2D>(config.TexturePath);
            if (texture == null)
            {
                GD.PrintErr($"Failed to load background texture: {config.TexturePath}");
                return null;
            }

            // Create sprite
            var sprite = new Sprite2D();
            sprite.Name = "Sprite";
            sprite.Texture = texture;
            sprite.Centered = false;
            
            // CRITICAL: Set texture filter to Nearest for pixel art
            sprite.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;

            layer.AddChild(sprite);

            return layer;
        }

        public void SwitchLevel(string newLevel)
        {
            if (_levelConfigs.ContainsKey(newLevel))
            {
                CurrentLevel = newLevel;
                SetupParallaxBackground(newLevel);
            }
            else
            {
                GD.PrintErr($"Unknown level: {newLevel}");
            }
        }
    }

    /// <summary>
    /// Configuration for a single background layer.
    /// </summary>
    public class BackgroundLayer
    {
        public string Name { get; set; }
        public string TexturePath { get; set; }
        public Vector2 MotionScale { get; set; }
        public Vector2 MotionMirroring { get; set; }

        public BackgroundLayer(string name, string texturePath, Vector2 motionScale, Vector2 motionMirroring)
        {
            Name = name;
            TexturePath = texturePath;
            MotionScale = motionScale;
            MotionMirroring = motionMirroring;
        }
    }

    /// <summary>
    /// Configuration for all layers in a level.
    /// </summary>
    public class LevelBackgroundConfig
    {
        public List<BackgroundLayer> Layers { get; set; } = new List<BackgroundLayer>();
    }
}
```

---

### Advanced TileMap System with Auto-Configuration

```csharp
using Godot;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    /// <summary>
    /// Advanced tilemap system with automatic tileset configuration.
    /// Handles multiple tilesets and provides level-building utilities.
    /// </summary>
    public partial class AdvancedTileMapSystem : Node2D
    {
        [Export] public string TilesetName { get; set; } = "jungle";
        [Export] public bool GenerateExampleLevel { get; set; } = false;
        [Export] public int TileSize { get; set; } = 16;
        
        private TileMap _tileMap;
        private TileSet _tileSet;
        private Dictionary<string, TileSetConfiguration> _tilesetConfigs;

        public override void _Ready()
        {
            InitializeTilesetConfigurations();
            CreateTileMapSystem();
            
            if (GenerateExampleLevel)
            {
                BuildExampleLevel();
            }
        }

        private void InitializeTilesetConfigurations()
        {
            _tilesetConfigs = new Dictionary<string, TileSetConfiguration>
            {
                ["jungle"] = new TileSetConfiguration
                {
                    TexturePath = "res://Assets/tilesets/jungle_tileset_16x16.png",
                    TileSize = new Vector2I(16, 16),
                    HasPhysics = true,
                    HasNavigation = false
                },
                ["dungeon"] = new TileSetConfiguration
                {
                    TexturePath = "res://Assets/tilesets/dungeon_tileset_16x16.png",
                    TileSize = new Vector2I(16, 16),
                    HasPhysics = true,
                    HasNavigation = false
                },
                ["castle"] = new TileSetConfiguration
                {
                    TexturePath = "res://Assets/tilesets/castle_tileset_16x16.png",
                    TileSize = new Vector2I(16, 16),
                    HasPhysics = true,
                    HasNavigation = false
                }
            };
        }

        private void CreateTileMapSystem()
        {
            _tileMap = new TileMap();
            _tileMap.Name = "LevelTileMap";
            
            // Get tileset configuration
            if (!_tilesetConfigs.TryGetValue(TilesetName, out var config))
            {
                GD.PrintErr($"Unknown tileset: {TilesetName}");
                return;
            }

            // Create tileset
            _tileSet = CreateAdvancedTileSet(config);
            
            if (_tileSet != null)
            {
                _tileMap.TileSet = _tileSet;
                AddChild(_tileMap);
                GD.Print($"TileMap created with {TilesetName} tileset");
            }
        }

        private TileSet CreateAdvancedTileSet(TileSetConfiguration config)
        {
            var texture = GD.Load<Texture2D>(config.TexturePath);
            if (texture == null)
            {
                GD.PrintErr($"Failed to load tileset: {config.TexturePath}");
                return null;
            }

            var tileSet = new TileSet();
            
            // Create atlas source
            var atlasSource = new TileSetAtlasSource();
            atlasSource.Texture = texture;
            atlasSource.TextureRegionSize = config.TileSize;
            atlasSource.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;

            // Add to tileset
            int sourceId = 0;
            tileSet.AddSource(atlasSource, sourceId);

            // Configure physics if needed
            if (config.HasPhysics)
            {
                tileSet.AddPhysicsLayer();
                ConfigureTilePhysics(atlasSource, sourceId);
            }

            // Configure navigation if needed
            if (config.HasNavigation)
            {
                tileSet.AddNavigationLayer();
            }

            return tileSet;
        }

        private void ConfigureTilePhysics(TileSetAtlasSource atlasSource, int sourceId)
        {
            // Configure collision for common tile types
			// In production, you'd iterate through all tiles and configure each
			
			// Example: Grass top tile at (0,0)
			var tileData = atlasSource.GetTileData(new Vector2I(0, 0), 0);
			if (tileData != null)
			{
				// Full tile collision
				var polygon = new Vector2[]
				{
					new Vector2(0, 0),
					new Vector2(TileSize, 0),
					new Vector2(TileSize, TileSize),
					new Vector2(0, TileSize)
				};
				
				tileData.SetCollisionPolygonsCount(0, 1);
				tileData.SetCollisionPolygonPoints(0, 0, polygon);
			}
		}

		private void BuildExampleLevel()
		{
			// Ground platform
			PaintHorizontalPlatform(new Vector2I(0, 30), 60);
			
			// Floating platforms
			PaintHorizontalPlatform(new Vector2I(10, 27), 8);
			PaintHorizontalPlatform(new Vector2I(22, 24), 10);
			PaintHorizontalPlatform(new Vector2I(38, 21), 8);
			PaintHorizontalPlatform(new Vector2I(52, 18), 12);
			
			GD.Print("Example level generated");
		}

		public void PaintHorizontalPlatform(Vector2I startPos, int width)
		{
			for (int i = 0; i < width; i++)
			{
				Vector2I tilePos = new Vector2I(startPos.X + i, startPos.Y);
				
				// Use appropriate tile based on position
				Vector2I atlasCoords;
				if (i == 0)
					atlasCoords = new Vector2I(0, 0); // Left edge
				else if (i == width - 1)
					atlasCoords = new Vector2I(2, 0); // Right edge
				else
					atlasCoords = new Vector2I(1, 0); // Middle

				_tileMap.SetCell(0, tilePos, 0, atlasCoords);
			}
		}

		public void PaintVerticalWall(Vector2I startPos, int height)
		{
			for (int i = 0; i < height; i++)
			{
				Vector2I tilePos = new Vector2I(startPos.X, startPos.Y - i);
				_tileMap.SetCell(0, tilePos, 0, new Vector2I(1, 1)); // Wall tile
			}
		}

		public void ClearArea(Vector2I topLeft, Vector2I bottomRight)
		{
			for (int x = topLeft.X; x <= bottomRight.X; x++)
			{
				for (int y = topLeft.Y; y <= bottomRight.Y; y++)
				{
					_tileMap.EraseCell(0, new Vector2I(x, y));
				}
			}
		}
	}

	/// <summary>
	/// Configuration for tileset creation.
	/// </summary>
	public class TileSetConfiguration
	{
		public string TexturePath { get; set; }
		public Vector2I TileSize { get; set; }
		public bool HasPhysics { get; set; }
		public bool HasNavigation { get; set; }
	}
}
```

---

## 📊 LEVEL DESIGN GUIDELINES

### Platform Spacing Calculations

Based on PlayerController.cs jump mechanics:
- **Jump velocity:** -400 px/s
- **Gravity:** 980 px/s²
- **Maximum jump height:** ~408 pixels (~25 tiles at 16px)

```csharp
// Calculate reachable height
// h = (v² / 2g) where v = 400, g = 980
// h = (400² / (2 × 980)) = 160000 / 1960 ≈ 81.6 pixels ≈ 5 tiles

// With running momentum (speed multiplier 1.5):
// Horizontal reach increases significantly
```

**Recommended Gaps:**

| Difficulty | Horizontal Gap | Vertical Gap | Tiles (16px) |
|------------|---------------|--------------|--------------|
| **Easy** | 160-200px | 64-80px | 10-12 horiz, 4-5 vert |
| **Medium** | 240-280px | 96-112px | 15-17 horiz, 6-7 vert |
| **Hard** | 320-360px | 128-144px | 20-22 horiz, 8-9 vert |
| **Expert** | 400-480px | 160-192px | 25-30 horiz, 10-12 vert |

---

### Level Size Recommendations

```csharp
// Level dimensions for 1920×1080 viewport

// Level 1 (Jungle) - Tutorial/Short
// Total width: 5760px (3× screen width)
// Total height: 1620px (1.5× screen height)
// Platform count: 15-20
// Enemy count: 10 Demonteks
// Estimated play time: 5-7 minutes

// Level 2 (Dungeon) - Medium
// Total width: 7680px (4× screen width)
// Total height: 2160px (2× screen height)
// Platform count: 25-30
// Enemy count: 15 Guards
// Estimated play time: 8-10 minutes

// Level 3 (Castle) - Long + Bosses
// Total width: 11520px (6× screen width)
// Total height: 3240px (3× screen height) for multi-floor
// Platform count: 40+
// Enemy count: 20+ mixed types
// Boss arenas: 2 large flat areas (960×270px each)
// Estimated play time: 15-20 minutes
```

---

## 🎯 COMPLETE ASSET WORKFLOW

### Week 1: Tilesets
- **Day 1:** Generate all 3 tilesets with MCP
- **Day 2:** Download and organize files
- **Day 3:** Import into Godot, create TileSet resources

### Week 2: Backgrounds (Your Choice - Option 1)
- **Day 1:** Generate Jungle backgrounds (4 layers)
- **Day 2:** Generate Dungeon backgrounds (3 layers)
- **Day 3:** Generate Castle backgrounds (3 layers)
- **Day 4:** Import all, test parallax

### Week 3: Level Building
- **Day 1-2:** Build Level 1 (Jungle)
- **Day 3-4:** Build Level 2 (Dungeon)
- **Day 5-7:** Build Level 3 (Castle)

**Total:** 3 weeks to complete all level assets and implementation

---

## 💡 EXPERT TIPS

**1. Test Parallax Immediately**
```csharp
// Create a simple test scene with just sky layer
// Verify scrolling works before adding more layers
```

**2. Use Consistent Naming**
```
Assets/backgrounds/{level}/
- sky_{width}.png
- mid_{width}.png
- foreground_{width}.png
```

**3. Check Texture Filter**
```csharp
// ALWAYS set to Nearest for pixel art
sprite.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
```

**4. Optimize Layer Sizes**
```
Far layers: 1920px (seamless loop is sufficient)
Mid layers: 3840px (2× for smooth scroll)
Near layers: 5760px (3× for dramatic depth)
```

---

## 🎊 YOU'RE READY TO BUILD LEVELS!

You now have:
✅ All MCP tileset generation calls
✅ All 3 background generation options
✅ Complete C# parallax system
✅ Advanced TileMap utilities
✅ Platform spacing guidelines
✅ Level size recommendations

**Start building your game levels! 🗺️🚀**
