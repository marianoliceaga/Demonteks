using Godot;
using System;
using System.Collections.Generic;

public partial class GenerateLevel3 : Node
{
    private Node2D _scene;
    private TileMap _tileMap;
    private Node2D _enemiesNode;
    private Node2D _pickupsNode;
    private Node2D _checkpointsNode;
    private Node2D _platformsNode;
    private Node2D _trapsNode;
    private Area2D _goliathArena;
    private Area2D _kingArena;

    private PackedScene _playerScene = GD.Load<PackedScene>("res://Scenes/Characters/Player.tscn");
    private PackedScene _guardScene = GD.Load<PackedScene>("res://Scenes/Characters/Guard.tscn");
    private PackedScene _mutantScene = GD.Load<PackedScene>("res://Scenes/Characters/Mutant.tscn");
    private PackedScene _goliathScene = GD.Load<PackedScene>("res://Scenes/Characters/Goliath.tscn");
    private PackedScene _kingScene = GD.Load<PackedScene>("res://Scenes/Characters/King.tscn");
    private PackedScene _checkpointScene = GD.Load<PackedScene>("res://Scenes/Prefabs/Checkpoint.tscn");
    private PackedScene _healthPickupScene = GD.Load<PackedScene>("res://Scenes/Items/HealthPickup.tscn");
    private PackedScene _weaponPickupScene = GD.Load<PackedScene>("res://Scenes/Prefabs/WeaponPickup.tscn");
    private PackedScene _floorTrapScene = GD.Load<PackedScene>("res://Scenes/Prefabs/FloorTrap.tscn");

    public override void _Ready()
    {
        GD.Print("Starting Level 3 Generation...");

        var packed = GD.Load<PackedScene>("res://Scenes/Levels/Level3_Castle.tscn");
        _scene = packed.Instantiate() as Node2D;

        _tileMap = _scene.GetNode<TileMap>("TileMap");
        _enemiesNode = _scene.GetNode<Node2D>("Enemies");
        _pickupsNode = _scene.GetNode<Node2D>("Pickups");
        _checkpointsNode = _scene.GetNode<Node2D>("Checkpoints");
        _platformsNode = _scene.GetNodeOrNull<Node2D>("Platforms");
        _trapsNode = _scene.GetNodeOrNull<Node2D>("Traps");
        _goliathArena = _scene.GetNode<Area2D>("GoliathArena");
        _kingArena = _scene.GetNode<Area2D>("KingArena");

        ClearLevel();
        BuildLevel();

        var newPacked = new PackedScene();
        var err = newPacked.Pack(_scene);
        if (err == Error.Ok)
        {
            ResourceSaver.Save(newPacked, "res://Scenes/Levels/Level3_Castle.tscn");
            GD.Print("Level 3 Generation Complete & Saved!");
        }
        else
        {
            GD.PrintErr("Failed to pack scene: " + err);
        }

        GetTree().Quit();
    }

    private void ClearLevel()
    {
        _tileMap.Clear();

        foreach (Node child in _enemiesNode.GetChildren()) { child.QueueFree(); _enemiesNode.RemoveChild(child); }
        foreach (Node child in _pickupsNode.GetChildren()) { child.QueueFree(); _pickupsNode.RemoveChild(child); }
        foreach (Node child in _checkpointsNode.GetChildren()) { child.QueueFree(); _checkpointsNode.RemoveChild(child); }

        if (_platformsNode != null)
        {
            foreach (Node child in _platformsNode.GetChildren()) { child.QueueFree(); _platformsNode.RemoveChild(child); }
        }
        if (_trapsNode != null)
        {
            foreach (Node child in _trapsNode.GetChildren()) { child.QueueFree(); _trapsNode.RemoveChild(child); }
        }
    }

