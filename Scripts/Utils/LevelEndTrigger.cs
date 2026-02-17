using Godot;

namespace Demonteks.Utils
{
    /// <summary>
    /// Triggers level completion when the player enters this area.
    /// Attach to the LevelEnd Area2D in each level scene.
    /// </summary>
    public partial class LevelEndTrigger : Area2D
    {
        private bool _triggered = false;

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node2D body)
        {
            if (_triggered) return;
            if (!body.IsInGroup("player")) return;

            _triggered = true;
            GD.Print("Level End reached! Completing level...");

            var gameManager = GetNode<Management.GameManager>("/root/GameManager");
            gameManager?.CompleteLevel();
        }
    }
}
