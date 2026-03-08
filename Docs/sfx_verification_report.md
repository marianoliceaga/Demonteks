# SFX Verification Report

**Important Context:** The Python script [check_unused_sfx.py](file:///d:/Demonteks/check_unused_sfx.py) searches the [.cs](file:///d:/Demonteks/Scripts/Enemy/DemontekAI.cs) and [.tscn](file:///d:/Demonteks/Scenes/Characters/King.tscn) files for *explicitly typed filenames* (like `"Demontek_Footstep.mp3"`). 

Because your game uses [CharacterAudioController.cs](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs) to load sounds **dynamically by keyword**, the Python script will *always* say they are "unused" unless we hardcode the specific string paths into the project. 

However, since we added the [PlayAction("Idle")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124), [PlayAction("Footstep")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124), and [PlayAction("Roar")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124) triggers into the AI's State Machine logic, those audio categories are now fully hooked up!

Here is how the previously "unassigned" sounds are mapped now based on the [CharacterAudioController](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#11-125)'s keyword dictionary:

## Normal Demontek
- [Demon_Whispers.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Demontek/Demon_Whispers.mp3) $\rightarrow$ **Roar category** (Plays upon entering Chase)
- [Demontek_Attack.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Demontek/Demontek_Attack.mp3) $\rightarrow$ **Attack category** (Plays on Attack)
- [Demontek_Death.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Demontek/Demontek_Death.mp3) $\rightarrow$ **Death category** (Plays on Death)
- [Demontek_Footstep.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Demontek/Demontek_Footstep.mp3) $\rightarrow$ **Footstep category** (Plays while moving in Patrol/Chase)
- [Demontek_Hit.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Demontek/Demontek_Hit.mp3) $\rightarrow$ **Hurt category** (Plays when taking damage)
- [Demontek_Idle.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Demontek/Demontek_Idle.mp3) $\rightarrow$ **Idle category** (Plays randomly during Idle state)

## Goliath
- [Goliath_Death_Collapse.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Death_Collapse.mp3) / [Goliath_Death_Cry.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Death_Cry.mp3) / [Goliath_Death_Roar_Voice.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Death_Roar_Voice.mp3) $\rightarrow$ **Death category**
- [Goliath_Hit.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Hit.mp3) / [Goliath_Stagger.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Stagger.mp3) $\rightarrow$ **Hurt category**
- [Goliath_Idle_Breathing.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Idle_Breathing.mp3) $\rightarrow$ **Idle category**
- [Goliath_Ground_Pound.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Ground_Pound.mp3) / [Goliath_Multi_Arm_Swipe.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Multi_Arm_Swipe.mp3) / [Goliath_Roar_Attack.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Roar_Attack.mp3) / [Goliath_Stomp.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Stomp.mp3) $\rightarrow$ **Attack category** (or Footstep for stomp)
- [Goliath_Entrance_Roar.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Entrance_Roar.mp3) / [Goliath_Phase2_Roar.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Phase2_Roar.mp3) / [Goliath_Phase3_Enrage.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Phase3_Enrage.mp3) / [Goliath_Victory_Roar.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Goliath/Goliath_Victory_Roar.mp3) $\rightarrow$ **Roar category** 

## Guard
- [Guard_Baton_Swing.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Guard/Guard_Baton_Swing.mp3) $\rightarrow$ **Slash category** (Wait, Boss AI doesn't trigger 'Slash' yet - see note below)
- [Guard_Death.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Guard/Guard_Death.mp3) $\rightarrow$ **Death category**
- [Guard_Hit.mp3](file:///d:/Demonteks/Assets/Audio/sfx/Enemies/Guard/Guard_Hit.mp3) $\rightarrow$ **Hurt category**
- `Guard_Idle.mp3` $\rightarrow$ **Idle category**
- `Guard_March.mp3` $\rightarrow$ **Footstep category**

## King
- `King_Hit.mp3` / `King_Stagger.mp3` $\rightarrow$ **Hurt category**
- `King_Death_Explosion.mp3` / `King_Death_Voice.mp3` $\rightarrow$ **Death category**
- `King_Entrance_01.mp3` / `King_Entrance_02.mp3` / `King_Taunt...` / `King_Mercy...` / `King_Phase...` $\rightarrow$ **Roar category**
- `King_Combat_01.mp3` / `King_Combat_02.mp3` / `King_Fire_Explosion.mp3` / `King_Fire_Projectile.mp3` $\rightarrow$ **Attack category**
- `King_Sword_Swing_Heavy.mp3` $\rightarrow$ **Slash category** (See Note Below)

## Mutant
- `Mutant_Attack_Heavy.mp3` $\rightarrow$ **Attack category**
- `Mutant_Death.mp3` / `Mutant_Death_Roar.mp3` $\rightarrow$ **Death category**
- `Mutant_Footstep_Heavy.mp3` $\rightarrow$ **Footstep category**
- `Mutant_Hit.mp3` $\rightarrow$ **Hurt category**
- `Mutant_Idle_Breathing.mp3` $\rightarrow$ **Idle category**

---

### Remaining Action Triggers Needed (User Decision)
The basic AI state machine doesn't differentiate between normal attacks and special Boss attacks (like "Slash", "Fireball", "Phase 2"). It just triggers [PlayAction("Attack")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124). 

If you want the Bosses to play their specific sounds (like `King_Sword_Swing_Heavy.mp3` via "Slash"), we have a few options:

1. **Change the audio map:** Map "swing", "slash", "combat", "projectile" into the generic **Attack category** in [CharacterAudioController.cs](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs), so it just picks randomly from their combat sounds during any attack.
2. **Add complexity to the AI:** Modify [DemontekAI.cs](file:///d:/Demonteks/Scripts/Enemy/DemontekAI.cs) to randomly pick between [PlayAction("Attack")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124) and [PlayAction("Slash")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124) / [PlayAction("Fireball")](file:///d:/Demonteks/Scripts/Environment/CharacterAudioController.cs#106-124).
3. **Leave them as is:** The dynamic controller picks anything containing "attack" anyway.
