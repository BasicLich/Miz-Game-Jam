using Godot;
using System;

public class Player : KinematicBody2D
{
	
	[Signal]
	public delegate void PlayerMotion(Vector2 currentLocation,Vector2 newLocation);

	
	int score = 0;
	public Vector2 velocity = new Vector2();

	int MoveTimeout = 0;

	int HoldCount;


	public void GetInput()
	{
		velocity = new Vector2();

		if (!Input.IsActionPressed("right") && !Input.IsActionPressed("left") &&
			!Input.IsActionPressed("down")&& !Input.IsActionPressed("up"))
		{
			HoldCount = 0;
		}



		if (Input.IsActionJustPressed("right"))
		{
			HoldCount += 1;
			velocity.x += 1;
			return;
		}
		if (Input.IsActionJustPressed("left"))
		{
			HoldCount += 1;
			velocity.x -= 1;
			return;
		}
		if (Input.IsActionJustPressed("down"))
		{
			HoldCount += 1;
			velocity.y += 1;
			return;
		}
		if (Input.IsActionJustPressed("up"))
		{
			HoldCount += 1;
			velocity.y -= 1;
			return;
		}   

	}

	public override void _Process(float delta)
	{

		if (MoveTimeout>0)
		{
			MoveTimeout -= 1;
		}

		GetInput();
		

			TileMap x = (TileMap)GetNode("../TileMap");
		if (x.GetCellv((Position / 16) + velocity) == 1 || (bool)FindNode("Motion").Get("motionFlag"))
		{


			FindNode("Motion").Call("motion",delta,Position,velocity);
			}



		((Camera2D)FindNode("Camera2D")).Align();
	}

	

	public void increaseScore(int value)
	{
		score += value;
		((Label)FindNode("Score")).Text = score.ToString();
	}

}
