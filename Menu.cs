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
	[Signal]
	public delegate void paletteUpdate();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("paletteUpdate", GetNode("Shader"), "update");
		Global.state = "menu";
		EmitSignal(nameof(menuSelection), menuSelectionIndex);
		menuSize = GetChildren().Count-7;
		if (Name=="Tutorial")
		{
			((TextureRect)FindNode("Back To Menu")).Modulate = new Color(1.3f, 1.3f, 1.3f, 1.3f);
		}
	}

public override void _Process(float delta)
 {
		if ((Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down")) && Name!="Tutorial")
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
						Global.score = 0;
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
					case 3:
						Global.playsoundname = "Select";
						GetTree().ChangeScene("res://Palette.tscn");
						break;
					case 4:
						 Global.playsoundname="Select";
						GetTree().ChangeScene("res://Menu.tscn");
						break;
			}
			}
			else if (Name == "Palette")
			{
				if (menuSelectionIndex != 5)
				{
					if (menuSelectionIndex ==0|| Global.paletteUnlocked[menuSelectionIndex-1] )
					{
						Global.playsoundname = "Select";
						Global.palette = menuSelectionIndex / 4.0f;
						EmitSignal(nameof(paletteUpdate));
					}
				}
				else
				{
					Global.playsoundname = "Select";
					GetTree().ChangeScene("res://Menu.tscn");
					
				}
			}
			else if (Name=="Tutorial")
			{
				Global.playsoundname = "Select";
				GetTree().ChangeScene("res://Menu.tscn");
			}
		}

		if (timerTrig)
		{
			((Panel)GetNode("Cover")).Modulate = new Color(1, 1, 1, 1.1f);
		}
		

	}


	private void _on_Timer_timeout()
	{
		GetTree().ChangeScene("res://Main.tscn");
	}
}



