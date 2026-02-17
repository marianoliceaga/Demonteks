using Godot;

namespace Demonteks.Utils
{
    /// <summary>
    /// Kill zone — respawns or kills the player when they fall off the map.
    /// Place below the level as an Area2D with a wide collision shape.
    /// </summary>
    public partial class KillZone : Area2D
    {
        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node2D body)
        {
            if (!body.IsInGroup("player")) return;

            GD.Print("Player fell into kill zone!");

            var gameManager = GetNode<Management.GameManager>("/root/GameManager");
            gameManager?.OnPlayerDied();
        }
    }
}
