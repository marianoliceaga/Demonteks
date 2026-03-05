using Godot;
using System;
using System.Collections.Generic;

namespace Demonteks.Player
{
    /// <summary>
    /// Manages player combat including weapons, attack execution, and hit detection.
    /// Supports multiple weapon types with different damage and attack patterns.
    /// </summary>
    public partial class PlayerCombat : Node2D
    {
        #region Export Variables
        [ExportCategory("Combat Settings")]
        [Export] public float AttackRange { get; set; } = 50f;
        [Export] public float AttackCooldown { get; set; } = 0.5f;
        [Export] public NodePath HitboxPath { get; set; }

        [ExportCategory("Starting Weapon")]
        [Export] public WeaponType StartingWeapon { get; set; } = WeaponType.Knife;
        #endregion

        #region Private Variables
        private Area2D _hitbox;
        private CollisionShape2D _hitboxShape;
        private float _attackCooldownTimer = 0f;
        private bool _isAttacking = false;
        private string _currentAttackAnim = "attack";

        private Dictionary<WeaponType, Weapon> _weapons = new();
        private Weapon _currentWeapon;
        private PlayerStats _playerStats;
        #endregion

        #region Signals
        [Signal] public delegate void AttackExecutedEventHandler(string weaponName, int damage);
        [Signal] public delegate void EnemyHitEventHandler(Node enemy, int damage);
        [Signal] public delegate void WeaponChangedEventHandler(string weaponName);
        [Signal] public delegate void WeaponPickedUpEventHandler(string weaponName);
        #endregion

        #region Godot Lifecycle
        public override void _Ready()
        {
            // Get references
            _hitbox = GetNode<Area2D>(HitboxPath);
            _hitboxShape = _hitbox.GetNode<CollisionShape2D>("CollisionShape2D");
            _playerStats = GetParent().GetNode<PlayerStats>("PlayerStats");

            // Initialize weapons
            InitializeWeapons();

            // Set starting weapon and unlock it
            EquipWeapon(StartingWeapon, unlockIfLocked: true);

            // Connect signals
            _hitbox.BodyEntered += OnHitboxBodyEntered;

            // Disable hitbox initially
            _hitbox.Monitoring = false;
            _hitbox.Monitorable = false;

            GD.Print($"PlayerCombat initialized. Starting weapon: {_currentWeapon.WeaponName}");
        }

        public override void _Process(double delta)
        {
            // Update cooldown
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer -= (float)delta;
            }
        }
        #endregion

        #region Weapon Initialization
        private void InitializeWeapons()
        {
            // Knife - Starting weapon (Part 1)
            _weapons[WeaponType.Knife] = new Weapon
            {
                Type = WeaponType.Knife,
                WeaponName = "Knife",
                BaseDamage = 10,
                AttackSpeed = 1.2f, // Attacks per second
                Range = 40f,
                Cooldown = 0.4f,
                Description = "A simple combat knife. Fast but weak."
            };

            // Machete - Found in Part 2
            _weapons[WeaponType.Machete] = new Weapon
            {
                Type = WeaponType.Machete,
                WeaponName = "Machete",
                BaseDamage = 20,
                AttackSpeed = 0.9f,
                Range = 60f,
                Cooldown = 0.55f,
                Description = "A heavy jungle blade. Slower but more powerful."
            };

            // Sword - Ancient weapon from ruins (Part 2)
            _weapons[WeaponType.Sword] = new Weapon
            {
                Type = WeaponType.Sword,
                WeaponName = "Ancient Sword",
                BaseDamage = 30,
                AttackSpeed = 1.0f,
                Range = 70f,
                Cooldown = 0.5f,
                Description = "An ancient blade with mysterious power."
            };

            // Demontek Saber - Taken from enemies (Part 3)
            _weapons[WeaponType.DemontekSaber] = new Weapon
            {
                Type = WeaponType.DemontekSaber,
                WeaponName = "Demontek Saber",
                BaseDamage = 35,
                AttackSpeed = 1.1f,
                Range = 65f,
                Cooldown = 0.45f,
                Description = "A curved blade from the Demontek arsenal."
            };

            // Sawtooth - Brutal Demontek weapon (Part 3)
            _weapons[WeaponType.Sawtooth] = new Weapon
            {
                Type = WeaponType.Sawtooth,
                WeaponName = "Sawtooth",
                BaseDamage = 40,
                AttackSpeed = 0.8f,
                Range = 55f,
                Cooldown = 0.6f,
                Description = "A serrated weapon designed for maximum carnage."
            };

            // Alien Sword - Elite Demontek weapon (Part 3)
            _weapons[WeaponType.AlienSword] = new Weapon
            {
                Type = WeaponType.AlienSword,
                WeaponName = "Alien Sword",
                BaseDamage = 50,
                AttackSpeed = 1.0f,
                Range = 75f,
                Cooldown = 0.5f,
                Description = "The finest blade in the Demontek armory."
            };
        }
        #endregion

