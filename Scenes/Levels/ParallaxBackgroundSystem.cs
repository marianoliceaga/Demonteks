using Godot;
using System.Collections.Generic;

namespace Demonteks.Environment
{
    public partial class ParallaxBackgroundSystem : Node2D
    {
        [Export] public string CurrentLevel { get; set; } = "Jungle";
        [Export] public bool AutoSetup { get; set; } = true;
        
        private ParallaxBackground _parallaxBg;

        public override void _Ready()
        {
            SetupParallaxBackground(CurrentLevel);
        }

        public void SetupParallaxBackground(string levelName)
        {
            if (_parallaxBg != null)
            {
                _parallaxBg.QueueFree();
            }

            _parallaxBg = new ParallaxBackground();
            _parallaxBg.Name = "ParallaxBackground";
            AddChild(_parallaxBg);

            var config = GetLevelConfig(levelName);
            if (config == null)
            {
                GD.PrintErr($"No background configuration found for level: {levelName}");
                return;
            }

            foreach (var layerConfig in config)
            {
                var layer = CreateParallaxLayer(layerConfig);
                if (layer != null)
                {
                    _parallaxBg.AddChild(layer);
                }
            }

            GD.Print($"Parallax background setup complete for {levelName}");
        }

        private List<BackgroundLayerConfig> GetLevelConfig(string levelName)
        {
            return levelName switch
            {
                "Jungle" => new List<BackgroundLayerConfig>
                {
                    new BackgroundLayerConfig("sky", new Vector2(0.2f, 0.2f), new Vector2(1920, 0), new Color(0.61f, 0.15f, 0.69f, 1f), "res://Assets/backgrounds/jungle/sky_1920x1080.png"),
                    new BackgroundLayerConfig("mountains", new Vector2(0.5f, 0.5f), new Vector2(3840, 0), new Color(0.29f, 0.08f, 0.55f, 1f), "res://Assets/backgrounds/jungle/trees_3840x1080.png"),
                    new BackgroundLayerConfig("foliage", new Vector2(0.7f, 0.7f), new Vector2(3840, 0), new Color(0.18f, 0.49f, 0.20f, 1f), "res://Assets/backgrounds/jungle/foliage_3840x1080.png"),
                    new BackgroundLayerConfig("foreground", new Vector2(1.2f, 1.0f), new Vector2(5760, 0), new Color(0.10f, 0.39f, 0.12f, 1f), "res://Assets/backgrounds/jungle/foreground_5760x1080.png")
                },
                "Dungeon" => new List<BackgroundLayerConfig>
                {
                    new BackgroundLayerConfig("wall", new Vector2(0.3f, 0.3f), new Vector2(1920, 0), new Color(0.26f, 0.26f, 0.26f, 1f), "res://Assets/backgrounds/dungeon/wall_1920x1080.png"),
                    new BackgroundLayerConfig("pillars", new Vector2(0.6f, 0.6f), new Vector2(3840, 0), new Color(0.36f, 0.25f, 0.22f, 1f), "res://Assets/backgrounds/dungeon/pillars_3840x1080.png"),
                    new BackgroundLayerConfig("chains", new Vector2(1.3f, 1.0f), new Vector2(3840, 0), new Color(0.22f, 0.33f, 0.28f, 1f), "res://Assets/backgrounds/dungeon/chains_3840x1080.png")
                },
                "Castle" => new List<BackgroundLayerConfig>
                {
                    new BackgroundLayerConfig("walls", new Vector2(0.2f, 0.2f), new Vector2(1920, 0), new Color(0.42f, 0.11f, 0.11f, 1f), "res://Assets/backgrounds/castle/walls_1920x1080.png"),
                    new BackgroundLayerConfig("interior", new Vector2(0.5f, 0.5f), new Vector2(3840, 0), new Color(0.55f, 0.14f, 0.14f, 1f), "res://Assets/backgrounds/castle/interior_3840x1080.png"),
                    new BackgroundLayerConfig("pillars", new Vector2(1.1f, 1.0f), new Vector2(3840, 0), new Color(0.67f, 0.18f, 0.18f, 1f), "res://Assets/backgrounds/castle/pillars_3840x1080.png")
                },
                _ => null
            };
        }

        private ParallaxLayer CreateParallaxLayer(BackgroundLayerConfig config)
        {
            var layer = new ParallaxLayer();
            layer.Name = $"{config.Name}Layer";
            layer.MotionScale = config.MotionScale;
            layer.MotionMirroring = config.MotionMirroring;

            var texture = GD.Load<Texture2D>(config.TexturePath);
            
            if (texture != null)
            {
                var sprite = new Sprite2D();
                sprite.Name = "Sprite";
                sprite.Texture = texture;
                sprite.Centered = false;
                sprite.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
                layer.AddChild(sprite);
            }
            else
            {
                var colorRect = new ColorRect();
                colorRect.Name = "Background";
                colorRect.Size = new Vector2(1920, 1080);
                colorRect.Color = config.BackgroundColor;
                layer.AddChild(colorRect);
            }

            return layer;
        }

        public void SwitchLevel(string newLevel)
        {
            var config = GetLevelConfig(newLevel);
            if (config != null)
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

    public class BackgroundLayerConfig
    {
        public string Name { get; set; }
        public Vector2 MotionScale { get; set; }
        public Vector2 MotionMirroring { get; set; }
        public Color BackgroundColor { get; set; }
        public string TexturePath { get; set; }

        public BackgroundLayerConfig(string name, Vector2 motionScale, Vector2 motionMirroring, Color backgroundColor, string texturePath)
        {
            Name = name;
            MotionScale = motionScale;
            MotionMirroring = motionMirroring;
            BackgroundColor = backgroundColor;
            TexturePath = texturePath;
        }
    }
}
