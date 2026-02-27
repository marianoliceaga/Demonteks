# Demontek Game Audio Prompts

---

## 🔊 SOUND EFFECTS — `generate_sound_effect()`

| Sound File Name | MCP Prompt |
|---|---|
| **PLAYER MOVEMENT** | |
| Footstep_Grass | `generate_sound_effect(text="Rhythmic footsteps on soft jungle grass and wet earth, steady pace, natural outdoor ambience, loopable", duration_seconds=3, output_path="Footstep_Grass.mp3")` |
| Footstep_Stone | `generate_sound_effect(text="Echoing footsteps on cold stone dungeon floor, steady pace, loopable", duration_seconds=3, output_path="Footstep_Stone.mp3")` |
| Dash | `generate_sound_effect(text="Fast whoosh dash movement burst, aerodynamic swoosh with slight wind impact", duration_seconds=0.6, output_path="Dash.mp3")` |
| Jump | `generate_sound_effect(text="Cartoon-style jump launch, upward spring leap with slight grunt exertion", duration_seconds=0.5, output_path="Jump.mp3")` |
| Land_Soft | `generate_sound_effect(text="Soft landing thud on grass or dirt, gentle impact with light compression", duration_seconds=0.4, output_path="Land_Soft.mp3")` |
| Land_Hard | `generate_sound_effect(text="Hard landing impact on stone floor, heavy thud with slight reverb echo", duration_seconds=0.5, output_path="Land_Hard.mp3")` |
| **PLAYER COMBAT** | |
| Player_Punch | `generate_sound_effect(text="Human fist punch impact, sharp knuckle crack on contact, action game style", duration_seconds=0.4, output_path="Player_Punch.mp3")` |
| Knife_Slash | `generate_sound_effect(text="Fast knife slash swipe through air, sharp blade whoosh", duration_seconds=0.4, output_path="Knife_Slash.mp3")` |
| Knife_Hit_Enemy | `generate_sound_effect(text="Knife blade connecting with enemy flesh, wet impact thud with metal ring", duration_seconds=0.4, output_path="Knife_Hit_Enemy.mp3")` |
| Machete_Slash | `generate_sound_effect(text="Heavy machete swing through air, thick blade whoosh with weight", duration_seconds=0.5, output_path="Machete_Slash.mp3")` |
| Machete_Hit_Enemy | `generate_sound_effect(text="Machete hitting enemy, heavy wet impact with thick blade thud", duration_seconds=0.5, output_path="Machete_Hit_Enemy.mp3")` |
| Sword_Swing | `generate_sound_effect(text="Metal sword swing through air, clean sharp blade swoosh with metallic ring", duration_seconds=0.6, output_path="Sword_Swing.mp3")` |
| **PLAYER DAMAGE & STATUS** | |
| Player_Hurt_Light | `generate_sound_effect(text="Player grunt of light pain, small hit reaction, short exhale of discomfort", duration_seconds=0.5, output_path="Player_Hurt_Light.mp3")` |
| Player_Hurt_Medium | `generate_sound_effect(text="Player medium damage grunt, sharp hit reaction, audible pain vocalization", duration_seconds=0.6, output_path="Player_Hurt_Medium.mp3")` |
| Player_Hurt_Heavy | `generate_sound_effect(text="Player taking heavy damage, loud painful groan with impact sound", duration_seconds=0.7, output_path="Player_Hurt_Heavy.mp3")` |
| Player_Death | `generate_sound_effect(text="Player death sound, dramatic collapsing breath with fading exhale, game over tone", duration_seconds=1.5, output_path="Player_Death.mp3")` |
| Player_Heal | `generate_sound_effect(text="Health healing sound, warm glowing chime with soft magical shimmer, restorative tone", duration_seconds=1.2, output_path="Player_Heal.mp3")` |
| Respawn | `generate_sound_effect(text="Respawn at checkpoint, rising digital tone with energy surge, coming back to life sound", duration_seconds=1.5, output_path="Respawn.mp3")` |
| **PLAYER INTERACTION** | |
| Checkpoint_Activate | `generate_sound_effect(text="Checkpoint activation, satisfying crystalline chime with energy pulse save sound", duration_seconds=1.0, output_path="Checkpoint_Activate.mp3")` |
| Lever_Pull | `generate_sound_effect(text="Mechanical lever pull, heavy metal clunk with gear click, dungeon switch activation", duration_seconds=0.8, output_path="Lever_Pull.mp3")` |
| **NORMAL DEMONTEK** | |
| Demontek_Idle | `generate_sound_effect(text="Demonic creature idle breathing with low guttural growl, ambient monster sound loop", duration_seconds=3, output_path="Demontek_Idle.mp3")` |
| Demontek_Attack | `generate_sound_effect(text="Demon creature swipe or bite attack, fast aggressive slash with growl burst", duration_seconds=0.6, output_path="Demontek_Attack.mp3")` |
| Demontek_Hit | `generate_sound_effect(text="Demon creature taking damage, short pained snarl and recoil grunt", duration_seconds=0.5, output_path="Demontek_Hit.mp3")` |
| Demontek_Death | `generate_sound_effect(text="Demon creature death cry, pained screech fading into silence", duration_seconds=1.2, output_path="Demontek_Death.mp3")` |
| Demontek_Footstep | `generate_sound_effect(text="Demon creature footstep, clawed scratching on stone floor", duration_seconds=0.4, output_path="Demontek_Footstep.mp3")` |
| **GUARD DEMONTEK** | |
| Guard_Idle | `generate_sound_effect(text="Armored demonic guard idle stance, low mechanical hum with subtle breath, alert posture sound", duration_seconds=3, output_path="Guard_Idle.mp3")` |
| Guard_Baton_Swing | `generate_sound_effect(text="Armored baton swing attack, heavy dense rod whoosh through air", duration_seconds=0.5, output_path="Guard_Baton_Swing.mp3")` |
| Guard_Hit | `generate_sound_effect(text="Armored guard taking hit, metallic clang impact with muffled grunt", duration_seconds=0.5, output_path="Guard_Hit.mp3")` |
| Guard_Death | `generate_sound_effect(text="Armored guard death sound, clanging armor collapse with death rattle", duration_seconds=1.2, output_path="Guard_Death.mp3")` |
| Guard_March | `generate_sound_effect(text="Heavy armored march footstep, thick metal boot on stone floor", duration_seconds=0.5, output_path="Guard_March.mp3")` |
| **MUTANT DEMONTEK** | |
| Mutant_Idle_Breathing | `generate_sound_effect(text="Mutant creature heavy labored breathing, deep raspy wet breath loop, disturbing idle sound", duration_seconds=3, output_path="Mutant_Idle_Breathing.mp3")` |
| Mutant_Attack_Heavy | `generate_sound_effect(text="Mutant powerful melee attack, massive heavy impact smash with grunt force", duration_seconds=0.7, output_path="Mutant_Attack_Heavy.mp3")` |
| Mutant_Hit | `generate_sound_effect(text="Tough mutant creature taking damage, deep pained grunt with thick body impact", duration_seconds=0.6, output_path="Mutant_Hit.mp3")` |
| Mutant_Death | `generate_sound_effect(text="Mutant death, loud roaring scream fading into gurgle, powerful creature dying", duration_seconds=1.8, output_path="Mutant_Death.mp3")` |
| Mutant_Footstep_Heavy | `generate_sound_effect(text="Mutant thunderous heavy footstep, massive creature step shaking the ground", duration_seconds=0.6, output_path="Mutant_Footstep_Heavy.mp3")` |
| **GOLIATH BOSS** | |
| Goliath_Idle_Breathing | `generate_sound_effect(text="Giant boss creature heavy breathing loop, massive deep respiratory cycle, ominous idle", duration_seconds=4, output_path="Goliath_Idle_Breathing.mp3")` |
| Goliath_Multi_Arm_Swipe | `generate_sound_effect(text="Four-armed giant boss multi-arm swipe, rapid successive swooshes overlapping, devastating attack", duration_seconds=0.9, output_path="Goliath_Multi_Arm_Swipe.mp3")` |
| Goliath_Ground_Pound | `generate_sound_effect(text="Giant boss ground pound, enormous impact explosion with shockwave rumble and debris", duration_seconds=1.2, output_path="Goliath_Ground_Pound.mp3")` |
| Goliath_Roar_Attack | `generate_sound_effect(text="Boss creature sonic roar attack, powerful bellowing blast wave, concussive sound burst", duration_seconds=1.5, output_path="Goliath_Roar_Attack.mp3")` |
| Goliath_Stomp | `generate_sound_effect(text="Giant boss heavy stomp, enormous foot impact on stone causing tremor rumble", duration_seconds=0.7, output_path="Goliath_Stomp.mp3")` |
| Goliath_Hit | `generate_sound_effect(text="Armored giant boss taking damage, heavy metallic clang with deep pained grunt", duration_seconds=0.6, output_path="Goliath_Hit.mp3")` |
| Goliath_Stagger | `generate_sound_effect(text="Boss armor breaking stagger moment, cracking metal shatter with creature stumble grunt", duration_seconds=1.0, output_path="Goliath_Stagger.mp3")` |
| Goliath_Death_Cry | `generate_sound_effect(text="Giant boss death cry begins, long agonized bellow starting defeat sequence", duration_seconds=2.0, output_path="Goliath_Death_Cry.mp3")` |
| Goliath_Death_Collapse | `generate_sound_effect(text="Massive boss body collapsing to ground, enormous thunderous impact with debris and dust", duration_seconds=2.5, output_path="Goliath_Death_Collapse.mp3")` |
| **KING DEMONTEK** | |
| King_Sword_Swing_Heavy | `generate_sound_effect(text="Heavy demonic king charged sword slam, powerful metallic crash with dark energy resonance", duration_seconds=0.9, output_path="King_Sword_Swing_Heavy.mp3")` |
| King_Fire_Projectile | `generate_sound_effect(text="Demonic king launching fire magic projectile, whooshing flame burst with dark magical charge", duration_seconds=0.8, output_path="King_Fire_Projectile.mp3")` |
| King_Fire_Explosion | `generate_sound_effect(text="Fire projectile explosion impact, fiery blast with dark demonic energy shockwave", duration_seconds=1.2, output_path="King_Fire_Explosion.mp3")` |
| King_Hit | `generate_sound_effect(text="Demonic king taking damage, regal pained grunt with echo of dark power", duration_seconds=0.6, output_path="King_Hit.mp3")` |
| King_Stagger | `generate_sound_effect(text="Demonic king entering stagger state, disoriented stumble sound with crackling dark energy faltering", duration_seconds=1.0, output_path="King_Stagger.mp3")` |
| King_Death_Explosion | `generate_sound_effect(text="Final boss death explosion, massive cataclysmic blast with dark energy implosion and echo", duration_seconds=3.0, output_path="King_Death_Explosion.mp3")` |
| **JUNGLE ENVIRONMENT** | |
| Birds_Chirping | `generate_sound_effect(text="Alien jungle bird chirping calls, exotic otherworldly avian sounds layered ambient loop", duration_seconds=5, output_path="Birds_Chirping.mp3")` |
| Bush_Rustle | `generate_sound_effect(text="Moving through dense jungle foliage, leaves and branches rustling, brushing sound", duration_seconds=0.8, output_path="Bush_Rustle.mp3")` |
| **DUNGEON ENVIRONMENT** | |
| Chains_Rattle | `generate_sound_effect(text="Heavy dungeon chains rattling and swaying, metallic clanking links, ominous ambient sound", duration_seconds=2, output_path="Chains_Rattle.mp3")` |
| Distant_Screams | `generate_sound_effect(text="Distant muffled prisoner screams in dungeon, far-off tortured cries, subtle and eerie", duration_seconds=4, output_path="Distant_Screams.mp3")` |
| Prison_Door_Creak | `generate_sound_effect(text="Heavy rusted prison door creaking open, slow grinding metal hinge scrape", duration_seconds=1.5, output_path="Prison_Door_Creak.mp3")` |
| **CASTLE ENVIRONMENT** | |
| Fire_Roaring | `generate_sound_effect(text="Multiple large torches roaring, crackling fire ambience, intense flame sound loop", duration_seconds=5, output_path="Fire_Roaring.mp3")` |
| Distant_Chanting | `generate_sound_effect(text="Distant demonic ritual chanting, dark cultist voices echoing through stone halls", duration_seconds=5, output_path="Distant_Chanting.mp3")` |
| Demon_Whispers | `generate_sound_effect(text="Subtle whispering demon voices, barely audible sinister murmurs, unsettling atmosphere", duration_seconds=5, output_path="Demon_Whispers.mp3")` |
| Hell_Portal_Hum | `generate_sound_effect(text="Hell portal energy hum, deep resonant dimensional rift vibration with dark magical oscillation loop", duration_seconds=5, output_path="Hell_Portal_Hum.mp3")` |
| **UI SOUNDS** | |
| Button_Hover | `generate_sound_effect(text="Soft UI hover sound, gentle mouse-over chime, clean interface feedback tone", duration_seconds=0.3, output_path="Button_Hover.mp3")` |
| Button_Click | `generate_sound_effect(text="UI button click select, satisfying crisp tap confirmation sound", duration_seconds=0.3, output_path="Button_Click.mp3")` |
| Menu_Open | `generate_sound_effect(text="Pause menu opening whoosh, quick interface slide-in with soft chime", duration_seconds=0.5, output_path="Menu_Open.mp3")` |
| Menu_Close | `generate_sound_effect(text="Menu closing swoosh, quick interface slide-out dismissal sound", duration_seconds=0.4, output_path="Menu_Close.mp3")` |
| Menu_Navigate | `generate_sound_effect(text="Menu navigation tick, subtle directional move between UI options", duration_seconds=0.2, output_path="Menu_Navigate.mp3")` |
| Error_Buzz | `generate_sound_effect(text="Error buzz, sharp negative feedback tone, invalid action denied sound", duration_seconds=0.5, output_path="Error_Buzz.mp3")` |
| Success_Chime | `generate_sound_effect(text="Success chime, pleasant ascending ding tone, positive action completion sound", duration_seconds=0.7, output_path="Success_Chime.mp3")` |
| Level_Complete | `generate_sound_effect(text="Level complete fanfare, triumphant short musical sting with celebration energy", duration_seconds=3.0, output_path="Level_Complete.mp3")` |
| Achievement_Unlock | `generate_sound_effect(text="Achievement unlock sound, bright rewarding chime with rising flourish", duration_seconds=1.5, output_path="Achievement_Unlock.mp3")` |
| Stat_Increase | `generate_sound_effect(text="Stat increase level up sound, positive ascending tone with power growth feeling", duration_seconds=1.0, output_path="Stat_Increase.mp3")` |
| Health_Low_Warning | `generate_sound_effect(text="Low health warning alert, urgent pulsing heartbeat tone, under 30 HP danger signal", duration_seconds=1.0, output_path="Health_Low_Warning.mp3")` |
| New_Weapon_Notification | `generate_sound_effect(text="New weapon unlocked notification, exciting discovery chime with metallic ring accent", duration_seconds=1.2, output_path="New_Weapon_Notification.mp3")` |
| Save_Game | `generate_sound_effect(text="Auto-save indicator sound, quick soft data-writing click with subtle confirmation chime", duration_seconds=0.8, output_path="Save_Game.mp3")` |

