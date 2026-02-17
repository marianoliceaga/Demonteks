using Godot;

namespace Demonteks.Items
{
    /// <summary>
    /// Health pickup that restores player HP when touched.
    /// Place in levels past the midpoint for extra survivability.
    /// </summary>
    public partial class HealthPickup : Area2D
    {
        [Export] public int HealAmount { get; set; } = 25;
        [Export] public bool OneTimeUse { get; set; } = true;

        private bool _collected = false;
        private AnimatedSprite2D _sprite;

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
            _sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

            // Pulse animation if sprite exists
            if (_sprite != null)
            {
                var tween = CreateTween().SetLoops();
                tween.TweenProperty(_sprite, "modulate:a", 0.6f, 0.8f);
                tween.TweenProperty(_sprite, "modulate:a", 1.0f, 0.8f);
            }
        }

        private void OnBodyEntered(Node2D body)
        {
            if (_collected) return;
            if (!body.IsInGroup("player")) return;

            // Try to heal the player
            var stats = body.GetNodeOrNull<Node>("PlayerStats");
            if (stats != null && stats.HasMethod("Heal"))
            {
                stats.Call("Heal", HealAmount);
                GD.Print($"Player healed for {HealAmount} HP!");
            }

            if (OneTimeUse)
            {
                _collected = true;

                // Quick fade-out effect then remove
                var tween = CreateTween();
                tween.TweenProperty(this, "modulate:a", 0.0f, 0.3f);
                tween.TweenCallback(Callable.From(QueueFree));
            }
        }
    }
}
