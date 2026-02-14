using Godot;
using System;

namespace Demonteks.UI
{
    /// <summary>
    /// Pause menu controller handling game pause and menu navigation.
    /// </summary>
    public partial class PauseMenuController : CanvasLayer
    {
        private Management.GameManager _gameManager;

        public override void _Ready()
        {
            _gameManager = GetNode<Management.GameManager>("/root/GameManager");
            
            // Hide initially
            Visible = false;
            
            GD.Print("PauseMenu ready");
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("pause"))
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            if (Visible)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void Pause()
        {
            Visible = true;
            _gameManager.PauseGame();
            GD.Print("Game paused");
        }

        private void Resume()
        {
            Visible = false;
            _gameManager.ResumeGame();
            GD.Print("Game resumed");
        }

        private void _on_resume_pressed()
        {
            Resume();
        }

        private void _on_restart_pressed()
        {
            Resume();
            _gameManager.RestartLevel();
            GD.Print("Restarting level...");
        }

        private void _on_options_pressed()
        {
            GD.Print("Options not implemented yet");
            // TODO: Create options submenu
        }

        private void _on_main_menu_pressed()
        {
            Resume();
            _gameManager.ReturnToMainMenu();
            GD.Print("Returning to main menu...");
        }
    }
}
