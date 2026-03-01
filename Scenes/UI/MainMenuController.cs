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

        private Control _optionsPanel;
        private CheckButton _musicCheck;
        private CheckButton _sfxCheck;
        private CheckButton _vibrationCheck;
        private Button _optionsBackButton;

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

                // Options references
                _optionsPanel = GetNode<Control>("OptionsPanel");
                _musicCheck = GetNode<CheckButton>("OptionsPanel/VBoxContainer/MusicCheck");
                _sfxCheck = GetNode<CheckButton>("OptionsPanel/VBoxContainer/SfxCheck");
                _vibrationCheck = GetNode<CheckButton>("OptionsPanel/VBoxContainer/VibrationCheck");
                _optionsBackButton = GetNode<Button>("OptionsPanel/VBoxContainer/OptionsBackButton");

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

                // Initialize Options UI states
                _musicCheck.ButtonPressed = _saveManager.GameOptions.MusicEnabled;
                _sfxCheck.ButtonPressed = _saveManager.GameOptions.SfxEnabled;
                _vibrationCheck.ButtonPressed = _saveManager.GameOptions.VibrationEnabled;

                // Connect Options signals
                _musicCheck.Toggled += OnMusicToggled;
                _sfxCheck.Toggled += OnSfxToggled;
                _vibrationCheck.Toggled += OnVibrationToggled;
                _optionsBackButton.Pressed += OnOptionsBackPressed;

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
            _optionsPanel.Visible = true;
        }

        private void OnOptionsBackPressed()
        {
            _optionsPanel.Visible = false;
        }

        private void OnMusicToggled(bool toggledOn)
        {
            _saveManager.GameOptions.MusicEnabled = toggledOn;
            _saveManager.SaveOptions();
        }

        private void OnSfxToggled(bool toggledOn)
        {
            _saveManager.GameOptions.SfxEnabled = toggledOn;
            _saveManager.SaveOptions();
        }

        private void OnVibrationToggled(bool toggledOn)
        {
            _saveManager.GameOptions.VibrationEnabled = toggledOn;
            _saveManager.SaveOptions();
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

            if (_musicCheck != null) _musicCheck.Toggled -= OnMusicToggled;
            if (_sfxCheck != null) _sfxCheck.Toggled -= OnSfxToggled;
            if (_vibrationCheck != null) _vibrationCheck.Toggled -= OnVibrationToggled;
            if (_optionsBackButton != null) _optionsBackButton.Pressed -= OnOptionsBackPressed;
        }
    }
}
