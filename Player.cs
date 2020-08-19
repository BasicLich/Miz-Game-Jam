using Godot;
using System;
using System.Collections.Generic;
public class Player : KinematicBody2D
{
	
	[Signal]
	public delegate void PlayerMotion(Vector2 newLocation);

	[Signal]
	public delegate void CardAttack(Attack attack);

	public int health;
	public List<Card> hand;
	int score = 0;
	public Vector2 velocity = new Vector2();

	int MoveTimeout = 0;
	public int selectedCardIndex = 0;
	int HoldCount;
	public override void _Ready()
	{
		health = 5;
		setSelectorPos();
	}

	public override void _Process(float delta)
	{

		if (MoveTimeout > 0)
		{
			MoveTimeout -= 1;
		}

		GetInput();

		TileMap x = (TileMap)GetNode("../TileMap");
		Node target = checkTileForEnemy();
		if (velocity != new Vector2(0, 0) && target == null)
			{
			((EnemyManager)GetParent().FindNode("Enemies")).moveEnemies(Position / 16, Position / 16 + velocity);
		}
		else if(velocity != new Vector2(0, 0))
		{
			((EnemyManager)GetParent().FindNode("Enemies")).moveEnemies(Position / 16, Position / 16 );
		}
		 target = checkTileForEnemy();
		if (target != null)
		{
			GD.Print(target.Name);
		}
		if ((x.GetCellv((Position / 16) + velocity) == 1 && target==null && velocity!=new Vector2(0,0)) || (bool)FindNode("Motion").Get("motionFlag"))
		{
			EmitSignal(nameof(PlayerMotion), Position/16+velocity);
			FindNode("Motion").Call("motion", delta, Position, velocity);
		}
		else if ((x.GetCellv((Position / 16) + velocity) == 1) && velocity != new Vector2(0, 0))
		{ GD.Print("ATTACK");
			EmitSignal(nameof(CardAttack), new Attack(hand[selectedCardIndex], Name, target.Name));
			
			

			hand.RemoveAt(selectedCardIndex);
			((CardDraw)FindNode("CardDraw")).Update();
			if (selectedCardIndex == hand.Count)
			{
				selectedCardIndex -= 1;
				setSelectorPos();
			}
			
		}

		((Camera2D)FindNode("Camera2D")).Align();
	}

	Node checkTileForEnemy()
	{
		//TODO this code is actually awful i can hopefully improve it in the refactor
		foreach (Enemy i in GetParent().FindNode("Enemies").GetChildren())
		{
			//GD.Print((i.Position == (Position + (velocity * 16)))+"  "+i.Position+" "+i.newPos +" "+);
			if (i.velocity != new Vector2(0,0) && !(i.hand.Count==0) && i.Position == (Position + (velocity * 16)))
			{
				GD.Print("a");
				return i;
			}
			else if (i.velocity != new Vector2(0, 0) && i.hand.Count == 0 && i.Position == (Position + (velocity * 32)))
			{
				GD.Print("b");
				return i;
			}
		   
			else if (i.velocity == new Vector2(0, 0) && i.Position == (Position + (velocity * 16)))
			{

				GD.Print("c");
				return i;
			}
		}
		return null;
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
