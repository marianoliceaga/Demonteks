# Demonteks 🎮

> A 2D action platformer where an explorer must fight through alien demons to escape an alien world and return to Earth.

![Version](https://img.shields.io/badge/version-0.1.0--alpha-blue)
![Godot](https://img.shields.io/badge/Godot-4.3%2B-blue)
![C#](https://img.shields.io/badge/C%23-.NET%208.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Android%20%7C%20iOS-green)
![License](https://img.shields.io/badge/license-MIT-green)

## 📖 Table of Contents

- [Overview](#overview)
- [Story](#story)
- [Features](#features)
- [Technical Stack](#technical-stack)
- [Installation](#installation)
- [Gameplay](#gameplay)
- [Level Design](#level-design)
- [Character Guide](#character-guide)
- [Weapons System](#weapons-system)
- [Enemy Types](#enemy-types)
- [Boss Fights](#boss-fights)
- [Controls](#controls)
- [Project Structure](#project-structure)
- [Development Guide](#development-guide)
- [Asset Generation](#asset-generation)
- [Building & Export](#building--export)
- [Architecture](#architecture)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [Credits](#credits)
- [License](#license)

---

## 🎯 Overview

**Demonteks** is a fast-paced 2D action platformer inspired by classics like Castlevania and modern indie hits like Celeste. The game combines:

- **Intense Combat**: Melee weapons with satisfying feedback
- **Platforming Challenges**: Jump across treetops, navigate dungeons, infiltrate castles
- **Enemy Variety**: From basic Demonteks to mutants, guards, and epic boss fights
- **Multiple Endings**: Mercy route (pacifist) vs Combat route
- **Mobile-First Design**: Touch controls optimized for mobile devices
- **Cross-Platform**: Windows, Android, and iOS support

The game is built with **Godot 4.3+** using **C#** for robust, maintainable code architecture, and features pixel art assets generated with **PixelLab**.

---

## 📚 Story

### Part 1: The Awakening (Level 1 - Jungle)

*The explorer wakes up confused, stands up, and looks at the sky—it is violet. The plants are giant compared to those on Earth; everything suggests a jungle on another planet. Immediately after, the explorer encounters a new alien species.*

*At first, he believes they are friendly and adorable, but they are actually an invasive species. As soon as he sees them, he starts joking about how small and silly they look. They become enraged and begin to chase him, trying to kill him, while he starts running and jumping across platforms in the treetops.*

*The aliens are like small demons, reddish in color; they are called Demonteks because they repeatedly say "tek" and their physical forms resemble demons. The explorer bears a distant resemblance to Indiana Jones, but without the hat or whip; he uses a knife.*

### Part 2: The Capture (Level 2 - Dungeon/Ruins)

*Once Part One is cleared, the explorer manages to take refuge in some ruins, but the Demonteks have already colonized them. Upon reaching the ruins, they capture the explorer, and he faints. He wakes up in a place resembling hell, with fire and almost everything colored red. He sees many Demonteks and a large castle made of reddish bricks. On the castle terrace, there is a giant Demontek with a beard and a crown: the King of the Demonteks.*

*When they bring the explorer to the King, he looks at him and orders the Demonteks in their own language to take him to the dungeon. When the explorer arrives at the dungeon, he sees other Demonteks locked up as well. One of them speaks to him, but since he doesn't understand, the alien uses signs to indicate there is a tunnel he could use to escape, though it is very dangerous.*

*In that place, there are ancient writings describing the attempted creation of portals to other dimensions and the encounter with humans. The writings also say that the Demonteks have existed since prehistory but remained hidden, which is why the exit to the real world is located in the castle. At that moment, you plan to defeat the King and then cross over to the real world.*

### Part 3: The Infiltration (Level 3 - Castle)

*The explorer must try to enter the castle stealthily by going around it and entering from the back. He must be careful and jump over traps on the floor; if he steps on one, 6 Demonteks appear and attack until they kill him, sending him back to the dungeon.*

*When the explorer enters, he faces several Demonteks using the weapons found in Part Two. As he progresses through the castle floors, he encounters increasingly powerful foes: first regular Demonteks, then muscular Mutant Demonteks, and finally the monstrous Goliath.*

### Part 4: The Finale

*Once our protagonist reaches the terrace and defeats the King Demontek, the portal to Earth opens. But before he crosses, they let him know that this is only the beginning of the war between humans and Demonteks.*

---

## ✨ Features

### Core Gameplay
- ⚔️ **Dynamic Combat System** - 6 melee weapons with unique stats
- 🏃 **Fluid Movement** - Run, jump, crouch, dodge
- 🎭 **State Machine AI** - Intelligent enemy behavior patterns
- 💾 **Save System** - Auto-save and manual checkpoints
- 📱 **Mobile Controls** - Virtual joystick and touch buttons

### Advanced Features
- 🛡️ **Multiple Endings** - Mercy route (no kills) or combat route
- 👑 **Boss Battles** - Goliath and King Demontek with unique mechanics
- 🗡️ **Weapon Progression** - Unlock better weapons as you advance
- 📊 **Statistics Tracking** - Deaths, kills, playtime, damage taken
- 🎨 **Pixel Art Style** - Retro aesthetic with modern effects
- 🌍 **Multiple Levels** - Jungle, Dungeon, Castle with unique challenges

### Technical Features
- 🏗️ **Component-Based Architecture** - Modular, maintainable code
- 🔧 **Modern C# Patterns** - Signals, async/await, LINQ
- 📐 **Hierarchical State Machines** - Clean state management
- 💽 **JSON Save System** - Human-readable save files
- 🔌 **Event-Driven Design** - Loose coupling via Godot signals
- 📏 **Pixel-Perfect Rendering** - Crisp pixel art at any resolution

---

## 🛠 Technical Stack

### Engine & Language
- **Godot Engine**: 4.3+ with .NET support
- **C# Version**: .NET 8.0+
- **Target Platforms**: Windows, Android, iOS

### Development Tools
- **IDE**: Visual Studio Code / Visual Studio 2022 / Rider
- **Version Control**: Git
- **Asset Creation**: PixelLab (via MCP), Aseprite (recommended)
- **Code Editor**: Antigravity / OpenCode for vibe coding

### Inspired By
- **Mechanics Reference**: [Levrault's 2D Platformer Demo](https://github.com/Levrault/godot-2d-action-adventure-platformer-demo)
  - State machine pattern
  - Signal-based architecture
  - Level management system
  
- **Modern C# Patterns**: [DCodius MazeWalker](https://github.com/DCodius/Game.MazeWalker)
  - Component-based design
  - Clean code structure
  - Mobile-first approach

### Architecture Pattern
- **No MVVM** - Direct component communication via signals
- **Entity-Component System** - Modular character capabilities
- **State Machines** - FSM for player and AI behavior
- **Manager Pattern** - Singleton managers for game state

---

## 💿 Installation

### Prerequisites

1. **Godot 4.3+** with .NET support
   ```bash
   # Download from godotengine.org/download
   # Ensure you get the .NET-enabled version
   ```

2. **.NET SDK 8.0+**
   ```bash
   # Check if installed
   dotnet --version
   
   # Download from dotnet.microsoft.com if needed
   ```

3. **Git** (for version control)
   ```bash
   git --version
   ```

### Quick Start

```bash
# Clone the repository
git clone https://github.com/yourusername/demonteks.git
cd demonteks

# Open in Godot Editor
godot -e project.godot

# Build C# solution (Godot will prompt)
# Or manually:
godot --build-solutions

# Run the game
# Press F5 or click the Play button in Godot Editor
```

### First-Time Setup

1. **Generate Assets** (See [ASSET_GENERATION_GUIDE.md](ASSET_GENERATION_GUIDE.md))
   - Create character sprites using PixelLab
   - Generate background layers
   - Export animation sprite sheets

2. **Configure Project**
   - Verify input mappings in Project Settings
   - Check mobile export templates are installed
   - Configure Android/iOS SDKs if building for mobile

3. **Test Build**
   ```bash
   # Desktop test
   godot --headless --build-solutions
   
   # Mobile test (requires templates)
   godot --export-debug Android
   ```

For detailed installation instructions, see [QUICK_START.md](QUICK_START.md).

---

## 🎮 Gameplay

### Core Mechanics

#### Movement System
- **Walk/Run**: Move with WASD or virtual joystick
- **Jump**: W key or touch button (with coyote time for forgiveness)
- **Crouch**: S key or touch button (dodge attacks, fit through gaps)
- **Attack**: E key or touch button (varies by weapon)
- **Interact**: F key (open portals, pick up weapons)

#### Combat System
- **Melee Weapons Only**: No ranged attacks
- **Weapon Types**: Knife → Machete → Sword → Demontek weapons
- **Damage System**: Each weapon has different power and speed
- **Hit Detection**: Precise hitbox collision for attacks
- **Invincibility Frames**: Brief immunity after taking damage

#### Health System
- **Starting HP**: 100
- **Damage Values**:
  - Normal Demontek: -5 HP
  - Mutant Demontek: -10 HP
  - Goliath: -20 HP
  - King Demontek: -30 HP
- **Death**: Respawn at last checkpoint
- **No Healing Items**: Strategic health management required

### Gameplay Loop

1. **Explore** - Navigate levels, find weapons
2. **Combat** - Fight or avoid enemies
3. **Progress** - Reach checkpoints, advance to next area
4. **Boss Fight** - Defeat Goliath and King
5. **Escape** - Enter portal to Earth

### Difficulty Progression

#### Level 1 (Jungle) - Easy
- 10 Normal Demonteks
- Focus on learning controls
- Wide platforms, forgiving jumps

#### Level 2 (Dungeon/Castle) - Medium
- Guards, more enemies
- Narrow corridors
- Traps that spawn 6 enemies

#### Level 3 (Castle Interior) - Hard
- Multiple floors
- Mutant Demonteks
- Goliath boss fight
- King Demontek final boss

### Mercy Route (Pacifist Ending)

- **Requirement**: Complete game without killing any enemies
- **Strategy**: Dodge, crouch, run past enemies
- **Reward**: Alternate ending dialogue
- **Achievement**: True pacifist

---

## 🗺 Level Design

### Level 1: Jungle Canopy

**Theme**: Purple alien jungle with giant plants
**Platforms**: Treetop branches, natural formations
**Length**: ~5 minutes
**Enemies**: 10 Normal Demonteks

**Layout**:
```
Start → Platform 1 → Platform 2 → Platform 3 → Platform 4 → Platform 5
  (2 enemies)   (2 enemies)   (3 enemies)   (2 enemies)   (1 enemy)
                                                             ↓
                                                        Ruins Entrance
```

**Challenges**:
- Learn jump timing
- First combat encounters
- Understand enemy patterns

### Level 2: Dungeon & Ruins

**Theme**: Underground prison, ancient ruins
**Areas**: Dungeon Cell → Tunnel → Courtyard → Ancient Armory
**Length**: ~8 minutes
**Enemies**: Guards, Normal Demonteks

**Layout**:
```
Dungeon Start → Escape Tunnel → Stone Puzzle → Ancient Writings
                    (Guards)      (Trap!)         ↓
                                            Weapon Cache
                                                 ↓
                                          Castle Exterior
```

**Challenges**:
- Stealth required
- Find weapon upgrades
- Avoid trap stones
- Read lore about portals

### Level 3: Castle Infiltration

**Theme**: Reddish brick castle, hellish atmosphere
**Floors**: Basement → Ground Floor → Second Floor → Third Floor → Terrace
**Length**: ~12 minutes
**Enemies**: All types, Goliath, King

**Layout**:
```
Basement (Start) → Ground Floor → Second Floor → Third Floor → Terrace
 (Guards)          (Demonteks)    (Mutants)      (Mutants)    (King Boss)
                                                      ↑
                                                  Goliath Boss
```

**Challenges**:
- Increasing enemy difficulty
- Respawn at floor 1 if you die
- Boss battle gauntlet
- Final confrontation

---

## 👤 Character Guide

### The Explorer

**Background**: Mysterious adventurer resembling Indiana Jones (without hat/whip)
**Personality**: Brave but sarcastic, underestimates threats initially
**Starting Equipment**: Combat knife

**Stats**:
- HP: 100
- Speed: 200 units/sec (300 when running)
- Jump: 400 units up
- Melee Range: 40-75 units (weapon dependent)

**Abilities**:
- Run (Shift key)
- Jump (with coyote time)
- Crouch (dodge attacks)
- Melee attack (weapon dependent)
- Weapon swapping (Q/Z keys)

---

## ⚔️ Weapons System

All weapons are melee-only with varying stats:

### Tier 1: Knife
- **Location**: Starting weapon
- **Damage**: 10
- **Speed**: 1.2 attacks/sec
- **Range**: 40 units
- **Description**: Fast but weak, good for testing enemies

### Tier 2: Machete
- **Location**: Found in dungeon armory (Level 2)
- **Damage**: 20
- **Speed**: 0.9 attacks/sec
- **Range**: 60 units
- **Description**: Heavy jungle blade, balanced stats

### Tier 3: Ancient Sword
- **Location**: Ancient ruins (Level 2)
- **Damage**: 30
- **Speed**: 1.0 attacks/sec
- **Range**: 70 units
- **Description**: Mysterious blade with good reach

### Tier 4: Demontek Saber
- **Location**: Dropped by enemies (Level 3)
- **Damage**: 35
- **Speed**: 1.1 attacks/sec
- **Range**: 65 units
- **Description**: Curved blade from Demontek arsenal

### Tier 5: Sawtooth
- **Location**: Dropped by Mutants (Level 3)
- **Damage**: 40
- **Speed**: 0.8 attacks/sec
- **Range**: 55 units
- **Description**: Brutal serrated weapon

### Tier 6: Alien Sword
- **Location**: Final floor before King (Level 3)
- **Damage**: 50
- **Speed**: 1.0 attacks/sec
- **Range**: 75 units
- **Description**: Elite weapon, best stats

### Weapon Selection

```csharp
// In-game controls
Q Key - Next weapon
Z Key - Previous weapon
E Key - Attack with current weapon

// On mobile
Swipe weapon icon to change
Tap attack button to strike
```

---

## 👾 Enemy Types

### Normal Demontek
- **HP**: 20
- **Damage**: 5
- **Speed**: 100 units/sec (chase)
- **Detection Range**: 150 units
- **Behavior**: Patrols, chases on sight, basic attack

### Guard Demontek
- **HP**: 25
- **Damage**: 5
- **Speed**: 80 units/sec
- **Detection Range**: 200 units (wide field)
- **Behavior**: Static patrol, calls reinforcements
- **Special**: Wears uniform and cap

### Mutant Demontek
- **HP**: 50
- **Damage**: 10
- **Speed**: 120 units/sec (fast!)
- **Detection Range**: 180 units
- **Behavior**: Aggressive, relentless chase
- **Special**: Exaggerated muscles, crazed expression

---

## 👑 Boss Fights

### Goliath (Mid-Boss)

**Location**: Castle Third Floor
**Type**: Demontek-Monster Fusion Experiment

**Stats**:
- HP: 200
- Damage: 20 per hit
- Speed: 70 units/sec (slow but powerful)
- Detection Range: 250 units

**Phases**:

**Phase 1** (200-100 HP):
- Slow ground pound attacks
- Wide hitbox, easy to dodge
- Telegraphed movements

**Phase 2** (100-0 HP):
- Enrages, faster attacks
- Summons 2 Normal Demonteks
- Jump attacks

**Strategy**:
- Hit-and-run tactics
- Use Ancient Sword or better
- Watch for telegraphed attacks
- Kill adds quickly in Phase 2

**Estimated Duration**: 2-3 minutes

---

### King Demontek (Final Boss)

**Location**: Castle Terrace
**Type**: Giant Demontek with Crown

**Stats**:
- HP: 500
- Damage: 30 per hit
- Speed: 90 units/sec
- Detection Range: 300 units (entire arena)

**Phases**:

**Phase 1** (500-350 HP):
- Slow sword swings
- Ground pound AOE
- Summons 2 Guards

**Phase 2** (350-150 HP):
- Faster attacks
- Fire projectiles (dodge by crouching)
- Summons 2 Mutants

**Phase 3** (150-0 HP):
- Berserker mode
- Combo attacks
- No adds, but much faster

**Strategy**:
- Use Alien Sword for max damage
- Prioritize add kills in early phases
- Learn attack patterns
- Save health for Phase 3 burst

**Mercy Route Alternative**:
- Can be spared if you haven't killed anyone
- Dialogue changes
- Portal opens without fight

**Estimated Duration**: 5-7 minutes

---

## 🎮 Controls

### Desktop (Keyboard)

| Action | Key | Description |
|--------|-----|-------------|
| Move Left | A | Move character left |
| Move Right | D | Move character right |
| Jump | W / Space | Jump (hold for higher) |
| Crouch | S / Down | Crouch/Duck |
| Run | Shift | Run faster |
| Attack | E | Attack with current weapon |
| Interact | F | Open portals, pick up items |
| Next Weapon | Q | Cycle to next weapon |
| Previous Weapon | Z | Cycle to previous weapon |
| Pause | ESC | Pause menu |

### Mobile (Touch)

| Control | Description |
|---------|-------------|
| Virtual Joystick | Left side of screen for movement |
| Jump Button | Right side, bottom |
| Attack Button | Right side, middle |
| Crouch Button | Right side, top |
| Weapon Swap | Swipe on weapon icon |

### Controller Support (Coming Soon)

Currently keyboard/touch only. Gamepad support planned for v1.0.

---

## 📁 Project Structure

```
demonteks/
├── Assets/
│   ├── characters/           # Character sprites and animations
│   │   ├── explorer/
│   │   ├── demontek/
│   │   ├── guard/
│   │   ├── mutant/
│   │   ├── goliath/
│   │   └── king/
│   ├── backgrounds/          # Level backgrounds
│   │   ├── jungle/
│   │   ├── dungeon/
│   │   └── castle/
│   ├── audio/               # Music and SFX
│   │   ├── music/
│   │   └── sfx/
│   └── fonts/               # UI fonts
│
├── Scenes/
│   ├── Characters/
│   │   ├── Player.tscn
│   │   └── Enemies/
│   │       ├── Demontek.tscn
│   │       ├── Guard.tscn
│   │       ├── Mutant.tscn
│   │       ├── Goliath.tscn
│   │       └── King.tscn
│   ├── Levels/
│   │   ├── Level1_Jungle.tscn
│   │   ├── Level2_Dungeon.tscn
│   │   └── Level3_Castle.tscn
│   ├── UI/
│   │   ├── MainMenu.tscn
│   │   ├── PauseMenu.tscn
│   │   ├── HUD.tscn
│   │   └── MobileControls.tscn
│   └── Prefabs/
│       ├── Checkpoint.tscn
│       ├── Portal.tscn
│       └── Trap.tscn
│
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs     # Main player logic
│   │   ├── PlayerStats.cs          # Health, stats
│   │   └── PlayerCombat.cs         # Weapons, attacks
│   ├── Enemy/
│   │   └── DemontekAI.cs          # Enemy AI and state machine
│   ├── Combat/
│   │   ├── Hitbox.cs
│   │   └── Hurtbox.cs
│   ├── Management/
│   │   ├── GameManager.cs         # Game state, levels
│   │   └── SaveManager.cs         # Persistence
│   ├── UI/
│   │   ├── MobileControls.cs      # Touch input
│   │   ├── HUDController.cs
│   │   └── MenuController.cs
│   └── Utils/
│       ├── StateMachine.cs
│       └── Extensions.cs
│
├── Docs/
│   ├── README.md                  # This file
│   ├── QUICK_START.md            # Setup guide
│   ├── ASSET_GENERATION_GUIDE.md # PixelLab guide
│   ├── API_REFERENCE.md          # Code documentation
│   └── DESIGN_DOCUMENT.md        # Game design doc
│
├── project.godot                 # Godot project file
├── Demonteks.csproj             # C# project file
├── Demonteks.sln                # Solution file
├── .gitignore
└── LICENSE
```

---

## 🔧 Development Guide

### Setting Up Development Environment

1. **IDE Setup**
   ```bash
   # Visual Studio Code (recommended)
   code --install-extension ms-dotnettools.csharp
   code --install-extension geequlim.godot-tools
   
   # Or use Visual Studio 2022 / JetBrains Rider
   ```

2. **Build Configuration**
   ```bash
   # Debug build
   godot --build-solutions
   
   # Release build
   dotnet build -c Release
   ```

3. **Run Tests** (Coming Soon)
   ```bash
   dotnet test
   ```

### Code Style

Following Microsoft C# conventions:
- PascalCase for public members
- camelCase for private fields (with underscore)
- Descriptive variable names
- XML documentation for public APIs

Example:
```csharp
/// <summary>
/// Handles player movement and input
/// </summary>
public partial class PlayerController : CharacterBody2D
{
    private Vector2 _velocity;
    
    /// <summary>
    /// Move the player in a direction
    /// </summary>
    public void Move(Vector2 direction)
    {
        _velocity = direction * MoveSpeed;
    }
}
```

### Architecture Patterns

#### State Machine Pattern
Used for player and enemy AI states:

```csharp
public interface IPlayerState
{
    void Enter(PlayerController player);
    IPlayerState Update(PlayerController player, float delta);
    void Exit(PlayerController player);
}

public class IdleState : IPlayerState { /* ... */ }
public class MoveState : IPlayerState { /* ... */ }
public class AttackState : IPlayerState { /* ... */ }
```

#### Signal-Based Events
Loose coupling via Godot signals:

```csharp
[Signal]
public delegate void HealthChangedEventHandler(int current, int max);

// Emit
EmitSignal(SignalName.HealthChanged, 80, 100);

// Connect
stats.HealthChanged += OnHealthChanged;
```

#### Component System
Modular character capabilities:

```
Player (CharacterBody2D)
├── PlayerStats (Node)
├── PlayerCombat (Node2D)
├── PlayerController (Script)
├── AnimatedSprite2D
└── CollisionShape2D
```

### Adding New Features

1. **New Enemy Type**
   ```csharp
   // 1. Add enum value
   public enum EnemyType { Normal, Guard, Mutant, YourNewType }
   
   // 2. Add stats in InitializeByType()
   case EnemyType.YourNewType:
       MaxHealth = 100;
       Damage = 15;
       // ...
       break;
   
   // 3. Create sprite assets
   // 4. Update animation dictionary
   ```

2. **New Weapon**
   ```csharp
   // Add to WeaponType enum
   public enum WeaponType { /* ... */, YourNewWeapon }
   
   // Add weapon data in InitializeWeapons()
   _weapons[WeaponType.YourNewWeapon] = new Weapon { /* ... */ };
   ```

3. **New Level**
   ```csharp
   // Create Level scene in Scenes/Levels/
   // Add to GameManager level list
   [Export] public string Level4Scene { get; set; } = "res://...";
   ```

---

## 🎨 Asset Generation

See [ASSET_GENERATION_GUIDE.md](ASSET_GENERATION_GUIDE.md) for complete details.

### Quick Start

1. **Character Sprites** (128x128 tiles)
   - Explorer (normal size)
   - Demontek (normal size)
   - Guard (normal size)
   - Mutant (normal size)
   - Goliath (fills tile)
   - King (fills tile)

2. **Animations Required**
   - idle (4 frames)
   - walk (8 frames)
   - run (6 frames)
   - jump (4 frames)
   - attack (6 frames)
   - hurt (3 frames)
   - death (5 frames)

3. **Background Layers**
   - Jungle (3 parallax layers)
   - Dungeon (2 layers)
   - Castle (3 layers)

### Using PixelLab (MCP)

```bash
# Generate Explorer character
pixellab generate character \
  --prompt "Rambo-style warrior, pixel art, 128x128" \
  --output assets/characters/explorer/

# Generate animation sheet
pixellab animate \
  --input explorer_base.png \
  --animations idle,walk,run,jump,attack,hurt,death \
  --output explorer_animations.png
```

---

## 📦 Building & Export

### Windows Build

```bash
# Export from Godot Editor
Project > Export > Windows Desktop

# Or command line
godot --export-release "Windows Desktop" demonteks.exe
```

### Android Build

1. **Install Android Build Template**
   ```
   Project > Install Android Build Template
   ```

2. **Configure Export Preset**
   ```
   Project > Export > Android
   
   Settings:
   - Min SDK: 26
   - Target SDK: 34
   - Architectures: arm64-v8a, armeabi-v7a
   ```

3. **Generate Keystore**
   ```bash
   keytool -genkey -v -keystore demonteks.keystore \
     -alias demonteks -keyalg RSA -keysize 2048 -validity 10000
   ```

4. **Export APK**
   ```bash
   godot --export-release Android demonteks.apk
   ```

### iOS Build (macOS only)

1. **Configure Export Preset**
   ```
   Project > Export > iOS
   
   Settings:
   - Bundle ID: com.yourstudio.demonteks
   - Min iOS: 12.0
   ```

2. **Export Xcode Project**
   ```bash
   godot --export-release iOS demonteks_ios/
   ```

3. **Build in Xcode**
   - Open .xcodeproj
   - Configure signing
   - Build for device/App Store

---

## 🏗 Architecture

### System Overview

```
┌─────────────────────────────────────────────┐
│           Game Entry Point                  │
│         (MainMenu / GameManager)            │
└─────────────────┬───────────────────────────┘
                  │
        ┌─────────▼─────────┐
        │   Level Loaded    │
        │  (Scene Manager)  │
        └─────────┬─────────┘
                  │
    ┌─────────────┼─────────────┐
    │             │             │
┌───▼───┐   ┌────▼────┐   ┌───▼───┐
│Player │   │ Enemies │   │  UI   │
└───┬───┘   └────┬────┘   └───┬───┘
    │            │            │
    │   ┌────────▼────────┐   │
    └───►  GameManager    ◄───┘
        │  (Autoload)     │
        └────────┬────────┘
                 │
        ┌────────▼────────┐
        │  SaveManager    │
        │  (Autoload)     │
        └─────────────────┘
```

### Component Communication

```
Player                  Enemy
  │                      │
  ├─ PlayerStats        ├─ DemontekAI
  │    │                │    │
  │    └─[signal]→      │    └─[signal]→
  │    HealthChanged    │    PlayerDetected
  │                     │
  ├─ PlayerCombat      StateManager
  │    │                │
  │    └─[signal]→     └─[processes]→
  │    AttackExecuted   State Transitions
  │
  └─ PlayerController
       │
       └─[input]→ Movement & Actions
```

### State Machine Flow

```
Player States:
Idle ⇄ Move ⇄ Jump → Fall → Idle
  ↓      ↓      ↓
Attack Attack Attack
  ↓      ↓      ↓
Idle   Idle   Idle

Enemy States:
Idle ⇄ Patrol → Detect → Chase → Attack
  ↑               ↓          ↓        ↓
  └───────────────┴──────────┴────────┘
                  Lost Player
```

---

## 🗓 Roadmap

### Phase 1: Core Mechanics (✅ Complete)
- [x] Player movement system
- [x] Combat system with weapons
- [x] Enemy AI with state machine
- [x] Basic level structure
- [x] Save/load system
- [x] Mobile controls

### Phase 2: Content Creation (🚧 In Progress)
- [ ] Generate all character sprites (0/6)
- [ ] Create all background layers (0/8)
- [ ] Build Level 1 - Jungle (0%)
- [ ] Build Level 2 - Dungeon (0%)
- [ ] Build Level 3 - Castle (0%)
- [ ] Implement Goliath boss fight (0%)
- [ ] Implement King boss fight (0%)

### Phase 3: Polish & Features (📅 Planned)
- [ ] Sound effects and music
- [ ] Particle effects
- [ ] Screen shake and juicy feedback
- [ ] Mercy route implementation
- [ ] Multiple endings
- [ ] Achievements system
- [ ] Localization (EN/ES)

### Phase 4: Testing & Balance (📅 Planned)
- [ ] Playtesting sessions
- [ ] Difficulty balancing
- [ ] Bug fixing
- [ ] Performance optimization
- [ ] Mobile device testing

### Phase 5: Launch (📅 Q2 2026)
- [ ] App store assets
- [ ] Marketing materials
- [ ] Google Play submission
- [ ] iOS App Store submission
- [ ] Steam release (Windows)
- [ ] Launch!

---

## 🤝 Contributing

We welcome contributions! Here's how:

1. **Fork the Repository**
2. **Create Feature Branch**
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. **Commit Changes**
   ```bash
   git commit -m 'Add AmazingFeature'
   ```
4. **Push to Branch**
   ```bash
   git push origin feature/AmazingFeature
   ```
5. **Open Pull Request**

### Contribution Guidelines

- Follow existing code style
- Add XML documentation for public APIs
- Write clear commit messages
- Test on desktop AND mobile before PR
- Update README if adding features

---

## 📜 Credits

### Development
- **Game Design**: Your Name
- **Programming**: Your Name
- **Level Design**: Your Name

### Art & Assets
- **Pixel Art**: Generated with PixelLab MCP
- **Inspiration**: Castlevania, Celeste, Hollow Knight

### Engines & Tools
- **Godot Engine**: [godotengine.org](https://godotengine.org)
- **PixelLab**: Asset generation
- **Aseprite**: Sprite editing (recommended)

### Special Thanks
- Levrault for 2D platformer demo inspiration
- DCodius for modern C# patterns
- GDQuest for educational content
- Godot community for support

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

**Note**: Asset licenses may vary. Please check individual asset sources.

---

## 📞 Contact & Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/demonteks/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/demonteks/discussions)
- **Email**: your.email@example.com
- **Discord**: Coming Soon

---

## 🎯 Quick Links

- [Download Latest Release](https://github.com/yourusername/demonteks/releases)
- [Play in Browser](https://yourusername.itch.io/demonteks) (Coming Soon)
- [Watch Gameplay Trailer](https://youtube.com/...) (Coming Soon)
- [Read Dev Blog](https://yourblog.com) (Coming Soon)

---

**Made with ❤️ using Godot Engine**

*Last updated: 2026-02-13*
