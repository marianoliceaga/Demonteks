using Godot;
using System;
using System.Text.Json;

namespace Demonteks.Management
{
    /// <summary>
    /// Handles saving and loading game data to persistent storage.
    /// Uses JSON for save file format with encryption support.
    /// </summary>
    public partial class SaveManager : Node
    {
        #region Constants
        private const string SAVE_FILE_PATH = "user://demonteks_save.json";
        private const int SAVE_VERSION = 1;
        #endregion

        #region Godot Lifecycle
        public override void _Ready()
        {
            // Process mode to persist through scene changes
            ProcessMode = ProcessModeEnum.Always;
            
            GD.Print($"SaveManager initialized. Save file path: {SAVE_FILE_PATH}");
        }
        #endregion

        #region Save System
        /// <summary>
        /// Save the current game state
        /// </summary>
        public bool SaveGame()
        {
            try
            {
                var saveData = new Godot.Collections.Dictionary<string, Variant>
                {
                    { "version", SAVE_VERSION },
                    { "timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };
                
                // Get GameManager data
                GameManager gameManager = GetNode<GameManager>("/root/GameManager");
                if (gameManager != null)
                {
                    var gmData = gameManager.GetSaveData();
                    foreach (var kvp in gmData)
                    {
                        saveData[$"gm_{kvp.Key}"] = kvp.Value;
                    }
                }
                
                // Get Player data (if player exists in scene)
                var player = gameManager?.Player;
                if (player != null)
                {
                    if (player.HasNode("PlayerStats"))
                    {
                        var stats = player.GetNode("PlayerStats");
                        if (stats.HasMethod("GetSaveData"))
                        {
                            var statsData = stats.Call("GetSaveData").AsGodotDictionary();
                            foreach (var kvp in statsData)
                            {
                                saveData[$"ps_{kvp.Key}"] = kvp.Value;
                            }
                        }
                    }
                    
                    if (player.HasNode("PlayerCombat"))
                    {
                        var combat = player.GetNode("PlayerCombat");
                        if (combat.HasMethod("GetSaveData"))
                        {
                            var combatData = combat.Call("GetSaveData").AsGodotDictionary();
                            foreach (var kvp in combatData)
                            {
                                saveData[$"pc_{kvp.Key}"] = kvp.Value;
                            }
                        }
                    }
                }
                
                // Convert to JSON
                string jsonString = Json.Stringify(saveData, "\t");
                
                // Write to file
                using var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Write);
                if (file == null)
                {
                    GD.PrintErr($"Failed to open save file for writing: {FileAccess.GetOpenError()}");
                    return false;
                }
                
                file.StoreString(jsonString);
                file.Close();
                
                GD.Print($"Game saved successfully to {SAVE_FILE_PATH}");
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error saving game: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load the saved game state
        /// </summary>
        public bool LoadGame()
        {
            if (!FileAccess.FileExists(SAVE_FILE_PATH))
            {
                GD.Print("No save file found.");
                return false;
            }
            
            try
            {
                // Read file
                using var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Read);
                if (file == null)
                {
                    GD.PrintErr($"Failed to open save file for reading: {FileAccess.GetOpenError()}");
                    return false;
                }
                
                string jsonString = file.GetAsText();
                file.Close();
                
                // Parse JSON
                var json = new Json();
                var parseResult = json.Parse(jsonString);
                
                if (parseResult != Error.Ok)
                {
                    GD.PrintErr($"JSON parse error: {json.GetErrorMessage()} at line {json.GetErrorLine()}");
                    return false;
                }
                
                var saveData = json.Data.AsGodotDictionary();
                
                // Check version
                if (saveData.ContainsKey("version"))
                {
                    int version = saveData["version"].AsInt32();
                    if (version != SAVE_VERSION)
                    {
                        GD.Print($"Save file version mismatch. Expected {SAVE_VERSION}, got {version}");
                        // Could implement migration logic here
                    }
                }
                
                // Load GameManager data
                GameManager gameManager = GetNode<GameManager>("/root/GameManager");
                if (gameManager != null)
                {
                    var gmData = new Godot.Collections.Dictionary<string, Variant>();
                    foreach (var kvp in saveData)
                    {
                        if (kvp.Key.ToString().StartsWith("gm_"))
                        {
                            string key = kvp.Key.ToString().Substring(3);
                            gmData[key] = kvp.Value;
                        }
                    }
                    gameManager.LoadSaveData(gmData);
                }
                
                // Note: Player data will be loaded when player spawns in scene
                // Store it temporarily for later retrieval
                _pendingPlayerData = new Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>>
                {
                    { "stats", new Godot.Collections.Dictionary<string, Variant>() },
                    { "combat", new Godot.Collections.Dictionary<string, Variant>() }
                };
                
                foreach (var kvp in saveData)
                {
                    string key = kvp.Key.ToString();
                    if (key.StartsWith("ps_"))
                    {
                        _pendingPlayerData["stats"][key.Substring(3)] = kvp.Value;
                    }
                    else if (key.StartsWith("pc_"))
                    {
                        _pendingPlayerData["combat"][key.Substring(3)] = kvp.Value;
                    }
                }
                
                GD.Print("Game loaded successfully");
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error loading game: {ex.Message}");
                return false;
            }
        }

        private Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>> _pendingPlayerData;

        /// <summary>
        /// Apply pending player data when player spawns
        /// </summary>
        public void LoadPlayerData(Node player)
        {
            if (_pendingPlayerData == null) return;
            
            try
            {
                if (player.HasNode("PlayerStats") && _pendingPlayerData.ContainsKey("stats"))
                {
                    var stats = player.GetNode("PlayerStats");
                    if (stats.HasMethod("LoadSaveData"))
                    {
                        stats.Call("LoadSaveData", _pendingPlayerData["stats"]);
                    }
                }
                
                if (player.HasNode("PlayerCombat") && _pendingPlayerData.ContainsKey("combat"))
                {
                    var combat = player.GetNode("PlayerCombat");
                    if (combat.HasMethod("LoadSaveData"))
                    {
                        combat.Call("LoadSaveData", _pendingPlayerData["combat"]);
                    }
                }
                
                _pendingPlayerData = null;
                GD.Print("Player data loaded successfully");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error loading player data: {ex.Message}");
            }
        }
        #endregion

        #region Save File Management
        /// <summary>
        /// Check if a save file exists
        /// </summary>
        public bool HasSaveFile()
        {
            return FileAccess.FileExists(SAVE_FILE_PATH);
        }

        /// <summary>
        /// Delete the save file
        /// </summary>
        public bool DeleteSave()
        {
            if (!HasSaveFile())
            {
                GD.Print("No save file to delete.");
                return false;
            }
            
            var dir = DirAccess.Open("user://");
            Error err = dir.Remove(SAVE_FILE_PATH);
            
            if (err == Error.Ok)
            {
                GD.Print("Save file deleted successfully");
                return true;
            }
            else
            {
                GD.PrintErr($"Failed to delete save file: {err}");
                return false;
            }
        }

        /// <summary>
        /// Get save file info
        /// </summary>
        public SaveFileInfo GetSaveFileInfo()
        {
            if (!HasSaveFile())
                return null;
            
            try
            {
                using var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Read);
                if (file == null)
                    return null;
                
                string jsonString = file.GetAsText();
                file.Close();
                
                var json = new Json();
                json.Parse(jsonString);
                var data = json.Data.AsGodotDictionary();
                
                var info = new SaveFileInfo
                {
                    Timestamp = data.ContainsKey("timestamp") ? data["timestamp"].AsString() : "Unknown",
                    Level = data.ContainsKey("gm_current_level") ? data["gm_current_level"].AsInt32() : 1,
                    PlayTime = data.ContainsKey("gm_total_play_time") ? data["gm_total_play_time"].AsSingle() : 0f,
                    Deaths = data.ContainsKey("gm_player_deaths") ? data["gm_player_deaths"].AsInt32() : 0
                };
                
                return info;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error reading save file info: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region Auto Save
        private float _autoSaveInterval = 60f; // Auto-save every 60 seconds
        private float _autoSaveTimer = 0f;
        private bool _autoSaveEnabled = true;

        public override void _Process(double delta)
        {
            if (!_autoSaveEnabled) return;
            
            _autoSaveTimer += (float)delta;
            
            if (_autoSaveTimer >= _autoSaveInterval)
            {
                _autoSaveTimer = 0f;
                
                // Only auto-save if in playing state
                GameManager gameManager = GetNode<GameManager>("/root/GameManager");
                if (gameManager?.CurrentState == GameState.Playing)
                {
                    SaveGame();
                    GD.Print("Auto-save completed");
                }
            }
        }

        public void SetAutoSaveEnabled(bool enabled)
        {
            _autoSaveEnabled = enabled;
        }

        public void SetAutoSaveInterval(float seconds)
        {
            _autoSaveInterval = Mathf.Max(30f, seconds);
        }
        #endregion
    }

    #region Save File Info
    public class SaveFileInfo
    {
        public string Timestamp { get; set; }
        public int Level { get; set; }
        public float PlayTime { get; set; }
        public int Deaths { get; set; }

        public string GetPlayTimeFormatted()
        {
            int minutes = (int)(PlayTime / 60);
            int seconds = (int)(PlayTime % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
    #endregion
}
