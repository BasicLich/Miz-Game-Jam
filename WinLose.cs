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
