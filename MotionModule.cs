using Godot;
using System;

public class MotionModule : Node
{

	[Export]
	float moveTime = 0.15f;
	public bool motionFlag = false;
	float motionPercentage = 0;
	public Vector2 lastPos;
	public Vector2 newPos;


	void motion(float delta, Vector2 PrevPos, Vector2 velocity)
	{
		if (!(velocity.x==0&& velocity.y==0)|| motionFlag == true)
		{
			if (motionPercentage >= 1)
			{
				//GD.Print(newPos);
				((Node2D)GetParent()).Position = newPos;
				if (GetParent().Name != "Player")
				{
					((Enemy)GetParent()).velocity = new Vector2(0, 0);
				}
				// GD.Print(((Node2D)GetParent()).Position);
				motionPercentage = 0;
				motionFlag = false;
				return;
			}
			if (motionPercentage == 0)
			{
				lastPos = PrevPos;
				newPos = lastPos + 16 * velocity;

				if (GetParent().Name == "Player")
				{
				}
				else
				{
				}

				motionFlag = true;
				//GD.Print(PrevPos);
			   // GD.Print(newPos);
			   // GD.Print("");
			}
			motionPercentage += delta / moveTime;
		   // GD.Print(motionPercentage);
   
			((Node2D)GetParent()).Position = (((1 - ease(motionPercentage)) * PrevPos) + (newPos * ease(motionPercentage)));

			((Sprite)GetParent().FindNode("Sprite")).Position = new Vector2(0, 20 * (motionPercentage * motionPercentage - motionPercentage));
		}
	}

	float ease(float x)
	{

		return (float)-(Math.Cos(Math.PI * x) - 1) / 2;

	}
}