        #region Weapon Management
        /// <summary>
        /// Equip a weapon by type
        /// </summary>
        public void EquipWeapon(WeaponType type, bool unlockIfLocked = false)
        {
            if (!_weapons.ContainsKey(type))
            {
                GD.PushWarning($"Weapon type {type} not found!");
                return;
            }

            _currentWeapon = _weapons[type];

            // Unlock the weapon if requested (for starting weapon)
            if (unlockIfLocked && !_currentWeapon.IsUnlocked)
            {
                _currentWeapon.IsUnlocked = true;
            }

            AttackRange = _currentWeapon.Range;
            AttackCooldown = _currentWeapon.Cooldown;

            // Update attack animation for new weapon
            UpdateWeaponAttackAnimation(type);

            EmitSignal(SignalName.WeaponChanged, _currentWeapon.WeaponName);
            GD.Print($"Equipped weapon: {_currentWeapon.WeaponName} (Unlocked: {_currentWeapon.IsUnlocked})");
        }

        /// <summary>
        /// Pick up a new weapon
        /// </summary>
        public void PickupWeapon(WeaponType type)
        {
            if (!_weapons.ContainsKey(type))
            {
                GD.PushWarning($"Weapon type {type} not found!");
                return;
            }

            Weapon weapon = _weapons[type];
            weapon.IsUnlocked = true;

            EmitSignal(SignalName.WeaponPickedUp, weapon.WeaponName);
            EquipWeapon(type);

            GD.Print($"Picked up new weapon: {weapon.WeaponName}");
        }

        /// <summary>
        /// Cycle to next available weapon
        /// </summary>
        public void CycleWeapon(bool forward = true)
        {
            List<WeaponType> unlockedWeapons = new List<WeaponType>();

            foreach (var kvp in _weapons)
            {
                if (kvp.Value.IsUnlocked)
                    unlockedWeapons.Add(kvp.Key);
            }

            if (unlockedWeapons.Count <= 1) return;

            int currentIndex = unlockedWeapons.IndexOf(_currentWeapon.Type);
            int nextIndex = forward ? (currentIndex + 1) % unlockedWeapons.Count
                                   : (currentIndex - 1 + unlockedWeapons.Count) % unlockedWeapons.Count;

            EquipWeapon(unlockedWeapons[nextIndex]);
        }

        /// <summary>
        /// Maps a WeaponType to the corresponding attack animation name in the player's SpriteFrames.
        /// Falls back to the base "attack" animation if weapon-specific one isn't defined.
        /// </summary>
        private void UpdateWeaponAttackAnimation(WeaponType type)
        {
            _currentAttackAnim = type switch
            {
                WeaponType.Knife => "attack",
                WeaponType.Machete => "attack_machete",
                WeaponType.Sword => "attack_sword",
                WeaponType.DemontekSaber => "attack_saber",
                WeaponType.Sawtooth => "attack_sawtooth",
                WeaponType.AlienSword => "attack_alien",
                _ => "attack"
            };
            GD.Print($"Attack animation set to: {_currentAttackAnim}");
        }
        #endregion

        #region Attack System
        /// <summary>
        /// Execute an attack with the current weapon
        /// </summary>
        public void ExecuteAttack()
        {
            if (!CanAttack)
            {
                GD.Print($"Cannot attack - CanAttack: {CanAttack}, IsAttacking: {_isAttacking}, Cooldown: {_attackCooldownTimer}, CanAct: {_playerStats.CanAct}, WeaponUnlocked: {(_currentWeapon != null ? _currentWeapon.IsUnlocked : false)}");
                return;
            }

            _isAttacking = true;
            _attackCooldownTimer = AttackCooldown;

            // Calculate damage with player stats
            int finalDamage = _playerStats.CalculateDamage(_currentWeapon.BaseDamage);

            // Enable hitbox for brief moment
            _hitbox.Monitoring = true;
            _hitbox.Monitorable = true;
            GD.Print($"Hitbox enabled - Position: {_hitbox.GlobalPosition}, Monitoring: {_hitbox.Monitoring}");

            EmitSignal(SignalName.AttackExecuted, _currentWeapon.WeaponName, finalDamage);

            // Check bodies already overlapping the hitbox (BodyEntered won't fire for these)
            // We use CallDeferred to allow physics to sync after enabling Monitoring
            CallDeferred(nameof(CheckOverlappingBodies));

            // Disable hitbox after attack duration
            GetTree().CreateTimer(0.4f).Timeout += () =>
            {
                _hitbox.Monitoring = false;
                _hitbox.Monitorable = false;
                _isAttacking = false;
                GD.Print("Hitbox disabled");
            };

            GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("../CharacterAudioController")?.PlayAction("Attack");

            GD.Print($"Attack executed: {_currentWeapon.WeaponName} ({finalDamage} damage)");
        }

