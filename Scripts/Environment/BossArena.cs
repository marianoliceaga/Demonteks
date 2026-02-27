using Godot;

namespace Demonteks.Environment
{
    /// <summary>
    /// Boss arena lock-in mechanic. When the player enters the arena:
    ///   - entrance and exit barriers become solid
    ///   - boss activates
    /// When the boss dies:
    ///   - barriers are removed
    ///   - if final boss, Earth Portal is spawned
    /// </summary>
    public partial class BossArena : Area2D
    {
        [Export] public NodePath BossPath { get; set; }
        [Export] public NodePath EntranceBarrierPath { get; set; }
        [Export] public NodePath ExitBarrierPath { get; set; }
        [Export] public NodePath PortalSpawnPointPath { get; set; }
        [Export] public bool SpawnPortalOnBossDeath { get; set; } = false;

        private Node2D _boss;
        private StaticBody2D _entranceBarrier;
        private StaticBody2D _exitBarrier;
        private Marker2D _portalSpawn;
        private bool _activated = false;

        private PackedScene _portalScene;

        public override void _Ready()
        {
            BodyEntered += OnPlayerEntered;

            if (BossPath != null)
                _boss = GetNodeOrNull<Node2D>(BossPath);

            if (EntranceBarrierPath != null)
                _entranceBarrier = GetNodeOrNull<StaticBody2D>(EntranceBarrierPath);

            if (ExitBarrierPath != null)
                _exitBarrier = GetNodeOrNull<StaticBody2D>(ExitBarrierPath);

            if (PortalSpawnPointPath != null)
                _portalSpawn = GetNodeOrNull<Marker2D>(PortalSpawnPointPath);

            // Hide barriers initially
            SetBarriers(false);

            if (SpawnPortalOnBossDeath)
                _portalScene = GD.Load<PackedScene>("res://Scenes/Prefabs/EarthPortal.tscn");
        }

        private void OnPlayerEntered(Node2D body)
        {
            if (_activated || !body.IsInGroup("player")) return;
            _activated = true;

            // Lock the arena
            SetBarriers(true);
            GD.Print("[BossArena] Player entered arena — barriers locked.");

            // Connect to boss death
            if (_boss != null && _boss.HasSignal("Died"))
            {
                _boss.Connect("Died", Callable.From(OnBossDied));
                GD.Print($"[BossArena] Connected to boss Died signal: {_boss.Name}");
            }
        }

        private void OnBossDied()
        {
            SetBarriers(false);
            GD.Print("[BossArena] Boss defeated — barriers removed.");

            if (SpawnPortalOnBossDeath && _portalScene != null && _portalSpawn != null)
            {
                var portal = _portalScene.Instantiate<Node2D>();
                GetParent().AddChild(portal);
                portal.GlobalPosition = _portalSpawn.GlobalPosition;
                GD.Print("[BossArena] Earth Portal spawned!");
            }
        }

        private void SetBarriers(bool active)
        {
            if (_entranceBarrier != null)
            {
                _entranceBarrier.Visible = active;
                _entranceBarrier.ProcessMode = active
                    ? ProcessModeEnum.Inherit
                    : ProcessModeEnum.Disabled;
            }

            if (_exitBarrier != null)
            {
                _exitBarrier.Visible = active;
                _exitBarrier.ProcessMode = active
                    ? ProcessModeEnum.Inherit
                    : ProcessModeEnum.Disabled;
            }
        }
    }
}
