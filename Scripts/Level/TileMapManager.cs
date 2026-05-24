using Godot;

namespace Demonteks.Levels
{
    public partial class TileMapManager : Node2D
    {
        [Export] public string TileSetPath { get; set; } = "res://Assets/tilesets/jungle_tileset_16x16.tres";
        [Export] public int TileSize { get; set; } = 16;

        private TileMap _tileMap;

        public override void _Ready()
        {
            InitializeTileMap();
        }

        private void InitializeTileMap()
        {
            _tileMap = new TileMap();
            _tileMap.Name = "LevelTileMap";

            var tileSet = GD.Load<TileSet>(TileSetPath);
            if (tileSet == null)
            {
                GD.PrintErr($"Failed to load TileSet: {TileSetPath}");
                return;
            }

            _tileMap.TileSet = tileSet;
            AddChild(_tileMap);

            GD.Print($"TileMap initialized with {TileSetPath}");
        }

        public void PaintHorizontalPlatform(Vector2I startTile, int width)
        {
            if (_tileMap == null) return;

            for (int x = 0; x < width; x++)
            {
                Vector2I tileCoords = new Vector2I(startTile.X + x, startTile.Y);
                Vector2I atlasCoords;

                if (x == 0)
                    atlasCoords = new Vector2I(0, 0);
                else if (x == width - 1)
                    atlasCoords = new Vector2I(2, 0);
                else
                    atlasCoords = new Vector2I(1, 0);

                _tileMap.SetCell(0, tileCoords, 0, atlasCoords);
            }
        }

        public void PaintVerticalWall(Vector2I startTile, int height)
        {
            if (_tileMap == null) return;

            for (int y = 0; y < height; y++)
            {
                Vector2I tileCoords = new Vector2I(startTile.X, startTile.Y - y);
                Vector2I atlasCoords;

                if (y == 0)
                    atlasCoords = new Vector2I(0, 0);
                else if (y == height - 1)
                    atlasCoords = new Vector2I(0, 2);
                else
                    atlasCoords = new Vector2I(0, 1);

                _tileMap.SetCell(0, tileCoords, 0, atlasCoords);
            }
        }

        public void PaintGround(int startX, int endX, int y)
        {
            if (_tileMap == null) return;

            for (int x = startX; x <= endX; x++)
            {
                Vector2I tileCoords = new Vector2I(x, y);
                _tileMap.SetCell(0, tileCoords, 0, new Vector2I(1, 0));
            }

            Vector2I leftEdge = new Vector2I(startX, y);
            _tileMap.SetCell(0, leftEdge, 0, new Vector2I(0, 0));

            Vector2I rightEdge = new Vector2I(endX, y);
            _tileMap.SetCell(0, rightEdge, 0, new Vector2I(2, 0));
        }

        public void ClearArea(Vector2I topLeft, Vector2I bottomRight)
        {
            if (_tileMap == null) return;

            for (int x = topLeft.X; x <= bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    _tileMap.EraseCell(0, new Vector2I(x, y));
                }
            }
        }

        public void BuildExampleJungleLevel()
        {
            if (_tileMap == null) return;

            GD.Print("Building example jungle level...");

            PaintGround(0, 60, 30);

            PaintHorizontalPlatform(new Vector2I(10, 27), 8);
            PaintHorizontalPlatform(new Vector2I(22, 24), 10);
            PaintHorizontalPlatform(new Vector2I(38, 21), 8);
            PaintHorizontalPlatform(new Vector2I(52, 18), 12);

            GD.Print("Example jungle level built!");
        }

        public void BuildExampleDungeonLevel()
        {
            if (_tileMap == null) return;

            GD.Print("Building example dungeon level...");

            PaintGround(0, 80, 30);

            PaintHorizontalPlatform(new Vector2I(8, 26), 6);
            PaintHorizontalPlatform(new Vector2I(18, 23), 8);
            PaintHorizontalPlatform(new Vector2I(32, 20), 10);
            PaintHorizontalPlatform(new Vector2I(50, 22), 8);

            GD.Print("Example dungeon level built!");
        }

        public void BuildExampleCastleLevel()
        {
            if (_tileMap == null) return;

            GD.Print("Building example castle level...");

            PaintGround(0, 100, 30);

            PaintHorizontalPlatform(new Vector2I(12, 25), 10);
            PaintHorizontalPlatform(new Vector2I(30, 22), 8);
            PaintHorizontalPlatform(new Vector2I(48, 19), 12);
            PaintHorizontalPlatform(new Vector2I(70, 24), 10);

            GD.Print("Example castle level built!");
        }
    }
}
