using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Node2D
{
	
	public int difficulty;
	bool timeout=false;
	bool inMotion = true;
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

	public void move(Vector2 dontUse, Vector2 playerLoc)
	{
		
		if (Math.Sqrt(Math.Pow(Position.x / 16 - playerLoc.x, 2) + Math.Pow(Position.y / 16 - playerLoc.y, 2)) < (2 + 2 * difficulty))
		{
			Vector2 path = (Vector2)GetParent().GetParent().FindNode("TileMap").Call("pathfind", Position, playerLoc);

			inMotion = true;
			FindNode("Motion").Call("motion", GetProcessDeltaTime(), Position, (-Position + path * 16) / 16);
		}
		else if (Math.Sqrt(Math.Pow(Position.x / 16 - playerLoc.x, 2) + Math.Pow(Position.y / 16 - playerLoc.y, 2)) < (4 + 2 * difficulty))
		{
			if (!timeout)
			{
				timeout = true;
				Vector2 path = (Vector2)GetParent().GetParent().FindNode("TileMap").Call("pathfind", Position, playerLoc);

				inMotion = true;
				FindNode("Motion").Call("motion", GetProcessDeltaTime(), Position, (-Position+path*16)/16);
			}
			else { timeout = false; }

		}
		else
		{
			if (!timeout)
			{
				timeout = true;

				inMotion = true;
				FindNode("Motion").Call("idle", GetProcessDeltaTime(), Position);
			}
			else { timeout = false; }
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	  {
		  if(inMotion)
		{
			FindNode("Motion").Call("motion", GetProcessDeltaTime(), Position, new Vector2(0,0));
		}
  }
}
