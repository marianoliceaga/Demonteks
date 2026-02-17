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
            try
            {
                // Get button references
                _newGameButton = GetNode<Button>("VBoxContainer/NewGameButton");
                _continueButton = GetNode<Button>("VBoxContainer/ContinueButton");
                _optionsButton = GetNode<Button>("VBoxContainer/OptionsButton");
                _quitButton = GetNode<Button>("VBoxContainer/QuitButton");

                // Connect signals in code (most reliable for Godot 4 C#)
                _newGameButton.Pressed += OnNewGamePressed;
                _continueButton.Pressed += OnContinuePressed;
                _optionsButton.Pressed += OnOptionsPressed;
                _quitButton.Pressed += OnQuitPressed;

                // Get managers
                _gameManager = GetNode<Management.GameManager>("/root/GameManager");
                _saveManager = GetNode<Management.SaveManager>("/root/SaveManager");

                if (_gameManager == null)
                {
                    GD.PrintErr("MainMenu: GameManager not found at /root/GameManager!");
                    return;
                }

                if (_saveManager == null)
                {
                    GD.PrintErr("MainMenu: SaveManager not found at /root/SaveManager!");
                    return;
                }

                // Check if save file exists
                if (!_saveManager.HasSaveFile())
                {
                    _continueButton.Disabled = true;
                }

                GD.Print("MainMenu loaded successfully");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"MainMenu _Ready error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void OnNewGamePressed()
        {
            GD.Print("Starting new game...");
            try
            {
                _gameManager.StartNewGame();
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error starting new game: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void OnContinuePressed()
        {
            if (_saveManager.HasSaveFile())
            {
                GD.Print("Continuing game...");
                _gameManager.ContinueGame();
            }
        }

        private void OnOptionsPressed()
        {
            GD.Print("Options not implemented yet");
            // TODO: Create options menu
        }

        private void OnQuitPressed()
        {
            GD.Print("Quitting game...");
            GetTree().Quit();
        }

        public override void _ExitTree()
        {
            // Disconnect signals to prevent memory leaks
            if (_newGameButton != null) _newGameButton.Pressed -= OnNewGamePressed;
            if (_continueButton != null) _continueButton.Pressed -= OnContinuePressed;
            if (_optionsButton != null) _optionsButton.Pressed -= OnOptionsPressed;
            if (_quitButton != null) _quitButton.Pressed -= OnQuitPressed;
        }
    }
}
