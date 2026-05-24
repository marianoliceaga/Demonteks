using Godot;
using System;
using System.Collections.Generic;

public partial class GenerateLevel2 : Node
{
    private Node2D _scene;
    private TileMap _tileMap;
    private Node2D _enemiesNode;
    private Node2D _pickupsNode;
    private Node2D _checkpointsNode;
    private Area2D _levelEnd;

    private PackedScene _playerScene = GD.Load<PackedScene>("res://Scenes/Characters/Player.tscn");
    private PackedScene _guardScene = GD.Load<PackedScene>("res://Scenes/Characters/Guard.tscn");
    private PackedScene _demontekScene = GD.Load<PackedScene>("res://Scenes/Characters/Demontek.tscn");
    private PackedScene _mutantScene = GD.Load<PackedScene>("res://Scenes/Characters/Mutant.tscn");
    private PackedScene _checkpointScene = GD.Load<PackedScene>("res://Scenes/Prefabs/Checkpoint.tscn");
    private PackedScene _healthPickupScene = GD.Load<PackedScene>("res://Scenes/Items/HealthPickup.tscn");
    private PackedScene _weaponPickupScene = GD.Load<PackedScene>("res://Scenes/Prefabs/WeaponPickup.tscn");
    private PackedScene _floorTrapScene = GD.Load<PackedScene>("res://Scenes/Prefabs/FloorTrap.tscn");

