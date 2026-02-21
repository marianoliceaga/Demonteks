# 🎨 Background Generation Quick Reference

## 📋 Quick Start

1. **Run the tracker:** Double-click `run_tracker.bat`
2. **Copy prompts:** Open `AI_BACKGROUND_PROMPTS.md`
3. **Generate:** Use external AI tool
4. **Download:** Save to `Assets/backgrounds/{level}/`
5. **Verify:** Run tracker again

## 🛠️ Recommended Tools & Settings

### ChatGPT Plus (DALL-E 3)
**Copy prompt from document + add:**
```
Generate as pixel art, 1920x1080 resolution, seamless horizontal tiling
```

**Pros:** Easy to use, good quality
**Cons:** May take 2-3 tries for perfect result

---

### Midjourney
**Copy prompt + add parameters:**
```
--ar 16:9 --tile --style raw --v 6 --q 2
```

**For wider layers:**
- 1920×1080: `--ar 16:9`
- 3840×1080: `--ar 16:4.5`
- 5760×1080: `--ar 16:3`

**Pros:** Best pixel art style, excellent tiling
**Cons:** Requires Discord, paid subscription

---

### Scenario.com
**Settings:**
- Model: Pixel Art
- Mode: Seamless Tile
- Dimensions: Exact size (1920×1080, 3840×1080, 5760×1080)
- Guidance: 7-9

**Pros:** Free tier available, built for games
**Cons:** 500 character prompt limit

---

### Leonardo.ai
**Model:** Pixel Art Diffusion XL
**Settings:**
- Dimensions: 1920×1080 (closest available)
- Guidance Scale: 7-9
- Steps: 30-40

**Pros:** Free tier, good quality
**Cons:** May need upscaling

## 📁 File Naming Convention

Place downloads in:
```
Assets/backgrounds/
├── jungle/
│   ├── sky_1920x1080.png
│   ├── trees_3840x1080.png
│   ├── foliage_3840x1080.png
│   └── foreground_5760x1080.png
├── dungeon/
│   ├── wall_1920x1080.png
│   ├── pillars_3840x1080.png
│   └── chains_3840x1080.png
└── castle/
	├── walls_1920x1080.png
	├── interior_3840x1080.png
	└── pillars_3840x1080.png
```

## ⚡ Pro Tips

### Seamless Tiling Test
After downloading, verify seamless tiling:
1. Open in image editor (Aseprite, Photoshop, GIMP)
2. Duplicate image and place side-by-side
3. Check for visible seams
4. Use "Offset" filter if needed

### Color Palette Matching
Ensure consistent colors across layers:
- **Jungle:** Purple (#6A1B9A, #9C27B0), Green (#2E7D32)
- **Dungeon:** Dark Grey (#424242), Brown (#5D4037)
- **Castle:** Red (#B71C1C), Orange (#FF6F00)

### Godot Import Settings
When importing in Godot:
```
Import Tab:
- Compress: Lossless
- Filter: Nearest (CRITICAL for pixel art!)
- Mipmaps: Off
- Repeat: Disabled
- Fix Alpha Border: Enabled
```

## 📊 Progress Tracking

Total files needed: 10

**Jungle (4 files):**
- [ ] sky_1920x1080.png
- [ ] trees_3840x1080.png
- [ ] foliage_3840x1080.png
- [ ] foreground_5760x1080.png

**Dungeon (3 files):**
- [ ] wall_1920x1080.png
- [ ] pillars_3840x1080.png
- [ ] chains_3840x1080.png

**Castle (3 files):**
- [ ] walls_1920x1080.png
- [ ] interior_3840x1080.png
- [ ] pillars_3840x1080.png

## ⏱️ Time Estimates

| Tool | Time per Image | Total Time |
|------|---------------|------------|
| DALL-E 3 | 2-5 min | 20-50 min |
| Midjourney | 1-2 min | 10-20 min |
| Scenario.com | 3-5 min | 30-50 min |
| Leonardo.ai | 2-4 min | 20-40 min |

Plus iteration time: 1-2 hours for refinements

## 🎯 Recommended Order

**Day 1: Far Layers (easiest, single screen)**
1. Jungle sky
2. Dungeon wall
3. Castle walls

**Day 2: Mid Layers (2× width)**
4. Jungle trees
5. Jungle foliage
6. Dungeon pillars
7. Castle interior

**Day 3: Foreground Layers (wider/more detail)**
8. Jungle foreground (5760px - widest)
9. Dungeon chains
10. Castle pillars

## 🚀 After Download

Once all backgrounds are downloaded:

1. **Run tracker** to verify all files
2. **Import to Godot** with correct settings
3. **Test parallax** in Godot scene
4. **Fix seams** if any are visible
5. **Adjust colors** for consistency

## 💾 Backup

After completion, consider:
1. Backup entire `Assets/backgrounds/` folder
2. Upload to cloud storage (Google Drive, Dropbox)
3. Create Git LFS repository if tracking in Git

---

**Need help?** See detailed prompts in `AI_BACKGROUND_PROMPTS.md`
**Implementation guide:** See `LEVEL_DESIGN_GUIDE.md`
