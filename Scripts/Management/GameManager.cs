using Godot;
using System;
using System.Collections.Generic;

namespace Demonteks.Management
{
	/// <summary>
	/// Central game manager handling level progression, checkpoints, and game state.
	/// Singleton pattern for global access.
	/// </summary>
	public partial class GameManager : Node
	{
		#region Singleton
		private static GameManager _instance;
		public static GameManager Instance => _instance;
		#endregion

		#region Export Variables
		[ExportCategory("Level Configuration")]
		[Export] public string Level1Scene { get; set; } = "res://Scenes/Levels/Level1_Jungle.tscn";
		[Export] public string Level2Scene { get; set; } = "res://Scenes/Levels/Level2_Dungeon.tscn";
		[Export] public string Level3Scene { get; set; } = "res://Scenes/Levels/Level3_Castle.tscn";
		[Export] public string MainMenuScene { get; set; } = "res://Scenes/UI/MainMenu.tscn";
		
		[ExportCategory("Player Spawn")]
		[Export] public NodePath PlayerSpawnPath { get; set; }
		#endregion

		#region Private Variables
		private Node2D _currentLevel;
		private Node2D _player;
		private SaveManager _saveManager;
		
		// Game State
		private GameState _gameState = GameState.MainMenu;
		private int _currentLevelIndex = 0;
		private Vector2 _currentCheckpoint = Vector2.Zero;
		private int _playerDeaths = 0;
		private float _totalPlayTime = 0f;
		
		// Level Progress
		private Dictionary<int, bool> _levelsCompleted = new Dictionary<int, bool>
		{
			{ 1, false },
			{ 2, false },
			{ 3, false }
		};
		
		// Statistics
		private int _enemiesKilled = 0;
		private int _totalDamageTaken = 0;
		private bool _mercyRouteActive = true; // Set to false if player kills
		#endregion

		#region Signals
		[Signal] public delegate void GameStateChangedEventHandler(string newState);
		[Signal] public delegate void LevelCompletedEventHandler(int levelIndex);
		[Signal] public delegate void CheckpointReachedEventHandler(Vector2 position);
		[Signal] public delegate void PlayerDiedEventHandler(int deathCount);
		[Signal] public delegate void GameOverEventHandler();
		[Signal] public delegate void GameWonEventHandler(bool mercyRoute);
		#endregion

		#region Godot Lifecycle
		public override void _Ready()
		{
			// Setup singleton
			if (_instance != null && _instance != this)
			{
				QueueFree();
				return;
			}
			_instance = this;
			
			// Get SaveManager
			_saveManager = GetNode<SaveManager>("/root/SaveManager");
			
			// Process mode to persist through scene changes
			ProcessMode = ProcessModeEnum.Always;
			
			GD.Print("GameManager initialized");
		}

		public override void _Process(double delta)
		{
			if (_gameState == GameState.Playing)
			{
				_totalPlayTime += (float)delta;
			}
		}
		#endregion

		#region Level Management
		/// <summary>
		/// Start a new game from the beginning
		/// </summary>
		public void StartNewGame()
		{
			ResetGameState();
			LoadLevel(1);
			ChangeGameState(GameState.Playing);
		}

		/// <summary>
		/// Continue from saved game
		/// </summary>
		public void ContinueGame()
		{
			if (_saveManager.LoadGame())
			{
				LoadLevel(_currentLevelIndex);
				ChangeGameState(GameState.Playing);
				GD.Print("Game loaded successfully");
			}
			else
			{
				GD.Print("No save file found, starting new game");
				StartNewGame();
			}
		}

		/// <summary>
		/// Load a specific level
		/// </summary>
		public void LoadLevel(int levelIndex)
		{
			_currentLevelIndex = levelIndex;
			
			string scenePath = levelIndex switch
			{
				1 => Level1Scene,
				2 => Level2Scene,
				3 => Level3Scene,
				_ => Level1Scene
			};
			
			GetTree().ChangeSceneToFile(scenePath);
			
			GD.Print($"Loading Level {levelIndex}: {scenePath}");
		}

