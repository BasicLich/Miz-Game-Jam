using Godot;
using System;

public class Menu : Node
{
	[Export]
	public int menuSelectionIndex;
	[Export]
	public int menuSize;

	bool timerTrig = false;
	[Signal]
	public delegate void menuSelection(int selection);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.ingame = false;
		EmitSignal(nameof(menuSelection), menuSelectionIndex);
		menuSize = GetChildren().Count-2;
	}

public override void _Process(float delta)
 {
		if (Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down"))
		{
			if (Input.IsActionJustPressed("ui_up"))
			{
				menuSelectionIndex = (menuSelectionIndex - 1) % menuSize;
			}
			else if (Input.IsActionJustPressed("ui_down"))
			{
				menuSelectionIndex = (menuSelectionIndex + 1) % menuSize;
			}
			EmitSignal(nameof(menuSelection), menuSelectionIndex);
			((AudioStreamPlayer)GetNode("/root/Menu/Audio/Select")).Play();
		}
		else if (Input.IsActionJustPressed("ui_accept"))
		{
			switch (menuSelectionIndex)
			{
				case 0:
					((Timer)FindNode("Timer")).Start();
					timerTrig = true;
					((AudioStreamPlayer)GetNode("/root/Menu/Audio/GameStart")).Play();
					break;
				case 1:
					GetTree().ChangeScene("res://Options.tscn");
					break;
				case 2:
					GetTree().Quit();
					break;
			}
		}

		if (timerTrig)
		{
			((Panel)GetNode("Cover")).Modulate = new Color(1, 1, 1, 1.5f - ((Timer)FindNode("Timer")).TimeLeft);
		}
		

	}

	

	private void _on_Timer_timeout()
	{
		GetTree().ChangeScene("res://Main.tscn");
	}
}



