using Godot;
using System;

public class Enemy : Node2D
{
	
	public int difficulty;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Texture tex;
		if (difficulty == 0)
		{
			 tex = GD.Load<Texture>("res://Tiles/enemy0.png");
		}
		else if (difficulty==1)
		{
			 tex = GD.Load<Texture>("res://Tiles/enemy1.png");
		}
		else
		{
			tex = GD.Load<Texture>("res://Tiles/enemy2.png");
		}
			((Sprite)GetNode("Sprite")).Texture = tex;
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
