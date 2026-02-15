using Godot;
using System;

namespace Demonteks.UI
{
    /// <summary>
    /// HUD controller for displaying player health, weapon, and stats.
    /// </summary>
    public partial class HUDController : CanvasLayer
    {
        private Label _healthLabel;
        private ProgressBar _healthBar;
        private Label _weaponLabel;
        private Label _statsLabel;
        
        private Management.GameManager _gameManager;
        private Node2D _player;

        public override void _Ready()
        {
            // Get UI references
            _healthLabel = GetNode<Label>("MarginContainer/VBoxContainer/TopBar/HealthLabel");
            _healthBar = GetNode<ProgressBar>("MarginContainer/VBoxContainer/HealthBar");
            _weaponLabel = GetNode<Label>("MarginContainer/VBoxContainer/TopBar/WeaponLabel");
            _statsLabel = GetNode<Label>("MarginContainer/VBoxContainer/StatsLabel");
            
            // Get game manager
            _gameManager = GetNode<Management.GameManager>("/root/GameManager");
            
            // Find player
            CallDeferred(nameof(ConnectToPlayer));
        }

        public override void _Process(double delta)
        {
            UpdateStats();
        }

        private void ConnectToPlayer()
        {
            _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
            
            if (_player != null)
            {
                // Connect to player stats
                var stats = _player.GetNodeOrNull("PlayerStats");
                if (stats != null)
                {
                    stats.Connect("HealthChanged", new Callable(this, nameof(OnHealthChanged)));
                }
                
                // Connect to player combat
                var combat = _player.GetNodeOrNull("PlayerCombat");
                if (combat != null)
                {
                    combat.Connect("WeaponChanged", new Callable(this, nameof(OnWeaponChanged)));
                }
                
                UpdateHealthDisplay();
            }
        }

        private void OnHealthChanged(int currentHealth, int maxHealth)
        {
            UpdateHealthDisplay();
        }

        private void OnWeaponChanged(string weaponName)
        {
            _weaponLabel.Text = $"Weapon: {weaponName}";
        }

        private void UpdateHealthDisplay()
        {
            if (_player == null) return;
            
            var stats = _player.GetNodeOrNull("PlayerStats");
            if (stats == null) return;
            
            int currentHealth = (int)stats.Get("CurrentHealth");
            int maxHealth = (int)stats.Get("MaxHealth");
            
            _healthLabel.Text = $"HP: {currentHealth}/{maxHealth}";
            _healthBar.MaxValue = maxHealth;
            _healthBar.Value = currentHealth;
            
            // Color based on health percentage
            float healthPercent = (float)currentHealth / maxHealth;
            if (healthPercent > 0.5f)
                _healthBar.Modulate = Colors.Green;
            else if (healthPercent > 0.25f)
                _healthBar.Modulate = Colors.Yellow;
            else
                _healthBar.Modulate = Colors.Red;
        }

        private void UpdateStats()
        {
            if (_gameManager == null) return;
            
            var stats = _gameManager.GetStatistics();
            int kills = (int)stats["enemies_killed"];
            int deaths = (int)stats["deaths"];
            float playTime = (float)stats["play_time"];
            
            int minutes = (int)(playTime / 60);
            int seconds = (int)(playTime % 60);
            
            _statsLabel.Text = $"Enemies Killed: {kills}\nDeaths: {deaths}\nTime: {minutes}:{seconds:D2}";
        }
    }
}
