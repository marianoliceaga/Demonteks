# 🚀 First Time Setup - Demonteks

## You successfully opened the project! Here's what to do next:

### Step 1: Build the C# Solution ✅

Godot will prompt you to build the C# solution. Click **"Build"**.

Alternatively, you can build manually:
```bash
# In Godot Editor: Project > Tools > C# > Build Solution
# Or from command line:
godot --build-solutions
```

### Step 2: Create Your First Scene

The project is currently set up with just the scripts and configuration. You need to create the game scenes.

#### Quick Test Scene (Already Created!)

Run `Scenes/Test/TestScene.tscn` to verify everything works:
1. Open `Scenes/Test/TestScene.tscn` in Godot
2. Press **F5** to run
3. You should see a label with instructions

#### Create the Player Scene

1. **Create new scene:**
   ```
   Scene > New Scene > 2D Scene
   Save as: Scenes/Characters/Player.tscn
   ```

2. **Add nodes:**
   ```
   Player (CharacterBody2D)
   ├── AnimatedSprite2D (for character sprite)
   ├── CollisionShape2D (for physics)
   ├── PlayerStats (Node)
   ├── PlayerCombat (Node2D)
   │   └── Hitbox (Area2D)
   │       └── CollisionShape2D
   └── Camera2D
   ```

3. **Attach the PlayerController script:**
   - Select the root "Player" node
   - Click the script icon (or press Ctrl+A)
   - Navigate to `Scripts/Player/PlayerController.cs`
   - Click "Open"

4. **Attach other scripts:**
   - Select "PlayerStats" node → Attach `Scripts/Player/PlayerStats.cs`
   - Select "PlayerCombat" node → Attach `Scripts/Player/PlayerCombat.cs`

5. **Configure NodePaths in Inspector:**
   - Select "Player" node
   - In Inspector, set:
     - `AnimatedSpritePath` = `AnimatedSprite2D`
     - `CollisionShapePath` = `CollisionShape2D`
   
   - Select "PlayerCombat" node
   - Set:
     - `HitboxPath` = `Hitbox`

6. **Add to group:**
   - Select "Player" node
   - Node tab > Groups > Add "player"

### Step 3: Generate Assets with PixelLab

Follow the **ASSET_GENERATION_GUIDE.md** to create all character sprites and backgrounds.

**Priority order:**
1. Explorer character (player sprite)
2. Normal Demontek (basic enemy)
3. Jungle backgrounds (Level 1)

### Step 4: Import Assets into Godot

1. **Copy generated assets to project:**
   ```bash
   cp -r generated_assets/characters/ Assets/characters/
   cp -r generated_assets/backgrounds/ Assets/backgrounds/
   ```

2. **Configure import settings:**
   - Select all sprite assets in FileSystem
   - Import tab:
     - Filter: **Nearest** (CRITICAL for pixel art!)
     - Mipmaps: Off
     - Repeat: Disabled (for characters)
   - Click **Reimport**

3. **Set up AnimatedSprite2D:**
   - Select AnimatedSprite2D in Player scene
   - In Inspector > Animation > Frames
   - Click "New SpriteFrames"
   - Add animations (idle, walk, run, jump, attack, hurt, death)
   - Load sprite sheets for each animation

### Step 5: Create a Test Level

1. **Create new scene:**
   ```
   Scene > New Scene > 2D Scene
   Save as: Scenes/Levels/TestLevel.tscn
   ```

2. **Add nodes:**
   ```
   TestLevel (Node2D)
   ├── ParallaxBackground (for backgrounds)
   │   ├── ParallaxLayer (sky)
   │   │   └── Sprite2D
   │   └── ParallaxLayer (foreground)
   │       └── Sprite2D
   ├── TileMap (for platforms)
   └── Player (instance of Player.tscn)
   ```

3. **Add platforms:**
   - Create TileMap or use StaticBody2D with CollisionShape2D
   - Place platforms for player to walk on

4. **Test:**
   - Set as main scene: Project > Project Settings > Run > Main Scene
   - Press **F5** to run

### Step 6: Create Enemies

Follow the same process as Player scene, but using `DemontekAI.cs`:

1. Create `Scenes/Characters/Enemies/Demontek.tscn`
2. Structure:
   ```
   Demontek (CharacterBody2D)
   ├── AnimatedSprite2D
   ├── CollisionShape2D
   ├── DetectionArea (Area2D)
   │   └── CollisionShape2D
   └── AttackArea (Area2D)
       └── CollisionShape2D
   ```
3. Attach `Scripts/Enemy/DemontekAI.cs` to root node
4. Set NodePaths in Inspector
5. Add to "enemies" group

### Common Issues & Fixes

#### "Cannot find type or namespace"
**Solution:** Build the C# solution first:
```
Project > Tools > C# > Build Solution
```

#### "Blurry sprites"
**Solution:** 
1. Select sprites in FileSystem
2. Import tab > Filter: **Nearest**
3. Reimport
4. Also check: Project Settings > Rendering > Textures > Default Texture Filter: Nearest

#### "GameManager not found"
**Solution:** GameManager and SaveManager are autoloads. If you see errors:
1. Check they're in Project Settings > Autoload
2. They load as singletons automatically
3. Access via `GameManager.Instance` in code

#### "MobileControls errors"
**Solution:** MobileControls.tscn doesn't exist yet. It's commented out in autoload. Create it when you need mobile controls, or keep it commented out for desktop-only testing.

### Recommended Development Order

1. ✅ Build C# solution
2. ✅ Create Player scene
3. ✅ Generate Explorer sprite with PixelLab
4. ✅ Import and set up animations
5. ✅ Create test level with platforms
6. ✅ Test player movement (WASD + jump)
7. ✅ Create Demontek enemy
8. ✅ Test combat
9. ✅ Build Level 1 (Jungle)
10. ✅ Continue following QUICK_START.md

### Next Steps

Once you have:
- Player scene with working movement
- At least one enemy
- A test level with platforms

Then you can start building the full game following the **README.md** roadmap!

---

## 📚 Documentation Guide

- **README.md** - Complete game overview, story, features
- **QUICK_START.md** - Full development workflow
- **ASSET_GENERATION_GUIDE.md** - PixelLab prompts for all assets
- **This file** - Just getting started

---

## 🆘 Need Help?

If you encounter issues:
1. Check the **Troubleshooting** section in QUICK_START.md
2. Verify all NodePaths are set correctly in Inspector
3. Ensure C# solution is built
4. Check Godot console for specific error messages

---

**Good luck! The codebase is solid and ready for you to build upon! 🎮**
