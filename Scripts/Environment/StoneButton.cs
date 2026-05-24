using Godot;
using System;

namespace Demonteks.Environment
{
    /// <summary>
    /// An interactive stone button that opens a connected door or wall.
    /// Needs an InteractionArea Area2D to detect player proximity.
    /// </summary>
    public partial class StoneButton : StaticBody2D
    {
        [Export] public NodePath TargetDoorPath { get; set; }

        private bool _isPressed = false;
        private Sprite2D _sprite;
        private Label _interactPrompt;
        private AudioStreamPlayer2D _audioPlayer;

        public override void _Ready()
        {
            _sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
            _interactPrompt = GetNodeOrNull<Label>("InteractionPrompt");

            if (_interactPrompt != null)
                _interactPrompt.Visible = false;

            // Set up audio
            _audioPlayer = new AudioStreamPlayer2D();
            _audioPlayer.Bus = "SFX";
            AddChild(_audioPlayer);
            var clip = ResourceLoader.Load<AudioStream>("res://Assets/audio/sfx/Player/Lever_Pull.mp3");
            if (clip != null) _audioPlayer.Stream = clip;

            var interactionArea = GetNodeOrNull<Area2D>("InteractionArea");
            if (interactionArea != null)
            {
                interactionArea.BodyEntered += OnBodyEntered;
                interactionArea.BodyExited += OnBodyExited;
            }
        }

        private void OnBodyEntered(Node2D body)
        {
            if (_isPressed) return;

            if (body.IsInGroup("player"))
            {
                if (_interactPrompt != null)
                    _interactPrompt.Visible = true;
            }
        }

        private void OnBodyExited(Node2D body)
        {
            if (body.IsInGroup("player"))
            {
                if (_interactPrompt != null)
                    _interactPrompt.Visible = false;
            }
        }

        public override void _Process(double delta)
        {
            if (_isPressed) return;

            // Check if player is near (prompt is visible)
            if (_interactPrompt != null && _interactPrompt.Visible)
            {
                if (Input.IsActionJustPressed("interact"))
                {
                    PressButton();
                }
            }
        }

        private void PressButton()
        {
            _isPressed = true;

            // Play lever sound
            _audioPlayer?.Play();

            if (_interactPrompt != null)
                _interactPrompt.QueueFree();

            // Visual feedback: sink the button down 4 pixels
            if (_sprite != null)
            {
                var tween = CreateTween();
                tween.TweenProperty(_sprite, "position:y", _sprite.Position.Y + 4, 0.2f);
            }

            // Open the target door
            if (TargetDoorPath != null)
            {
                var door = GetNodeOrNull<Node2D>(TargetDoorPath);
                if (door != null)
                {
                    GD.Print("StoneButton pressed — opening door!");
                    // Hide and disable collision of the door
                    var doorTween = CreateTween();
                    doorTween.TweenProperty(door, "modulate:a", 0.0f, 0.5f);
                    doorTween.TweenCallback(Callable.From(door.QueueFree));
                }
            }
        }
    }
}
