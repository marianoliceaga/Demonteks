using Godot;
using System;

namespace Demonteks.Player
{
    /// <summary>
    /// Manages player statistics including health, damage modifiers, and status effects.
    /// Handles save/load integration for persistent player data.
    /// </summary>
    public partial class PlayerStats : Node
    {
        #region Export Variables
        [ExportCategory("Health")]
        [Export] public int MaxHealth { get; set; } = 100;
        [Export] public int CurrentHealth { get; private set; } = 100;
        [Export] public float InvincibilityTime { get; set; } = 1.0f; // Time after hit where player can't be damaged

        [ExportCategory("Combat")]
        [Export] public float DamageMultiplier { get; set; } = 1.0f;
        [Export] public int BaseMeleeDamage { get; set; } = 10;

        [ExportCategory("Status")]
        [Export] public bool IsDead { get; private set; } = false;
        [Export] public bool IsInvincible { get; private set; } = false;
        #endregion

        #region Private Variables
        private float _invincibilityTimer = 0f;
        private bool _canAct = true;
        private bool _lowHealthWarningFired = false;
        #endregion

        #region Signals
        [Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
        [Signal] public delegate void DiedEventHandler();
        [Signal] public delegate void RevivedEventHandler();
        [Signal] public delegate void DamageTakenEventHandler(int damage);
        [Signal] public delegate void HealedEventHandler(int amount);
        [Signal] public delegate void MaxHealthChangedEventHandler(int newMaxHealth);
        #endregion

        #region Godot Lifecycle
        public override void _Ready()
        {
            CurrentHealth = MaxHealth;
            GD.Print($"PlayerStats initialized. Health: {CurrentHealth}/{MaxHealth}");
        }

        public override void _Process(double delta)
        {
            if (IsInvincible)
            {
                _invincibilityTimer -= (float)delta;
                if (_invincibilityTimer <= 0)
                {
                    IsInvincible = false;
                }
            }
        }
        #endregion

        #region Health Management
        /// <summary>
        /// Apply damage to the player
        /// </summary>
        public void TakeDamage(int damage)
        {
            // Allow massive damage (e.g. from KillZone) to bypass invincibility
            if (IsDead || (IsInvincible && damage < 9999)) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
            EmitSignal(SignalName.DamageTaken, damage);

            // Start invincibility frames
            IsInvincible = true;
            _invincibilityTimer = InvincibilityTime;

            GD.Print($"Player took {damage} damage. Health: {CurrentHealth}/{MaxHealth}");

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else if (IsLowHealth && !_lowHealthWarningFired)
            {
                _lowHealthWarningFired = true;
                // Heavy breathing
                GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("../CharacterAudioController")?.PlayAction("Idle");
                // UI low-health warning
                Demonteks.UI.UIAudioController.Instance?.PlaySound(Demonteks.UI.UIAudioController.UiSound.HealthLowWarning);
            }
        }

        /// <summary>
        /// Heal the player
        /// </summary>
        public void Heal(int amount)
        {
            if (IsDead) return;

            int oldHealth = CurrentHealth;
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
            int actualHealing = CurrentHealth - oldHealth;

            if (actualHealing > 0)
            {
                EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
                EmitSignal(SignalName.Healed, actualHealing);

                // Reset low-health warning if health recovered
                if (!IsLowHealth)
                    _lowHealthWarningFired = false;

                GD.Print($"Player healed for {actualHealing}. Health: {CurrentHealth}/{MaxHealth}");
            }
        }

        /// <summary>
        /// Restore health to maximum
        /// </summary>
        public void FullHeal()
        {
            Heal(MaxHealth);
        }

        /// <summary>
        /// Set max health (used for upgrades or buffs)
        /// </summary>
        public void SetMaxHealth(int newMaxHealth)
        {
            MaxHealth = newMaxHealth;
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            EmitSignal(SignalName.MaxHealthChanged, MaxHealth);
            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
        }
        #endregion

        #region Death
        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            _canAct = false;
            EmitSignal(SignalName.Died);

            GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("../CharacterAudioController")?.PlayAction("Death");
            GD.Print("Player died!");
        }

        /// <summary>
        /// Revive the player (e.g., after respawn)
        /// </summary>
        public void Revive()
        {
            IsDead = false;
            _canAct = true;
            FullHeal();

            GetNodeOrNull<Demonteks.Environment.CharacterAudioController>("../CharacterAudioController")?.PlayAction("WakeUp");
            GD.Print("Player revived!");

            EmitSignal(SignalName.Revived);
        }
        #endregion

        #region Combat Stats
        /// <summary>
        /// Calculate final damage output
        /// </summary>
        public int CalculateDamage(int baseDamage)
        {
            return Mathf.RoundToInt(baseDamage * DamageMultiplier);
        }

        /// <summary>
        /// Apply temporary damage buff
        /// </summary>
        public void ApplyDamageBuff(float multiplier, float duration)
        {
            DamageMultiplier *= multiplier;

            // Create timer to remove buff
            var timer = GetTree().CreateTimer(duration);
            timer.Timeout += () => DamageMultiplier /= multiplier;
        }
        #endregion

        #region Save/Load
        /// <summary>
        /// Get save data for persistence
        /// </summary>
        public Godot.Collections.Dictionary<string, Variant> GetSaveData()
        {
            return new Godot.Collections.Dictionary<string, Variant>
            {
                { "max_health", MaxHealth },
                { "current_health", CurrentHealth },
                { "damage_multiplier", DamageMultiplier }
            };
        }

        /// <summary>
        /// Load data from save
        /// </summary>
        public void LoadSaveData(Godot.Collections.Dictionary<string, Variant> data)
        {
            if (data.ContainsKey("max_health"))
                MaxHealth = data["max_health"].AsInt32();

            if (data.ContainsKey("current_health"))
                CurrentHealth = data["current_health"].AsInt32();

            if (data.ContainsKey("damage_multiplier"))
                DamageMultiplier = data["damage_multiplier"].AsSingle();

            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
        }
        #endregion

        #region Properties
        public float HealthPercentage => (float)CurrentHealth / MaxHealth;
        public bool IsFullHealth => CurrentHealth >= MaxHealth;
        public bool IsLowHealth => CurrentHealth < (MaxHealth * 0.3f);
        public bool CanAct => _canAct && !IsDead;
        #endregion
    }
}
