using Godot;
using System;

public class Menu : Node
{
	[Export]
	public int menuSelectionIndex=0;
	[Export]
	public int menuSize;

	bool timerTrig = false;
	[Signal]
	public delegate void menuSelection(int selection);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.state = "menu";
		EmitSignal(nameof(menuSelection), menuSelectionIndex);
		menuSize = GetChildren().Count-5;
	}

public override void _Process(float delta)
 {
		if (Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down"))
		{
			if (Input.IsActionJustPressed("ui_up"))
			{
				if (menuSelectionIndex==0)
				{
					menuSelectionIndex = menuSize - 1;
				
				}
				else
				{
					menuSelectionIndex--;
				}
			}
			else if (Input.IsActionJustPressed("ui_down"))
			{
				menuSelectionIndex = (menuSelectionIndex + 1) % menuSize;
			}
			EmitSignal(nameof(menuSelection), menuSelectionIndex);
			((AudioStreamPlayer)GetNode("/root/"+Name+"/Audio/Select")).Play();
		}
		else if (Input.IsActionJustPressed("ui_accept"))
		{
			if (Name == "Menu")
			{
				switch (menuSelectionIndex)
				{
					case 0:
						((Timer)FindNode("Timer")).Start();
						timerTrig = true;
						((AudioStreamPlayer)GetNode("/root/Menu/Audio/GameStart")).Play();
						break;
					case 1:
						Global.playsoundname="Select";
						GetTree().ChangeScene("res://Options.tscn");
						break;
					case 2:
						Global.playsoundname = "Select";
						GetTree().ChangeScene("res://Tutorial.tscn");
						break;
					case 3:
						GetTree().Quit();
						break;
				}
			}
			else if (Name == "Options")
			{
				switch (menuSelectionIndex)
				{
					case 2:
						Global.playsoundname = "Select";
						OS.WindowFullscreen = !OS.WindowFullscreen;
						break;
					case 4:
						 Global.playsoundname="Select";
						GetTree().ChangeScene("res://Menu.tscn");
						break;
			}
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



