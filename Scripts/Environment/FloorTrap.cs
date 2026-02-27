using Godot;

namespace Demonteks.Environment
{
    /// <summary>
    /// One-shot floor trap that ambushes the player with 6 Demonteks when triggered.
    /// </summary>
    public partial class FloorTrap : Area2D
    {
        [Export] public PackedScene DemontekScene { get; set; }
        [Export]
        public Vector2[] SpawnOffsets { get; set; } = new Vector2[]
        {
            new Vector2(-80, 0),
            new Vector2(80, 0),
            new Vector2(-160, 0),
            new Vector2(160, 0),
            new Vector2(-40, -80),
            new Vector2(40, -80)
        };

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
            Monitoring = false;
            Monitorable = false;

            // Flash the marker red briefly
            if (GetNodeOrNull("TrapMarker") is ColorRect marker)
                marker.Color = new Color(1f, 0.1f, 0.1f, 0.9f);

            // Spawn enemies
            if (DemontekScene == null)
            {
                DemontekScene = GD.Load<PackedScene>("res://Scenes/Characters/Demontek.tscn");
            }

            var parent = GetParent();
            foreach (Vector2 offset in SpawnOffsets)
            {
                var demontek = DemontekScene.Instantiate<Node2D>();
                parent.AddChild(demontek);
                demontek.GlobalPosition = GlobalPosition + offset;
                GD.Print($"[FloorTrap] Spawned Demontek at {demontek.GlobalPosition}");
            }

            GD.Print("[FloorTrap] Ambush triggered!");
        }
    }
}