		/// <summary>
		/// Complete current level and advance
		/// </summary>
		public void CompleteLevel()
		{
			_levelsCompleted[_currentLevelIndex] = true;
			EmitSignal(SignalName.LevelCompleted, _currentLevelIndex);
			
			_saveManager.SaveGame();
			
			if (_currentLevelIndex < 3)
			{
				LoadLevel(_currentLevelIndex + 1);
			}
			else
			{
				// Game completed!
				WinGame();
			}
		}

		/// <summary>
		/// Restart current level
		/// </summary>
		public void RestartLevel()
		{
			LoadLevel(_currentLevelIndex);
		}

		/// <summary>
		/// Return to main menu
		/// </summary>
		public void ReturnToMainMenu()
		{
			ChangeGameState(GameState.MainMenu);
			GetTree().ChangeSceneToFile(MainMenuScene);
		}
		#endregion

		#region Checkpoint System
		/// <summary>
		/// Set a checkpoint position
		/// </summary>
		public void SetCheckpoint(Vector2 position)
		{
			_currentCheckpoint = position;
			EmitSignal(SignalName.CheckpointReached, position);
			_saveManager.SaveGame();
			
			GD.Print($"Checkpoint set at {position}");
		}

		/// <summary>
		/// Respawn player at last checkpoint
		/// </summary>
		public void RespawnPlayer()
		{
			if (_player != null && _currentCheckpoint != Vector2.Zero)
			{
				_player.GlobalPosition = _currentCheckpoint;
				
				// Reset player stats
				if (_player.HasMethod("Revive"))
				{
					_player.Call("Revive");
				}
			}
		}
		#endregion

		#region Game State
		/// <summary>
		/// Change game state
		/// </summary>
		private void ChangeGameState(GameState newState)
		{
			_gameState = newState;
			EmitSignal(SignalName.GameStateChanged, newState.ToString());
			
			GD.Print($"Game state changed to: {newState}");
		}

		/// <summary>
		/// Handle player death
		/// </summary>
		public void OnPlayerDied()
		{
			_playerDeaths++;
			EmitSignal(SignalName.PlayerDied, _playerDeaths);
			
			GD.Print($"Player died. Total deaths: {_playerDeaths}");
			
			// Respawn after delay
			GetTree().CreateTimer(2.0f).Timeout += RespawnPlayer;
		}

		/// <summary>
		/// Handle game over
		/// </summary>
		//TODO Verify this
		///private void GameOver()
		//{
		//	ChangeGameState(GameState.GameOver);
		//	EmitSignal(SignalName.GameOver);
			
		//	GD.Print("Game Over!");
		//}

		/// <summary>
		/// Handle game win
		/// </summary>
		private void WinGame()
		{
			ChangeGameState(GameState.Won);
			EmitSignal(SignalName.GameWon, _mercyRouteActive);
			
			GD.Print($"Game Won! Mercy Route: {_mercyRouteActive}");
		}

		/// <summary>
		/// Reset all game state variables
		/// </summary>
		private void ResetGameState()
		{
			_currentLevelIndex = 1;
			_currentCheckpoint = Vector2.Zero;
			_playerDeaths = 0;
			_totalPlayTime = 0f;
			_enemiesKilled = 0;
			_totalDamageTaken = 0;
			_mercyRouteActive = true;
			
			foreach (int level in _levelsCompleted.Keys)
			{
				_levelsCompleted[level] = false;
			}
		}
		#endregion

		#region Statistics
		/// <summary>
		/// Record enemy kill (disables mercy route)
		/// </summary>
		public void RecordEnemyKilled()
		{
			_enemiesKilled++;
			_mercyRouteActive = false;
			
			GD.Print($"Enemy killed. Total: {_enemiesKilled}. Mercy route: {_mercyRouteActive}");
		}

