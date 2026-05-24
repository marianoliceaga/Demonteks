using Godot;

namespace Demonteks.UI
{
    /// <summary>
    /// Plays a video clip before transitioning to a game scene.
    /// Supports skip via any key press.
    /// NOTE: Godot 4 supports .ogv (Ogg Theora) video only.
    /// Convert MP4s with: ffmpeg -i input.mp4 output.ogv
    /// </summary>
    public partial class VideoIntroController : Node
    {
        [Export] public string VideoPath { get; set; } = "";
        [Export] public string NextScene { get; set; } = "";

        private VideoStreamPlayer _player;
        private bool _transitioning = false;

        public override void _Ready()
        {
            _player = GetNodeOrNull<VideoStreamPlayer>("VideoStreamPlayer");

            if (_player == null)
            {
                GD.PushWarning("[VideoIntro] No VideoStreamPlayer found — skipping to next scene.");
                TransitionToNext();
                return;
            }

            if (!string.IsNullOrEmpty(VideoPath) && FileAccess.FileExists(VideoPath))
            {
                var stream = GD.Load<VideoStream>(VideoPath);
                if (stream != null)
                {
                    _player.Stream = stream;
                    _player.Play();
                    _player.Finished += TransitionToNext;
                }
                else
                {
                    GD.Print($"[VideoIntro] Failed to load video at '{VideoPath}' — skipping.");
                    TransitionToNext();
                }
            }
            else
            {
                GD.Print($"[VideoIntro] No video at '{VideoPath}' — skipping.");
                TransitionToNext();
            }
        }

        public override void _Input(InputEvent @event)
        {
            // Skip on any key/button press
            if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
                TransitionToNext();
            else if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
                TransitionToNext();
        }

        private void TransitionToNext()
        {
            if (_transitioning) return;
            _transitioning = true;

            if (!string.IsNullOrEmpty(NextScene) && ResourceLoader.Exists(NextScene))
            {
                GetTree().ChangeSceneToFile(NextScene);
            }
            else
            {
                GD.PushWarning($"[VideoIntro] NextScene '{NextScene}' not found!");
                GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
            }
        }
    }
}
