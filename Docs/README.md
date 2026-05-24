# Demonteks 🎮

THIS IS THE ROADMAP FOR THE GAME, IT'S STILL WORK IN PROGRESS.

> A 2D action platformer where an explorer must fight through alien demons to escape an alien world and return to Earth.

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
- **IDE / Code Editor**: Visual Studio Code, Antigravity, OpenCode
- **LLMs**: GLM 4.7, Sonnet 4.6, Gemini Pro 3.1
- **Version Control**: Git
- **Asset Creation**: PixelLab via MCP (characters sprites and animations), ElevenLabs via MCP (audio SFX), AI Music (music), layer.ai (level backgrounds)

## Architecture Pattern
- **No MVVM** - Direct component communication via signals
- **Entity-Component System** - Modular character capabilities
- **State Machines** - FSM for player and AI behavior
- **Manager Pattern** - Singleton managers for game state

---
## 🎮 Controls

### Desktop (Keyboard)

| Action | Key | Description |
|--------|-----|-------------|
| Move Left | A | Move character left |
| Move Right | D | Move character right |
| Jump | W / Space | Jump (hold for higher) |
| Crouch | S / Down | Crouch/Duck | **NOT IMPLEMENTED**
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

Currently keyboard/touch only. Gamepad support planned for v1.5.

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
- **Email**: mliceaga@gmail.com
- **Discord**: https://discord.gg/Ey5j8hDBMN

---

## 🎯 Quick Links

- [Download Latest Release](https://github.com/yourusername/demonteks/releases)
- [Play in Browser](https://yourusername.itch.io/demonteks) (Coming Soon)
- [Watch Gameplay Trailer](https://youtube.com/...) (Coming Soon)
- [Read Dev Blog](https://yourblog.com) (Coming Soon)

---

*Last updated: 2026-03-08*
