# DEMONTEKS — Game Design Document
### Version 1.0 | 2D Platformer | Godot 4 (C#) | Windows · Android · iOS

---

## 1. Game Overview

| Field | Details |
|---|---|
| **Title** | Demonteks |
| **Genre** | 2D Action-Adventure Platformer |
| **Engine** | Godot 4.x — C# scripting |
| **Target Platforms** | Windows, Android, iOS |
| **Pixel Art Tooling** | PixelLab MCP (sprite & tile generation) |
| **Vibe Coding Env** | Antigravity / OpenCode |
| **Inspired By** | Levrault's Godot 2D Action-Adventure Platformer Demo |
| **Art Style** | Retro pixel art — 128×128 characters, multi-layered parallax backgrounds |
| **Player Count** | Single-player |

---

## 2. Story Synopsis

The explorer wakes up disoriented in an alien jungle under a violet sky. Giant alien flora towers around him. He is immediately confronted by the Demonteks — small, reddish, demon-like creatures that repeat the word "tek." Mistaking them for harmless curiosities, the explorer mocks them, triggering an enraged pursuit through the jungle canopy.

After surviving the treetop platforms, the explorer finds refuge in ancient ruins — only to be captured. He wakes in a hellish underground realm: the Demonteks' world. From a dungeon, he escapes through a tunnel, discovers ancient weapons in hidden ruins, and storms the King's fortress. His ultimate goal: defeat the Demontek King, activate the inter-dimensional portal, and return to Earth.

A final warning before the portal closes: *"This is only the beginning of the war."*

---

## 3. Narrative Structure

### Part 0 — Awakening (Cutscene)
Parallax intro scene: violet sky, alien jungle, explorer waking. No gameplay. Sets tone and introduces the Demonteks.

### Part 1 — Jungle Canopy
Treetop platforming across jungle branches. The explorer fights or outmaneuvers 10 Demonteks using only a knife. This acts as the tutorial zone.

### Part 2 — Dungeon & Tunnel
After capture and blackout, the explorer wakes in the dungeon. He escapes through a dangerous tunnel, neutralizes guards, and discovers a hidden armoury with ancient weapons behind a stone-trigger wall.

### Part 3 — The Fortress
A multi-floor castle assault: stealth approach through the courtyard, traps in the outer walls, escalating combat through three floors, culminating in the Goliath boss fight and the King Demontek showdown on the terrace.

### Part 4 — Epilogue (Cutscene)
The portal opens. The explorer crosses to Earth. The dimensional link severs. A voice warns: *"This is only the beginning."*

---

## 4. Core Mechanics

Inspired by Levrault's architecture: every character (player and enemies) is built on a **Hierarchical Finite State Machine (HFSM)**. States are C# classes, transitions are driven by signals and conditions, and each state is responsible for input polling, physics, and animation.

### 4.1 Player State Machine

```
PlayerStateMachine
├── GroundedState
│   ├── IdleState
│   ├── WalkState
│   ├── RunState
│   ├── CrouchState
│   ├── AttackState
│   │   ├── AttackLight (first strike)
│   │   ├── AttackMedium (combo hit 2)
│   │   └── AttackHeavy (finisher / charged)
│   └── InteractState (F key — pick up items, open portal)
├── AirState
│   ├── JumpState
│   ├── DoubleJumpState
│   ├── FallState
│   └── AirAttackState
├── StealthState
│   ├── CrouchWalkState
│   └── LedgeGrabState
├── HitState (stagger on damage)
└── DeathState (triggers respawn flow)
```

### 4.2 Movement & Controls

#### Desktop (Keyboard)
| Action | Key |
|---|---|
| Move Left | A / ← |
| Move Right | D / → |
| Crouch | S / ↓ |
| Jump / Double Jump | W / Space |
| Light Attack | E |
| Interact / Pick Up | F |
| Run (hold) | Shift |
| Pause | Escape |

