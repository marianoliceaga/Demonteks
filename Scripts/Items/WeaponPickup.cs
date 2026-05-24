using Godot;
using Demonteks.Player;

namespace Demonteks.Items
{
    /// <summary>
    /// Weapon pickup that gives the player a new weapon when they press the interact key.
    /// Place in levels to allow the player to acquire and switch weapons.
    /// The weapon type is set per-instance via the exported WeaponToPickup property.
    /// </summary>
    public partial class WeaponPickup : Area2D
    {
        [Export] public WeaponType WeaponToPickup { get; set; } = WeaponType.Machete;
        [Export] public bool OneTimeUse { get; set; } = true;

        private bool _collected = false;
        private Sprite2D _sprite;
        private Node2D _playerNearby;
        private Label _interactLabel;
        private AudioStreamPlayer2D _audioPlayer;

        // Weapon display names matching PlayerCombat definitions
        private static readonly System.Collections.Generic.Dictionary<WeaponType, string> WeaponNames = new()
        {
            { WeaponType.Knife,         "Knife" },
            { WeaponType.Machete,       "Machete" },
            { WeaponType.Sword,         "Ancient Sword" },
            { WeaponType.DemontekSaber, "Demontek Saber" },
            { WeaponType.Sawtooth,      "Sawtooth" },
            { WeaponType.AlienSword,    "Alien Sword" }
        };

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
            BodyExited += OnBodyExited;

            _sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
            _interactLabel = GetNodeOrNull<Label>("InteractLabel");

            if (_interactLabel != null)
            {
                string name = WeaponNames.TryGetValue(WeaponToPickup, out var n) ? n : WeaponToPickup.ToString();
                _interactLabel.Text = $"[F] Pick up {name}";
                _interactLabel.Visible = false;
            }

            // Set up pickup sound
            _audioPlayer = new AudioStreamPlayer2D();
            _audioPlayer.Bus = "SFX";
            AddChild(_audioPlayer);

            // Try weapon-specific pickup sound, fall back to generic
            var clip = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/Player/Player_Heal.mp3");
            if (clip == null)
                clip = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/Player/Player_Heal.mp3");
            if (clip != null)
                _audioPlayer.Stream = clip;

            // Floating bob animation
            if (_sprite != null)
            {
                var tween = CreateTween().SetLoops();
                tween.TweenProperty(_sprite, "position:y", -4f, 0.7f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
                tween.TweenProperty(_sprite, "position:y", 4f, 0.7f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
            }

            // Glow pulse on modulate
            var glowTween = CreateTween().SetLoops();
            glowTween.TweenProperty(this, "modulate:v", 0.75f, 0.6f);
            glowTween.TweenProperty(this, "modulate:v", 1.0f, 0.6f);

            GD.Print($"WeaponPickup ready: {WeaponToPickup}");
        }

        public override void _Process(double delta)
        {
            if (_playerNearby != null && !_collected)
            {
                if (Input.IsActionJustPressed("interact"))
                {
                    PickupWeapon();
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
                    _interactLabel.Visible = true;
            }
        }

        private void OnBodyExited(Node2D body)
        {
            if (body == _playerNearby)
            {
                _playerNearby = null;
                if (_interactLabel != null)
                    _interactLabel.Visible = false;
            }
        }

        private void PickupWeapon()
        {
            if (_playerNearby == null || _collected) return;

            var combat = _playerNearby.GetNodeOrNull<PlayerCombat>("PlayerCombat");
            if (combat == null)
            {
                GD.PushWarning("WeaponPickup: PlayerCombat node not found on player!");
                return;
            }

            combat.PickupWeapon(WeaponToPickup);
            GD.Print($"Player picked up: {WeaponToPickup}");

            // Save the game immediately so weapon unlocks persist through death or quit
            var saveManager = GetNodeOrNull<Management.SaveManager>("/root/SaveManager");
            if (saveManager != null)
            {
                saveManager.SaveGame();
            }

            _audioPlayer?.Play();

            if (OneTimeUse)
            {
                _collected = true;

                if (_interactLabel != null)
                    _interactLabel.Visible = false;

                // Fade out and remove
                var tween = CreateTween();
                tween.TweenProperty(this, "modulate:a", 0.0f, 0.3f);
                tween.TweenCallback(Callable.From(QueueFree));
            }
        }
    }
}
