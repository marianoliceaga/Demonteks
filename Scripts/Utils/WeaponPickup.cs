using Godot;
using System;

namespace Demonteks.Utils
{
    /// <summary>
    /// Weapon pickup that player can collect to unlock new weapons.
    /// </summary>
    public partial class WeaponPickup : Area2D
    {
        [Export] public Player.WeaponType WeaponType { get; set; } = Player.WeaponType.Machete;
        
        private Label _label;
        private Label _interactLabel;
        private Node2D _playerNearby;
        private bool _isPickedUp = false;

        public override void _Ready()
        {
            _label = GetNode<Label>("Label");
            _interactLabel = GetNode<Label>("InteractLabel");
            
            // Set weapon name
            _label.Text = WeaponType.ToString().ToUpper();
            
            // Hide interact label initially
            _interactLabel.Visible = false;
        }

        public override void _Process(double delta)
        {
            if (_playerNearby != null && !_isPickedUp)
            {
                // Check for F key press
                if (Input.IsActionJustPressed("interact"))
                {
                    PickupWeapon();
                }
            }
        }

        private void _on_body_entered(Node2D body)
        {
            if (body.IsInGroup("player") && !_isPickedUp)
            {
                _playerNearby = body;
                _interactLabel.Visible = true;
            }
        }

        private void _on_body_exited(Node2D body)
        {
            if (body == _playerNearby)
            {
                _playerNearby = null;
                _interactLabel.Visible = false;
            }
        }

        private void PickupWeapon()
        {
            if (_playerNearby == null || _isPickedUp) return;
            
            _isPickedUp = true;
            
            // Give weapon to player
            var combat = _playerNearby.GetNodeOrNull("PlayerCombat");
            if (combat != null && combat.HasMethod("PickupWeapon"))
            {
                combat.Call("PickupWeapon", (int)WeaponType);
                
                // Show pickup message
                _label.Text = $"{WeaponType} ACQUIRED!";
                _interactLabel.Visible = false;
                
                // Fade out and remove
                var tween = CreateTween();
                tween.TweenProperty(this, "modulate:a", 0.0f, 1.0f);
                tween.TweenCallback(Callable.From(QueueFree));
                
                GD.Print($"Player picked up {WeaponType}!");
            }
        }
    }
}
