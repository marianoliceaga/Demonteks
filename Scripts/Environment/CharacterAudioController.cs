using Godot;
using System;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    /// <summary>
    /// Dynamically loads and plays Character SFX by action name matching.
    /// Eliminates the need for strict file renaming by using string contains logic.
    /// </summary>
    public partial class CharacterAudioController : Node2D
    {
        [Export(PropertyHint.Dir)]
        public string AudioDirectory { get; set; } = "res://Assets/audio/sfx/Player";

        private AudioStreamPlayer2D _audioPlayer;

        // Maps an action name (e.g. "Attack") to a list of matching AudioStreams
        private Dictionary<string, List<AudioStream>> _actionCache = new Dictionary<string, List<AudioStream>>(StringComparer.OrdinalIgnoreCase);
        private RandomNumberGenerator _rng = new RandomNumberGenerator();

        public override void _Ready()
        {
            _audioPlayer = new AudioStreamPlayer2D();
            _audioPlayer.Bus = "SFX";
            AddChild(_audioPlayer);
            _rng.Randomize();

            LoadAudioFiles();
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
                        // Ignore import files, but process the base extension if they show up
                        if (fileName.EndsWith(".import"))
                        {
                            fileName = dir.GetNext();
                            continue;
                        }

                        string path = $"{AudioDirectory}/{fileName}";
                        var stream = ResourceLoader.Load<AudioStream>(path);
                        if (stream != null)
                        {
                            CategorizeStream(fileName, stream);
                        }
                    }
                    fileName = dir.GetNext();
                }
            }
            else
            {
                GD.PrintErr($"CharacterAudioController: Failed to open directory {AudioDirectory}");
            }
        }

        private void CategorizeStream(string fileName, AudioStream stream)
        {
            var semanticMap = new Dictionary<string, string[]>
            {
                { "Attack", new[] { "attack", "combat", "punch", "pound", "projectile", "explosion" } },
                { "Slash", new[] { "slash", "swing" } },
                { "Hurt", new[] { "hurt", "hit", "stagger" } },
                { "Death", new[] { "death", "collapse", "cry" } },
                { "Jump", new[] { "jump", "land" } },
                { "Dash", new[] { "dash", "escape" } },
                { "Footstep", new[] { "footstep", "march", "stomp" } },
                { "SeeEnemies", new[] { "seeenemies", "seeportal", "wakeup", "entrance" } },
                { "Idle", new[] { "idle", "breathing" } },
                { "Roar", new[] { "roar", "taunt", "mercy", "enrage", "phase", "whispers", "chanting", "screams" } },
                { "Interact", new[] { "checkpoint", "lever", "heal" } },
                { "Victory", new[] { "victory", "respawn" } }
            };

            string lowerFileName = fileName.ToLower();

            foreach (var kvp in semanticMap)
            {
                string actionName = kvp.Key;
                foreach (var keyword in kvp.Value)
                {
                    if (lowerFileName.Contains(keyword))
                    {
                        if (!_actionCache.ContainsKey(actionName))
                        {
                            _actionCache[actionName] = new List<AudioStream>();
                        }
                        _actionCache[actionName].Add(stream);
                        break; // Only categorize once per action property
                    }
                }
            }
        }

        /// <summary>
        /// Plays an audio file that matches the action name (e.g., "Attack").
        /// Automatically picks one randomly if there are multiple matches.
        /// </summary>
        public void PlayAction(string actionName)
        {
            if (_actionCache.TryGetValue(actionName, out var streams) && streams.Count > 0)
            {
                int index = _rng.RandiRange(0, streams.Count - 1);
                _audioPlayer.Stream = streams[index];
                _audioPlayer.Play();
            }
            else
            {
                // Un-comment for debugging missing sounds
                // GD.Print($"CharacterAudioController: No sound mapped for action '{actionName}' in {AudioDirectory}");
            }
        }
    }
}
