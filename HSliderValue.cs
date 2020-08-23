using Godot;
using System;

public class HSliderValue : HSlider
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(GetParent().Name=="Music Volume")
		{ Value = Global.musicVol; }
		else
		{
			Value = Global.sfxVol;
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
