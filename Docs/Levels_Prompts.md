Create a Godot 4.6 2D platformer level scene called "Level1_JungleCanopy" for a game called Demonteks.

CONTEXT:
The explorer wakes on an alien jungle planet. The sky is violet. Plants are giant. He must jump across treetop platforms while 10 small reddish demon-like creatures (Demonteks) chase and attack him. This is the tutorial level. The explorer moves with WASD (A=left, D=right, S=crouch, W=jump) and attacks with E (knife). He can also crouch-jump to dodge attacks.

SCENE STRUCTURE:
- Root node: Node2D named "Level1_JungleCanopy"
- TileMap node using a tileset described as: giant alien tree trunks, purple/green foliage, glowing moss platforms, violet sky
- Parallax background with 3 ParallaxLayer children:
  - Layer 1 (motion_scale 0.05): violet/deep purple sky with distant alien fog
  - Layer 2 (motion_scale 0.35): mid-ground giant alien tree silhouettes and floating spores
  - Layer 3 (motion_scale 0.9): foreground alien leaves and light mist overlay
- Camera2D following the player with limits set to level bounds

TILESET (describe locally in Antigravity):
- Tree-trunk platforms: wide, gnarled, purple-brown bark
- Branch platforms: thinner horizontal branches with glowing green moss
- Glowing moss jump pads (optional bounce effect)
- Background: giant alien fern leaves, bioluminescent vines
- Tile size: 32x32 px

PLATFORM LAYOUT:
- Start platform: large trunk base, ground level, left side of screen
- Series of 12-15 branch platforms at varying heights, forming a left-to-right ascending path
- Some platforms are short (2 tiles) requiring precise jumps; some are long (6-8 tiles) for combat space
- Gaps between platforms: 2–4 tiles wide, requiring W (jump) to cross
- One tall tree trunk the player must climb using staggered branch platforms
- A safe endpoint platform (larger, 8 tiles) at the far right top, with a cave entrance marked by a stone arch — this leads to Level 2

ENTITIES:
- CharacterBody2D "Explorer" at start position (96, 400):
  - CollisionShape2D (capsule)
  - AnimatedSprite2D (idle, run, jump, crouch, attack_knife)
  - Knife attack hitbox: Area2D active on E press, in front of player
  - Script: handles WASD movement, S crouch, W jump, E attack, F interact
- 10 x CharacterBody2D "Demontek" distributed across mid and late platforms:
  - Small, reddish, demon-like sprite
  - AnimatedSprite2D (idle, walk, attack, death)
  - Simple patrol AI: walks platform edge-to-edge, chases player within 200px range
  - Emits "tek" sound on aggro
  - Dies in 1 knife hit; plays death animation then queue_free()
- Floor traps: Area2D nodes at 3 locations on branches — if explorer walks into them (not jumping or crouching), spawn 2 bonus Demonteks

COLLECTIBLES / INTERACTABLES:
- 3 x glowing alien fruit pickups (restore health, Area2D + CollisionShape2D)
- 1 x stone arch portal at endpoint: Area2D, interactable with F key → loads Level2_Dungeon

SCENE SIGNALS & AUTOLOAD:
- When all 10 Demonteks are dead AND explorer reaches endpoint Area2D → trigger cutscene: CanvasLayer with RichTextLabel showing Story Part 2 text, then transition to Level2_Dungeon

CAMERA:
- Camera2D, limit_left=0, limit_right=3200, limit_top=-600, limit_bottom=600
- Smoothing enabled, smoothing_speed=5

OUTPUT:
Produce the full .tscn file and any attached .cs scripts. Use Godot 4.6 syntax (func _physics_process, CharacterBody2D.move_and_slide() with no arguments, velocity property). Comment each section.
---------------------------------------------------------------
Create a Godot 4.6 2D platformer level scene called "Level2_DungeonTunnel" for Demonteks.

CONTEXT:
The explorer was captured at jungle ruins and wakes in a dungeon that resembles hell — red-tinted, fiery, dark stone. He must escape through a dangerous tunnel, neutralize Demontek guards, then find a hidden armory by pressing a trigger stone in a ruined area.

SCENE STRUCTURE:
- Root: Node2D "Level2_DungeonTunnel"
- TileMap with two tileset zones (flagged by position X range):
  - Zone A (Dungeon, x 0–1600): dark stone bricks, iron bars, torch sconces, damp floor tiles, red ambient glow
  - Zone B (Tunnel, x 1600–3200): raw earth walls, wooden support beams, glowing fungi clusters
