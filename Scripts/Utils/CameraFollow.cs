using Godot;

namespace Demonteks.Utils
{
    /// <summary>
    /// Camera that smoothly follows the player.
    /// Add as child of the level root or the Player node.
    /// </summary>
    public partial class CameraFollow : Camera2D
    {
        [Export] public NodePath TargetPath { get; set; }
        [Export] public float SmoothSpeed { get; set; } = 5.0f;
        [Export] public Vector2 FollowOffset { get; set; } = new Vector2(0, -30);

        /// <summary>
        /// Horizontal limits. Set to 0 to disable.
        /// </summary>
        [ExportCategory("Limits")]
        [Export] public int LimitLeftValue { get; set; } = 0;
        [Export] public int LimitRightValue { get; set; } = 3200;
        [Export] public int LimitTopValue { get; set; } = -600;
        [Export] public int LimitBottomValue { get; set; } = 1500;

        private Node2D _target;

        public override void _Ready()
        {
            // Try to find the target
            if (TargetPath != null && !TargetPath.IsEmpty)
            {
                _target = GetNode<Node2D>(TargetPath);
            }

            if (_target == null)
            {
                // Auto-find player in scene
                CallDeferred(nameof(FindPlayer));
            }

            // Set camera limits
            LimitLeft = LimitLeftValue;
            LimitRight = LimitRightValue;
            LimitTop = LimitTopValue;
            LimitBottom = LimitBottomValue;

            // Enable smooth follow
            PositionSmoothingEnabled = true;
            PositionSmoothingSpeed = SmoothSpeed;

            // Make this the current camera
            MakeCurrent();

            GD.Print("CameraFollow ready");
        }

        private void FindPlayer()
        {
            _target = GetTree().GetFirstNodeInGroup("player") as Node2D;
            if (_target != null)
            {
                GD.Print($"CameraFollow: Found player at {_target.GlobalPosition}");
            }
            else
            {
                GD.PrintErr("CameraFollow: No player found in 'player' group");
            }
        }

        public override void _Process(double delta)
        {
            if (_target == null) return;
            GlobalPosition = _target.GlobalPosition + FollowOffset;
        }

        /// <summary>
        /// Update camera limits at runtime (e.g., for different zones).
        /// </summary>
        public void SetLimits(int left, int right, int top, int bottom)
        {
            LimitLeft = left;
            LimitRight = right;
            LimitTop = top;
            LimitBottom = bottom;
        }
    }
}
