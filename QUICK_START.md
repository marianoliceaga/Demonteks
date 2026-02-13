# Demonteks - Quick Start Guide

## 🚀 Getting Started

### Prerequisites Checklist

- [ ] **Godot 4.3+** with .NET support installed
- [ ] **.NET SDK 8.0+** installed
- [ ] **Git** for version control
- [ ] **PixelLab MCP** access for asset generation
- [ ] **Platform SDKs** (optional, for export):
  - Android Studio (Android build)
  - Xcode (iOS build, macOS only)

---

## 📦 Installation

### 1. Clone/Download Project

```bash
git clone https://github.com/yourusername/demonteks.git
cd demonteks
```

Or download and extract the ZIP file.

### 2. Open in Godot

```bash
# Option A: Command line
godot -e project.godot

# Option B: Godot Editor
# Open Godot > Import > Navigate to demonteks/project.godot
```

### 3. Build C# Solution

When Godot opens, it will prompt to build the C# solution. Click **Build**.

Alternatively, from command line:
```bash
godot --build-solutions
```

---

## 🎨 Generating Assets

### Using PixelLab MCP

1. **Read the Asset Generation Guide:**
   ```
   Open: ASSET_GENERATION_GUIDE.md
   ```

2. **Generate Characters (6 sprites):**
   - Use PixelLab prompts from guide
   - Save to `/assets/characters/[character_name]/`
   - Generate base sprite + animation sheets

3. **Generate Backgrounds (8 scenes):**
   - Create parallax layers where needed
   - Save to `/assets/backgrounds/`

4. **Verify Asset Structure:**
   ```
   /assets
     /characters
       /explorer
         explorer_base.png
         explorer_idle_sheet.png
         explorer_walk_sheet.png
         ...
     /backgrounds
       bg_jungle_layer1.png
       bg_jungle_layer2.png
       ...
   ```

---

## 🔧 Project Configuration

### Import Settings

After importing assets into Godot:

1. **Select all sprite assets**
2. **Import tab settings:**
   - Filter: **Nearest** (critical for pixel art!)
   - Mipmaps: **Off**
   - Repeat: **Disabled**
3. Click **Reimport**

### Scene Setup

#### Player Scene
1. Open `Scenes/Characters/Player.tscn`
2. Add AnimatedSprite2D
3. Import sprite sheets
4. Configure animations:
   - idle: 4 frames @ 60ms
   - walk: 8 frames @ 80ms
   - run: 6 frames @ 60ms
   - jump: 4 frames
   - attack_knife: 6 frames @ 50ms
   - hurt: 3 frames @ 100ms
   - death: 5 frames @ 120ms

#### Enemy Scene Template
1. Open `Scenes/Characters/Enemies/Demontek.tscn`
2. Duplicate for each enemy type
3. Configure collision shapes
4. Set up attack areas

---

## 🎮 Testing the Game

### Play from Editor

1. **Select main scene:**
   ```
   Project > Project Settings > Application > Run
   Main Scene: res://Scenes/MainMenu.tscn
   ```

2. **Run game:**
   ```
   Press F5 or click Play button
   ```

### Test Mobile Controls

1. Enable mobile emulation:
   ```
   Debug > Deploy with Remote Debug > Android/iOS
   ```

2. Or test on device:
   ```
   Project > Export > [Platform] > Export Project
   Install APK/IPA on device
   ```

---

## 📱 Exporting for Mobile

### Android Export

1. **Install Android Build Template:**
   ```
   Project > Install Android Build Template
   ```

2. **Configure Export Preset:**
   ```
   Project > Export > Add > Android
   
   Settings:
   - Min SDK: 26
   - Target SDK: 34
   - Architectures: arm64-v8a, armeabi-v7a
   - Screen Orientation: Landscape
   - Permissions: (none required)
   ```

3. **Generate Keystore (for release):**
   ```bash
   keytool -genkey -v -keystore demonteks.keystore \
     -alias demonteks -keyalg RSA -keysize 2048 -validity 10000
   ```

4. **Export APK:**
   ```
   Project > Export > Android > Export Project
   Save as: demonteks.apk
   ```

### iOS Export (macOS only)

1. **Configure Export Preset:**
   ```
   Project > Export > Add > iOS
   
   Settings:
   - Bundle ID: com.yourstudio.demonteks
   - Version: 1.0.0
   - Min iOS Version: 12.0
   - Target Device: iPhone/iPad
   - Orientation: Landscape
   ```

2. **Export Xcode Project:**
   ```
   Project > Export > iOS > Export Project
   Save as: demonteks_ios/
   ```

3. **Build in Xcode:**
   ```bash
   open demonteks_ios/demonteks.xcodeproj
   # Configure signing & capabilities
   # Build for device or App Store
   ```

---

## 🧪 Development Workflow

### Recommended Development Order

1. **Week 1: Core Systems**
   - ✅ Player movement working
   - ✅ Enemy AI functional
   - ✅ Combat system tested

2. **Week 2: Asset Integration**
   - Generate all sprites in PixelLab
   - Import into Godot
   - Configure animations

