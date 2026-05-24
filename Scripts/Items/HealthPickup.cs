using Godot;

namespace Demonteks.Items
{
    /// <summary>
    /// Health pickup that restores player HP when player presses interact key.
    /// Place in levels past the midpoint for extra survivability.
    /// </summary>
    public partial class HealthPickup : Area2D
    {
        [Export] public int HealAmount { get; set; } = 25;
        [Export] public bool OneTimeUse { get; set; } = true;

        private bool _collected = false;
        private AnimatedSprite2D _sprite;
        private Node2D _playerNearby;
        private Label _interactLabel;
        private AudioStreamPlayer2D _audioPlayer;

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
            BodyExited += OnBodyExited;
            _sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            _interactLabel = GetNodeOrNull<Label>("InteractLabel");

            if (_interactLabel != null)
            {
                _interactLabel.Visible = false;
            }

            // Set up audio
            _audioPlayer = new AudioStreamPlayer2D();
            _audioPlayer.Bus = "SFX";
            AddChild(_audioPlayer);
            var clip = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/Player/Player_Heal.mp3");
            if (clip != null) _audioPlayer.Stream = clip;

            // Pulse animation if sprite exists
            if (_sprite != null)
            {
                var tween = CreateTween().SetLoops();
                tween.TweenProperty(_sprite, "modulate:a", 0.6f, 0.8f);
                tween.TweenProperty(_sprite, "modulate:a", 1.0f, 0.8f);
            }
        }

        public override void _Process(double delta)
        {
            if (_playerNearby != null && !_collected)
            {
                if (Input.IsActionJustPressed("interact"))
                {
                    PickupHealth();
                }
            }
        }

        private void OnBodyEntered(Node2D body)
        {
            if (_collected) return;
            if (body.IsInGroup("player"))
            {
                _playerNearby = body;
                if (_interactLabel != null)
                {
                    _interactLabel.Visible = true;
                }
            }
        }

        private void OnBodyExited(Node2D body)
        {
            if (body == _playerNearby)
            {
                _playerNearby = null;
                if (_interactLabel != null)
                {
                    _interactLabel.Visible = false;
                }
            }
        }

        private void PickupHealth()
        {
            if (_playerNearby == null || _collected) return;

            // Try to heal the player
            var stats = _playerNearby.GetNodeOrNull<Demonteks.Player.PlayerStats>("PlayerStats");
            if (stats != null)
            {
                // Optionally prevent pickup if full health
                if (stats.IsFullHealth)
                {
                    GD.Print("Player is already at full health.");
                    return;
                }

                stats.Heal(HealAmount);
                GD.Print($"Player healed for {HealAmount} HP!");

                // Play heal sound
                _audioPlayer?.Play();

                if (OneTimeUse)
                {
                    _collected = true;

                    if (_interactLabel != null)
                    {
                        _interactLabel.Visible = false;
                    }

                    // Quick fade-out effect then remove
                    var tween = CreateTween();
                    tween.TweenProperty(this, "modulate:a", 0.0f, 0.3f);
                    tween.TweenCallback(Callable.From(QueueFree));
                }
            }
        }
    }
}
