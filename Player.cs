using Godot;
using System;
using System.Collections.Generic;
public class Player : KinematicBody2D
{
	
	[Signal]
	public delegate void PlayerMotion(Vector2 currentLocation,Vector2 newLocation);

	public List<Card> hand;
	int score = 0;
	public Vector2 velocity = new Vector2();

	int MoveTimeout = 0;
	public int selectedCardIndex = 0;
	int HoldCount;

	public override void _Ready()
	{
		setSelectorPos();
	}

	public void GetInput()
	{
		velocity = new Vector2();

		if (!Input.IsActionPressed("right") && !Input.IsActionPressed("left") &&
			!Input.IsActionPressed("down")&& !Input.IsActionPressed("up"))
		{
			HoldCount = 0;
		}

		if(Input.IsKeyPressed(90))
		{
			int count = 1;
			foreach (Card i in hand)
			{
				i.Print();
				GD.Print(count);
				count += 1;
			}
			GD.Print("");
		}
		if(Input.IsKeyPressed(16777217))
		{
			GetTree().Quit();
		}

		if(Input.IsActionJustPressed("ui_right"))
		{
			if((selectedCardIndex+1)%8==0)
			{
				selectedCardIndex -= 7;
			}
			else if ( !(selectedCardIndex + 1 > hand.Count-1))
			{
				selectedCardIndex += 1;
			}
			else
			{ selectedCardIndex -= (selectedCardIndex % 8); }
			setSelectorPos();

		}

		if (Input.IsActionJustPressed("ui_left"))
		{
			if (!(selectedCardIndex % 8 == 0))
			{
				selectedCardIndex -= 1;
			}
			else if (!(selectedCardIndex + 7>hand.Count-1))
			{
				selectedCardIndex += 7;
			}
			else
			{
				selectedCardIndex = hand.Count - 1;
			}
			setSelectorPos();

		}

		if (Input.IsActionJustPressed("ui_down"))
		{
			if ((selectedCardIndex + 8)> hand.Count-1)
			{
				selectedCardIndex = selectedCardIndex%8;
			}
			else
			{
				selectedCardIndex += 8;
			}
			setSelectorPos();

		}

		if (Input.IsActionJustPressed("ui_up"))
		{
			if ((selectedCardIndex - 8) < 0)
			{
				while (selectedCardIndex + 8 < hand.Count)
				{
					selectedCardIndex += 8;
				}
			}
			else
			{
				selectedCardIndex -= 8;
			}
			setSelectorPos();

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
		((Label)GetParent().FindNode("CanvasLayer").FindNode("Score")).Text = score.ToString();
	}

	void setSelectorPos()
	{
		((Node2D)FindNode("Selector")).Position = ((CardDraw)FindNode("CardDraw")).baseLoc() + ((CardDraw)FindNode("CardDraw")).CardGUIOffset((selectedCardIndex));
	}

}
