using Godot;
using System;
using System.Collections.Generic;

namespace Demonteks.UI
{
    /// <summary>
    /// Game Over screen controller.
    /// </summary>
    public partial class GameOverController : CanvasLayer
    {
        private Label _statsLabel;
        private Management.GameManager _gameManager;

        public override void _Ready()
        {
            _statsLabel = GetNode<Label>("VBoxContainer/StatsLabel");
            _gameManager = GetNode<Management.GameManager>("/root/GameManager");

            // Display final stats
            DisplayStats();

            // Play game-over sound
            UIAudioController.Instance?.PlaySound(UIAudioController.UiSound.ErrorBuzz);

            GD.Print("Game Over screen displayed");
        }

        private void DisplayStats()
        {
            Dictionary<string, object> stats = _gameManager.GetStatistics();

            int deaths = (int)stats["deaths"];
            int kills = (int)stats["enemies_killed"];
            float playTime = (float)stats["play_time"];

            int minutes = (int)(playTime / 60);
            int seconds = (int)(playTime % 60);

            _statsLabel.Text = $"Deaths: {deaths}\nEnemies Killed: {kills}\nTime Played: {minutes}:{seconds:D2}";
        }

        private void _on_retry_pressed()
        {
            _gameManager.RestartLevel();
            QueueFree();
        }

        private void _on_main_menu_pressed()
        {
            _gameManager.ReturnToMainMenu();
            QueueFree();
        }
    }
}
