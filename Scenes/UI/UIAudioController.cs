using Godot;

namespace Demonteks.UI
{
    /// <summary>
    /// Global UI Audio Controller that automatically hooks up interaction sounds to all buttons in the game.
    /// Loaded as an AutoLoad to ensure it tracks the entire SceneTree.
    /// </summary>
    public partial class UIAudioController : Node
    {
        private AudioStreamPlayer _sfxPlayer;
        private AudioStream _clickStream;
        private AudioStream _hoverStream;

        public override void _Ready()
        {
            ProcessMode = ProcessModeEnum.Always;

            _sfxPlayer = new AudioStreamPlayer();
            _sfxPlayer.Bus = "SFX";
            AddChild(_sfxPlayer);

            _clickStream = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/UI_Sounds/Button_Click.mp3");
            _hoverStream = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/UI_Sounds/Button_Hover.mp3");

            // Attach to already existing nodes
            ConnectButtonsInTree(GetTree().Root);

            // Hook up the signal to track any nodes added dynamically or during scene changes
            GetTree().NodeAdded += OnNodeAdded;
        }

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
            if (_clickStream != null)
            {
                _sfxPlayer.Stream = _clickStream;
                _sfxPlayer.Play();
            }
        }

        private void OnButtonHovered()
        {
            if (_hoverStream != null)
            {
                _sfxPlayer.Stream = _hoverStream;
                _sfxPlayer.Play();
            }
        }
    }
}
