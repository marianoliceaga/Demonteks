using Godot;
using System;

namespace Demonteks.UI
{
    /// <summary>
    /// Main menu controller handling navigation and game start.
    /// </summary>
    public partial class MainMenuController : Control
    {
        private Button _newGameButton;
        private Button _continueButton;
        private Button _optionsButton;
        private Button _quitButton;
        
        private Management.GameManager _gameManager;
        private Management.SaveManager _saveManager;

        public override void _Ready()
        {
            // Get button references
            _newGameButton = GetNode<Button>("VBoxContainer/NewGameButton");
            _continueButton = GetNode<Button>("VBoxContainer/ContinueButton");
            _optionsButton = GetNode<Button>("VBoxContainer/OptionsButton");
            _quitButton = GetNode<Button>("VBoxContainer/QuitButton");
            
            // Get managers
            _gameManager = GetNode<Management.GameManager>("/root/GameManager");
            _saveManager = GetNode<Management.SaveManager>("/root/SaveManager");
            
            // Check if save file exists
            if (!_saveManager.HasSaveFile())
            {
                _continueButton.Disabled = true;
            }
            
            GD.Print("MainMenu loaded");
        }

        private void _on_new_game_pressed()
        {
            GD.Print("Starting new game...");
            _gameManager.StartNewGame();
        }

        private void _on_continue_pressed()
        {
            if (_saveManager.HasSaveFile())
            {
                GD.Print("Continuing game...");
                _gameManager.ContinueGame();
            }
        }

        private void _on_options_pressed()
        {
            GD.Print("Options not implemented yet");
            // TODO: Create options menu
        }

        private void _on_quit_pressed()
        {
            GD.Print("Quitting game...");
            GetTree().Quit();
        }
    }
}