---

## 🎙️ VOICE LINES — `eleven_text_to_speech()`

| Sound File Name | MCP Prompt |
|---|---|
| **PLAYER — EFFORT SOUNDS** | |
| Player_Jump_01 | `eleven_text_to_speech(text="Huh!", voice_description="Young adult male explorer, athletic effort grunt, short upward jump exertion, energetic and determined", stability=0.30, similarity_boost=0.80, style=0.70, speed=1.2, output_path="Player_Jump_01.mp3")` |
| Player_Jump_02 | `eleven_text_to_speech(text="Hyah!", voice_description="Young adult male explorer, sharp explosive jump grunt, action hero energy, punchy and quick", stability=0.25, similarity_boost=0.80, style=0.75, speed=1.3, output_path="Player_Jump_02.mp3")` |
| Player_Jump_03 | `eleven_text_to_speech(text="Nngh!", voice_description="Young adult male explorer, strained effort sound, pushing off hard surface, tense and focused", stability=0.30, similarity_boost=0.80, style=0.65, speed=1.2, output_path="Player_Jump_03.mp3")` |
| Player_Attack_01 | `eleven_text_to_speech(text="Hyah!", voice_description="Young adult male explorer, aggressive combat attack shout, sharp and forceful strike effort", stability=0.25, similarity_boost=0.80, style=0.85, speed=1.4, output_path="Player_Attack_01.mp3")` |
| Player_Attack_02 | `eleven_text_to_speech(text="Raaah!", voice_description="Young adult male explorer, fierce battle cry during attack, raw aggression and physical effort", stability=0.20, similarity_boost=0.80, style=0.90, speed=1.4, output_path="Player_Attack_02.mp3")` |
| Player_Attack_03 | `eleven_text_to_speech(text="Take this!", voice_description="Young adult male explorer, determined combat grunt while striking, gritty and intense", stability=0.30, similarity_boost=0.80, style=0.80, speed=1.3, output_path="Player_Attack_03.mp3")` |
| Player_Hurt_01 | `eleven_text_to_speech(text="Argh!", voice_description="Young adult male explorer, sharp pain reaction to light hit, surprised and pained grunt", stability=0.25, similarity_boost=0.80, style=0.75, speed=1.2, output_path="Player_Hurt_01.mp3")` |
| Player_Hurt_02 | `eleven_text_to_speech(text="Ugh... damn it!", voice_description="Young adult male explorer, medium damage pain reaction, wincing through gritted teeth, hurt but fighting on", stability=0.30, similarity_boost=0.80, style=0.80, speed=1.1, output_path="Player_Hurt_02.mp3")` |
| Player_Hurt_03 | `eleven_text_to_speech(text="AAARGH!", voice_description="Young adult male explorer, heavy damage scream, intense raw pain, desperate and strained", stability=0.15, similarity_boost=0.80, style=0.95, speed=1.0, output_path="Player_Hurt_03.mp3")` |
| Player_Death | `eleven_text_to_speech(text="No... noooo!", voice_description="Young adult male explorer, death scream, desperate fading cry as life ends, tragic and emotional", stability=0.15, similarity_boost=0.80, style=0.95, speed=0.9, output_path="Player_Death.mp3")` |
| Player_Heavy_Breathing | `eleven_text_to_speech(text="*heavy labored breathing* Come on... come on...", voice_description="Young adult male explorer, exhausted near-death heavy breathing, wheezing and desperate, barely holding on at low HP", stability=0.20, similarity_boost=0.85, style=0.85, speed=0.8, output_path="Player_Heavy_Breathing.mp3")` |
| **PLAYER — STORY LINES** | |
| Player_WakeUp | `eleven_text_to_speech(text="Where... where am I?", voice_description="Young adult male explorer, disoriented and confused waking up, groggy and alarmed, looking around an unknown alien world", stability=0.40, similarity_boost=0.85, style=0.60, speed=0.9, output_path="Player_WakeUp.mp3")` |
| Player_SeeEnemies | `eleven_text_to_speech(text="What are these creatures?!", voice_description="Young adult male explorer, shocked and frightened discovering demons for the first time, disbelief and fear in voice", stability=0.35, similarity_boost=0.85, style=0.70, speed=1.0, output_path="Player_SeeEnemies.mp3")` |
| Player_Escape | `eleven_text_to_speech(text="I have to escape!", voice_description="Young adult male explorer, urgent and desperate, rallying himself with determination, fear mixed with resolve", stability=0.30, similarity_boost=0.85, style=0.75, speed=1.1, output_path="Player_Escape.mp3")` |
| Player_SeePortal | `eleven_text_to_speech(text="There's the portal!", voice_description="Young adult male explorer, excited and relieved spotting the exit, hope surging through exhaustion, breathless discovery", stability=0.35, similarity_boost=0.85, style=0.70, speed=1.2, output_path="Player_SeePortal.mp3")` |
| Player_Victory | `eleven_text_to_speech(text="I'm going home!", voice_description="Young adult male explorer, triumphant and emotional victory cry, overwhelming relief and joy, voice cracking with emotion", stability=0.30, similarity_boost=0.85, style=0.80, speed=1.1, output_path="Player_Victory.mp3")` |
| **GOLIATH BOSS** | |
| Goliath_Entrance_Roar | `eleven_text_to_speech(text="GRAAAAAAHHH!", voice_description="Massive ancient giant boss creature, earth-shaking entrance roar, thunderous and primal, the sound of a mountain waking up, terrifying and overwhelming", stability=0.15, similarity_boost=0.85, style=0.95, speed=0.7, output_path="Goliath_Entrance_Roar.mp3")` |
| Goliath_Phase2_Roar | `eleven_text_to_speech(text="RAAAAAAAHHHH!", voice_description="Massive giant boss enraged at half health, louder more furious roar than before, wounded animal fury, armor cracking, voice distorting with rage", stability=0.10, similarity_boost=0.85, style=1.0, speed=0.65, output_path="Goliath_Phase2_Roar.mp3")` |
| Goliath_Phase3_Enrage | `eleven_text_to_speech(text="RRAAAAAAAARRRGHHH!", voice_description="Massive giant boss full berserker enrage, most intense roar yet, unhinged and catastrophic, the sound of pure primal destruction unleashed", stability=0.05, similarity_boost=0.85, style=1.0, speed=0.6, output_path="Goliath_Phase3_Enrage.mp3")` |
| Goliath_Death_Roar | `eleven_text_to_speech(text="NOOOOOOOOO...", voice_description="Massive giant boss dying, long mournful death roar fading slowly, power draining away, ground-shaking collapse cry, tragic and enormous", stability=0.15, similarity_boost=0.85, style=0.95, speed=0.55, output_path="Goliath_Death_Roar.mp3")` |
| Goliath_Victory_Roar | `eleven_text_to_speech(text="HRAAAAAAHHHH!", voice_description="Massive giant boss triumphant victory roar over fallen player, dominant and terrifying, alpha predator claiming supremacy", stability=0.15, similarity_boost=0.85, style=0.95, speed=0.70, output_path="Goliath_Victory_Roar.mp3")` |
| **KING DEMONTEK — ENTRANCE** | |
| King_Entrance_01 | `eleven_text_to_speech(text="Another human... how amusing.", voice_description="Powerful ancient demon king, deep resonant regal voice with dark echo, cold amusement and absolute contempt, languid and unhurried, he has seen it all before", stability=0.55, similarity_boost=0.85, style=0.75, speed=0.85, output_path="King_Entrance_01.mp3")` |
| King_Entrance_02 | `eleven_text_to_speech(text="You dare enter MY domain?!", voice_description="Powerful ancient demon king, deep regal voice erupting into furious disbelief, royal authority and volcanic rage, walls shake when he speaks", stability=0.30, similarity_boost=0.85, style=0.90, speed=1.0, output_path="King_Entrance_02.mp3")` |
| **KING DEMONTEK — COMBAT TAUNTS** | |
| King_Taunt_01 | `eleven_text_to_speech(text="Is that all you've got?", voice_description="Powerful ancient demon king, deeply condescending and bored, sneering dark amusement, he is barely trying, voice dripping with superiority", stability=0.50, similarity_boost=0.85, style=0.80, speed=0.90, output_path="King_Taunt_01.mp3")` |
| King_Taunt_02 | `eleven_text_to_speech(text="Pathetic!", voice_description="Powerful ancient demon king, sharp single word dismissal, cold disdain and disgust, clipped and venomous", stability=0.40, similarity_boost=0.85, style=0.85, speed=1.1, output_path="King_Taunt_02.mp3")` |
| King_Taunt_03 | `eleven_text_to_speech(text="Feel my power!", voice_description="Powerful ancient demon king, commanding battle roar, dark energy surging, voice reverberating with demonic force and absolute dominance", stability=0.25, similarity_boost=0.85, style=0.95, speed=1.05, output_path="King_Taunt_03.mp3")` |
| **KING DEMONTEK — PHASE TRANSITIONS** | |
| King_Phase2 | `eleven_text_to_speech(text="You're stronger than I thought...", voice_description="Powerful ancient demon king, rare begrudging respect, surprise barely concealed beneath pride, reassessing the player with cold calculating tone", stability=0.50, similarity_boost=0.85, style=0.70, speed=0.85, output_path="King_Phase2.mp3")` |
| King_Phase3 | `eleven_text_to_speech(text="ENOUGH! I'll crush you myself!", voice_description="Powerful ancient demon king fully enraged, royal composure shattered, voice exploding with fury and dark power, maximum threat energy", stability=0.15, similarity_boost=0.85, style=1.0, speed=1.1, output_path="King_Phase3.mp3")` |
| **KING DEMONTEK — MERCY ROUTE** | |
| King_Mercy_01 | `eleven_text_to_speech(text="Wait... you spared my people?", voice_description="Powerful ancient demon king, stunned disbelief softening into confusion, ancient pride cracking, something almost vulnerable beneath the dark power", stability=0.55, similarity_boost=0.85, style=0.60, speed=0.80, output_path="King_Mercy_01.mp3")` |
| King_Mercy_02 | `eleven_text_to_speech(text="Perhaps... there's another way.", voice_description="Powerful ancient demon king, quiet contemplative tone, centuries of war-weariness surfacing, reluctant but genuine consideration of peace", stability=0.60, similarity_boost=0.85, style=0.55, speed=0.75, output_path="King_Mercy_02.mp3")` |
| King_Mercy_03 | `eleven_text_to_speech(text="Go, human. Take your portal home.", voice_description="Powerful ancient demon king, solemn and dignified, a king granting mercy with weight and gravity, ancient pride intact but choosing peace", stability=0.65, similarity_boost=0.85, style=0.50, speed=0.80, output_path="King_Mercy_03.mp3")` |
| **KING DEMONTEK — COMBAT ROUTE** | |
| King_Combat_01 | `eleven_text_to_speech(text="You've killed too many...", voice_description="Powerful ancient demon king, voice low and dangerous with grief and fury, not ranting but coldly lethal, the quiet before a storm", stability=0.45, similarity_boost=0.85, style=0.80, speed=0.85, output_path="King_Combat_01.mp3")` |
| King_Combat_02 | `eleven_text_to_speech(text="I'll avenge them ALL!", voice_description="Powerful ancient demon king, full berserker rage battle cry, voice cracking with grief-fueled fury, all royal composure gone, pure vengeance", stability=0.15, similarity_boost=0.85, style=1.0, speed=1.1, output_path="King_Combat_02.mp3")` |
| King_Death | `eleven_text_to_speech(text="This... isn't... over...", voice_description="Powerful ancient demon king dying, voice fading with each word, defiant to the last breath, ancient power guttering out like a dying flame, chilling final words", stability=0.35, similarity_boost=0.85, style=0.85, speed=0.65, output_path="King_Death.mp3")` |
