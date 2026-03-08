# 🗺️ Godot TileMap Setup Guide

## ✅ TileSets Created

All three TileSet resources have been created:

- **Jungle**: `Assets/tilesets/jungle_tileset_16x16.tres`
- **Dungeon**: `Assets/tilesets/dungeon_tileset_16x16.tres`
- **Castle**: `Assets/tilesets/castle_tileset_16x16.tres`

Each TileSet:
- 16×16 pixel tiles
- 16 tiles total (4×4 grid)
- Physics collision enabled
- Platform layer collision

## 🎮 Setting Up TileMap in Godot

### Method 1: Using TileMapManager Script (Recommended)

1. **Add TileMapManager to your level:**
   ```
   Right-click level scene → Add Child Node → TileMapManager
   ```

2. **Configure TileSet path:**
   ```
   Inspector → TileSetPath → res://Assets/tilesets/jungle_tileset_16x16.tres
   ```

3. **Call build methods in _Ready():**
   ```csharp
   // In your level script
   public override void _Ready()
   {
       var tileMapManager = GetNode<TileMapManager>("TileMap");
       tileMapManager.TileSetPath = "res://Assets/tilesets/jungle_tileset_16x16.tres";
       tileMapManager.BuildExampleJungleLevel();
   }
   ```

### Method 2: Manual TileMap Setup

1. **Create TileMap node:**
   - Right-click level scene → Add Child → TileMap

2. **Assign TileSet:**
   - Inspector → Tile Set → Load jungle_tileset_16x16.tres

3. **Select tile to paint:**
   - Select TileMap node
   - Click "Select a Tile" in bottom panel
   - Choose tile from atlas (0,0 for left edge, 1,0 for middle, 2,0 for right edge)

4. **Paint tiles:**
   - Click and drag on the level to paint

## 🎨 Tile Coordinate Reference

### Jungle Tileset (16 tiles, 4×4 grid):

```
(0,0)  (1,0)  (2,0)  (3,0)  ← Platform tiles (left, middle, right, single)
(0,1)  (1,1)  (2,1)  (3,1)  ← Variation tiles
(0,2)  (1,2)  (2,2)  (3,2)  ← Variation tiles
(0,3)  (1,3)  (2,3)  (3,3)  ← Variation tiles
```

**Common usage:**
- `(0,0)` - Left edge of platform
- `(1,0)` - Middle of platform (repeatable)
- `(2,0)` - Right edge of platform
- `(3,0)` - Single tile platform

## 📏 Pixel-to-Tile Conversion

```
Tile Position = Pixel Position / Tile Size
Tile Size = 16 pixels

Example:
- Player at (240, 500) → Tile (15, 31)
- Platform at (480, 400) → Tile (30, 25)
```

## 🔧 Physics Configuration

TileSets already configured with:
- **Collision Layer**: 4 (Platforms)
- **Collision Mask**: 1 (Player)

Player and enemies will collide with tiles automatically.

## 🎯 Level Building Tips

### Platform Spacing

Based on player jump height (400 px/s):
- **Easy gap**: 2-3 tiles (32-48 px)
- **Medium gap**: 4-5 tiles (64-80 px)
- **Hard gap**: 6-7 tiles (96-112 px)

### Creating a Basic Platform

```csharp
// Using TileMapManager
PaintHorizontalPlatform(new Vector2I(10, 25), 8);
```

Or manually in Godot editor:
1. Select TileMap node
2. Select tile (1,0) for middle
3. Paint 6 tiles in a row
4. Select tile (0,0) for left edge, paint first tile
5. Select tile (2,0) for right edge, paint last tile

## 📝 Updating Level Scenes

### Current Levels (with placeholder platforms)

Update these scenes to use TileMap:
- `Scenes/Levels/Level1_Jungle.tscn` → Use jungle tileset
- `Scenes/Levels/Level2_Dungeon.tscn` → Use dungeon tileset
- `Scenes/Levels/Level3_Castle.tscn` → Use castle tileset

### Step-by-Step:

1. **Open level scene** in Godot editor
2. **Delete placeholder nodes:**
   - Remove all `Platforms` → `Platform*` nodes
3. **Add TileMap:**
   - Add Child → TileMap
4. **Assign TileSet:**
   - Inspector → Tile Set → Load appropriate .tres file
5. **Paint level:**
   - Use TileMap to create platforms
6. **Test collision:**
   - Press F5 to test
   - Verify player lands on tiles

## 🚀 Using TileMapManager Script

Available methods:

```csharp
// Paint horizontal platform
PaintHorizontalPlatform(Vector2I startTile, int width);

// Paint vertical wall
PaintVerticalWall(Vector2I startTile, int height);

// Paint ground from X to X at Y
PaintGround(int startX, int endX, int y);

// Clear area
ClearArea(Vector2I topLeft, Vector2I bottomRight);

// Build example levels
BuildExampleJungleLevel();
BuildExampleDungeonLevel();
BuildExampleCastleLevel();
```

## 🎨 Customizing TileSets

### Changing Collision

1. Open `Assets/tilesets/jungle_tileset_16x16.tres` in Godot
2. Select a tile
3. In Inspector → Physics → Polygon 0
4. Adjust collision polygon points if needed

### Adding Custom Tiles

1. Edit PNG in image editor
2. Save to `Assets/tilesets/`
3. Reimport in Godot (drag to editor)
4. TileSet will update automatically

## 📊 Quick Reference

| Task | Command/Shortcut |
|------|------------------|
| Paint tiles | Click and drag with TileMap selected |
| Erase tiles | Right-click or Shift+click |
| Select tile | Click tile in bottom panel |
| Move camera | Middle mouse drag |
| Zoom | Scroll wheel |
| Toggle grid | G |
| Toggle snap | Y |

## ⚠️ Common Issues

**Issue**: Tiles appear blurry
- **Fix**: Ensure `textures/canvas_textures/default_texture_filter=0` in project.godot

**Issue**: Player falls through tiles
- **Fix**: Check collision layers match (Platforms=4, Player=1)

**Issue**: Wrong tileset in level
- **Fix**: Verify TileSet path in TileMap node Inspector

## 🎯 Next Steps

1. ✅ TileSets created and configured
2. 🔄 Update level scenes to use TileMap
3. 🎨 Paint level layouts in Godot editor
4. 🧪 Test platform collision
5. 🌄 Add parallax backgrounds (when ready)

---

**For parallax backgrounds**: See `BACKGROUND_GENERATION_GUIDE.md` and `LEVEL_DESIGN_GUIDE.md`