		/// <summary>
		/// Record damage taken by player
		/// </summary>
		public void RecordDamageTaken(int damage)
		{
			_totalDamageTaken += damage;
		}

		/// <summary>
		/// Get game statistics
		/// </summary>
		public Dictionary<string, object> GetStatistics()
		{
			return new Dictionary<string, object>
			{
				{ "level", _currentLevelIndex },
				{ "deaths", _playerDeaths },
				{ "play_time", _totalPlayTime },
				{ "enemies_killed", _enemiesKilled },
				{ "damage_taken", _totalDamageTaken },
				{ "mercy_route", _mercyRouteActive }
			};
		}
		#endregion

		#region Save/Load Integration
		/// <summary>
		/// Get save data
		/// </summary>
		public Godot.Collections.Dictionary<string, Variant> GetSaveData()
		{
			return new Godot.Collections.Dictionary<string, Variant>
			{
				{ "current_level", _currentLevelIndex },
				{ "checkpoint_x", _currentCheckpoint.X },
				{ "checkpoint_y", _currentCheckpoint.Y },
				{ "player_deaths", _playerDeaths },
				{ "total_play_time", _totalPlayTime },
				{ "enemies_killed", _enemiesKilled },
				{ "total_damage_taken", _totalDamageTaken },
				{ "mercy_route_active", _mercyRouteActive },
				{ "level_1_complete", _levelsCompleted[1] },
				{ "level_2_complete", _levelsCompleted[2] },
				{ "level_3_complete", _levelsCompleted[3] }
			};
		}

		/// <summary>
		/// Load save data
		/// </summary>
		public void LoadSaveData(Godot.Collections.Dictionary<string, Variant> data)
		{
			if (data.ContainsKey("current_level"))
				_currentLevelIndex = data["current_level"].AsInt32();
			
			if (data.ContainsKey("checkpoint_x") && data.ContainsKey("checkpoint_y"))
				_currentCheckpoint = new Vector2(
					data["checkpoint_x"].AsSingle(),
					data["checkpoint_y"].AsSingle()
				);
			
			if (data.ContainsKey("player_deaths"))
				_playerDeaths = data["player_deaths"].AsInt32();
			
			if (data.ContainsKey("total_play_time"))
				_totalPlayTime = data["total_play_time"].AsSingle();
			
			if (data.ContainsKey("enemies_killed"))
				_enemiesKilled = data["enemies_killed"].AsInt32();
			
			if (data.ContainsKey("total_damage_taken"))
				_totalDamageTaken = data["total_damage_taken"].AsInt32();
			
			if (data.ContainsKey("mercy_route_active"))
				_mercyRouteActive = data["mercy_route_active"].AsBool();
			
			if (data.ContainsKey("level_1_complete"))
				_levelsCompleted[1] = data["level_1_complete"].AsBool();
			
			if (data.ContainsKey("level_2_complete"))
				_levelsCompleted[2] = data["level_2_complete"].AsBool();
			
			if (data.ContainsKey("level_3_complete"))
				_levelsCompleted[3] = data["level_3_complete"].AsBool();
		}
		#endregion

		#region Pause System
		public void PauseGame()
		{
			GetTree().Paused = true;
			ChangeGameState(GameState.Paused);
		}

		public void ResumeGame()
		{
			GetTree().Paused = false;
			ChangeGameState(GameState.Playing);
		}
		#endregion

		#region Properties
		public int CurrentLevel => _currentLevelIndex;
		public Vector2 CurrentCheckpoint => _currentCheckpoint;
		public int PlayerDeaths => _playerDeaths;
		public float TotalPlayTime => _totalPlayTime;
		public bool IsMercyRoute => _mercyRouteActive;
		public GameState CurrentState => _gameState;
		public Node2D Player
		{
			get => _player;
			set => _player = value;
		}
		#endregion
	}

	#region Game State Enum
	public enum GameState
	{
		MainMenu,
		Playing,
		Paused,
		GameOver,
		Won
	}
	#endregion
}
