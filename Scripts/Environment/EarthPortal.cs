using Godot;

namespace Demonteks.Environment
{
    /// <summary>
    /// Interactable portal that ends the game when the player presses the interact key.
    /// Spawned by BossArena after King Demontek is defeated.
    /// </summary>
    public partial class EarthPortal : Area2D
    {
        [Export] public string InteractAction { get; set; } = "interact";

        private bool _playerInRange = false;
        private Label _label;
        private AnimatedSprite2D _sprite;

        public override void _Ready()
        {
            BodyEntered += (body) =>
            {
                if (body.IsInGroup("player"))
                {
                    _playerInRange = true;
                    if (_label != null) _label.Visible = true;
                }
            };

            BodyExited += (body) =>
            {
                if (body.IsInGroup("player"))
                {
                    _playerInRange = false;
                    if (_label != null) _label.Visible = false;
                }
            };

            _label = GetNodeOrNull<Label>("InteractionLabel");
            _sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

            if (_label != null) _label.Visible = false;
        }

        public override void _Input(InputEvent @event)
        {
            if (!_playerInRange) return;

            if (@event.IsActionPressed(InteractAction))
            {
                GD.Print("[EarthPortal] Player chose to escape through the portal!");
                var gm = GetNode<Management.GameManager>("/root/GameManager");
                gm?.WinGame();
            }
        }

        public override void _Process(double delta)
        {
            // Gentle rotation / pulse for the portal glow
            if (_sprite != null)
                _sprite.RotationDegrees += (float)(delta * 45.0);
        }
    }
}
