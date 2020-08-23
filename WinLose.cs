using Godot;
using System;

public class WinLose : Label
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var saveGame = new File();
		
		saveGame.Open("user://savegame.save", File.ModeFlags.ReadWrite);
		string unlockString = "";

		//Check for unlocks
		if (Global.win && !Global.paletteUnlocked[0])
		{
			saveGame.Store8(1);
			unlockString += "Bloody Nose Palette Unlocked! Palettes Can Be Swapped In Options Menu\n\n";
			Global.paletteUnlocked[0] = true;
		}
		else
		{
			saveGame.Seek(1);

		}
		if (Global.score>=1500 && !Global.paletteUnlocked[1])
		{
			unlockString += "Autumn Memory Palette Unlocked!\n\n";
			saveGame.Store8(1);
			Global.paletteUnlocked[1] = true;
		}
		else
		{
			saveGame.Seek(2);
		}
		if (Global.score >= 2000 && !Global.paletteUnlocked[2])
		{
			unlockString += "Muted Emotion Palette Unlocked!\n\n";
			saveGame.Store8(1);
			Global.paletteUnlocked[2] = true;
		}
		else
		{
			saveGame.Seek(3);
		}
		if (Global.score >= 2500 && !Global.paletteUnlocked[3])
		{
			unlockString += "90s Nostalgia Palette Unlocked!\n\n";
			saveGame.Store8(1);
			Global.paletteUnlocked[3] = true;
		}
		else
		{
			saveGame.Seek(4);
		}

		if (Global.score>Global.highScore && Global.win)
		{
			unlockString += "New Highscore!";
			saveGame.Store32((uint)Global.score);
			Global.highScore = Global.score;
		}
		else
		{
			saveGame.GetLine();
		}
		((Label)GetParent().GetNode("UnlockText")).Text = unlockString;

		((TextureRect)GetParent().FindNode("Continue")).Modulate = new Color(1.3f, 1.3f, 1.3f, 1.3f);
		Global.state = "menu";
		if (Global.win)
		{
			((AudioStreamPlayer)GetNode("/root/Node/Audio/Win")).Play();
			Text = "You Win!";
		}
		else
		{
			((AudioStreamPlayer)GetNode("/root/Node/Audio/Lose")).Play();
			Text = "You Lose!"; }
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
		if (Input.IsActionJustPressed("ui_accept"))
		{
			Global.playsoundname = "Select";
			GetTree().ChangeScene("res://Menu.tscn");
		}
  }
}