3. **Week 3: Level 1 (Jungle)**
   - Build platforms
   - Place enemies
   - Test gameplay loop

4. **Week 4: Levels 2-3**
   - Dungeon layout
   - Castle design
   - Checkpoint system

5. **Week 5: Boss Fights**
   - Goliath mechanics
   - King Demontek phases
   - Pacifist route

6. **Week 6: Polish & Mobile**
   - UI refinement
   - Touch controls
   - Performance optimization

7. **Week 7: Testing & Balancing**
   - Playtest all levels
   - Balance difficulty
   - Fix bugs

8. **Week 8: Launch Prep**
   - App store assets
   - Descriptions
   - Submit for review

---

## 🐛 Troubleshooting

### Common Issues

**Problem:** C# scripts not compiling
**Solution:**
```bash
# Rebuild solution
godot --build-solutions

# Clear build cache
rm -rf .godot/mono/
godot --build-solutions
```

**Problem:** Mobile controls not appearing
**Solution:**
- Check GameManager is detecting mobile platform
- Verify MobileControls.tscn is in autoload
- Ensure ShowControls = true

**Problem:** Pixel art looks blurry
**Solution:**
- Set texture filter to **Nearest**
- Enable pixel snapping in project settings
- Check viewport stretch mode is "canvas_items"

**Problem:** Performance issues on mobile
**Solution:**
- Reduce max FPS to 30 in battery saver
- Disable particle effects
- Use sprite batching
- Profile with Godot profiler

**Problem:** Save file not loading
**Solution:**
- Check file exists: `user://demonteks_save.json`
- Verify JSON format is valid
- Ensure SaveManager is in autoload

---

## 📊 Performance Optimization

### Mobile Performance Checklist

- [ ] Texture compression enabled (ETC2/PVRTC)
- [ ] Sprite batching configured (max 16 commands)
- [ ] Pixel snapping enabled
- [ ] Max FPS set to 60 (30 in battery saver)
- [ ] Particle effects limited
- [ ] Audio streams compressed
- [ ] No shader effects on low-end devices
- [ ] Collision shapes optimized

### Profiling

```
Debug > Start Profiling
Monitor:
- FPS
- Physics time
- Render time
- Memory usage
```

Target: 16ms frame time (60 FPS) or 33ms (30 FPS)

---

## 🔒 Security & Privacy

### Data Collection
This game collects **no personal data**.

### Permissions (Android)
- No permissions required
- All data stored locally

### Cloud Save (Optional)
- Uses Google Play Games / Game Center APIs
- User must opt-in
- Only saves game progress

---

## 📝 Version History

### v0.1.0-alpha (Current)
- Core player movement system
- Basic enemy AI
- Combat system with 4 weapons
- Save/load functionality
- Mobile touch controls
- 3 levels (in progress)

### Planned for v0.2.0-beta
- All assets generated
- Boss fights implemented
- Mercy system complete
- Multiple endings

### Planned for v1.0.0 (Release)
- Full game content
- Polished UI
- Sound effects & music
- App store submission

---

## 🎯 Next Steps

1. **Generate Assets:**
   - Follow ASSET_GENERATION_GUIDE.md
   - Create all 6 characters
   - Create all 8 backgrounds

2. **Build First Level:**
   - Open Scenes/Levels/Level1_Jungle.tscn
   - Add background layers
   - Place platforms
   - Add 10 Demontek enemies

3. **Test Gameplay Loop:**
   - Run game
   - Test controls (desktop + mobile)
   - Verify combat works
   - Test death/respawn

4. **Iterate & Polish:**
   - Adjust difficulty
   - Refine animations
   - Add sound effects
   - Optimize performance

---

## 📚 Additional Resources

### Godot Documentation
- [C# Basics](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/index.html)
- [2D Movement](https://docs.godotengine.org/en/stable/tutorials/2d/2d_movement.html)
- [Mobile Export](https://docs.godotengine.org/en/stable/tutorials/export/index.html)

### Pixel Art Resources
- [Lospec Palette List](https://lospec.com/palette-list)
- [Pixel Art Tutorials](https://www.pixilart.com/blog/pixel-art-tutorials)
- [Aseprite](https://www.aseprite.org/) (recommended tool)

### Game Design
- [Undertale Design Analysis](https://www.youtube.com/watch?v=RWQcuBigOj0)
- [2D Platformer Design](https://www.gamedeveloper.com/design/in-depth-2d-platformer-character-controller)

---

## 💡 Tips for Success

1. **Start Small:** Get one level perfect before building all three
2. **Test Early:** Play your game constantly during development
3. **Mobile First:** Test on actual devices, not just emulator
4. **Iterate on Feedback:** Playtest with others, watch them play
5. **Polish Matters:** Small details (animations, sounds) make a huge difference
6. **Backup Often:** Commit to Git frequently
7. **Enjoy the Process:** Game development is a marathon, not a sprint

---

**Ready to begin? Start with:**
```bash
# 1. Open project
godot -e project.godot

# 2. Review ASSET_GENERATION_GUIDE.md

# 3. Generate first character sprite (Explorer)

# 4. Build and test!
```

Good luck, and have fun creating Demonteks! 🎮🚀
