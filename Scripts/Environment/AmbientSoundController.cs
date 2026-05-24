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
            _audioPlayer.Bus = "SFX";
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

        // All known ambient audio files per directory.
        // DirAccess.Open("res://...") does NOT work in exported builds — files must be listed explicitly.
        private static readonly Dictionary<string, string[]> _knownFiles = new()
        {
            ["res://Assets/audio/sfx/Level1_Jungle"] = new[]
            {
                "Birds_Chirping.ogg", "Bush_Rustle.ogg",
            },
            ["res://Assets/audio/sfx/Level2_Dungeon"] = new[]
            {
                "Chains_Rattle.mp3", "Distant_Screams.mp3", "Prison_Door_Creak.mp3",
            },
            ["res://Assets/audio/sfx/Level3_Castle"] = new[]
            {
                "Demon_Whispers.mp3", "Distant_Chanting.mp3", "Fire_Roaring.mp3", "Hell_Portal_Hum.mp3",
            },
        };

        private void LoadAudioFiles()
        {
            if (string.IsNullOrEmpty(AudioDirectory)) return;

            if (!_knownFiles.TryGetValue(AudioDirectory, out var fileNames))
            {
                GD.PrintErr($"AmbientSoundController: No known file list for directory '{AudioDirectory}'. Add it to _knownFiles.");
                return;
            }

            foreach (var fileName in fileNames)
            {
                string path = $"{AudioDirectory}/{fileName}";
                if (!ResourceLoader.Exists(path))
                    continue;

                var stream = ResourceLoader.Load<AudioStream>(path);
                if (stream != null)
                    _soundClips.Add(stream);
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
