using Godot;
using System;

public class Player : KinematicBody2D
{
	
	[Signal]
	public delegate void PlayerMotion(Vector2 newLocation);

	[Export]
	float moveTime = 1;
	int score = 0;
	public Vector2 velocity = new Vector2();
	bool moveFlag = false;
	float motionPercentage = 0;
	Vector2 moveDirection;
	Vector2 PrevPos;
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



		if (Input.IsActionPressed("right"))
		{
			HoldCount += 1;
			velocity.x += 1;
			return;
		}
		if (Input.IsActionPressed("left"))
		{
			HoldCount += 1;
			velocity.x -= 1;
			return;
		}
		if (Input.IsActionPressed("down"))
		{
			HoldCount += 1;
			velocity.y += 1;
			return;
		}
		if (Input.IsActionPressed("up"))
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
		
		if ((moveFlag) || ((HoldCount==1 || HoldCount>17) && !(moveFlag) && (!(velocity.x==0) || !(velocity.y==0)) && MoveTimeout<=0))
		{
			TileMap x = (TileMap)GetNode("../TileMap");
			if (x.GetCellv((Position / 16) + velocity) == 1)
			{
				motion(x,delta);
			}
			else
			{
				MoveTimeout = 10;
			}
		}

		((Camera2D)FindNode("Camera2D")).Align();
	}

	void motion(TileMap x, float delta)
	{
		if (motionPercentage>=1)
		{
			Position = PrevPos + moveDirection * 16;
			motionPercentage = 0;
			moveFlag = false;
			return;
		}
		if (motionPercentage==0)
		{
			GD.Print((Position / 16) + velocity);
			EmitSignal(nameof(PlayerMotion), (Position / 16) + velocity);
			moveDirection = velocity;
			moveFlag = true;
			PrevPos = Position;
		}
		motionPercentage += delta/moveTime;
		//GD.Print(motionPercentage);
		Position = PrevPos + moveDirection * 16 * ease(motionPercentage);
		((Sprite)FindNode("Sprite")).Position = new Vector2(0,20*(motionPercentage * motionPercentage - motionPercentage));
	}

	float ease(float x)
	{

	return (float)-(Math.Cos(Math.PI*x) - 1) / 2;

	}

	public void increaseScore(int value)
	{
		score += value;
		((Label)FindNode("Score")).Text = score.ToString();
	}

}
