using Godot;
using System;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    /// <summary>
    /// Randomly plays one of several AudioStreams at random intervals.
    /// </summary>
    public partial class AmbientSoundController : Node2D
    {
        [Export(PropertyHint.Dir)]
        public string AudioDirectory { get; set; } = "res://Assets/audio/sfx/Level1_Jungle";

        [Export]
        public float MinWaitTime { get; set; } = 5f;

        [Export]
        public float MaxWaitTime { get; set; } = 15f;

        private List<AudioStream> _soundClips = new List<AudioStream>();
        private AudioStreamPlayer _audioPlayer;
        private Timer _timer;
        private RandomNumberGenerator _rng;

        public override void _Ready()
        {
            _audioPlayer = new AudioStreamPlayer();
            AddChild(_audioPlayer);

            _timer = new Timer();
            _timer.OneShot = true;
            _timer.Timeout += OnTimerTimeout;
            AddChild(_timer);

            _rng = new RandomNumberGenerator();
            _rng.Randomize();

            LoadAudioFiles();
            StartTimer();
        }

        private void LoadAudioFiles()
        {
            if (string.IsNullOrEmpty(AudioDirectory)) return;

            using var dir = DirAccess.Open(AudioDirectory);
            if (dir != null)
            {
                dir.ListDirBegin();
                string fileName = dir.GetNext();
                while (fileName != "")
                {
                    if (!dir.CurrentIsDir() && (fileName.EndsWith(".mp3") || fileName.EndsWith(".ogg") || fileName.EndsWith(".wav") || fileName.EndsWith(".mp3.import")))
                    {
                        if (fileName.EndsWith(".import"))
                        {
                            fileName = dir.GetNext();
                            continue;
                        }

                        string path = $"{AudioDirectory}/{fileName}";
                        var stream = ResourceLoader.Load<AudioStream>(path);
                        if (stream != null)
                        {
                            _soundClips.Add(stream);
                        }
                    }
                    fileName = dir.GetNext();
                }
            }
            else
            {
                GD.PrintErr($"AmbientSoundController: Failed to open directory {AudioDirectory}");
            }
        }

        private void StartTimer()
        {
            if (_soundClips.Count == 0) return;

            float waitTime = _rng.RandfRange(MinWaitTime, MaxWaitTime);
            _timer.Start(waitTime);
        }

        private void OnTimerTimeout()
        {
            if (_soundClips.Count > 0)
            {
                int index = _rng.RandiRange(0, _soundClips.Count - 1);
                var clip = _soundClips[index];
                if (clip != null)
                {
                    _audioPlayer.Stream = clip;
                    _audioPlayer.Play();
                }
            }

            // Restart timer for next clip
            StartTimer();
        }
    }
}
