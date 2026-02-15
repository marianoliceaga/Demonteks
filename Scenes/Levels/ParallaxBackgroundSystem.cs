using Godot;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    /// <summary>
    /// Complete parallax background management system for all levels.
    /// Supports dynamic level switching and layer configuration.
    /// </summary>
    public partial class ParallaxBackgroundSystem : Node2D
    {
        [Export] public string CurrentLevel { get; set; } = "Jungle";
        [Export] public bool AutoSetup { get; set; } = true;
        
        private ParallaxBackground _parallaxBg;
        private Dictionary<string, LevelBackgroundConfig> _levelConfigs;

        public override void _Ready()
        {
            InitializeLevelConfigs();
            
            if (AutoSetup)
            {
                SetupParallaxBackground(CurrentLevel);
            }
        }

        private void InitializeLevelConfigs()
        {
            _levelConfigs = new Dictionary<string, LevelBackgroundConfig>
            {
                ["Jungle"] = new LevelBackgroundConfig
                {
                    Layers = new List<BackgroundLayer>
                    {
                        new BackgroundLayer("sky", "res://Assets/backgrounds/jungle/Jungle_Layer1.png", 
                            new Vector2(0.2f, 0.2f), new Vector2(1920, 0)),
                        new BackgroundLayer("trees", "res://Assets/backgrounds/jungle/Jungle_Layer2.png", 
                            new Vector2(0.5f, 0.5f), new Vector2(3840, 0)),
                        new BackgroundLayer("foliage", "res://Assets/backgrounds/jungle/Jungle_Layer3.png", 
                            new Vector2(0.7f, 0.7f), new Vector2(5760, 0)),
                        //new BackgroundLayer("foreground", "res://Assets/backgrounds/jungle/foreground_5760.png", 
                        //    new Vector2(1.2f, 1.0f), new Vector2(5760, 0))
                    }
                },
                
                ["Dungeon"] = new LevelBackgroundConfig
                {
                    Layers = new List<BackgroundLayer>
                    {
                        new BackgroundLayer("wall", "res://Assets/backgrounds/dungeon/Dungeon_Layer1.png", 
                            new Vector2(0.3f, 0.3f), new Vector2(1920, 0)),
                        new BackgroundLayer("pillars", "res://Assets/backgrounds/dungeon/pillars_3840.png", 
                            new Vector2(0.6f, 0.6f), new Vector2(3840, 0)),
                        new BackgroundLayer("chains", "res://Assets/backgrounds/dungeon/chains_3840.png", 
                            new Vector2(1.3f, 1.0f), new Vector2(3840, 0))
                    }
                },
                
                ["Castle"] = new LevelBackgroundConfig
                {
                    Layers = new List<BackgroundLayer>
                    {
                        new BackgroundLayer("walls", "res://Assets/backgrounds/castle/walls_1920.png", 
                            new Vector2(0.2f, 0.2f), new Vector2(1920, 0)),
                        new BackgroundLayer("interior", "res://Assets/backgrounds/castle/interior_3840.png", 
                            new Vector2(0.5f, 0.5f), new Vector2(3840, 0)),
                        new BackgroundLayer("pillars", "res://Assets/backgrounds/castle/pillars_3840.png", 
                            new Vector2(1.1f, 1.0f), new Vector2(3840, 0))
                    }
                }
            };
        }

        public void SetupParallaxBackground(string levelName)
        {
            // Clear existing background if present
            if (_parallaxBg != null)
            {
                _parallaxBg.QueueFree();
            }

            // Create new parallax background
            _parallaxBg = new ParallaxBackground();
            _parallaxBg.Name = "ParallaxBackground";
            AddChild(_parallaxBg);

            // Get level configuration
            if (!_levelConfigs.TryGetValue(levelName, out var config))
            {
                GD.PrintErr($"No background configuration found for level: {levelName}");
                return;
            }

            // Create all layers
            foreach (var layerConfig in config.Layers)
            {
                var layer = CreateParallaxLayer(layerConfig);
                if (layer != null)
                {
                    _parallaxBg.AddChild(layer);
                }
            }

            GD.Print($"Parallax background setup complete for {levelName}");
        }

        private ParallaxLayer CreateParallaxLayer(BackgroundLayer config)
        {
            var layer = new ParallaxLayer();
            layer.Name = $"{config.Name}Layer";
            layer.MotionScale = config.MotionScale;
            layer.MotionMirroring = config.MotionMirroring;

            // Load texture
            var texture = GD.Load<Texture2D>(config.TexturePath);
            if (texture == null)
            {
                GD.PrintErr($"Failed to load background texture: {config.TexturePath}");
                return null;
            }

            // Create sprite
            var sprite = new Sprite2D();
            sprite.Name = "Sprite";
            sprite.Texture = texture;
            sprite.Centered = false;
            
            // CRITICAL: Set texture filter to Nearest for pixel art
            sprite.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;

            layer.AddChild(sprite);

            return layer;
        }

        public void SwitchLevel(string newLevel)
        {
            if (_levelConfigs.ContainsKey(newLevel))
            {
                CurrentLevel = newLevel;
                SetupParallaxBackground(newLevel);
            }
            else
            {
                GD.PrintErr($"Unknown level: {newLevel}");
            }
        }
    }

    /// <summary>
    /// Configuration for a single background layer.
    /// </summary>
    public class BackgroundLayer
    {
        public string Name { get; set; }
        public string TexturePath { get; set; }
        public Vector2 MotionScale { get; set; }
        public Vector2 MotionMirroring { get; set; }

        public BackgroundLayer(string name, string texturePath, Vector2 motionScale, Vector2 motionMirroring)
        {
            Name = name;
            TexturePath = texturePath;
            MotionScale = motionScale;
            MotionMirroring = motionMirroring;
        }
    }

    /// <summary>
    /// Configuration for all layers in a level.
    /// </summary>
    public class LevelBackgroundConfig
    {
        public List<BackgroundLayer> Layers { get; set; } = new List<BackgroundLayer>();
    }
}