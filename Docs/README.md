# Demonteks 🎮

> **A polished, feature-complete 2D action platformer built in Godot 4.6 using C#.** Fight alien demons, discover ancient weapons, explore hand-crafted environments, and escape back to Earth.

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Story](#-story)
- [Features](#-features)
- [Technical Stack](#-technical-stack)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Gameplay & Controls](#-gameplay--controls)
- [Weapon Progression](#-weapon-progression)
- [Enemy AI & Bosses](#-enemy-ai--bosses)
- [Level Design](#-level-design)
- [Save & Persistence](#-save--persistence)
- [Building & Exporting](#-building--exporting)
- [Credits](#-credits)
- [License](#-license)

---

## 🎯 Overview

**Demonteks** is a fast-paced 2D action platformer inspired by classics like *Castlevania* and modern indie hits like *Celeste* and *Hollow Knight*. The game features:

- **Intense Combat**: A responsive 6-weapon melee system with satisfying hitstops, screenshakes, and visual feedback.
- **Platforming Challenges**: Precise, physics-based controls with coyote time and jump buffering.
- **Intelligent Enemy AI**: 5 unique enemy types powered by a hierarchical state machine.
- **Dynamic Progression**: A complete JSON-based save system with checkpoints, real-time statistics, and auto-saves.
- **Cross-Platform Design**: Fully optimized for Desktop (keyboard) and Mobile (virtual touch joystick and custom buttons).

---

## 📚 Story

### Part 1: The Awakening (Level 1 - Jungle)
The explorer wakes up disoriented on an alien planet under a violet sky, surrounded by giant plants. He encounters small, reddish alien demons—the **Demonteks** (named after the "tek" sounds they make). Believing they are harmless, the explorer jokes about their appearance, provoking their rage. Armed only with a basic survival Knife, he must jump across treetop platforms and fight his way to safety.

### Part 2: The Capture (Level 2 - Dungeon/Ruins)
Seeking refuge in ancient ruins, the explorer is ambushed and captured by the Demonteks. He wakes up locked in a reddish stone dungeon deep inside the Demontek territory. Guided by mysterious writings describing ancient portals and a friendly captive alien who signs a way out, the explorer escapes through a dangerous sewer tunnel, neutralizes the guards, and retrieves ancient weapons hidden in the ruins.

### Part 3: The Infiltration (Level 3 - Castle)
The explorer reaches the towering Castle of the Demonteks. To bypass the heavy front defenses, he must infiltrate from the back, avoiding floor traps that trigger massive alien ambushes. Fighting floor by floor, he battles basic Demonteks, muscular mutants, and the massive mid-boss **Goliath** before ascending the spiral stairs to the terrace.

### Part 4: The Finale (The Terrace)
On the castle terrace, the explorer faces **King Demontek** in an epic final battle. Defeating the King opens the portal back to Earth. As the explorer steps through, the dimensional link is severed, but the threat looms: this is only the beginning of a larger conflict between humans and Demonteks.

---

## ✨ Features

- ⚔️ **Responsive Combat** - 6 unique melee weapons with tailored damage, speed, and ranges.
- 🏃 **Fluid Platforming** - Custom physics loop with coyote time and jump buffering.
- 🧠 **Hierarchical State Machines** - Custom FSM structure powering clean, predictable states for both the player and enemies.
- 💾 **State Persistence** - JSON serialization for player health, unlocked weapons, current weapon, level progression, and stats.
- 📊 **Statistics Tracking** - Real-time monitoring of deaths, kills, damage taken, and playtime.
- 📱 **Mobile & Desktop Support** - Custom virtual joystick overlay with auto-detection for touch inputs.
- 🎬 **Storyboards** - Fully integrated story intro, cinematics, level transition system, and victory scenes.

---

## 🛠 Technical Stack

### Engine & Language
- **Godot Engine**: 4.6+ with .NET support
- **C# Version**: .NET 8.0+
- **Target Platforms**: Windows (Desktop), Android, iOS (Mobile)

### Development Tools
- **Asset Creation**: PixelLab MCP (character sprites, animations, and tilesets), ElevenLabs MCP (sound effects), layer.ai (parallax backgrounds)
- **Codebase Management**: Visual Studio Code, Antigravity C# IDE
- **Version Control**: Git

---

## 🏗 Architecture

Demonteks utilizes a modular, event-driven design avoiding the overhead of MVVM, prioritizing fast performance and clean code separation:

- **Entity-Component Pattern**: Character properties (stats, combat, physics) are split into standalone nodes.
- **Signal-Based Communication**: Components talk to each other and the UI using Godot's C# event signals, preventing tight coupling.
- **Autoload Managers**: Singletons like `GameManager`, `SaveManager`, and `AudioManager` handle global game flow, persistence, and mix/audio routing.
- **Hierarchical State Machine**:
  - **Player States**: `Idle`, `Move`, `Jump`, `Fall`, `Attack`, `Hurt`, `Death`
  - **Enemy AI States**: `Idle`, `Patrol`, `Chase`, `Attack`, `Return`, `Hurt`, `Death`

---

## 📁 Project Structure

```
demonteks/
├── Assets/
│   ├── characters/           # Pixel-art sheets (Explorer, Demonteks, Bosses)
│   ├── backgrounds/          # 3-layer parallax backgrounds (Jungle, Dungeon, Castle)
│   ├── audio/               # Complete sound effects and background music
│   ├── tilesets/            # 16x16px collision-mapped tilesets (Jungle, Dungeon, Castle)
│   └── decorations/         # Ambient decorations (vines, alien trees, crystals)
├── Scenes/
│   ├── Characters/
│   │   ├── Player.tscn       # Explorer character with camera and combat components
│   │   └── Enemies/          # All 5 enemy scenes
│   ├── Levels/
│   │   ├── Level1_Jungle.tscn
│   │   ├── Level2_Dungeon.tscn
│   │   └── Level3_Castle.tscn
│   ├── UI/
│   │   ├── MainMenu.tscn
│   │   ├── HUD.tscn          # Health, weapon, level, and mercy tracker
│   │   ├── PauseMenu.tscn
│   │   ├── GameOver.tscn     # Displays player stats on death
│   │   └── Victory.tscn      # Displays completion time, stats, and victory ending
│   └── Prefabs/
│       ├── Checkpoint.tscn   # Saves progress and sets respawn locations
│       ├── WeaponPickup.tscn # Collectible weapon node
│       └── Trap.tscn         # Ambush trap triggering spawns
├── Scripts/
│   ├── Player/               # PlayerController.cs, PlayerCombat.cs, PlayerStats.cs
│   ├── Enemy/                # DemontekAI.cs (AI logic and state transitions)
│   ├── Management/           # GameManager.cs, SaveManager.cs, AudioManager.cs
│   ├── UI/                   # HUDController.cs, Menu controllers, StoryIntroController.cs
│   └── Utils/                # Checkpoint.cs, CameraFollow.cs, KillZone.cs
├── project.godot             # Godot project settings
├── Demonteks.csproj          # C# project references
└── Demonteks.sln             # C# solution solution
```

---

## 🎮 Gameplay & Controls

### Desktop (Keyboard)

| Action | Key | Description |
|--------|-----|-------------|
| **Move Left** | `A` | Move explorer left |
| **Move Right**| `D` | Move explorer right |
| **Jump**      | `W` / `Space` | Jump (hold for higher jump, coyote time active) |
| **Run**       | `Shift` | Toggle running speed (1.5x multiplier) |
| **Attack**    | `E` | Strike with current weapon |
| **Interact**  | `F` | Interact with checkpoints, pickups, and portals |
| **Next Weapon** | `Q` | Cycle to next unlocked weapon |
| **Prev Weapon** | `Z` | Cycle to previous unlocked weapon |
| **Pause**     | `ESC` | Open Pause Menu (freezes game state) |

### Mobile (Touch Controls)
- **Virtual Joystick**: Positioned on the left side of the screen for character movement.
- **Interactive Touch Buttons**: Custom buttons on the right side of the screen mapped for Jump, Attack, and Weapon cycling.

---

## ⚔️ Weapon Progression

The game features six distinct melee weapons that unlock sequentially. Unlocks are saved automatically at checkpoints:

| Weapon | Damage | Attack Delay | Story Location |
|--------|--------|--------------|----------------|
| **Survival Knife** | 10 | 1.2s | Spawn weapon (Level 1 starting area) |
| **Machete** | 20 | 0.9s | Found at the end of Level 1 (Jungle) |
| **Sword** | 30 | 1.0s | Hidden in ruins (Level 2 Dungeon) |
| **Demontek Saber** | 35 | 1.1s | Guard tower chamber (Level 2 Courtyard) |
| **Sawtooth Blade** | 40 | 0.8s | Looted on Floor 2 of Level 3 (Castle) |
| **Alien Sword** | 50 | 1.0s | Left chamber on Floor 3 (before Goliath) |

---

## 👹 Enemy AI & Bosses

All enemies use a custom **Hierarchical State Machine** matching their specific archetypes:

1. **Normal Demontek**: Basic patrolling unit. Alert range of 150px. Attacks with short punches. (20 HP, 5 Damage)
2. **Guard Demontek**: Elite patrol sentinel. Alerts at 200px. High alert speed. (25 HP, 5 Damage)
3. **Mutant Demontek**: Fast, heavily armored muscular variant. Chases aggressively. (50 HP, 10 Damage)
4. **Goliath (Mid-Boss)**: Giant mutated beast. Locks the player in the arena on Floor 3 of the Castle. (200 HP, 20 Damage)
5. **King Demontek (Final Boss)**: Leader of the colony on the Castle Terrace. Features multiple combat phases, heavy screenshake, and custom animations. (500 HP, 30 Damage)

---

## 🗺️ Level Design

### Level 1: Jungle Canopy
A vibrant alien jungle with hand-painted treetops and 6 platforms of varying heights. Players must navigate gaps, fight/dodge 10 patrolling Demonteks, pick up the Machete, and reach the exit portal.

### Level 2: Dungeon Escape
A dark, atmospheric stone fortress. The player starts locked in a prison cell (active respawn checkpoint). Players must escape through sewer tunnels containing 3 Guard Demonteks and find the hidden chamber with the Sword and Demontek Saber.

### Level 3: Castle Infiltration
An epic 4-floor siege:
- **Exterior**: Back entrance filled with floor traps triggering 6-enemy ambushes.
- **Floor 1**: Ground floor filled with stone columns and basic Demonteks.
- **Floor 2**: Intermediate barracks holding Mutant Demonteks and advanced weapon pickups.
- **Floor 3**: Large open hall hosting the massive Goliath boss fight.
- **Terrace**: Castle rooftop hosting the final battle with King Demontek and the portal back to Earth.

---

## 💾 Save & Persistence

The game features a robust serialization system handled by `SaveManager.cs`:
- **File Format**: Clean JSON saved locally in the user's application data (`user://demonteks_save.json`).
- **Autosave**: Triggers automatically every 60 seconds of active playtime.
- **Checkpoints**: Activating a green checkpoint flag saves the player's health, unlocked weapons, current weapon, level progression, and stats.
- **Respawn System**: Dying in combat or falling into the void instantly loads the last active checkpoint.

---

## 📦 Building & Exporting

Ensure you have **Godot 4.6+ (.NET)** and **.NET SDK 8.0** installed.

### Compiling the Solution
To compile the C# project assemblies:
```bash
# Debug Build
godot --build-solutions

# Production Build
dotnet build -c Release
```

### Exporting for Desktop (Windows)
```bash
# From command line
godot --export-release "Windows Desktop" demonteks.exe
```

### Exporting for Mobile (Android)
1. Install Godot's Android Build Templates inside the editor: `Project > Install Android Build Template`.
2. Generate a custom release keystore:
   ```bash
   keytool -genkey -v -keystore demonteks.keystore -alias demonteks -keyalg RSA -keysize 2048 -validity 10000
   ```
3. Enter your keystore credentials in the Android Export settings.
4. Export the project:
   ```bash
   godot --export-release Android demonteks.apk
   ```

---

## 👥 Credits

- **Game Design & Programming**: Mariano Liceaga
- **Visuals**: PixelLab MCP & layer.ai
- **Audio & SFX**: ElevenLabs C# Sound Synthesizer
- **Physics Models**: Inspired by Celeste and Hollow Knight character models

---

## 📜 License

This project is licensed under the GNU Affero General Public License v3.0 - see the `LICENSE` file in the root directory for details.
