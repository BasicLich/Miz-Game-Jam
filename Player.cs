using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	float moveTime = 1;
	public Vector2 velocity = new Vector2();
	bool moveFlag = false;
	float motionPercentage = 0;
	Vector2 moveDirection;
	Vector2 PrevPos;

	public void GetInput()
	{
		velocity = new Vector2();

		{
			if (Input.IsActionJustPressed("right"))
			{
				velocity.x += 1;
			}
			if (Input.IsActionJustPressed("left"))
			{
				velocity.x -= 1;
			}
			if (Input.IsActionJustPressed("down"))
			{
				velocity.y += 1;
			}
			if (Input.IsActionJustPressed("up"))
			{
				velocity.y -= 1;
			}
		}

	}

	public override void _Process(float delta)
	{
		GetInput();
		
		if ((moveFlag) || (!(moveFlag) && (!(velocity.x==0) || !(velocity.y==0))))
		{
			motion(delta);
		}
	}

	void motion(float delta)
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
			moveDirection = velocity;
			moveFlag = true;
			PrevPos = Position;
		}
		motionPercentage += delta/moveTime;
		GD.Print(motionPercentage);
		Position = PrevPos + moveDirection * 16 * motionPercentage;

	}
}
