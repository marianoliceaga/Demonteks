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
        private Management.SaveManager _saveManager;

        // UI References
        private VBoxContainer _mainVBox;
        private Control _optionsPanel;
        private CheckButton _musicCheck;
        private CheckButton _sfxCheck;
        private CheckButton _vibrationCheck;

        public override void _Ready()
        {
            _gameManager = GetNode<Management.GameManager>("/root/GameManager");
            _saveManager = GetNode<Management.SaveManager>("/root/SaveManager");

            // Get node references
            _mainVBox = GetNode<VBoxContainer>("VBoxContainer");
            _optionsPanel = GetNode<Control>("OptionsPanel");
            _musicCheck = GetNode<CheckButton>("OptionsPanel/VBoxContainer/MusicCheck");
            _sfxCheck = GetNode<CheckButton>("OptionsPanel/VBoxContainer/SfxCheck");
            _vibrationCheck = GetNode<CheckButton>("OptionsPanel/VBoxContainer/VibrationCheck");

            // Connect signals for Options
            _musicCheck.Toggled += OnMusicToggled;
            _sfxCheck.Toggled += OnSfxToggled;
            _vibrationCheck.Toggled += OnVibrationToggled;

            // Hide initially
            Visible = false;
            _optionsPanel.Visible = false;
            _mainVBox.Visible = true;

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
            _mainVBox.Visible = true;
            _optionsPanel.Visible = false;

            // Update Option UI states when opening
            if (_saveManager != null)
            {
                _musicCheck.ButtonPressed = _saveManager.GameOptions.MusicEnabled;
                _sfxCheck.ButtonPressed = _saveManager.GameOptions.SfxEnabled;
                _vibrationCheck.ButtonPressed = _saveManager.GameOptions.VibrationEnabled;
            }

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
            _mainVBox.Visible = false;
            _optionsPanel.Visible = true;
        }

        private void _on_options_back_pressed()
        {
            _optionsPanel.Visible = false;
            _mainVBox.Visible = true;
        }

        private void OnMusicToggled(bool toggledOn)
        {
            if (_saveManager == null) return;
            _saveManager.GameOptions.MusicEnabled = toggledOn;
            _saveManager.SaveOptions();
        }

        private void OnSfxToggled(bool toggledOn)
        {
            if (_saveManager == null) return;
            _saveManager.GameOptions.SfxEnabled = toggledOn;
            _saveManager.SaveOptions();
        }

        private void OnVibrationToggled(bool toggledOn)
        {
            if (_saveManager == null) return;
            _saveManager.GameOptions.VibrationEnabled = toggledOn;
            _saveManager.SaveOptions();
        }

        private void _on_main_menu_pressed()
        {
            Resume();
            _gameManager.ReturnToMainMenu();
            GD.Print("Returning to main menu...");
        }
    }
}
