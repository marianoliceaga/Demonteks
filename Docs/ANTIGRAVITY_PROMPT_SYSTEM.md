# Demonteks — Antigravity Prompt System

> **Master reference of prompts** for iteratively building the Demonteks 2D platformer in **Godot 4.6 C#** using Google's Antigravity AI coding assistant.
> Skip cinematic/story-telling sequences (those will use Google Veo later).

---

## Table of Contents

1. [Project Overview & Context](#1-project-overview--context)
2. [Phase 0 — Fix Missing Scene Files (Player & Enemies)](#2-phase-0--fix-missing-scene-files)
3. [Phase 1 — SpriteFrames & Animation Wiring](#3-phase-1--spriteframes--animation-wiring)
4. [Phase 2 — Level 1: Jungle Canopy](#4-phase-2--level-1-jungle-canopy)
5. [Phase 3 — Level 2: Dungeon Escape](#5-phase-3--level-2-dungeon-escape)
6. [Phase 4 — Level 3: Castle Infiltration](#6-phase-4--level-3-castle-infiltration)
7. [Phase 5 — HUD, Menus & Game Flow](#7-phase-5--hud-menus--game-flow)
8. [Phase 6 — Polish, Audio & QA](#8-phase-6--polish-audio--qa)
9. [Phase 7 — Iterative Bug-Fix & Expansion](#9-phase-7--iterative-bug-fix--expansion)

---

## 1. Project Overview & Context

**Paste this block at the start of every new Antigravity conversation** so the AI has full context.

````text
PROMPT 0 — PROJECT CONTEXT (paste at the start of each session)

I'm building a 2D side-scrolling platformer called "Demonteks" in Godot 4.6 with C#.
The project lives at D:\Demonteks.

STORY SUMMARY (skip cinematics for now):
An explorer wakes up on an alien planet with a violet sky and giant jungle plants.
He encounters small reddish alien demons called "Demonteks" (they say "tek").
The explorer resembles Indiana Jones (no hat/whip, uses a knife).

- Level 1 (Jungle): Fight 10 Demonteks jumping across treetop platforms. Weapons: knife.
- Level 2 (Dungeon): Captured and jailed. Escape through a tunnel, neutralize guards,
  find ancient weapons in hidden ruins. Respawn at dungeon cell on death.
- Level 3 (Castle): Infiltrate the castle from the back.
  Floor 1: Normal Demonteks. Floor 2: Mutant Demonteks (upgrade to Demontek weapons).
  Floor 3: Goliath boss (Demontek-monster fusion). Terrace: King Demontek final boss.
  Kill the King → portal to Earth opens → game won.

CONTROLS: WASD movement (A=left, D=right, W=jump, S=crouch), E=attack, F=interact,
  Shift=run, Q/Z=cycle weapons, Space=alt jump, Esc=pause.

DAMAGE TABLE:
  Normal Demontek: 5 HP | Guard: 5 HP | Mutant: 10 HP | Goliath: 20 HP | King: 30 HP
  Player starts with 100 HP.

WEAPONS (unlocked progressively):
  Part 1: Knife
  Part 2: Machete, Sword (found in ancient ruins)
  Part 3: DemontekSaber, Sawtooth, AlienSword (looted from defeated Mutants)

EXISTING PROJECT STRUCTURE:
  Assets/characters/{explorer,demontek,guard_demontek,mutant_demontek,goliath,king_demontek}/
    animations/{breathing-idle,cross-punch,walking-8-frames,running-6-frames,
                jumping-1,falling-back-death,taking-punch}/
    {east,west,south,north}/frame_000.png ... frame_00N.png
  Assets/tilesets/{jungle,dungeon,castle}_tileset_16x16.{png,tres,json}
  Assets/backgrounds/jungle/{sky,mid,foreground}_*.png
  Assets/backgrounds/{castle,dungeon}/

  Scripts/Player/{PlayerController.cs, PlayerCombat.cs, PlayerStats.cs}
  Scripts/Enemy/DemontekAI.cs
  Scripts/Management/{GameManager.cs, SaveManager.cs}
  Scripts/Level/TileMapManager.cs
  Scripts/Utils/{Checkpoint.cs, WeaponPickup.cs}
  Scripts/UI/{GameOverController.cs, PauseMenuController.cs, VictoryController.cs, MobileControls.cs}

  Scenes/Levels/{Level1_Jungle.tscn, Level2_Dungeon.tscn, Level3_Castle.tscn}
  Scenes/UI/{MainMenu.tscn, HUD.tscn, PauseMenu.tscn, GameOver.tscn, Victory.tscn, MobileControls.tscn}
  Scenes/Prefabs/{Checkpoint.tscn, Platform.tscn, WeaponPickup.tscn}

  Autoloads: GameManager, SaveManager, MobileControls
  Physics layers: Player(1), Enemies(2), World(3), Platforms(4), Items(5), Hazards(6), PlayerHitbox(7), EnemyHitbox(8)
  Viewport: 480×270 (canvas_items stretch), pixel-perfect rendering, nearest-neighbor filtering.

ANIMATION MAPPING (for this 2D side-scroller, use east/west directions only):
  breathing-idle → "idle"
  walking-8-frames → "walk"
  running-6-frames → "run"
  jumping-1 → "jump"
  cross-punch → "attack"
  taking-punch → "hurt"
  falling-back-death → "death"

Keep all code in C# with proper namespaces (Demonteks.Player, Demonteks.Enemy, etc.).
Always use pixel-perfect settings (nearest-neighbor filtering on textures).
````

---

## 2. Phase 0 — Fix Missing Scene Files

The levels reference `Player.tscn` and enemy `.tscn` files that need to exist with proper node structure.

### Prompt 0-A: Create Player.tscn

````text
PROMPT 0-A — CREATE PLAYER SCENE

The level scenes reference "res://Scenes/Characters/Player.tscn" but the file is missing.
Create Player.tscn as a CharacterBody2D scene with this node tree:

Player (CharacterBody2D)
├── AnimatedSprite2D
├── CollisionShape2D (CapsuleShape2D or RectangleShape2D, sized for a ~29px character on 48px canvas)
├── AttackHitbox (Area2D)
│   └── CollisionShape2D (disabled by default, enabled during attack frames)
├── HurtBox (Area2D)
│   └── CollisionShape2D
├── PlayerStats (Node — attach Scripts/Player/PlayerStats.cs)
├── PlayerCombat (Node — attach Scripts/Player/PlayerCombat.cs)
└── Camera2D (follow player, pixel-snapping enabled)

Attach Scripts/Player/PlayerController.cs to the root CharacterBody2D.
Set collision layer to "Player" (layer 1), mask to "World" (3), "Platforms" (4), "Hazards" (6).
AttackHitbox collision layer = "PlayerHitbox" (7), mask = "Enemies" (2).
HurtBox collision layer = "Player" (1), mask = "EnemyHitbox" (8).

Set floor_snap_length = 4.0 and up_direction = Vector2.Up on the CharacterBody2D.
Set texture_filter to "Nearest" on the AnimatedSprite2D.
Leave the SpriteFrames resource empty for now — we'll populate it in Phase 1.
````

### Prompt 0-B: Create Demontek.tscn (Base Enemy)

````text
PROMPT 0-B — CREATE DEMONTEK ENEMY SCENE

Create "res://Scenes/Characters/Enemies/Demontek.tscn" as a CharacterBody2D:

Demontek (CharacterBody2D)
├── AnimatedSprite2D
├── CollisionShape2D (sized for the small demon character)
├── DetectionArea (Area2D — range ~120px, detects Player layer)
│   └── CollisionShape2D (CircleShape2D, radius ~120)
├── AttackRange (Area2D — range ~30px, detects Player layer)
│   └── CollisionShape2D (CircleShape2D, radius ~30)
├── AttackHitbox (Area2D — deals damage to Player)
│   └── CollisionShape2D (disabled by default)
└── HurtBox (Area2D — receives damage)
    └── CollisionShape2D

Attach Scripts/Enemy/DemontekAI.cs to the root.
Set the EnemyType export to "Normal".
Collision layer = "Enemies" (2), mask = "World" (3), "Platforms" (4).
DetectionArea mask = "Player" (1).
AttackHitbox layer = "EnemyHitbox" (8), mask = "Player" (1).
HurtBox layer = "Enemies" (2), mask = "PlayerHitbox" (7).

Export variables should default to: MaxHealth=20, AttackDamage=5, MoveSpeed=40, PatrolDistance=100.
Set texture_filter to "Nearest" on AnimatedSprite2D.
````

### Prompt 0-C: Create Guard, Mutant, Goliath, King Scenes

````text
PROMPT 0-C — CREATE REMAINING ENEMY SCENES

Using Demontek.tscn as a template, create four additional enemy scenes. Each is a CharacterBody2D
with the same node structure but different export values and EnemyType:

1. "res://Scenes/Characters/Enemies/Guard.tscn"
   EnemyType = Guard, MaxHealth=30, AttackDamage=5, MoveSpeed=50, DetectionRange=150, PatrolDistance=120
   (Guards have wider detection, used in Level 2 dungeon)

2. "res://Scenes/Characters/Enemies/Mutant.tscn"
   EnemyType = Mutant, MaxHealth=60, AttackDamage=10, MoveSpeed=55, DetectionRange=130
   (Muscular Demonteks on Castle Floor 2)

3. "res://Scenes/Characters/Enemies/Goliath.tscn"
   EnemyType = Goliath, MaxHealth=200, AttackDamage=20, MoveSpeed=30, DetectionRange=200
   Bigger collision shape. Boss behavior (no patrol, always aggressive).

4. "res://Scenes/Characters/Enemies/King.tscn"
   EnemyType = King, MaxHealth=500, AttackDamage=30, MoveSpeed=35, DetectionRange=250
   Biggest collision shape. Final boss behavior.

All enemies use the DemontekAI.cs script with the appropriate EnemyType selected.
Collision layers same as Demontek.tscn.
Set texture_filter to "Nearest" on all AnimatedSprite2D nodes.
SpriteFrames left empty — populated in Phase 1.
````

---

## 3. Phase 1 — SpriteFrames & Animation Wiring

### Prompt 1-A: Create SpriteFrames for Explorer

````text
PROMPT 1-A — WIRE EXPLORER ANIMATIONS

Create a SpriteFrames resource for the Player's AnimatedSprite2D using the explorer's
individual frame PNGs. Since this is a 2D side-scroller, we only need east and west directions.

For each animation, load frames from:
  res://Assets/characters/explorer/animations/{animation_name}/east/frame_000.png ... frame_00N.png

Create these SpriteFrames animations:
  "idle"     ← breathing-idle/east   (4 frames, 6 FPS, loop=true)
  "walk"     ← walking-8-frames/east (8 frames, 10 FPS, loop=true)
  "run"      ← running-6-frames/east (6 frames, 10 FPS, loop=true)
  "jump"     ← jumping-1/east        (all frames, 8 FPS, loop=false)
  "attack"   ← cross-punch/east      (5 frames, 12 FPS, loop=false)
  "hurt"     ← taking-punch/east     (all frames, 8 FPS, loop=false)
  "death"    ← falling-back-death/east (all frames, 8 FPS, loop=false)

For facing left: use the FlipH property on AnimatedSprite2D when moving left,
rather than creating separate west animations. This saves memory.

Save the SpriteFrames as "res://Assets/characters/explorer/explorer_sprite_frames.tres".
Then, in Player.tscn, assign this SpriteFrames to the AnimatedSprite2D node.
Set the default animation to "idle".

Make sure PlayerController.UpdateAnimation() maps states to these animation names correctly.
If there are mismatches, update PlayerController.cs to use these exact names.
````

### Prompt 1-B: Create SpriteFrames for Demontek

````text
PROMPT 1-B — WIRE DEMONTEK ANIMATIONS

Create a SpriteFrames resource for the Demontek enemy using individual frame PNGs.
Load from: res://Assets/characters/demontek/animations/{animation_name}/east/

Animations to create:
  "idle"   ← breathing-idle   (4 frames, 6 FPS, loop=true)
  "walk"   ← cross-punch/north (using available direction — check what exists)
  "attack" ← cross-punch      (5 frames, 10 FPS, loop=false)
  "hurt"   ← taking-punch     (all frames, 8 FPS, loop=false)
  "death"  ← falling-back-death (all frames, 8 FPS, loop=false)

NOTE: The demontek may not have a dedicated walk animation. Check the available directories:
  Assets/characters/demontek/animations/
If walking-8-frames exists, use it. Otherwise, reuse idle or create a simple walk from
breathing-idle with slightly faster FPS (8 FPS).

Save as "res://Assets/characters/demontek/demontek_sprite_frames.tres".
Assign to Demontek.tscn's AnimatedSprite2D.

Make sure DemontekAI.UpdateAnimation() uses these exact animation names.
If there are mismatches, update DemontekAI.cs accordingly.
````

### Prompt 1-C: Create SpriteFrames for Guard, Mutant, Goliath, King

````text
PROMPT 1-C — WIRE REMAINING ENEMY ANIMATIONS

Create SpriteFrames for each remaining enemy type. For each character, check what
animations actually exist in their Assets/characters/{name}/animations/ folder and
create the best possible set.

1. Guard (Assets/characters/guard_demontek/animations/)
   Create: idle, walk, attack, hurt, death
   Save as: res://Assets/characters/guard_demontek/guard_sprite_frames.tres
   Assign to Guard.tscn

2. Mutant (Assets/characters/mutant_demontek/animations/)
   Create: idle, walk, run, attack, hurt, death
   Save as: res://Assets/characters/mutant_demontek/mutant_sprite_frames.tres
   Assign to Mutant.tscn

3. Goliath (Assets/characters/goliath/animations/)
   Create: idle, walk, attack, hurt, death (slower FPS for lumbering feel — 5-6 FPS walk)
   Save as: res://Assets/characters/goliath/goliath_sprite_frames.tres
   Assign to Goliath.tscn

4. King (Assets/characters/king_demontek/animations/)
   Create: idle, walk, attack, hurt, death
   Save as: res://Assets/characters/king_demontek/king_sprite_frames.tres
   Assign to King.tscn

For each: check which animation directories exist, use east direction frames,
handle missing animations by reusing closest available (e.g., idle → walk if no walk).
FlipH for left-facing. Adjust the AnimatedSprite2D scale if needed — bosses (Goliath, King)
should appear bigger than normal enemies.

After creating all SpriteFrames, verify DemontekAI.UpdateAnimation() handles all animation
names for all enemy types. Update the method if needed.
````

---

## 4. Phase 2 — Level 1: Jungle Canopy

### Prompt 2-A: Redesign Level 1 Layout

````text
PROMPT 2-A — REDESIGN LEVEL 1 JUNGLE CANOPY

Rebuild Level1_Jungle.tscn with a proper platformer layout. The current level has a flat
ground and only 3 Demonteks — we need 10 Demonteks on treetop platforms.

LEVEL DESIGN:
- The level should scroll horizontally from left to right, approximately 3000px wide.
- Use the jungle_tileset_16x16.tres for the TileMap ground and platforms.
- The ground level should be at y=256 (row 16 in tilemap coords at 16px tiles).
- Create elevated platforms of varying heights (y=192, y=160, y=128) representing treetop platforms.
- Platforms should require jumping to reach, with gaps between them.
- Place 10 Demontek instances spread across the level on various platforms.
- The player starts at the far left (x=48, y=240).
- The level end trigger should be at the far right.

PARALLAX BACKGROUND:
- Keep the existing 3-layer parallax (sky, mid, foreground) using the jungle backgrounds.
- Sky: motion_scale=0.1  (slowest)
- Mid: motion_scale=0.4
- Foreground: motion_scale=0.7

CAMERA:
- The Player's Camera2D should have limits set:
  limit_left=0, limit_top=0, limit_right=3000, limit_bottom=270
  position_smoothing_enabled=true, drag_horizontal_enabled=true

Add a Node2D called "LevelEnd" with an Area2D at the far right (x=2900) that triggers
level completion when the player enters it. Connect this to GameManager.CompleteLevel().

Remove the old instruction labels — we'll add a proper HUD later.
````

### Prompt 2-B: Add Weapon Pickup to Level 1

````text
PROMPT 2-B — ADD KNIFE PICKUP TO LEVEL 1

The explorer starts without a weapon in the story. Add a WeaponPickup node near the
starting area of Level 1.

1. Place a WeaponPickup instance (from Scenes/Prefabs/WeaponPickup.tscn) at position (96, 240).
2. Set its WeaponType export to "Knife".
3. Make sure WeaponPickup.cs properly signals PlayerCombat.PickupWeapon() when the player
   presses the interact key (F) near it.
4. The pickup should have a small visual indicator (blinking sprite or label "Press F").

If WeaponPickup.tscn doesn't have an AnimatedSprite2D or visual indicator, add one.
The player should start unarmed and must pick up the knife before they can attack effectively.
````

---

## 5. Phase 3 — Level 2: Dungeon Escape

### Prompt 3-A: Redesign Level 2 Layout

````text
PROMPT 3-A — REDESIGN LEVEL 2 DUNGEON ESCAPE

Rebuild Level2_Dungeon.tscn with proper dungeon level design.

LEVEL STRUCTURE (3 distinct sections, left to right, ~4000px wide):

Section 1 — DUNGEON CELL (x=0 to x=600):
  - Small enclosed area with stone walls (use dungeon_tileset_16x16.tres).
  - Player spawns at (64, 240).
  - A Checkpoint at the spawn point (this is the death-respawn point).
  - The tunnel entrance is a breakable wall or door at the right side.
  - Place 2 Guard enemies patrolling near the tunnel entrance.

Section 2 — TUNNEL ESCAPE (x=600 to x=2400):
  - Narrow, winding tunnel with platforms of varying heights.
  - Hazardous pits (fall = respawn at last checkpoint).
  - Place 4 Guard enemies throughout the tunnel.
  - Add a Checkpoint midway through the tunnel (x=1500).
  - The tunnel gets progressively more complex with tighter jumps.

Section 3 — ANCIENT RUINS (x=2400 to x=4000):
  - Open area with ruined structures.
  - A stone button (interactive object, press F) opens a hidden chamber.
  - Inside the hidden chamber: WeaponPickup (Machete) and WeaponPickup (Sword).
  - Place 3 Guard enemies guarding the ruins.
  - Level end trigger at far right → transitions to Level 3.

Use dungeon_tileset_16x16.tres for all TileMap layers.
Use dungeon background images from Assets/backgrounds/dungeon/ for parallax (if they exist),
otherwise use a dark ColorRect background (Color 0.15, 0.1, 0.12).

Set Camera2D limits for the full level width.
The death respawn sends the player back to the dungeon cell (the first Checkpoint).
````

### Prompt 3-B: Interactive Stone Button Mechanic

````text
PROMPT 3-B — CREATE INTERACTIVE STONE BUTTON

Create an interactive stone button mechanic for Level 2.

1. Create a new scene: "res://Scenes/Prefabs/StoneButton.tscn"
   StoneButton (StaticBody2D)
   ├── Sprite2D (just a colored rectangle or use a tileset tile for now)
   ├── CollisionShape2D (small rectangle)
   └── InteractionArea (Area2D, detects Player layer)
       └── CollisionShape2D

2. Create "res://Scripts/Environment/StoneButton.cs":
   - Namespace: Demonteks.Environment
   - Extends StaticBody2D
   - Export: NodePath to the door/wall it opens (HiddenChamberDoor)
   - When player is in InteractionArea and presses "interact" (F key):
     → Play a pressing animation (shift Sprite2D down by 2px)
     → Emit a "ButtonPressed" signal
     → Open the connected door (remove/move the blocking StaticBody2D)
     → Show brief text: "A hidden entrance opened!"
   - Single use — can't be pressed again after activation.

3. In Level 2, place the StoneButton at the entrance to the hidden chamber.
   Connect it to a blocking wall node (StaticBody2D with ColorRect) that disappears when pressed.
````

---

## 6. Phase 4 — Level 3: Castle Infiltration

### Prompt 4-A: Redesign Level 3 Castle

````text
PROMPT 4-A — REDESIGN LEVEL 3 CASTLE INFILTRATION

Rebuild Level3_Castle.tscn as a multi-section vertical castle level.
Use castle_tileset_16x16.tres for all TileMap work.

LEVEL STRUCTURE (vertical progression with horizontal sections):

EXTERIOR — BACK ENTRANCE (x=0 to x=800, ground level):
  - The explorer approaches the castle from behind.
  - Floor traps: Area2D nodes on the ground. If player touches one, spawn 6 Demonteks
    at nearby positions. Use a script TrapTrigger.cs to handle this.
  - Place 3 floor traps with visible but subtle markers (slightly different tile color).
  - A door at x=750 leads inside the castle.

FLOOR 1 — GROUND FLOOR (x=800 to x=2000, same y level):
  - Large hall with platforms.
  - Place 6 normal Demontek enemies.
  - A staircase/ladder platform system at x=1900 leads up to Floor 2.
  - Checkpoint at x=800 (entrance to castle). Death on Floor 1 = respawn here.

FLOOR 2 — SECOND FLOOR (x=800 to x=2000, y offset -300):
  - Similar layout but with 4 Mutant Demonteks.
  - After clearing, the player finds 3 WeaponPickups: DemontekSaber, Sawtooth, AlienSword.
  - Staircase to Floor 3 at x=1900. Death here = respawn at Floor 1 entrance.

FLOOR 3 — THIRD FLOOR (x=800 to x=2000, y offset -600):
  - 6 Mutant Demonteks (more densely packed).
  - After clearing, path to Goliath boss arena.
  - GOLIATH BOSS ARENA (x=2000 to x=2600, y offset -600):
    A large open platform. Goliath spawns at center.
    Lock the player in (block exits until Goliath defeated).

TERRACE — FINAL BOSS (x=800 to x=2000, y offset -900):
  - Access unlocked after Goliath defeated.
  - King Demontek stands at the center.
  - After defeating King → spawn a Portal object at x=1400
  - Portal: interactable (F key) → triggers GameManager.WinGame()

Use castle backgrounds from Assets/backgrounds/castle/ for parallax (if available),
otherwise use a dark red/crimson ColorRect (Color 0.4, 0.08, 0.08).
````

### Prompt 4-B: Floor Trap System

````text
PROMPT 4-B — CREATE FLOOR TRAP SYSTEM

Create an ambush trap system for Level 3's exterior.

1. Create "res://Scenes/Prefabs/FloorTrap.tscn":
   FloorTrap (Area2D)
   ├── CollisionShape2D (flat rectangle on the ground)
   └── TrapMarker (Sprite2D or ColorRect — subtle visual hint)

2. Create "res://Scripts/Environment/FloorTrap.cs":
   - Namespace: Demonteks.Environment
   - When the player's body enters the Area2D:
     → Trigger is disabled (one-time use)
     → Instantiate 6 Demontek enemies at predefined spawn points around the trap
     → Spawn points should be exported as an array of Vector2 offsets
     → The spawned Demonteks should immediately target the player
   - If the player dies from the ambush, they respawn at the dungeon cell
     (use GameManager.RespawnPlayer() which goes to last checkpoint)
   - The spawned enemies should use the same Demontek.tscn scene.

3. In Level 3's exterior section, place 3 FloorTrap instances with spawn offsets
   that create a surrounding ambush pattern.
````

### Prompt 4-C: Boss Lock-in Mechanic

````text
PROMPT 4-C — BOSS ARENA LOCK-IN AND PORTAL

Create the boss arena lock mechanic and earth portal for Level 3.

1. Create "res://Scripts/Environment/BossArena.cs":
   - When the player enters the boss arena Area2D trigger:
     → Close entrance and exit doors (StaticBody2D barriers appear)
     → Boss (Goliath or King) activates and begins fighting
   - When the boss is killed (connect to DemontekAI's Died signal):
     → Remove the barriers
     → If this was the King: spawn the EarthPortal at the designated position

2. Create "res://Scenes/Prefabs/EarthPortal.tscn":
   EarthPortal (Area2D)
   ├── AnimatedSprite2D (a glowing portal effect — use a simple colored ellipse for now)
   ├── CollisionShape2D
   └── InteractionLabel (Label "Press F to return to Earth")

3. Create "res://Scripts/Environment/EarthPortal.cs":
   - When player is in range and presses "interact":
     → Call GameManager.WinGame()
     → Transition to Victory scene

4. Update GameManager.WinGame() if needed to properly handle the transition
   to Scenes/UI/Victory.tscn.
````

---

## 7. Phase 5 — HUD, Menus & Game Flow

### Prompt 5-A: Fix HUD

````text
PROMPT 5-A — FIX AND IMPROVE HUD

Review and fix Scenes/UI/HUD.tscn and Scripts/UI/HUDController.cs.

The HUD should display:
  - Health bar (red bar showing CurrentHealth/MaxHealth, positioned top-left)
  - Current weapon name (text label, positioned below health bar)
  - Weapon icon (placeholder colored rectangle per weapon type)
  - Level name (centered top)

Make sure the HUD:
  1. Connects to PlayerStats.HealthChanged signal to update the health bar
  2. Connects to PlayerCombat signals to update weapon display
  3. Uses a CanvasLayer to stay on screen regardless of camera movement
  4. Has proper anchoring for the 480×270 viewport

Ensure HUD.tscn is instanced in each level scene, or better yet, instantiate it
from GameManager when a level loads so it persists across levels.
````

### Prompt 5-B: Fix Main Menu

````text
PROMPT 5-B — FIX MAIN MENU FLOW

Review and fix Scenes/UI/MainMenu.tscn and Scripts/UI/MainMenuController.cs.

MainMenu should have:
  - Title "DEMONTEKS" centered, large font
  - "New Game" button → calls GameManager.StartNewGame() → loads Level 1
  - "Continue" button → calls GameManager.ContinueGame() → loads from save
  - "Quit" button → GetTree().Quit()

Make sure:
  1. MainMenu is set as the main scene in project.godot (already done: run/main_scene)
  2. GameManager.StartNewGame() resets all state and loads Level1_Jungle.tscn
  3. GameManager.ContinueGame() loads SaveManager data and the saved level
  4. Button connections work properly
  5. Style the menu with the game's dark theme (dark purple/red background)

Also fix GameManager.LoadLevel() to use the correct scene paths:
  Level 1: "res://Scenes/Levels/Level1_Jungle.tscn"
  Level 2: "res://Scenes/Levels/Level2_Dungeon.tscn"
  Level 3: "res://Scenes/Levels/Level3_Castle.tscn"
````

### Prompt 5-C: Fix Game Over & Victory

````text
PROMPT 5-C — FIX GAME OVER AND VICTORY SCREENS

Review and fix the GameOver and Victory scenes.

GameOver.tscn / GameOverController.cs:
  - "GAME OVER" title
  - "Retry" button → GameManager.RestartLevel() (respawns at last checkpoint)
  - "Main Menu" button → GameManager.ReturnToMainMenu()
  - Show death count or hint text

Victory.tscn / VictoryController.cs:
  - "YOU ESCAPED!" title
  - "The portal closes behind you... but this is only the beginning."
  - Show stats: enemies killed, time played, damage taken
  - "Main Menu" button → GameManager.ReturnToMainMenu()
  - "New Game+" button (future feature, disabled for now)

Make sure GameManager properly transitions to/from these scenes.
Verify that PauseMenu.tscn also works (Esc key pauses, shows resume/quit options).
````

### Prompt 5-D: Level Transitions

````text
PROMPT 5-D — IMPLEMENT LEVEL TRANSITION SYSTEM

Create a proper level transition system:

1. Create "res://Scenes/UI/LevelTransition.tscn":
   A CanvasLayer with a full-screen ColorRect that fades to black and back.

2. Create "res://Scripts/UI/LevelTransition.cs":
   - FadeOut(duration) → AnimationPlayer tween alpha from 0 to 1
   - FadeIn(duration) → AnimationPlayer tween alpha from 1 to 0
   - TransitionToScene(scenePath) → FadeOut → load scene → FadeIn

3. Update GameManager.LoadLevel() to use the LevelTransition:
   - When completing a level: fade out, load next level, fade in
   - On death: brief fade out, respawn at checkpoint, fade in
   - Keep it simple: ~0.5s fade out, ~0.5s fade in

4. Add LevelTransition as an autoload or instantiate it in GameManager._Ready().

5. Update GameManager.CompleteLevel() to show a brief "Level Complete!" text
   before transitioning to the next level.
````

---

## 8. Phase 6 — Polish, Audio & QA

### Prompt 6-A: Animation Polish

````text
PROMPT 6-A — POLISH ANIMATIONS AND VISUAL FEEDBACK

Review all animation playback and add visual polish:

1. Player visual feedback:
   - On taking damage: flash the sprite white for 0.1s (modulate shader or tween)
   - During invincibility: blink the sprite (toggle Visible every 0.1s)
   - On death: play death animation, then fade out
   - Attack animation should match weapon range (larger weapons = slightly longer)

2. Enemy visual feedback:
   - On taking damage: flash red for 0.1s
   - On death: play death animation, then spawn a small particle effect, then queue_free()
   - Boss enemies (Goliath, King): screen shake on their attacks

3. Camera effects:
   - Small screen shake on player taking damage (Camera2D offset jitter)
   - Larger screen shake during boss attacks

4. Make sure all AnimatedSprite2D nodes have their animation_finished signal connected
   for one-shot animations (attack, hurt, death) to properly transition back to idle.

All visual effects should be done via code (tweens, shader params) — no need for
external particle assets right now.
````

### Prompt 6-B: Audio System (Placeholder)

````text
PROMPT 6-B — ADD PLACEHOLDER AUDIO SYSTEM

Create a basic audio management system. We don't have audio files yet, but set up
the infrastructure so we can drop in sounds later.

1. Create "res://Scripts/Management/AudioManager.cs":
   - Autoload singleton
   - AudioStreamPlayer nodes for: BGM, SFX_Attack, SFX_Hurt, SFX_Jump, SFX_Pickup, SFX_Boss
   - Methods: PlayBGM(stream), PlaySFX(sfxName), StopBGM()
   - Volume controls for BGM and SFX separately

2. Add AudioManager as autoload in project.godot.

3. Add PlaySFX calls (commented out for now) in:
   - PlayerController: jump, attack
   - PlayerCombat: hit enemy
   - PlayerStats: take damage, die
   - DemontekAI: attack, die
   - WeaponPickup: pickup

4. Add an empty "res://Assets/audio/" folder with subfolders: bgm/, sfx/

This sets up the audio pipeline without requiring actual audio files.
We can generate or find sounds later.
````

---

## 9. Phase 7 — Iterative Bug-Fix & Expansion

Use these prompts as needed to fix issues and extend the game.

### Prompt 7-A: General Debug

````text
PROMPT 7-A — DEBUG [DESCRIBE ISSUE]

I'm experiencing the following issue in my Demonteks game:

[Describe the exact problem here, e.g.:]
- "The player falls through platforms on Level 1"
- "Demonteks don't chase the player when in range"
- "The attack hitbox never connects with enemies"
- "The game crashes when loading Level 2"

[Include any error messages from the Godot console]

Please:
1. Identify the root cause
2. Fix the relevant script(s)
3. Verify the fix doesn't break other functionality
4. Explain what was wrong and why the fix works
````

### Prompt 7-B: Add More Enemies to a Level

````text
PROMPT 7-B — ADD MORE ENEMIES TO [LEVEL]

Level [1/2/3] needs more enemies for better gameplay challenge.

Current enemy count: [N]
Desired enemy count: [M]

Add [M-N] more [Demontek/Guard/Mutant] enemies to Level[X].tscn, placing them on
existing platforms with appropriate spacing (minimum 100px between enemies).
Make sure each new enemy:
  - Has a unique node name
  - Is placed on a platform (not floating mid-air)
  - Has patrol range that fits its platform
  - Uses the correct enemy scene

Test by running the level and confirming all enemies patrol and chase correctly.
````

### Prompt 7-C: Fix Collision Issues

````text
PROMPT 7-C — FIX COLLISION/PHYSICS ISSUES

The player/enemies have collision problems. Review and fix:

1. Check that all physics layers are correctly assigned:
   - Player: layer 1, mask 3,4,6
   - Enemies: layer 2, mask 3,4
   - World/ground TileMap: layer 3
   - Platforms: layer 4
   - Items/pickups: layer 5
   - Hazards: layer 6
   - PlayerHitbox (attack): layer 7, mask 2
   - EnemyHitbox (attack): layer 8, mask 1

2. Verify the CharacterBody2D floor detection:
   - floor_snap_length should be set
   - up_direction = Vector2.Up
   - Move_and_slide() is being called properly

3. Check that one-way platforms (if any) have the one_way property set.

4. Make sure TileMap collision layers are set to "World" (layer 3).
````

### Prompt 7-D: Extend or Adjust a Level

````text
PROMPT 7-D — EXTEND LEVEL [X]

I want to make changes to Level [1/2/3]:

[Describe changes, e.g.:]
- "Make Level 1 longer with more platforms between x=1500 and x=2000"
- "Add a hidden area in Level 2 accessible by crouching through a low passage"
- "Add more floor traps to Level 3's exterior"
- "Make the Goliath boss arena larger"

Please update the relevant .tscn file, placing new nodes at sensible positions.
Test that the camera limits are updated to accommodate any size changes.
If adding new enemy spawns or interactables, make sure they're properly configured.
````

### Prompt 7-E: Update Game Balance

````text
PROMPT 7-E — REBALANCE GAME DIFFICULTY

The game feels too [easy/hard]. Adjust the following:

PLAYER STATS (PlayerStats.cs exports):
  MaxHealth: [new value, default 100]
  InvincibilityTime: [default 1.0s]

ENEMY STATS (DemontekAI.cs InitializeByType method):
  Normal - HP:[20] Damage:[5] Speed:[40]
  Guard  - HP:[30] Damage:[5] Speed:[50]
  Mutant - HP:[60] Damage:[10] Speed:[55]
  Goliath- HP:[200] Damage:[20] Speed:[30]
  King   - HP:[500] Damage:[30] Speed:[35]

WEAPON DAMAGE (PlayerCombat.cs InitializeWeapons):
  Knife: [10] | Machete: [15] | Sword: [20]
  DemontekSaber: [25] | Sawtooth: [30] | AlienSword: [35]

Adjust the values in brackets as needed for the desired difficulty.
Make sure damage values create meaningful progression (each new weapon should
kill enemies faster than the last).
````

### Prompt 7-F: Build & Run Verification

````text
PROMPT 7-F — BUILD AND VERIFY THE GAME

Run a full verification of the Demonteks project:

1. Build the C# solution:
   cd D:\Demonteks
   dotnet build Demonteks.sln

2. Fix any compilation errors.

3. Check that all scene references are valid:
   - Player.tscn exists and has the correct script attached
   - All enemy .tscn files exist and reference DemontekAI.cs
   - Level scenes reference existing scenes and resources
   - Autoloads in project.godot point to existing files

4. Verify the SpriteFrames resources exist and reference valid texture files.

5. List any missing files or broken references.

6. After fixing issues, rebuild and confirm 0 errors.
````

---

## Quick Reference — Prompt Execution Order

| Step | Prompt | What it does |
|------|--------|-------------|
| 1 | **0 (Context)** | Paste at session start |
| 2 | **0-A** | Create Player.tscn |
| 3 | **0-B** | Create Demontek.tscn |
| 4 | **0-C** | Create Guard, Mutant, Goliath, King .tscn |
| 5 | **1-A** | Wire Explorer animations |
| 6 | **1-B** | Wire Demontek animations |
| 7 | **1-C** | Wire Guard, Mutant, Goliath, King animations |
| 8 | **7-F** | Build & verify (checkpoint) |
| 9 | **2-A** | Redesign Level 1 |
| 10 | **2-B** | Add knife pickup to Level 1 |
| 11 | **7-F** | Build & verify (checkpoint) |
| 12 | **3-A** | Redesign Level 2 |
| 13 | **3-B** | Create stone button mechanic |
| 14 | **7-F** | Build & verify (checkpoint) |
| 15 | **4-A** | Redesign Level 3 |
| 16 | **4-B** | Create floor trap system |
| 17 | **4-C** | Create boss lock-in & portal |
| 18 | **7-F** | Build & verify (checkpoint) |
| 19 | **5-A** | Fix HUD |
| 20 | **5-B** | Fix Main Menu |
| 21 | **5-C** | Fix Game Over & Victory |
| 22 | **5-D** | Level transitions |
| 23 | **7-F** | Build & verify (checkpoint) |
| 24 | **6-A** | Polish animations |
| 25 | **6-B** | Audio system scaffold |
| 26 | **7-F** | Final build & verify |

> **After each build checkpoint**, use **Prompt 7-A** (debug) to fix any issues
> before proceeding to the next phase.

---

## Tips for Using These Prompts

1. **Always start with Prompt 0** (context) in a new conversation.
2. **One prompt per conversation turn** — let the AI complete each phase before moving on.
3. **After each phase**, run the game in Godot to test manually, then use **Prompt 7-A** to describe any issues.
4. **Save frequently** in Godot and commit to git between phases.
5. **Prompts 7-A through 7-F** are reusable templates — copy and fill in the brackets.
6. **If the AI makes mistakes**, don't restart — use Prompt 7-A with the specific error.
7. **Level design values** (positions, sizes) may need tweaking — the prompts provide reasonable starting points but adjust based on playtesting.