        /// <summary>
        /// Checks bodies already overlapping the hitbox at the moment of attack.
        /// BodyEntered does NOT fire for bodies already inside when Monitoring is enabled.
        /// </summary>
        private void CheckOverlappingBodies()
        {
            if (!_isAttacking) return;

            foreach (Node2D body in _hitbox.GetOverlappingBodies())
            {
                GD.Print($"Overlapping body found: {body.Name}, IsEnemy: {body.IsInGroup("enemies")}");
                ApplyDamageToBody(body);
            }
        }

        /// <summary>
        /// Called when a body enters the hitbox during the attack swing.
        /// </summary>
        private void OnHitboxBodyEntered(Node2D body)
        {
            GD.Print($"Hitbox BodyEntered: {body.Name}, IsAttacking: {_isAttacking}, IsEnemy: {body.IsInGroup("enemies")}");

            if (!_isAttacking) return;

            ApplyDamageToBody(body);
        }

        /// <summary>
        /// Applies damage to a body if it is an enemy.
        /// </summary>
        private void ApplyDamageToBody(Node2D body)
        {
            if (!body.IsInGroup("enemies")) return;

            int damage = _playerStats.CalculateDamage(_currentWeapon.BaseDamage);
            EmitSignal(SignalName.EnemyHit, body, damage);

            // Try to damage DemontekAI specifically
            if (body is Demonteks.Enemy.DemontekAI demontek)
            {
                demontek.TakeDamage(damage);
                GD.Print($"Hit Demontek: {body.Name} for {damage} damage");
            }
            // Fallback for other objects with TakeDamage method
            else if (body.HasMethod("TakeDamage"))
            {
                body.Call("TakeDamage", damage);
                GD.Print($"Hit generic enemy: {body.Name} for {damage} damage");
            }
            else
            {
                GD.PushWarning($"Enemy {body.Name} has no supported damage method!");
            }
        }
        #endregion

        #region Save/Load
        /// <summary>
        /// Get save data for weapons
        /// </summary>
        public Godot.Collections.Dictionary<string, Variant> GetSaveData()
        {
            var weaponData = new Godot.Collections.Array();

            foreach (var weapon in _weapons.Values)
            {
                if (weapon.IsUnlocked)
                {
                    weaponData.Add((int)weapon.Type);
                }
            }

            return new Godot.Collections.Dictionary<string, Variant>
            {
                { "unlocked_weapons", weaponData },
                { "current_weapon", (int)_currentWeapon.Type }
            };
        }

        /// <summary>
        /// Load weapon data from save
        /// </summary>
        public void LoadSaveData(Godot.Collections.Dictionary<string, Variant> data)
        {
            if (data.ContainsKey("unlocked_weapons"))
            {
                var weaponData = data["unlocked_weapons"].AsGodotArray();
                foreach (var weaponInt in weaponData)
                {
                    WeaponType type = (WeaponType)weaponInt.AsInt32();
                    if (_weapons.ContainsKey(type))
                    {
                        _weapons[type].IsUnlocked = true;
                    }
                }
            }

            if (data.ContainsKey("current_weapon"))
            {
                WeaponType type = (WeaponType)data["current_weapon"].AsInt32();
                EquipWeapon(type);
            }
        }
        #endregion

        #region Properties
        public bool CanAttack => !_isAttacking && _attackCooldownTimer <= 0 && _playerStats.CanAct;
        public Weapon CurrentWeapon => _currentWeapon;
        public bool IsAttacking => _isAttacking;
        /// <summary>The animation name to play for the current weapon's attack.</summary>
        public string CurrentAttackAnim => _currentAttackAnim;
        public List<Weapon> UnlockedWeapons
        {
            get
            {
                var unlocked = new List<Weapon>();
                foreach (var weapon in _weapons.Values)
                {
                    if (weapon.IsUnlocked)
                        unlocked.Add(weapon);
                }
                return unlocked;
            }
        }
        #endregion
    }

    #region Weapon Data Structures
    public enum WeaponType
    {
        Knife,
        Machete,
        Sword,
        DemontekSaber,
        Sawtooth,
        AlienSword
    }

    public class Weapon
    {
        public WeaponType Type { get; set; }
        public string WeaponName { get; set; }
        public int BaseDamage { get; set; }
        public float AttackSpeed { get; set; }
        public float Range { get; set; }
        public float Cooldown { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; } = false;
    }
    #endregion
}
