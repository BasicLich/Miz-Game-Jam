using Godot;
using System;
using System.Collections.Generic;
public class Player : KinematicBody2D
{
	
	[Signal]
	public delegate void PlayerMotion(Vector2 newLocation);

	[Signal]
	public delegate void CardAttack(Attack attack);

    [Signal]
    public delegate void HealthChange(int health);

	public int health;
	public List<Card> hand;
	int score = 0;
	public Vector2 velocity = new Vector2();

	float MoveTimeout = 0;
	public int selectedCardIndex = 0;
	int HoldCount;
	public override void _Ready()
	{
		health = 5;
        score = Global.score;
        increaseScore(0);
		setSelectorPos();
		GetTree().Root.Connect("size_changed", this, "setSelectorPos");

	}

	public override void _Process(float delta)
	{
        if (GetParent().FindNode("TileMap").HasNode("ExitDoor"))
        {
             if (Position == ((Node2D)GetNode("/root/Scene/TileMap/ExitDoor")).Position)
            {
                Global.floorLevel += 1;
                Global.score = score;
                if (Global.floorLevel>5-Global.difficulty*5)
                {
                    GD.Print("win");
                    Global.win = true;
                    GetTree().ChangeScene("res://EndScreen.tscn");
                }
                GetTree().ReloadCurrentScene();
             }
        }


		GetInput();
		MoveTimeout += delta;
		if (HoldCount==1 || HoldCount>20 && MoveTimeout>0.2)
		{

		TileMap x = (TileMap)GetNode("../TileMap");
		Node target = checkTileForEnemy();
		if (x.GetCellv((Position / 16) + velocity) == 1 && velocity != new Vector2(0, 0) && target == null)
			{
			((EnemyManager)GetParent().FindNode("Enemies")).moveEnemies(Position / 16, Position / 16 + velocity);
		}
		else if(x.GetCellv((Position / 16) + velocity) == 1 && velocity != new Vector2(0, 0))
		{
			((EnemyManager)GetParent().FindNode("Enemies")).moveEnemies(Position / 16, Position / 16 );
		}
		target = checkTileForEnemy();

		if ((x.GetCellv((Position / 16) + velocity) == 1 && target==null && velocity!=new Vector2(0,0)))
		{
            //Play sound
            if(((AudioStreamPlayer)GetNode("/root/Scene/Audio/Walk")).PitchScale>=1)
                {
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/Walk")).PitchScale = 0.9f;
                }
            else
                { ((AudioStreamPlayer)GetNode("/root/Scene/Audio/Walk")).PitchScale = 1.1f; }

            ((AudioStreamPlayer)GetNode("/root/Scene/Audio/Walk")).Play();

            EmitSignal(nameof(PlayerMotion), Position/16+velocity);
			MoveTimeout = 0;
			((MotionModule)FindNode("Motion")).motionFlag = true;
		}

		else if ((x.GetCellv((Position / 16) + velocity) == 1) && velocity != new Vector2(0, 0) && hand.Count > 0)
		{ GD.Print("ATTACK");
			EmitSignal(nameof(CardAttack), new Attack(hand[selectedCardIndex], Name, target.Name));
			
			

			hand.RemoveAt(selectedCardIndex);
			((CardDraw)FindNode("CardDraw")).Update();
			if (selectedCardIndex == hand.Count)
			{
				selectedCardIndex -= 1;
                if (hand.Count==0)
                    {
                        GetNode("Selector").QueueFree();
                    }
				setSelectorPos();
			}
		}
			
		}
		if (((MotionModule)FindNode("Motion")).motionFlag)
		{
			FindNode("Motion").Call("motion", delta, Position, velocity);
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
				return i;
			}
			else if (i.velocity != new Vector2(0, 0) && i.hand.Count == 0 && i.Position == (Position + (velocity * 32)))
			{
				return i;
			}
		   
			else if (i.velocity == new Vector2(0, 0) && i.Position == (Position + (velocity * 16)))
			{

				return i;
			}
		}
		return null;
	}

	public void GetInput()
	{

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

		if (Input.IsActionPressed("right") || Input.IsActionPressed("left")|| Input.IsActionPressed("up")|| Input.IsActionPressed("down"))
		{
			HoldCount +=1;
		}
		
			velocity = new Vector2();
		

		if (Input.IsActionPressed("right"))
		{
			velocity.x += 1;
			return;
		}
		if (Input.IsActionPressed("left"))
		{
			velocity.x -= 1;
			return;
		}
		if (Input.IsActionPressed("down"))
		{
			velocity.y += 1;
			return;
		}
		if (Input.IsActionPressed("up"))
		{
			velocity.y -= 1;
			return;
		}   

	}

	
    //its reasonable to merge these two functions in the refactor
	public void takeDamage(int amount)
	{
		health -= amount;
        ((SpriteTransformer)FindNode("Sprite").FindNode("SpriteTransformer")).spriteFlashing = true;
        ((AudioStreamPlayer)GetNode("/root/Scene/Audio/SelfHurt")).PitchScale = (float)GD.RandRange(0.9, 1.1);
        ((AudioStreamPlayer)GetNode("/root/Scene/Audio/SelfHurt")).Play();
        if (health==0)
        {
            Global.win = false;
            GetTree().ChangeScene("res://EndScreen.tscn");
        }
        EmitSignal(nameof(HealthChange), health);
    }

    public void increaseHealth()
    {
        health++;
        EmitSignal(nameof(HealthChange), health);
    }

    public void increaseScore(int value)
	{
		score += value;
		((Label)GetParent().FindNode("CanvasLayer").FindNode("Score")).Text = "Score: "+score.ToString();
	}

	void setSelectorPos()
	{
		((Node2D)FindNode("Selector")).Position = ((CardDraw)FindNode("CardDraw")).baseLoc() + ((CardDraw)FindNode("CardDraw")).CardGUIOffset((selectedCardIndex));
	}

 

}
