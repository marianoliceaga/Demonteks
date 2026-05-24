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
            if (body.IsInGroup("player"))
            {
                GD.Print("Player fell into kill zone!");

                if (body.HasMethod("TakeDamage"))
                {
                    body.Call("TakeDamage", 9999);
                }
                else
                {
                    var gameManager = GetNode<Management.GameManager>("/root/GameManager");
                    gameManager?.OnPlayerDied();
                }
            }
            else if (body.IsInGroup("enemies"))
            {
                GD.Print($"Enemy {body.Name} fell into kill zone, respawning...");

                if (body.HasMethod("Respawn"))
                {
                    body.Call("Respawn");
                }
            }
        }
    }
}
