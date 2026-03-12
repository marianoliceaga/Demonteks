using Godot;
using System.Collections.Generic;

namespace Demonteks.Management
{
    /// <summary>
    /// Audio manager for playing SFX and music.
    /// Autoload singleton — access via /root/AudioManager.
    /// Audio files can be placed in res://Assets/audio/sfx/ and res://Assets/audio/music/.
    /// </summary>
    public partial class AudioManager : Node
    {
        // SFX bus
        private AudioStreamPlayer _sfxPlayer;
        // Music bus
        private AudioStreamPlayer _musicPlayer;

        // Cache for loaded audio
        private Dictionary<string, AudioStream> _audioCache = new();

        [Export] public float SfxVolume { get; set; } = 0.8f;
        [Export] public float MusicVolume { get; set; } = 0.5f;

        public override void _Ready()
        {
            // Create SFX player
            _sfxPlayer = new AudioStreamPlayer();
            _sfxPlayer.Bus = "SFX";
            AddChild(_sfxPlayer);

            // Create Music player
            _musicPlayer = new AudioStreamPlayer();
            _musicPlayer.Bus = "Music";
            AddChild(_musicPlayer);

            ProcessMode = ProcessModeEnum.Always;
            GD.Print("AudioManager initialized (no audio files loaded yet)");
        }

        /// <summary>
        /// Play a sound effect by resource path.
        /// Example: PlaySFX("res://Assets/audio/sfx/hit.wav")
        /// </summary>
        public void PlaySFX(string path)
        {
            var stream = LoadAudio(path);
            if (stream != null)
            {
                _sfxPlayer.Stream = stream;
                _sfxPlayer.VolumeDb = Mathf.LinearToDb(SfxVolume);
                _sfxPlayer.Play();
            }
        }

        /// <summary>
        /// Play a named SFX shortcut. Falls back gracefully if file missing.
        /// </summary>
        public void PlaySFXNamed(string name)
        {
            string path = $"res://Assets/audio/sfx/{name}.wav";
            if (ResourceLoader.Exists(path))
            {
                PlaySFX(path);
            }
            else
            {
                // Try .ogg
                path = $"res://Assets/audio/sfx/{name}.ogg";
                if (ResourceLoader.Exists(path))
                    PlaySFX(path);
                // Silently ignore if no file — allows development without audio
            }
        }

        /// <summary>
        /// Play background music. Loops automatically.
        /// Example: PlayMusic("res://Assets/audio/music/jungle_theme.ogg")
        /// </summary>
        public void PlayMusic(string path)
        {
            var stream = LoadAudio(path);
            if (stream != null)
            {
                // If it's the exact same stream and it's already playing, do nothing to allow continuous playback
                if (_musicPlayer.Stream == stream && _musicPlayer.Playing)
                {
                    return;
                }
                
                _musicPlayer.Stream = stream;
                _musicPlayer.VolumeDb = Mathf.LinearToDb(MusicVolume);
                _musicPlayer.Play();
            }
        }

        /// <summary>
        /// Stop currently playing music.
        /// </summary>
        public void StopMusic()
        {
            _musicPlayer.Stop();
        }

        /// <summary>
        /// Load audio with caching.
        /// </summary>
        private AudioStream LoadAudio(string path)
        {
            if (_audioCache.TryGetValue(path, out var cached))
                return cached;

            if (!ResourceLoader.Exists(path))
            {
                GD.PrintErr($"AudioManager: Audio file not found: {path}");
                return null;
            }

            var stream = GD.Load<AudioStream>(path);
            if (stream != null)
                _audioCache[path] = stream;

            return stream;
        }
    }
}
