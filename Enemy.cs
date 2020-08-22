using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Node2D
{
	public int health;
	public int difficulty;
	bool timeout=false;
	public bool inMotion = true;
	public Vector2 velocity;
	public List<Card> hand;

   
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		((Timer)FindNode("Timer")).Connect("timeout", this, "queue_free");

		Texture tex;
		if (difficulty == 0)
		{
			health = 1;
			 tex = GD.Load<Texture>("res://Tiles/enemy0.png");
		}
		else if (difficulty==1)
		{
			health = 2;
			tex = GD.Load<Texture>("res://Tiles/enemy1.png");
		}
		else
		{
			health = 3;
			tex = GD.Load<Texture>("res://Tiles/enemy2.png");
		}
			((Sprite)GetNode("Sprite")).Texture = tex;
		
	}

	public void choosePath(Vector2 playerLoc)
	{
		if (Math.Sqrt(Math.Pow(Position.x / 16 - playerLoc.x, 2) + Math.Pow(Position.y / 16 - playerLoc.y, 2)) < (4 + 2 * difficulty))
		{
			if (hand.Count > 0)
			{
				Vector2 path = (Vector2)GetParent().GetParent().FindNode("TileMap").Call("pathfind", Position, playerLoc);
				velocity = (-Position + path * 16) / 16;
			}
			else
			{

				Vector2 difference = -playerLoc + (Position / 16);
				if (Math.Abs(difference.x) > Math.Abs(difference.y))
				{
					velocity = new Vector2(difference.x / Math.Abs(difference.x), 0);
				}
				else
				{
					velocity = new Vector2(0, difference.y / Math.Abs(difference.y));
				}

			}

		}
		else
		{
			int rand = (int)Math.Round(GD.RandRange(0, 3));
			velocity = new Vector2(0, 0);
			switch (rand)
			{
				case 0:
					velocity.x = 1;
					break;
				case 1:
					velocity.y = 1;
					break;
				case 2:
					velocity.x = -1;
					break;
				case 3:
					velocity.y = -1;
					break;
			}
		}

	}

	public void checkForTimeout(Vector2 playerLoc)
	{
		if (Math.Sqrt(Math.Pow(Position.x / 16 - playerLoc.x, 2) + Math.Pow(Position.y / 16 - playerLoc.y, 2)) < Math.Floor(3 + 1.5 * difficulty))
		{
			timeout = true;
		}
		else
		{
			if (timeout)
			{
				timeout = false;
			}
			else {
				timeout = true; }
		}
	}

	public void takeDamage()
	{
		health--;
		((SpriteTransformer)FindNode("Sprite").FindNode("SpriteTransformer")).spriteFlashing = true;
		if (health < 1)
		{
			((AudioStreamPlayer)GetNode("/root/Scene/Audio/MonsterDead")).PitchScale = (float)GD.RandRange(0.9, 1.1);
			((AudioStreamPlayer)GetNode("/root/Scene/Audio/MonsterDead")).Play();
			((Player)GetNode("/root/Scene/Player")).increaseScore(20 + 30 * difficulty);
			((Timer)FindNode("Timer")).WaitTime = 0.05f;
			((Timer)FindNode("Timer")).Start();

		}
		else
		{
			((AudioStreamPlayer)GetNode("/root/Scene/Audio/MonsterHurt")).PitchScale = (float)GD.RandRange(0.9, 1.1);
			((AudioStreamPlayer)GetNode("/root/Scene/Audio/MonsterHurt")).Play();
		}
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		FindNode("Motion").Call("motion", GetProcessDeltaTime(), Position, velocity);
	}
}
