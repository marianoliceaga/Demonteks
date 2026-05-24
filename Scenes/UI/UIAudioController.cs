using Godot;

namespace Demonteks.UI
{
    /// <summary>
    /// Global UI Audio Controller that automatically hooks up interaction sounds to all buttons in the game.
    /// Loaded as an AutoLoad to ensure it tracks the entire SceneTree.
    /// Also exposes a static PlaySound() method for non-button UI events.
    /// </summary>
    public partial class UIAudioController : Node
    {
        // --- Singleton access ---
        public static UIAudioController Instance { get; private set; }

        private AudioStreamPlayer _sfxPlayer;

        // Pre-loaded UI sound streams
        private AudioStream _clickStream;
        private AudioStream _hoverStream;
        private AudioStream _menuOpenStream;
        private AudioStream _menuCloseStream;
        private AudioStream _menuNavigateStream;
        private AudioStream _achievementStream;
        private AudioStream _errorStream;
        private AudioStream _healthLowStream;
        private AudioStream _levelCompleteStream;
        private AudioStream _saveGameStream;
        private AudioStream _statIncreaseStream;
        private AudioStream _successStream;
        private AudioStream _newWeaponStream;

        public override void _Ready()
        {
            Instance = this;
            ProcessMode = ProcessModeEnum.Always;

            _sfxPlayer = new AudioStreamPlayer();
            _sfxPlayer.Bus = "SFX";
            AddChild(_sfxPlayer);

            // Load all UI sounds
            _clickStream = Load("Button_Click.mp3");
            _hoverStream = Load("Button_Hover.mp3");
            _menuOpenStream = Load("Menu_Open.mp3");
            _menuCloseStream = Load("Menu_Close.mp3");
            _menuNavigateStream = Load("Menu_Navigate.mp3");
            _achievementStream = Load("Achievement_Unlock.mp3");
            _errorStream = Load("Error_Buzz.mp3");
            _healthLowStream = Load("Health_Low_Warning.mp3");
            _levelCompleteStream = Load("Level_Complete.mp3");
            _saveGameStream = Load("Save_Game.mp3");
            _statIncreaseStream = Load("Stat_Increase.mp3");
            _successStream = Load("Success_Chime.mp3");
            _newWeaponStream = Load("New_Weapon_Notification.mp3");

            // Attach to already existing nodes
            ConnectButtonsInTree(GetTree().Root);

            // Hook up the signal to track any nodes added dynamically or during scene changes
            GetTree().NodeAdded += OnNodeAdded;
        }

        private static AudioStream Load(string fileName)
            => ResourceLoader.Load<AudioStream>($"res://Assets/audio/sfx/UI_Sounds/{fileName}");

        // ── Public API ──────────────────────────────────────────────────────────

        public enum UiSound
        {
            Click,
            Hover,
            MenuOpen,
            MenuClose,
            MenuNavigate,
            AchievementUnlock,
            ErrorBuzz,
            HealthLowWarning,
            LevelComplete,
            SaveGame,
            StatIncrease,
            SuccessChime,
            NewWeaponNotification,
        }

        /// <summary>
        /// Play a UI sound from anywhere in code:
        ///   UIAudioController.Instance?.PlaySound(UIAudioController.UiSound.LevelComplete);
        /// </summary>
        public void PlaySound(UiSound sound)
        {
            AudioStream stream = sound switch
            {
                UiSound.Click => _clickStream,
                UiSound.Hover => _hoverStream,
                UiSound.MenuOpen => _menuOpenStream,
                UiSound.MenuClose => _menuCloseStream,
                UiSound.MenuNavigate => _menuNavigateStream,
                UiSound.AchievementUnlock => _achievementStream,
                UiSound.ErrorBuzz => _errorStream,
                UiSound.HealthLowWarning => _healthLowStream,
                UiSound.LevelComplete => _levelCompleteStream,
                UiSound.SaveGame => _saveGameStream,
                UiSound.StatIncrease => _statIncreaseStream,
                UiSound.SuccessChime => _successStream,
                UiSound.NewWeaponNotification => _newWeaponStream,
                _ => null
            };

            if (stream != null)
            {
                _sfxPlayer.Stream = stream;
                _sfxPlayer.Play();
            }
        }

        // ── Auto-connect buttons ────────────────────────────────────────────────

        private void OnNodeAdded(Node node)
        {
            if (node is BaseButton button)
            {
                ConnectButton(button);
            }
        }

        private void ConnectButtonsInTree(Node root)
        {
            if (root is BaseButton button)
            {
                ConnectButton(button);
            }

            foreach (Node child in root.GetChildren())
            {
                ConnectButtonsInTree(child);
            }
        }

        private void ConnectButton(BaseButton button)
        {
            // Connect pressed signal
            if (!button.IsConnected("pressed", new Callable(this, MethodName.OnButtonPressed)))
            {
                button.Connect("pressed", new Callable(this, MethodName.OnButtonPressed));
            }

            // Connect hover/focus signals
            if (!button.IsConnected("mouse_entered", new Callable(this, MethodName.OnButtonHovered)))
            {
                button.Connect("mouse_entered", new Callable(this, MethodName.OnButtonHovered));
            }
            if (!button.IsConnected("focus_entered", new Callable(this, MethodName.OnButtonHovered)))
            {
                button.Connect("focus_entered", new Callable(this, MethodName.OnButtonHovered));
            }
        }

        private void OnButtonPressed()
        {
            PlaySound(UiSound.Click);
        }

        private void OnButtonHovered()
        {
            PlaySound(UiSound.Hover);
        }
    }
}
