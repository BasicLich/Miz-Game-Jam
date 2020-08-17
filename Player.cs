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
    int MoveTimeout = 0;



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

        if (MoveTimeout>0)
        {
            MoveTimeout -= 1;
        }

		GetInput();
		
		if ((moveFlag) || (!(moveFlag) && (!(velocity.x==0) || !(velocity.y==0)) && MoveTimeout<=0))
		{
            TileMap x = (TileMap)GetNode("../TileMap");
            if (x.GetCellv((Position / 16) + new Vector2(1, 0) + velocity) == 1)
            {
                motion(delta);
            }
            else
            {
                MoveTimeout = 10;
            }
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
		//GD.Print(motionPercentage);
		Position = PrevPos + moveDirection * 16 * ease(motionPercentage);

	}

	float ease(float x)
	{

	return (float)-(Math.Cos(Math.PI*x) - 1) / 2;

	}
}