    public override void _Ready()
    {
        GD.Print("Starting Level 2 Generation...");

        var packed = GD.Load<PackedScene>("res://Scenes/Levels/Level2_Dungeon.tscn");
        _scene = packed.Instantiate() as Node2D;

        _tileMap = _scene.GetNode<TileMap>("TileMap");
        _enemiesNode = _scene.GetNode<Node2D>("Enemies");
        _pickupsNode = _scene.GetNode<Node2D>("Pickups");
        _checkpointsNode = _scene.GetNode<Node2D>("Checkpoints");
        _levelEnd = _scene.GetNode<Area2D>("LevelEnd");

        ClearLevel();
        BuildLevel();

        var newPacked = new PackedScene();
        var err = newPacked.Pack(_scene);
        if (err == Error.Ok)
        {
            ResourceSaver.Save(newPacked, "res://Scenes/Levels/Level2_Dungeon.tscn");
            GD.Print("Level 2 Generation Complete & Saved!");
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
    }

    private void BuildLevel()
    {
        int currentX = 0;
        int floorY = 24; // Y coordinate in tiles

        // Start Room
        CreatePlatform(currentX, floorY, 30);
        AddCheckpoint(currentX + 5, floorY - 1);

        var player = _scene.GetNode<Node2D>("Player");
        player.Position = new Vector2((currentX + 5) * 16, (floorY - 2) * 16);
        currentX += 30;

        // Gap
        currentX += 5;

        // Zone 1: The Ruins
        CreatePlatform(currentX, floorY, 40);
        AddEnemy(_demontekScene, currentX + 15, floorY - 1);
        AddEnemy(_guardScene, currentX + 30, floorY - 1);
        currentX += 40;

        // Platforming section
        for (int i = 0; i < 5; i++)
        {
            currentX += 4;
            CreatePlatform(currentX, floorY - i * 2, 6);
            if (i == 2) AddPickup(_healthPickupScene, currentX + 3, floorY - i * 2 - 1);
            currentX += 6;
        }

        currentX += 4;
        floorY -= 8;
        CreatePlatform(currentX, floorY, 50);
        AddCheckpoint(currentX + 5, floorY - 1);
        AddEnemy(_demontekScene, currentX + 20, floorY - 1);
        AddEnemy(_guardScene, currentX + 40, floorY - 1);
        currentX += 50;

        // Drop down into Trapped Hallways
        currentX += 4;
        floorY += 12;
        CreatePlatform(currentX, floorY, 80);

        // Add Floor Traps
        AddFloorTrap(currentX + 20, floorY);
        AddFloorTrap(currentX + 40, floorY);
        AddFloorTrap(currentX + 60, floorY);

        AddEnemy(_demontekScene, currentX + 30, floorY - 1);
        AddEnemy(_guardScene, currentX + 50, floorY - 1);
        AddEnemy(_mutantScene, currentX + 70, floorY - 3); // Mutants are taller

        AddWeaponPickup(currentX + 75, floorY - 1, 2); // Machete
        currentX += 80;

        // Zone 3: Mutant Pits
        currentX += 6;
        CreatePlatform(currentX, floorY, 20);
        AddCheckpoint(currentX + 5, floorY - 1);
        currentX += 20;

        currentX += 8;
        CreatePlatform(currentX, floorY, 30);
        AddEnemy(_mutantScene, currentX + 15, floorY - 3);
        currentX += 30;

        currentX += 8;
        CreatePlatform(currentX, floorY, 30);
        AddEnemy(_mutantScene, currentX + 15, floorY - 3);
        currentX += 30;

        // Zone 4: The Ascend
        AddPickup(_healthPickupScene, currentX - 5, floorY - 1);

        for (int i = 0; i < 8; i++)
        {
            currentX += 6;
            floorY -= 3;
            CreatePlatform(currentX, floorY, 8);
            if (i % 2 == 1) AddEnemy(_guardScene, currentX + 4, floorY - 1);
            currentX += 8;
        }

        currentX += 6;
        CreatePlatform(currentX, floorY, 60);
        AddCheckpoint(currentX + 5, floorY - 1);
        AddEnemy(_mutantScene, currentX + 20, floorY - 3);
        AddEnemy(_mutantScene, currentX + 40, floorY - 3);

        AddWeaponPickup(currentX + 45, floorY - 1, 3); // Sword

        // Level End
        _levelEnd.Position = new Vector2((currentX + 50) * 16, (floorY - 2) * 16);
        currentX += 60;

        // Update camera limit
        var cam = _scene.GetNode<Camera2D>("Camera");
        cam.Set("LimitRightValue", currentX * 16);
        cam.Set("LimitRight", currentX * 16);

        var killZone = _scene.GetNode<Area2D>("KillZone");
        killZone.Position = new Vector2((currentX * 16) / 2, (floorY + 20) * 16);
        if (killZone.GetNode<CollisionShape2D>("CollisionShape2D").Shape is RectangleShape2D rect)
        {
            rect.Size = new Vector2(currentX * 16, 64);
        }
    }

    private void CreatePlatform(int startX, int y, int length)
    {
        for (int i = 0; i < length; i++)
        {
            // Top grass (1,0)
            _tileMap.SetCell(0, new Vector2I(startX + i, y), 0, new Vector2I(1, 0));
            // Deep dirt (1,1)
            for (int j = 1; j < 6; j++)
            {
                _tileMap.SetCell(0, new Vector2I(startX + i, y + j), 0, new Vector2I(1, 1));
            }
        }
    }

    private void AddEnemy(PackedScene scene, int x, int y)
    {
        var enemy = scene.Instantiate() as Node2D;
        enemy.Position = new Vector2(x * 16, y * 16);
        _enemiesNode.AddChild(enemy);
        enemy.Owner = _scene;
    }

    private void AddPickup(PackedScene scene, int x, int y)
    {
        var pickup = scene.Instantiate() as Node2D;
        pickup.Position = new Vector2(x * 16, y * 16);
        _pickupsNode.AddChild(pickup);
        pickup.Owner = _scene;
    }

    private void AddWeaponPickup(int x, int y, int weaponTypeInt)
    {
        var pickup = _weaponPickupScene.Instantiate() as Node2D;
        pickup.Position = new Vector2(x * 16, y * 16);
        pickup.Set("WeaponType", weaponTypeInt);
        _pickupsNode.AddChild(pickup);
        pickup.Owner = _scene;
    }

    private void AddFloorTrap(int x, int y)
    {
        var trap = _floorTrapScene.Instantiate() as Node2D;
        trap.Position = new Vector2(x * 16, y * 16);
        _scene.AddChild(trap);
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
