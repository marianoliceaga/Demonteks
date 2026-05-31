# 🎮 Demonteks - Complete Game Guide

Welcome to the definitive guide for **Demonteks**, a highly polished C#-driven 2D action platformer. This document covers the game's mechanics, level structure, combat flow, and general gameplay strategy.

---

## 🎯 Gameplay Core Loop

The core loop of **Demonteks** blends fast-paced melee combat and precise platforming across three levels:

```
                  ┌───────────────────────┐
                  │       Main Menu       │
                  └───────────┬───────────┘
                              │  New Game
                  ┌───────────▼───────────┐
                  │ Level 1: Jungle       │ ◄─── Survival Knife start
                  └───────────┬───────────┘
                              │  portal
                  ┌───────────▼───────────┐
                  │ Level 2: Dungeon      │ ◄─── Escape cell, find Sword & Saber
                  └───────────┬───────────┘
                              │  portal
                  ┌───────────▼───────────┐
                  │ Level 3: Castle       │ ◄─── Vertical siege with traps
                  └───────────┬───────────┘
                              │
            ┌─────────────────┴─────────────────┐
     [ Goliath Boss ]                    [ King Demontek ]
   Floor 3, 200 HP Arena                Terrace, 500 HP Finale
            │                                   │
            └─────────────────┬─────────────────┘
                              │
                    ┌─────────▼─────────┐
                    │      Victory      │
                    └───────────────────┘
```

---

## ⚙️ Core Platforming & Mechanics

To master Demonteks, players must get comfortable with the precise physics-based controls:

### 1. Jump Physics
- **Coyote Time (0.1s)**: Gives the player a brief 0.1-second window to jump after walking off a platform edge. Crucial for completing Level 1's treetop gaps.
- **Jump Buffer (0.15s)**: If you press the jump key up to 0.15 seconds before touching the ground, the jump executes immediately upon landing.
- **Variable Jump Height**: Holding down the jump key (`W` or `Space`) yields a higher jump. Releasing it early cuts vertical velocity, ideal for tight ceiling sections in Level 2.
- **Double Jump**: The explorer has a double-jump ability that allows executing an additional jump mid-air. Perfect for correcting jump arcs.

---

## ⚔️ Melee Combat & Weapon Progression

Melee combat is snappy, featuring brief **hitstops (micro-freezes)** and **screenshakes** that match the impact of each weapon. The explorer unlocks weapons progressively, which are persisted automatically at checkpoints:

```
[ Survival Knife ] ──► [ Machete ] ──► [ Sword ] ──► [ Demontek Saber ] ──► [ Sawtooth ] ──► [ Alien Sword ]
      10 dmg              20 dmg         30 dmg            35 dmg            40 dmg             50 dmg
```

1. **Survival Knife** (10 HP Damage, 1.2s delay): Spawn weapon found in the starting canopy of Level 1. Fast but low range.
2. **Machete** (20 HP Damage, 0.9s delay): Unlocked near the exit portal of Level 1. Balanced starting blade.
3. **Sword** (30 HP Damage, 1.0s delay): Hidden inside a brick chamber in the Level 2 Dungeon. Excellent reach.
4. **Demontek Saber** (35 HP Damage, 1.1s delay): Recovered from the elite guard tower in the Level 2 Courtyard.
5. **Sawtooth Blade** (40 HP Damage, 0.8s delay): Found locked inside a barracks chest on Floor 2 of the Level 3 Castle. High attack speed.
6. **Alien Sword** (50 HP Damage, 1.0s delay): The ultimate weapon, located in a chest on Floor 3 of the Castle right before the Goliath arena. Required for maximum damage output during the final encounters.

---

## 👹 Enemy Archetypes & Combat Guide

Enemies are governed by an intelligent state machine and require distinct tactical approaches:

### 1. Normal Demontek (20 HP, 5 Damage)
- **Behavior**: Patrols platforms, alerts at 150px, and chases using standard running speeds. Attacks with a basic cross punch.
- **Counter**: Easy to dispatch with standard jump-attacks.

### 2. Guard Demontek (25 HP, 5 Damage)
- **Behavior**: Blue-tinted sentinels guarding cells and ruins in Level 2. Alert range is expanded to 200px and they patrol with high alertness.
- **Counter**: Guards have larger sight lines, but patrol slow routes. Wait until they turn away before engaging.

### 3. Mutant Demontek (50 HP, 10 Damage)
- **Behavior**: Muscular, 1.15x-scaled red demons found in the castle barracks of Level 3. Highly aggressive with extremely fast chase speeds.
- **Counter**: Keep your distance! Their attacks deal double damage. Utilize jump-back attack patterns or run to trigger one-way platform jumps where they cannot easily climb.

---

## 👑 Boss Battle Strategies

Both bosses display dedicated health bars at the bottom of the HUD and feature lock-in mechanics.

### Boss 1: Goliath (200 HP, 20 Damage)
Located in the Floor 3 arena of the Castle. On entering, heavy stone gates close behind you.
- **Attack Phase**: Goliath moves slowly but deals devastating damage. He periodically executes a ground smash that causes a heavy screen jitter.
- **Strategy**: Wait for Goliath to wind up his ground smash, jump over the shockwave, landing directly behind him to land a series of hits. Use the Sawtooth or Alien Sword for maximum damage.

### Boss 2: King Demontek (500 HP, 30 Damage)
Located on the Castle Terrace under the open sky.
- **Phase 1 (500 - 300 HP)**: Standard royal swings. Keep moving and use short jumps to hit the King's head.
- **Phase 2 (300 - 150 HP)**: King gains a red rage aura, increasing his move speed and cutting his attack cooldowns in half. Dash away when he initiates his 3-hit combo.
- **Phase 3 (150 - 0 HP)**: The King unleashes rapid strikes. You must dodge quickly, jump over his shockwaves, and execute counter-attacks during his weapon recovery frames.

---

## 💡 Pro Tips for Playtesting & Speedrunning

1. **Void Survival**: If you fall into a void pit, you will take 20 damage and respawn at the last active checkpoint. However, enemies pushed into the void as part of your attack will die permanently.
2. **Aggro Reset**: In Level 2, if you are being chased by multiple Guard Demonteks, run to the nearest checkpoint flag. Touching a checkpoint resets enemy aggression patterns temporarily, giving you time to plan your escape.
3. **Trap Courtyard**: Be careful when running across Level 3's exterior courtyard. Traps trigger heavy ambushes that can quickly overwhelm your health bar. Take out the spawned enemies one by one or leap past them quickly to reach the castle entrance door.
