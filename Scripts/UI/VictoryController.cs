using Godot;
using System;
using System.Collections.Generic;

namespace Demonteks.UI
{
    /// <summary>
    /// Victory screen controller with multiple endings based on player choices.
    /// </summary>
    public partial class VictoryController : CanvasLayer
    {
        private RichTextLabel _endingText;
        private Label _statsLabel;
        private Management.GameManager _gameManager;

        public override void _Ready()
        {
            _endingText = GetNode<RichTextLabel>("VBoxContainer/EndingText");
            _statsLabel = GetNode<Label>("VBoxContainer/StatsLabel");
            _gameManager = GetNode<Management.GameManager>("/root/GameManager");

            // Display ending based on player's route
            DisplayEnding();
            DisplayStats();

            // Play victory fanfare
            UIAudioController.Instance?.PlaySound(UIAudioController.UiSound.LevelComplete);
            // Delay achievement sound slightly so both are audible
            GetTree().CreateTimer(1.0f).Timeout += () =>
                UIAudioController.Instance?.PlaySound(UIAudioController.UiSound.AchievementUnlock);

            GD.Print("Victory screen displayed");
        }

        private void DisplayEnding()
        {
            Dictionary<string, object> stats = _gameManager.GetStatistics();
            bool mercyRoute = (bool)stats["mercy_route"];

            if (mercyRoute)
            {
                // Pacifist/Mercy Ending
                _endingText.Text = @"[center][color=lime]MERCY ENDING[/color]

You crossed through the portal back to Earth, the connection
between worlds severed behind you. 

But you did so without taking a single Demontek life.

The King Demontek, moved by your restraint, whispers through
the closing portal:

[i]""You showed mercy when you could have shown violence.
Perhaps... humans and Demonteks need not be enemies.
This is not the end, explorer. This is a new beginning.""[/i]

[color=yellow]Achievement Unlocked: True Pacifist[/color][/center]";
            }
            else
            {
                // Combat Ending
                _endingText.Text = @"[center][color=orange]COMBAT ENDING[/color]

You crossed through the portal back to Earth, leaving a trail
of defeated Demonteks in your wake.

As the portal closes, you hear the King's dying words:

[i]""You may have won this battle, human, but you have started
a war that will consume both our worlds.
They will come for you. We all will.""[/i]

The connection is severed, but you know this is only
the beginning of the war between humans and Demonteks.

[color=red]The battle is won... but at what cost?[/color][/center]";
            }
        }

        private void DisplayStats()
        {
            Dictionary<string, object> stats = _gameManager.GetStatistics();

            int deaths = (int)stats["deaths"];
            int kills = (int)stats["enemies_killed"];
            float playTime = (float)stats["play_time"];
            bool mercyRoute = (bool)stats["mercy_route"];

            int minutes = (int)(playTime / 60);
            int seconds = (int)(playTime % 60);

            string routeText = mercyRoute ? "Mercy (Pacifist)" : "Combat";

            _statsLabel.Text = $@"Final Statistics:
Deaths: {deaths}
Enemies Killed: {kills}
Time Played: {minutes}:{seconds:D2}
Route: {routeText}";
        }

        private void _on_main_menu_pressed()
        {
            _gameManager.ReturnToMainMenu();
            QueueFree();
        }
    }
}
