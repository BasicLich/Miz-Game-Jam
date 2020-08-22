using Godot;
using System;

public class ClickableMenu: TextureRect
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export]
	int menuIndex;

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
		}
		else
		{
			Modulate = new Color(1f, 1f, 1f, 1f);
		}
	}
}


