# DEMONTEKS — Game Design Document
### Version 1.0.0 | 2D Action Platformer | Godot 4.6 (C#) | Windows · Android · iOS

---

## 1. Game Overview

| Field | Details |
|---|---|
| **Title** | Demonteks |
| **Genre** | 2D Action-Adventure Platformer |
| **Engine** | Godot 4.6 — C# scripting |
| **Target Platforms** | Windows Desktop, Android, iOS |
| **Art Style** | Retro pixel art — 128×128 characters, 3-layer parallax backgrounds |
| **Player Count** | Single-player |

---

## 2. Story Synopsis

The explorer wakes up disoriented in an alien jungle under a violet sky. Giant alien flora towers around him. He is immediately confronted by the Demonteks — small, reddish, demon-like creatures that repeat the word "tek." Mistaking them for harmless curiosities, the explorer jokes about their appearance, provoking their rage. Armed only with a basic survival Knife, he must jump across treetop platforms and fight his way to safety.

After surviving the treetop platforms, the explorer finds refuge in ancient ruins — only to be captured. He wakes in a hellish underground realm: the Demonteks' world. From a dungeon, he escapes through a sewer tunnel, discovers ancient weapons in hidden ruins, and storms the King's fortress. His ultimate goal: defeat the Demontek King, activate the inter-dimensional portal, and return to Earth.

---

## 3. Narrative & Level Structure

### Level 1 — Jungle Canopy
Treetop platforming across jungle branches. The explorer must navigate platforms and gaps, dodging or fighting 10 patrolling Demonteks. He discovers his survival Knife near the starting area and the Machete near the exit portal. This level serves as the mechanics tutorial.

### Level 2 — Dungeon Escape
Following capture, the explorer wakes up imprisoned in a cell (active checkpoint spawn). He escapes through narrow, dark tunnels containing 3 Guard Demonteks. An interactive stone button opens a hidden chamber containing the ancient Sword and the Demontek Saber.

### Level 3 — Castle Infiltration
An epic vertical siege:
- **Exterior**: Back entrance filled with ambush floor traps. Stepping on a trap spawns 6 Demonteks.
- **Floor 1**: Ground floor barracks with basic Demonteks.
- **Floor 2**: Hall containing Mutant Demonteks and the Sawtooth blade pickup.
- **Floor 3**: Large open room holding the ultimate Alien Sword and the giant Goliath boss arena (gates lock upon entering).
- **Terrace**: Castle rooftop hosting King Demontek, the final encounter, and the Earth portal.

---

## 4. Core Mechanics

Every character (player and enemies) is built on a clean **Hierarchical Finite State Machine (HFSM)** managed through modular C# components.

### 4.1 Player State Machine
```
PlayerStateMachine
├── GroundedState
│   ├── IdleState
│   ├── WalkState
│   ├── RunState (Shift key - 1.5x speed)
│   └── AttackState (E key - triggers equipped weapon swing)
├── AirState
│   ├── JumpState
│   └── FallState
├── HurtState (takes punch, invincibility frames trigger)
└── DeathState (triggers GameOver and respawn at last checkpoint)
```

### 4.2 Controls

#### Desktop (Keyboard)
- **Move Left**: `A`
- **Move Right**: `D`
- **Jump**: `W` / `Space` (variable height, holding yields a higher jump)
- **Attack**: `E`
- **Interact / Pick Up**: `F`
- **Run**: `Shift` (holds to toggle running speed)
- **Next Weapon**: `Q`
- **Prev Weapon**: `Z`
- **Pause**: `ESC`

#### Mobile (Virtual touch joystick)
A custom virtual joystick on the left side of the screen handles movement. Tap buttons on the right side handle Jump, Attack, and Weapon swap actions. Auto-detects touch capability to show/hide the HUD overlay.

---

## 5. Combat & Weapon Specifications

Melee attacks have tactile impact, executing brief **hitstops (time-scale freeze)** and **screenshakes** that scale with weapon magnitude.

### 5.1 Player Weapons
The player can unlock six distinct weapons, which persist in the local JSON save file:

| Weapon | Base Damage | Cooldown (s) | Range (px) | Description |
|---|---|---|---|---|
| **Knife** | 10 | 0.40 | 40 | Simple survival knife. Fast but short-ranged. |
| **Machete** | 20 | 0.55 | 60 | Heavy jungle blade. Balanced speed and power. |
| **Ancient Sword** | 30 | 0.50 | 70 | Ruined broadsword. Excellent horizontal reach. |
| **Demontek Saber** | 35 | 0.45 | 65 | Elite curved saber. High attack speed. |
| **Sawtooth** | 40 | 0.60 | 55 | Serrated war blade. Devastating close-range force. |
| **Alien Sword** | 50 | 0.50 | 75 | Elite final broadsword. Unrivaled range and damage. |

---

## 6. Enemy AI & Specifications

All enemies are driven by the `DemontekAI.cs` controller. It uses raycasts for safe ledge detection and obstacle step-climbing (up to 14px heights).

### 6.1 Stats Table
| Enemy Archetype | Max HP | Damage | Chase Speed | Detection Range | Attack Cooldown |
|---|---|---|---|---|---|
| **Normal Demontek** | 15 | 8 | 130 | 180px | 0.6s |
| **Guard Demontek** | 30 | 15 | 120 | 250px | 0.6s |
| **Mutant Demontek** | 45 | 20 | 160 | 220px | 0.6s |
| **Goliath (Mid-Boss)** | 113 | 25 | 100 | 300px | 0.8s |
| **King Demontek (Final Boss)**| 248 | 30 | 110 | 350px | 1.4s |

### 6.2 Boss Phase Mechanics
- **Goliath**: Giant 2x-scaled monster. Ground slam hits execute screenshakes and a large-radius AoE damage box. Gates lock the player in the arena on Floor 3 of the Castle.
- **King Demontek**: Terrace boss with 3-phase progression. As health drains, move speed increases and attack cooldowns cut in half. The final phase adds lightning visual sweeps requiring fast jumps to dodge.

---

## 7. Systems & Persistence

### 7.1 Save & Checkpoints
- **SaveManager.cs**: Local JSON serialization saving to `user://demonteks_save.json`. Stores player health, unlocked weapons, current weapon, level progression, and stats.
- **Autosave**: Fires automatically every 60 seconds of active playtime.
- **Checkpoints**: Touching a checkpoint flag saves progress. Falling in the void (takes 20 damage) or dying in combat immediately respawns the explorer at the last active flag.