- Parallax background (3 layers):
  - Layer 1 (0.05): deep red/black hellish void with ember particles
  - Layer 2 (0.35): distant dungeon archways and iron gates
  - Layer 3 (0.9): foreground torch flame overlay and mist

TILESET (describe locally):
- Dungeon: 32x32 dark stone brick (wall, floor, ceiling), iron bar tile (for cell doors), torch sconce wall tile
- Tunnel: dirt/earth tile, wooden plank support tile, glowing fungi tile (emits soft green light)

LAYOUT — ZONE A (Dungeon):
- Explorer respawn point: a locked cell (4x3 tiles of iron bars), bottom-left. A friendly Demontek NPC in adjacent cell gestures toward a breakable wall tile to the right
- Breakable wall: 3 tiles wide, crumbles when explorer attacks with E — reveals tunnel entrance
- 3 guard Demonteks patrolling the dungeon corridor outside cells
- Torch sconces every 6 tiles on walls

LAYOUT — ZONE B (Tunnel):
- Narrow corridor 4 tiles high, winding right and slightly upward
- 5 wooden support beam platforms the explorer must jump between as ground dips into pits
- 4 Demontek guards (tougher, require 2 knife hits)
- 2 pressure-plate floor traps (Area2D): if triggered, 3 Demonteks spawn from side alcoves
- Glowing fungi clusters provide light pools; dark areas between them reduce visibility (modulate darkness overlay)
- At tunnel end (x ≈ 3000): a ruined chamber — stone rubble, broken columns

HIDDEN ARMORY:
- In the ruined chamber: one specific stone tile marked with a subtle glow (TileMap custom data or separate Sprite2D)
- When explorer presses F near it: stone slides away (AnimationPlayer), revealing a stairway down
- Armory room below: 3 weapon pickups on pedestals:
  - Ancient Sword (Area2D + Sprite2D, label "Ancient Sword")
  - Stone Spear (Area2D + Sprite2D)
  - Bone Shield (Area2D + Sprite2D, grants temporary block on S hold)
- Wall inscriptions: RichTextLabel triggered by F key proximity → displays Story Part 3 text as ancient writings
- After reading and collecting a weapon → CanvasLayer cutscene with Part 3 story text, then transition to Level3_Fortress

ENTITIES:
- CharacterBody2D "Explorer" respawns at dungeon cell start on death; retains HP between checkpoints
- Checkpoint: Area2D at tunnel entrance — if explorer passes it, death respawns here instead of cell
- Demontek guards: same AI as Level 1 but 2-hit kill; drop "tek token" collectible

RESPAWN SYSTEM:
- GameState Autoload tracks last_checkpoint (Vector2)
- On explorer death: reload scene, reposition Explorer to last_checkpoint

OUTPUT:
Full .tscn and .cs scripts. Godot 4.6 syntax. Comment all sections clearly.
-------------------------------------------------------------------------------------------------
Create a Godot 4.6 2D platformer level scene called "Level3_Fortress" for Demonteks.

CONTEXT:
The explorer must stealthily approach a reddish demon castle, enter from the back, and fight through 3 increasingly difficult castle floors, defeat the Goliath boss, then face the Demontek King on the terrace to open the portal home.

SCENE STRUCTURE:
- Root: Node2D "Level3_Fortress"
- TileMap with 5 distinct zone sections (by x/y region):
  - Courtyard (approach): reddish stone paving, fortress outer walls, alien torches
  - Castle Floor 1: dark stone walls, alien armour suits on pedestals, red banners
  - Castle Floor 2: same as Floor 1, more ornate, chains on walls
  - Castle Floor 3 / Goliath Arena: wider room, cracked floor, reddish lava cracks
  - Castle Terrace: open ramparts, alien sky, stone altar at center
- Parallax background (3 layers each zone):
  - Courtyard layers: hellish red sky, distant castle silhouette, foreground alien torch smoke
  - Castle Interior layers: dark void, faint dungeon arches, foreground banner sway overlay
  - Terrace layers: alien open sky (violet-to-red gradient), distant alien cityscape, foreground rampart edge overlay

TILESET (describe locally):
- Courtyard: 32x32 reddish stone pavers, outer wall tiles, alien torch tile (animated flame)
- Interior: dark stone wall, banner tile (red with alien sigil), armour suit decorative tile
- Staircase: spiral stone step tile, iron railing tile
- Terrace: rampart tile, stone altar tile, alien sky backdrop

