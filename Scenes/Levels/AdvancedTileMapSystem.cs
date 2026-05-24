using Godot;
using System.Collections.Generic;

namespace Demonteks.Environment
{
	/// <summary>
	/// Advanced tilemap system with automatic tileset configuration.
	/// Handles multiple tilesets and provides level-building utilities.
	/// </summary>
	public partial class AdvancedTileMapSystem : Node2D
	{
		[Export] public string TilesetName { get; set; } = "jungle";
		[Export] public bool GenerateExampleLevel { get; set; } = false;
		[Export] public int TileSize { get; set; } = 16;

		private TileMap _tileMap;
		private TileSet _tileSet;
		private Dictionary<string, TileSetConfiguration> _tilesetConfigs;

		public override void _Ready()
		{
			InitializeTilesetConfigurations();
			CreateTileMapSystem();

			if (GenerateExampleLevel)
			{
				BuildExampleLevel();
			}
		}

		private void InitializeTilesetConfigurations()
		{
			_tilesetConfigs = new Dictionary<string, TileSetConfiguration>
			{
				["jungle"] = new TileSetConfiguration
				{
					TexturePath = "res://Assets/tilesets/jungle_tileset_16x16.png",
					TileSize = new Vector2I(16, 16),
					HasPhysics = true,
					HasNavigation = false
				},
				["dungeon"] = new TileSetConfiguration
				{
					TexturePath = "res://Assets/tilesets/dungeon_tileset_16x16.png",
					TileSize = new Vector2I(16, 16),
					HasPhysics = true,
					HasNavigation = false
				},
				["castle"] = new TileSetConfiguration
				{
					TexturePath = "res://Assets/tilesets/castle_tileset_16x16.png",
					TileSize = new Vector2I(16, 16),
					HasPhysics = true,
					HasNavigation = false
				}
			};
		}

		private void CreateTileMapSystem()
		{
			_tileMap = new TileMap();
			_tileMap.Name = "LevelTileMap";

			// Get tileset configuration
			if (!_tilesetConfigs.TryGetValue(TilesetName, out var config))
			{
				GD.PrintErr($"Unknown tileset: {TilesetName}");
				return;
			}

			// Create tileset
			_tileSet = CreateAdvancedTileSet(config);

			if (_tileSet != null)
			{
				_tileMap.TileSet = _tileSet;
				AddChild(_tileMap);
				GD.Print($"TileMap created with {TilesetName} tileset");
			}
		}

		private TileSet CreateAdvancedTileSet(TileSetConfiguration config)
		{
			var texture = GD.Load<Texture2D>(config.TexturePath);
			if (texture == null)
			{
				GD.PrintErr($"Failed to load tileset: {config.TexturePath}");
				return null;
			}

			var tileSet = new TileSet();

			// Create atlas source
			var atlasSource = new TileSetAtlasSource();
			atlasSource.Texture = texture;
			atlasSource.TextureRegionSize = config.TileSize;

			// Add to tileset
			int sourceId = 0;
			tileSet.AddSource(atlasSource, sourceId);

			// Configure physics if needed
			if (config.HasPhysics)
			{
				tileSet.AddPhysicsLayer();
				ConfigureTilePhysics(atlasSource, sourceId);
			}

			// Configure navigation if needed
			if (config.HasNavigation)
			{
				tileSet.AddNavigationLayer();
			}

			return tileSet;
		}

		private void ConfigureTilePhysics(TileSetAtlasSource atlasSource, int sourceId)
		{
			// Configure collision for common tile types
			// In production, you'd iterate through all tiles and configure each

			// Example: Grass top tile at (0,0)
			var tileData = atlasSource.GetTileData(new Vector2I(0, 0), 0);
			if (tileData != null)
			{
				// Full tile collision
				var polygon = new Vector2[]
				{
					new Vector2(0, 0),
					new Vector2(TileSize, 0),
					new Vector2(TileSize, TileSize),
					new Vector2(0, TileSize)
				};

				tileData.SetCollisionPolygonsCount(0, 1);
				tileData.SetCollisionPolygonPoints(0, 0, polygon);
			}
		}

		private void BuildExampleLevel()
		{
			// Ground platform
			PaintHorizontalPlatform(new Vector2I(0, 30), 60);

			// Floating platforms
			PaintHorizontalPlatform(new Vector2I(10, 27), 8);
			PaintHorizontalPlatform(new Vector2I(22, 24), 10);
			PaintHorizontalPlatform(new Vector2I(38, 21), 8);
			PaintHorizontalPlatform(new Vector2I(52, 18), 12);

			GD.Print("Example level generated");
		}

		public void PaintHorizontalPlatform(Vector2I startPos, int width)
		{
			for (int i = 0; i < width; i++)
			{
				Vector2I tilePos = new Vector2I(startPos.X + i, startPos.Y);

				// Use appropriate tile based on position
				Vector2I atlasCoords;
				if (i == 0)
					atlasCoords = new Vector2I(0, 0); // Left edge
				else if (i == width - 1)
					atlasCoords = new Vector2I(2, 0); // Right edge
				else
					atlasCoords = new Vector2I(1, 0); // Middle

				_tileMap.SetCell(0, tilePos, 0, atlasCoords);
			}
		}

		public void PaintVerticalWall(Vector2I startPos, int height)
		{
			for (int i = 0; i < height; i++)
			{
				Vector2I tilePos = new Vector2I(startPos.X, startPos.Y - i);
				_tileMap.SetCell(0, tilePos, 0, new Vector2I(1, 1)); // Wall tile
			}
		}

		public void ClearArea(Vector2I topLeft, Vector2I bottomRight)
		{
			for (int x = topLeft.X; x <= bottomRight.X; x++)
			{
				for (int y = topLeft.Y; y <= bottomRight.Y; y++)
				{
					_tileMap.EraseCell(0, new Vector2I(x, y));
				}
			}
		}
	}

	/// <summary>
	/// Configuration for tileset creation.
	/// </summary>
	public class TileSetConfiguration
	{
		public string TexturePath { get; set; }
		public Vector2I TileSize { get; set; }
		public bool HasPhysics { get; set; }
		public bool HasNavigation { get; set; }
	}
}