#### Mobile (Virtual Gamepad)
A D-pad overlay is rendered at runtime for Android and iOS. Buttons map to the same actions. All controls use Godot's `InputMap` so remapping is centralized.

```csharp
// Detect platform and show/hide virtual controls
if (OS.GetName() == "Android" || OS.GetName() == "iOS")
    GetNode<CanvasLayer>("VirtualControls").Visible = true;
```

### 4.3 Combat System

**Melee Combo Attacks** — tapping the attack key chains up to 3 hits. A cooldown timer between combos prevents button-mashing spam (same approach as Levrault's `cooldown` Timer node).

| Hit | Weapon | Damage | Timing Window |
|---|---|---|---|
| Light (1st) | Knife stab | 10 | 0–0.3 s |
| Medium (2nd) | Knife slash | 15 | 0.1–0.5 s |
| Heavy (3rd) | Charged slash | 25 | hold 0.5 s |
| Air Slash | Any weapon | 12 | mid-air only |

**Weapon Progression:**
- Part 1: Knife
- Part 2: Machete (unlocked in armoury)
- Part 3 (2nd floor): Alien sabre (looted from Demonteks)
- Part 3 (3rd floor): Sawtooth sword, Alien energy sword

**Hit-stop / Momentum Management** — on a successful hit, the game freezes for 2–4 frames using `Engine.TimeScale`, then eases back in with a curve (as implemented in Levrault's momentum system). This gives attacks weight and impact feel.

### 4.4 Stealth Mechanics (Part 3)

- Crouching reduces the explorer's detection radius.
- Guards have a cone-shaped `Area2D` detection zone (field of view).
- Guards not in alert state patrol a fixed path using `NavigationAgent2D`.
- Floor traps trigger when the explorer steps into their `Area2D`; bypassed by jumping or crouching.
- If triggered: 6 Demonteks spawn and pursue the explorer → death → respawn at dungeon entrance.

### 4.5 Health, Damage & Respawn

| Character | Max HP | Notes |
|---|---|---|
| Explorer | 100 | Shown as pixel-art health bar (HUD) |
| Demontek | 30 | Patrol/follow AI |
| Guard Demontek | 40 | Patrols, has detection cone |
| Mutant Demontek | 60 | Faster, more aggressive |
| Goliath | 200 | Boss: 3 attack phases |
| King Demontek | 300 | Boss: 4 attack phases, summons Demonteks |

**Damage Table:**
| Attacker | Damage |
|---|---|
| Demontek bite | −5 HP |
| Mutant Demontek strike | −10 HP |
| Goliath slam/charge | −20 HP |
| King Demontek | −30 HP |
| Trap trigger (indirect) | Death (respawn) |

**Respawn Rules:**
- Part 1: Respawn at part start.
- Part 2: Respawn at dungeon entrance.
- Part 3 (ground floor trigger): Respawn at dungeon.
- Part 3 (1st floor cleared): Respawn at castle 1st floor.
- Part 3 (2nd floor cleared): Respawn at castle 2nd floor.

---

## 5. Enemy AI

All enemies use the same HFSM pattern as the player. Each enemy has `Idle`, `Patrol`, `Chase`, `Attack`, `Hit`, and `Death` states.

### 5.1 Demontek (Normal)
- **Idle**: Stands still, plays idle animation, repeats "tek" sound.
- **Patrol**: Moves back and forth between two waypoints.
- **Chase**: Activates when player enters detection `Area2D` (radius ~200 px). Pursues player using `move_toward`.
- **Attack**: Within melee range (~40 px), lunges and bites.
- **Behaviour quirk**: Demonteks cluster — when one spots the player, nearby ones are alerted via a signal broadcast.

### 5.2 Guard Demontek
Same as normal Demontek plus:
- Cone FOV detection using `RayCast2D`.
- Patrols a fixed route.
- Can be neutralized from behind (sneak attack deals 3× damage).

### 5.3 Mutant Demontek
- Faster movement speed, higher jump height.
- Two-hit combo attack.
- Enrages (movement speed ×1.5) when below 50% HP.

### 5.4 Goliath (Mini-Boss — Castle 3rd Floor)
Three phases:
1. **Phase 1 (100–60% HP)**: Ground slam (AoE shock wave), slow charge.
2. **Phase 2 (60–30% HP)**: Faster charge, throws debris projectiles.
3. **Phase 3 (<30% HP)**: Berserk — continuous charge, large AoE slams.

### 5.5 King Demontek (Final Boss — Castle Terrace)
Four phases + summon mechanic:
1. **Phase 1**: Staff swings, slow projectile orbs.
2. **Phase 2**: Summons 3 Demonteks every 15 seconds.
3. **Phase 3**: Teleports across the terrace, rapid staff combos.
4. **Phase 4 (<25% HP)**: Crown glows, all attacks gain fire AoE.

---

## 6. Level Design

### Level 1 — Jungle Canopy
- **Background**: Giant trees with glowing alien flora, layered parallax (5 layers).
- **Platforms**: Animated branch platforms at varying heights; some bounce/sway.
- **Enemies**: 10 Demonteks placed at key choke points.
- **Secrets**: One hidden knife upgrade tucked in foliage.
- **Exit**: Falling/climbing down into the ruins triggers the Part 1→2 cutscene.

### Level 2a — Dungeon
- **Background**: Dark stone walls, flickering torches, iron cage cells.
- **Mechanics**: No enemies initially; find the imprisoned friendly Demontek (sign language tutorial for the tunnel).
- **Interactable**: Stone wall button (`F` key) opens tunnel entrance.

### Level 2b — Tunnel
- **Background**: Narrow underground passage, dripping water, faint red glow ahead.
- **Mechanics**: Guard patrols, darkness (limited visibility radius around explorer), pressure-plate traps.
- **Enemies**: 3–5 Guards.
- **Discovery**: Ancient armoury room triggered by pressing a specific stone — reveals machete and story tablets.

### Level 3 — Castle Courtyard
- **Background**: Reddish stone fortress exterior, torches, alien banners.
- **Mechanics**: Stealth approach — detection cones, floor traps. Alerts spawn 6 Demonteks.
- **Exit**: Castle back door (interactable after the courtyard is cleared).

### Level 3a — Castle 1st Floor
- **Background**: Castle interior hallway — suits of alien armour, dim lighting.
- **Enemies**: Mix of normal Demonteks and a Guard patrol.
- **Progression**: Clear all enemies → staircase to 2nd floor unlocks.

### Level 3b — Castle 2nd Floor
- **Background**: Wider hall, alien tapestries, red ambient lighting.
- **Enemies**: Mutant Demonteks. Weapon loot drop after clearing (alien sabre / sawtooth).
- **Progression**: Defeat all → staircase to 3rd floor.

### Level 3c — Castle 3rd Floor + Goliath
- **Background**: Large arena-style room with crumbling pillars.
- **Enemies**: High-density Mutant Demonteks, then Goliath boss encounter.
- **Progression**: Defeat Goliath → terrace access opens.

### Level 3d — Castle Terrace (Final Battle)
- **Background**: Open-air terrace, violet sky visible, floating altar with the inactive portal.
- **Boss**: King Demontek — 4-phase fight.
- **Victory**: King defeated → portal activates → interaction with `F` key → ending cutscene.

---

## 7. UI & HUD

Inspired by Levrault's UI system — all HUD elements are in a dedicated `CanvasLayer` so they render above the game world and are camera-independent.

| Element | Description |
|---|---|
| **Health Bar** | Pixel-art segmented bar (top-left). Drains smoothly using a tween. |
| **Weapon Icon** | Current weapon sprite (bottom-left), swaps on pickup. |
| **Combo Timer** | Small cooldown arc bar below the attack button / key prompt. |
| **Interact Prompt** | "Press F" bubble appears near interactable objects. |
| **Boss Health Bar** | Full-width bar at the bottom of the screen during boss fights. |
| **Story Panels** | Letterboxed cutscene overlays with scrolling text. |
| **Pause Screen** | Semi-transparent overlay — Resume / Restart / Main Menu. |
| **Game Over Screen** | "You Died" pixel text with Retry / Main Menu. |
| **Main Menu** | Title card, Start, Credits, Exit. |
| **Loading Screen** | Alien rune animation between scene transitions. |

---

## 8. Audio Design

| Category | Description |
|---|---|
| **BGM — Jungle** | Tense alien atmosphere, tribal percussion |
| **BGM — Dungeon/Tunnel** | Low drone, tension building |
| **BGM — Castle** | Ominous march, escalates per floor |
| **BGM — Boss Fights** | High-energy percussive battle theme |
| **SFX — Attack** | Knife swish, blade impact, combo stinger |
| **SFX — Demonteks** | "Tek tek" voice clip (pitched variants per type) |
| **SFX — UI** | Retro 8-bit menu blips |
| **SFX — Portal** | Electric hum, dimensional tear on activation |

---

## 9. Technical Architecture (Godot C#)

### 9.1 Project Structure

```
demonteks/
├── addons/
├── assets/
│   ├── characters/         # PixelLab-generated sprite sheets (128×128)
│   │   ├── explorer/
│   │   ├── demontek/
│   │   ├── guard/
│   │   ├── mutant/
│   │   ├── king/
│   │   └── goliath/
│   ├── tilesets/           # PixelLab-generated tile sheets
│   │   ├── jungle/
│   │   ├── dungeon/
│   │   ├── tunnel/
│   │   ├── castle_exterior/
│   │   ├── castle_interior/
│   │   └── terrace/
│   ├── backgrounds/        # Parallax layer PNGs
│   ├── ui/                 # HUD assets
│   ├── audio/
│   └── vfx/
├── scenes/
│   ├── levels/
│   │   ├── Level1_JungleCanopy.tscn
│   │   ├── Level2a_Dungeon.tscn
│   │   ├── Level2b_Tunnel.tscn
│   │   ├── Level3_Courtyard.tscn
│   │   ├── Level3a_Castle1F.tscn
│   │   ├── Level3b_Castle2F.tscn
│   │   ├── Level3c_Castle3F.tscn
│   │   └── Level3d_Terrace.tscn
│   ├── characters/
│   │   ├── Explorer.tscn
│   │   ├── Demontek.tscn
│   │   ├── Guard.tscn
│   │   ├── Mutant.tscn
│   │   ├── Goliath.tscn
│   │   └── King.tscn
│   ├── ui/
│   │   ├── HUD.tscn
│   │   ├── PauseMenu.tscn
│   │   ├── GameOver.tscn
│   │   ├── MainMenu.tscn
│   │   └── LoadingScreen.tscn
│   ├── cutscenes/
│   │   ├── Intro.tscn
│   │   ├── Capture.tscn
│   │   ├── AncientWritings.tscn
│   │   └── Ending.tscn
│   └── shared/
│       ├── VirtualControls.tscn
│       ├── Camera2D.tscn
│       └── Checkpoint.tscn
├── scripts/
│   ├── StateMachine/
│   │   ├── StateMachine.cs
│   │   ├── State.cs
│   │   └── states/
│   │       ├── player/
│   │       └── enemies/
│   ├── Player/
│   │   ├── Player.cs
│   │   ├── PlayerHealth.cs
│   │   └── PlayerInventory.cs
│   ├── Enemies/
│   │   ├── EnemyBase.cs
│   │   ├── Demontek.cs
│   │   ├── Guard.cs
│   │   ├── Mutant.cs
│   │   ├── Goliath.cs
│   │   └── King.cs
│   ├── Systems/
│   │   ├── GameManager.cs      (autoload singleton)
│   │   ├── SaveSystem.cs
│   │   ├── AudioManager.cs     (autoload singleton)
│   │   └── InputManager.cs
│   └── UI/
│       ├── HUD.cs
│       ├── BossHealthBar.cs
│       └── VirtualControls.cs
└── export_presets.cfg          (Windows, Android, iOS configs)
```

### 9.2 State Machine Base (C#)

```csharp
// State.cs — abstract base
public abstract class State : Node
{
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void HandleInput(InputEvent @event) {}
    public virtual void Update(double delta) {}
    public virtual void PhysicsUpdate(double delta) {}
}

// StateMachine.cs — drives transitions
public partial class StateMachine : Node
{
    [Export] public State InitialState { get; set; }
    private State _current;

    public override void _Ready() => TransitionTo(InitialState);

    public void TransitionTo(State next)
    {
        _current?.Exit();
        _current = next;
        _current.Enter();
    }

    public override void _Input(InputEvent @event) => _current?.HandleInput(@event);
    public override void _Process(double delta) => _current?.Update(delta);
    public override void _PhysicsProcess(double delta) => _current?.PhysicsUpdate(delta);
}
```

### 9.3 Platform Export Configuration

```
[Windows]
  platform = Windows Desktop
  architecture = x86_64

[Android]
  platform = Android
  min_sdk = 24 (Android 7.0)
  target_sdk = 34
  screen_orientation = landscape
  virtual_keyboard = false  # handled by VirtualControls scene

[iOS]
  platform = iOS
  min_version = 14.0
  screen_orientation = LandscapeLeft + LandscapeRight
```

### 9.4 Hit-Stop Implementation

```csharp
// Called by AttackState on successful hit
public static async Task HitStop(float duration = 0.05f, float targetScale = 0.1f)
{
    Engine.TimeScale = targetScale;
    await ToSignal(Engine.GetMainLoop() as SceneTree,
                   SceneTree.SignalName.ProcessFrame);
    var tween = CreateTween();
    tween.TweenProperty(Engine.GetSingleton("Engine"), "time_scale",
                        1.0f, duration)
         .SetTrans(Tween.TransitionType.Cubic)
         .SetEase(Tween.EaseType.Out);
}
```

### 9.5 Save System

Progress is stored in `user://save.json` using Godot's user data directory (works cross-platform). Saved state includes: current level, respawn checkpoint, unlocked weapons, HP.

---

## 10. Asset Pipeline

### 10.1 Characters (PixelLab MCP)

All characters are generated at **128×128 pixels** with **8 directional views** (or 4 for simpler NPCs). Canvas size 128 px means:
- Explorer, Demontek, Guard, Mutant: character fills ~60% of canvas (normal-sized).
- King and Goliath: character fills 100% of canvas (imposing scale).

| # | Character | PixelLab Description | Size | Body Type |
|---|---|---|---|---|
| 1 | Explorer | Indiana Jones-style adventurer, khaki shirt, rolled sleeves, knife on belt | Normal (60%) | humanoid |
| 2 | Demontek | Small reddish alien demon, reptilian skin, tiny horns, fanged grin | Normal (60%) | humanoid |
| 3 | Guard | Demontek in military uniform, cap, belt with badge | Normal (60%) | humanoid |
| 4 | Mutant Demontek | Demontek with enormous muscles, bulging eyes, crazed expression, cracks in skin | Normal (60%) | humanoid |
| 5 | King Demontek | Giant reddish Demontek, long beard, ornate crown, staff, imposing | Full tile | humanoid |
| 6 | Goliath | Massive fusion of Demontek and monster, armored fists, scarred, intimidating | Full tile | humanoid |

**Animations required for each character** (via `animate_character`):
`idle`, `walking`, `running-6-frames`, `jumping-1`, `attack` (light, medium, heavy mapped to `lead-jab`, `cross-punch`, `high-kick`), `taking-punch`, `falling-back-death`

### 10.2 Tilesets & Backgrounds

Generated via PixelLab's `create_sidescroller_tileset` and `create_isometric_tile` for decorative tiles.

| Scene | Tileset Description |
|---|---|
| Jungle Canopy | Giant alien tree trunks, purple/green foliage, glowing moss platforms |
| Dungeon | Dark stone bricks, iron bars, torch sconces, damp floor |
| Tunnel | Raw earth tunnel, wooden supports, glowing fungi |
| Courtyard | Reddish stone paving, fortress walls, alien torches |
| Castle Interior | Dark stone walls, suits of alien armour, red banners |
| Castle Staircase | Spiral stone stairs, iron railings, consistent with interior |
| Castle Terrace | Open ramparts, alien sky horizon, stone altar |

### 10.3 Parallax Background Layers

Each background uses 5 layers scrolling at different speeds:
- Layer 1 (farthest): Sky / distant fog — speed 0.05×
- Layer 2: Distant scenery silhouettes — speed 0.15×
- Layer 3: Mid background details — speed 0.35×
- Layer 4: Near background objects — speed 0.65×
- Layer 5 (closest): Foreground overlay (leaves, mist) — speed 0.9×

---

## 11. Development Workflow (Vibe Coding via Antigravity / OpenCode)

### 11.1 Recommended Workflow

1. **Ideate & Describe** — use this GDD as the single source of truth in the AI context.
2. **Scene Scaffold** — prompt the AI to generate empty Godot scene files and C# scripts following the architecture in §9.1.
3. **State Machine First** — implement and test the player state machine before level design begins.
4. **Asset Generation Loop**:
   - Describe the character/tile in natural language.
   - Call PixelLab MCP → receive sprite sheet.
   - Import into Godot → configure `AnimationPlayer` or `AnimatedSprite2D`.
   - Iterate descriptions until visual quality is satisfactory.
5. **Level Iteration** — build levels in Godot's TileMap editor using generated tilesets.
6. **Enemy AI** — implement one enemy type at a time, test fully before next.
7. **Polish Pass** — hit-stop, screen shake (`Camera2D` offset tween), particle VFX, audio.
8. **Platform Test** — run on desktop first, then export to Android emulator, then iOS simulator.

### 11.2 Prompting Tips for Antigravity / OpenCode

- Always include the GDD section number when asking for code: *"Implement the StateMachine from §9.2"*.
- Reference Levrault's pattern explicitly: *"Use the same state-driven enemy AI pattern as github.com/Levrault/godot-2d-action-adventure-platformer-demo"*.
- For PixelLab calls, always specify: tile size, body type, detail level, shading, outline, and whether the character should fill the tile.
- Keep the AI session context loaded with the full GDD to avoid architectural drift.

---

## 12. Milestones

| Milestone | Deliverable |
|---|---|
| M0 — Setup | Godot project created, export presets configured, PixelLab assets for Explorer + Demontek |
| M1 — Core Loop | Player state machine complete, knife combat, one test level |
| M2 — Part 1 Complete | Jungle Canopy level playable, all 10 Demonteks, HUD |
| M3 — Part 2 Complete | Dungeon + Tunnel levels, Guard AI, armoury discovery |
| M4 — Part 3 (Castle) | All castle floors playable, Mutant AI, weapon pickups |
| M5 — Boss Fights | Goliath + King Demontek fully implemented |
| M6 — Polish | Cutscenes, audio, hit-stop, screen shake, parallax |
| M7 — Platform QA | Windows build, Android APK, iOS IPA — all tested |
| M8 — Release | Final asset pass, main menu, credits, build submission |

---

## 13. Appendix A — Key References

- **Levrault Demo (Inspiration)**: https://github.com/Levrault/godot-2d-action-adventure-platformer-demo
- **Godot 4 C# Docs**: https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/
- **Godot Mobile Export**: https://docs.godotengine.org/en/stable/tutorials/export/
- **PixelLab MCP**: Pixel art character and tile generator (integrated via MCP tool calls)
- **Antigravity / OpenCode**: AI-assisted vibe coding environment for Godot

---

*Document maintained alongside the project. Update version number on each structural change.*