    private void BuildLevel()
    {
        int currentX = 0;
        int floorY = 24; // Y coordinate in tiles

        // Start Room
        CreatePlatform(currentX, floorY, 20);
        AddCheckpoint(currentX + 5, floorY - 1);

        var player = _scene.GetNode<Node2D>("Player");
        player.Position = new Vector2((currentX + 5) * 16, (floorY - 2) * 16);
        currentX += 20;

        // Gap
        currentX += 4;

        // Zone 1: Irregular Terrain
        CreatePlatform(currentX, floorY, 15);
        AddEnemy(_guardScene, currentX + 5, floorY - 1);
        AddEnemy(_mutantScene, currentX + 12, floorY - 3);
        currentX += 15;

        currentX += 3;
        floorY -= 4;
        CreatePlatform(currentX, floorY, 10);
        AddEnemy(_guardScene, currentX + 5, floorY - 1);
        currentX += 10;

        currentX += 3;
        floorY += 6;
        CreatePlatform(currentX, floorY, 20);
        AddEnemy(_guardScene, currentX + 5, floorY - 1);
        AddWeaponPickup(currentX + 10, floorY - 1, 3); // Demontek Saber
        AddEnemy(_mutantScene, currentX + 18, floorY - 3);
        currentX += 20;

        // Gap and traps
        currentX += 6;
        CreatePlatform(currentX, floorY, 25);
        AddCheckpoint(currentX + 2, floorY - 1);
        AddFloorTrap(currentX + 8, floorY);
        AddFloorTrap(currentX + 14, floorY);
        AddEnemy(_mutantScene, currentX + 20, floorY - 3);
        currentX += 25;

        // Zone 2: Goliath Arena
        currentX += 4;
        CreatePlatform(currentX, floorY, 40);
        AddPickup(_healthPickupScene, currentX + 2, floorY - 1);

        var goliath = AddEnemy(_goliathScene, currentX + 20, floorY - 6); // Goliath is tall
        goliath.Name = "Goliath";

        _goliathArena.Position = new Vector2((currentX + 20) * 16, (floorY - 4) * 16);

        currentX += 40;

        // Zone 3: Post-Goliath challenges
        currentX += 5;
        floorY += 2;
        CreatePlatform(currentX, floorY, 30);
        AddCheckpoint(currentX + 5, floorY - 1);
        AddFloorTrap(currentX + 12, floorY);
        AddEnemy(_guardScene, currentX + 18, floorY - 1);
        AddWeaponPickup(currentX + 25, floorY - 1, 4); // Sawtooth
        currentX += 30;

        // Ascending Spiral-like stairs to the King
        // We make a series of small platforms going up and back, or just up right zigzag
        currentX += 5;
        AddPickup(_healthPickupScene, currentX - 2, floorY - 1);

        for (int i = 0; i < 15; i++)
        {
            CreatePlatform(currentX, floorY, 6);
            if (i % 3 == 1)
            {
                AddEnemy(_guardScene, currentX + 3, floorY - 1);
            }
            if (i % 4 == 2)
            {
                // Add a health pickup on the stairs occasionally
                AddPickup(_healthPickupScene, currentX + 4, floorY - 1);
            }

            // Go up and right
            currentX += 8;
            floorY -= 4;

            // Go slightly left occasionally to make it irregular/spiral
            if (i % 2 == 1)
            {
                CreatePlatform(currentX - 2, floorY, 4);
            }
        }

        // Zone 4: The King Final Boss Arena
        currentX += 2;
        CreatePlatform(currentX, floorY, 50);
        AddCheckpoint(currentX + 2, floorY - 1);
        AddWeaponPickup(currentX + 6, floorY - 1, 5); // Alien Sword
        AddPickup(_healthPickupScene, currentX + 10, floorY - 1);

        var king = AddEnemy(_kingScene, currentX + 25, floorY - 8);
        king.Name = "King";

        _kingArena.Position = new Vector2((currentX + 25) * 16, (floorY - 4) * 16);

        // Update camera limit to encompass the whole level
        currentX += 50;
        var cam = _scene.GetNode<Camera2D>("Camera");
        cam.Set("LimitRightValue", currentX * 16 + 800);
        cam.Set("LimitRight", currentX * 16 + 800);
        cam.Set("LimitTopValue", -2000);
        cam.Set("LimitTop", -2000);

        var killZone = _scene.GetNode<Area2D>("KillZone");
        killZone.Position = new Vector2((currentX * 16) / 2, 800);
        if (killZone.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D rect)
        {
            rect.Size = new Vector2(currentX * 16 + 2000, 64);
        }
    }

    private void CreatePlatform(int startX, int y, int length)
    {
        for (int i = 0; i < length; i++)
        {
            // Top (1,0)
            _tileMap.SetCell(0, new Vector2I(startX + i, y), 0, new Vector2I(1, 0));
            // Deep (1,1)
            for (int j = 1; j < 15; j++)
            {
                _tileMap.SetCell(0, new Vector2I(startX + i, y + j), 0, new Vector2I(1, 1));
            }
        }
    }

    private Node2D AddEnemy(PackedScene scene, int x, int y)
    {
        var enemy = scene.Instantiate() as Node2D;
        enemy.Position = new Vector2(x * 16, y * 16);
        _enemiesNode.AddChild(enemy);
        enemy.Owner = _scene;
        return enemy; // to set names for bosses
    }

    private void AddPickup(PackedScene scene, int x, int y)
    {
        var pickup = scene.Instantiate() as Node2D;
        pickup.Position = new Vector2(x * 16, y * 16);
        if (_pickupsNode == null) return;
        _pickupsNode.AddChild(pickup);
        pickup.Owner = _scene;
    }

    private void AddWeaponPickup(int x, int y, int weaponTypeInt)
    {
        var pickup = _weaponPickupScene.Instantiate() as Node2D;
        pickup.Position = new Vector2(x * 16, y * 16);
        pickup.Set("WeaponType", weaponTypeInt);
        pickup.Set("WeaponToPickup", weaponTypeInt);
        _pickupsNode.AddChild(pickup);
        pickup.Owner = _scene;
    }

    private void AddFloorTrap(int x, int y)
    {
        if (_trapsNode == null) return;
        var trap = _floorTrapScene.Instantiate() as Node2D;
        trap.Position = new Vector2(x * 16, y * 16);
        _trapsNode.AddChild(trap);
        trap.Owner = _scene;
        // Also carve out the platform tiles for the trap
        for (int i = 0; i < 4; i++)
        {
            _tileMap.SetCell(0, new Vector2I(x + i, y), -1, new Vector2I(-1, -1));
        }
    }

    private void AddCheckpoint(int x, int y)
    {
        var cp = _checkpointScene.Instantiate() as Node2D;
        cp.Position = new Vector2(x * 16, y * 16);
        _checkpointsNode.AddChild(cp);
        cp.Owner = _scene;
    }
}
