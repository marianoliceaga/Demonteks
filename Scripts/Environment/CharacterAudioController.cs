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

        // All known audio files per subdirectory.
        // DirAccess.Open("res://...") does NOT work in exported builds — files must be listed explicitly.
        private static readonly Dictionary<string, string[]> _knownFiles = new()
        {
            ["res://Assets/audio/sfx/Player"] = new[]
            {
                "Checkpoint_Activate.mp3", "Dash.mp3", "Footstep_Stone.mp3", "Jump.mp3",
                "Knife_Hit_Enemy.mp3", "Knife_Slash.mp3", "Land_Hard.mp3", "Land_Soft.mp3",
                "Lever_Pull.mp3", "Machete_Hit_Enemy.mp3", "Machete_Slash.mp3",
                "Player_Attack_01.mp3", "Player_Attack_02.mp3", "Player_Attack_03.mp3",
                "Player_Death.mp3", "Player_Death_Voice.mp3", "Player_Escape.mp3",
                "Player_Heal.mp3", "Player_Heavy_Breathing.mp3",
                "Player_Hurt_01.mp3", "Player_Hurt_02.mp3", "Player_Hurt_03.mp3",
                "Player_Hurt_Heavy.mp3", "Player_Hurt_Light.mp3", "Player_Hurt_Medium.mp3",
                "Player_Jump_01.mp3", "Player_Jump_02.mp3", "Player_Jump_03.mp3",
                "Player_Punch.mp3", "Player_SeeEnemies.mp3", "Player_SeePortal.mp3",
                "Player_Victory.mp3", "Player_WakeUp.mp3", "Respawn.mp3", "Sword_Swing.mp3",
            },
            ["res://Assets/audio/sfx/Enemies/Demontek"] = new[]
            {
                "Demon_Whispers.mp3", "Demontek_Attack.mp3", "Demontek_Death.mp3",
                "Demontek_Footstep.mp3", "Demontek_Hit.mp3", "Demontek_Idle.mp3",
                "Distant_Screams.mp3", "TekTek1.mp3",
            },
            ["res://Assets/audio/sfx/Enemies/Goliath"] = new[]
            {
                "Goliath_Death_Collapse.mp3", "Goliath_Death_Cry.mp3", "Goliath_Death_Roar_Voice.mp3",
                "Goliath_Entrance_Roar.mp3", "Goliath_Ground_Pound.mp3", "Goliath_Hit.mp3",
                "Goliath_Idle_Breathing.mp3", "Goliath_Multi_Arm_Swipe.mp3", "Goliath_Phase2_Roar.mp3",
                "Goliath_Phase3_Enrage.mp3", "Goliath_Roar_Attack.mp3", "Goliath_Stagger.mp3",
                "Goliath_Stomp.mp3", "Goliath_Victory_Roar.mp3", "TekTek4.mp3",
            },
            ["res://Assets/audio/sfx/Enemies/Guard"] = new[]
            {
                "Guard_Baton_Swing.mp3", "Guard_Death.mp3", "Guard_Hit.mp3",
                "Guard_Idle.mp3", "Guard_March.mp3", "TekTek2.mp3",
            },
            ["res://Assets/audio/sfx/Enemies/King"] = new[]
            {
                "King_Combat_01.mp3", "King_Combat_02.mp3", "King_Death_Explosion.mp3",
                "King_Death_Voice.mp3", "King_Entrance_01.mp3", "King_Entrance_02.mp3",
                "King_Fire_Explosion.mp3", "King_Fire_Projectile.mp3", "King_Hit.mp3",
                "King_Mercy_01.mp3", "King_Mercy_02.mp3", "King_Mercy_03.mp3",
                "King_Phase2.mp3", "King_Phase3.mp3", "King_Stagger.mp3",
                "King_Sword_Swing_Heavy.mp3", "King_Taunt_01.mp3", "King_Taunt_02.mp3",
                "King_Taunt_03.mp3", "Knife_Hit_Enemy.mp3", "Knife_Slash.mp3", "TekTek5.mp3",
            },
            ["res://Assets/audio/sfx/Enemies/Mutant"] = new[]
            {
                "Mutant_Attack_Heavy.mp3", "Mutant_Death.mp3", "Mutant_Death_Roar.mp3",
                "Mutant_Footstep_Heavy.mp3", "Mutant_Hit.mp3", "Mutant_Idle_Breathing.mp3",
                "TekTek3.mp3",
            },
            ["res://Assets/audio/sfx/Enemies/TekTekVoices"] = new[]
            {
                "TekTek1.mp3", "TekTek2.mp3", "TekTek3.mp3", "TekTek4.mp3", "TekTek5.mp3",
            },
        };

        private void LoadAudioFiles()
        {
            if (string.IsNullOrEmpty(AudioDirectory)) return;

            if (!_knownFiles.TryGetValue(AudioDirectory, out var fileNames))
            {
                GD.PrintErr($"CharacterAudioController: No known file list for directory '{AudioDirectory}'. Add it to _knownFiles.");
                return;
            }

            foreach (var fileName in fileNames)
            {
                string path = $"{AudioDirectory}/{fileName}";
                if (!ResourceLoader.Exists(path))
                    continue;

                var stream = ResourceLoader.Load<AudioStream>(path);
                if (stream != null)
                    CategorizeStream(fileName, stream);
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
                { "Jump", new[] { "jump" } },
                { "Land", new[] { "land" } },
                { "Dash", new[] { "dash", "escape" } },
                { "Footstep", new[] { "footstep", "march", "stomp" } },
                { "SeeEnemies", new[] { "seeenemies", "seeportal", "wakeup", "entrance" } },
                { "Idle", new[] { "idle", "breathing", "tektek" } },
                { "Roar", new[] { "roar", "taunt", "mercy", "enrage", "phase", "whispers", "chanting", "screams" } },
                { "Interact", new[] { "checkpoint", "lever", "heal" } },
                { "Victory", new[] { "victory", "respawn" } },
                { "Voice", new[] { "tektek", "voice", "mumble" } }
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
