using Godot;
using System;

public class SelectableMenuItem : TextureRect
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export]
	int menuIndex;
	[Export]
	string name;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		GetParent().Connect("menuSelection", this, "updateSelection");

		((Label)FindNode("Text")).Text = Name;
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	void updateSelection(int selection)
	{
		if (selection == menuIndex)
		{
			Modulate = new Color(1.3f, 1.3f, 1.3f, 1.3f);
			if(HasNode("HSlider"))
			{
				((HSlider)FindNode("HSlider")).GrabFocus();
			}
		}
		else
		{
			Modulate = new Color(1f, 1f, 1f, 1f);
		}
	}
	private void _on_HSlider_value_changed(float value)
	{
		Global.playsoundname = "Select";
		if (name=="music")
		{
			Global.musicVol= value;
		}
		else
		{
			Global.sfxVol = value ;
		}
	}
	
}