ZONE 1 — COURTYARD (Stealth Approach):
- Explorer enters from left; castle back wall visible at right (x ≈ 800)
- 4 Demontek sentries on patrol routes (back and forth, 300px range)
- Stealth mechanic: if explorer is seen (LOS raycast from sentry hits explorer without crouch), sentry raises alarm → 4 more Demonteks spawn
- 6 floor trap tiles (Area2D): if stepped on without jumping over, 6 Demonteks spawn immediately and attack until explorer dies → respawn at dungeon start (Level 2 respawn point) — use a SceneTransition to Level2 start
- Crouch (S) reduces hitbox height, allowing player to slip under sentry LOS cones
- Castle back entrance: wooden door, opened with F key after courtyard is clear OR bypassed by jumping to a ledge above

ZONE 2 — CASTLE FLOOR 1:
- Multi-platform interior, 3 vertical levels connected by ladders/ramps
- 6 standard Demonteks armed with sabers (2-hit kill); explorer can now use ancient weapons from Level 2
- Weapon upgrade pickup: "Demontek Saber" on a fallen enemy after 4 kills
- If explorer dies here → respawn at Floor 1 entrance
- Staircase to Floor 2: locked until all Floor 1 enemies defeated

ZONE 3 — CASTLE FLOOR 2:
- Similar layout, wider rooms
- 4 standard Demonteks + 3 Mutant Demonteks (larger, 3-hit kill, deal 2x damage)
- Weapon pickup: "Alien Sawtooth" and "Alien Sword" on pedestals mid-room
- If explorer dies → respawn at Floor 2 entrance
- Staircase to Floor 3 unlocks after all enemies cleared

ZONE 4 — GOLIATH ARENA (Floor 3 / Boss Room):
- Large open room (800x400 px play area), cracked floor with glowing lava lines
- Phase 1: 5 Mutant Demonteks — clear them to trigger boss cutscene
- Boss: CharacterBody2D "Goliath" — massive Demontek-monster fusion
  - AnimatedSprite2D with phases: idle, charge, slam, roar, death
  - HP: 20 hits; takes damage from any weapon
  - Attacks:
    - Charge (runs at player, must jump over)
    - Ground Slam (Area2D shockwave on floor, must jump)
    - Roar (spawns 2 small Demonteks)
  - Defeated: plays death animation + roar SFX, drops "King's Key" item
- If explorer dies to Goliath → respawn at Floor 3 entrance

ZONE 5 — CASTLE TERRACE (Final Boss):
- Open ramparts, stone altar at center, alien sky horizon
- King Demontek: CharacterBody2D "KingDemontek"
  - Large, bearded, crowned; reddish robes
  - HP: 30 hits
  - Attacks:
    - Scepter beam (Area2D projectile, dodge by jumping or crouching)
    - Summon wave: calls 3 Demonteks every 30 seconds
    - Stomp: shockwave, same as Goliath slam
  - On death: AnimationPlayer triggers throne crumbling, portal Sprite2D activates on altar
- Portal: Area2D on altar; explorer presses F to enter → triggers final cutscene (Story Part 4) via CanvasLayer RichTextLabel, then fade to EndScreen scene

RESPAWN RULES (implement via GameState Autoload):
- Courtyard death: back to Level2 dungeon start
- Floor 1 death: Floor 1 entrance
- Floor 2 death: Floor 2 entrance  
- Floor 3 / Goliath death: Floor 3 entrance
- King death = game won

CAMERA:
- Camera2D per zone, transitions smoothly (Tween) when explorer enters new zone Area2D trigger
- Each zone has defined camera limits

GENERAL RULES FOR ALL THREE LEVELS:
- Floor traps: implemented as Area2D with body_entered signal; check if explorer velocity.y == 0 (on ground) to confirm not jumping; crouching (collision shape shrunk) also bypasses
- All Demontek AI uses CharacterBody2D + NavigationAgent2D or simple state machine (patrol/chase/attack)
- Explorer's attack hitbox is an Area2D child, enabled for 0.2s on E press, then disabled
- Use Godot 4.6 syntax throughout: CharacterBody2D, move_and_slide() no arguments, @export, @onready, signal declarations with signal keyword
- All scripts commented section by section
- Produce complete .tscn scene files and all referenced .cs scripts

OUTPUT for this prompt: Full Level3_Fortress.tscn + all .cs scripts (Explorer.gd if not already produced, Demontek.gd, GoliathBoss.gd, KingDemontek.gd, GameState.gd autoload).