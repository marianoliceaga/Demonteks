using Godot;
using System;

namespace Demonteks.Utils
{
    /// <summary>
    /// Checkpoint that saves player position and game state.
    /// </summary>
    public partial class Checkpoint : Area2D
    {
        [Export] public bool IsActivated { get; private set; } = false;

        private Label _activatedLabel;
        private ColorRect _flag;
        private Management.GameManager _gameManager;
        private Management.SaveManager _saveManager;
        private AudioStreamPlayer2D _audioPlayer;

        public override void _Ready()
        {
            _activatedLabel = GetNode<Label>("ActivatedLabel");
            _flag = GetNode<ColorRect>("Flag");

            _gameManager = GetNode<Management.GameManager>("/root/GameManager");
            _saveManager = GetNode<Management.SaveManager>("/root/SaveManager");

            // Set up audio
            _audioPlayer = new AudioStreamPlayer2D();
            _audioPlayer.Bus = "SFX";
            AddChild(_audioPlayer);
            var clip = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/Player/Checkpoint_Activate.mp3");
            if (clip != null) _audioPlayer.Stream = clip;

            // Set initial state
            if (!IsActivated)
            {
                _flag.Color = new Color(0.5f, 0.5f, 0.5f, 1f); // Gray when inactive
            }
        }

        private void _on_body_entered(Node2D body)
        {
            if (body.IsInGroup("player") && !IsActivated)
            {
                ActivateCheckpoint();
            }
        }

        private void ActivateCheckpoint()
        {
            IsActivated = true;

            // Play activation sound
            _audioPlayer?.Play();

            // Play save-game UI chime
            Demonteks.UI.UIAudioController.Instance?.PlaySound(Demonteks.UI.UIAudioController.UiSound.SaveGame);

            // Update visual
            _flag.Color = new Color(0f, 1f, 0.5f, 1f); // Green when active
            _activatedLabel.Visible = true;

            // Save game state
            _gameManager.SetCheckpoint(GlobalPosition);
            _saveManager.SaveGame();

            // Hide label after 2 seconds
            GetTree().CreateTimer(2.0f).Timeout += () => _activatedLabel.Visible = false;

            GD.Print($"Checkpoint activated at {GlobalPosition}");
        }
    }
}
