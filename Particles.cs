using Godot;
using System;

public class Particles : Particles2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private void _on_Timer_timeout()
	{
		Emitting = false;
	}

	private void _on_Timer2_timeout()
	{
		QueueFree();
	}
	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}





