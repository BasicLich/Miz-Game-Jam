using Godot;
using System;

public class Coin : Node2D
{
	int selfValue;
	[Export]
	int coinValue = 10;
	[Export]
	int gemValue = 50;
	[Signal]
	public delegate void CoinCollected(int value);
    [Signal]
    public delegate void HeartCollected();

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Connect("CoinCollected", GetParent().GetParent().FindNode("Player"), "increaseScore");
        Connect("HeartCollected", GetParent().GetParent().FindNode("Player"), "increaseHealth");
    }

	public void setType(int type)
	{
		
		if (type == 0)
		{
			selfValue = coinValue;
			var coin_tex = GD.Load<Texture>("res://Tiles/coin.png");
			((Sprite)GetNode("Sprite")).Texture=coin_tex;
		}
		else if (type==1)
		{
			selfValue = gemValue;
			var coin_tex = GD.Load<Texture>("res://Tiles/gem.png");
			((Sprite)GetNode("Sprite")).Texture = coin_tex;
		}
        else if (type == 2)
        {
            selfValue = 0;
            var tex = GD.Load<Texture>("res://Tiles/heart.png");
            ((Sprite)GetNode("Sprite")).Texture = tex;
        }
    }

	void checkForCollection(Vector2 playerLoc)
	{
		if (playerLoc == (Position / 16))
		{
			//TODO Play sound
			
            if (selfValue==0)
            {
                if (((Player)GetNode("/root/Scene/Player")).health < 5)
                {
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/FindHeart")).PitchScale = (float)GD.RandRange(0.9, 1.1);
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/FindHeart")).Play();
                    EmitSignal(nameof(HeartCollected));
                    QueueFree();
                }
            }
            else
            {
                if(selfValue==gemValue)
                {
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/FindGem")).PitchScale = (float)GD.RandRange(0.9, 1.1);
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/FindGem")).Play();
                }
                else
                {
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/FindCoin")).PitchScale = (float)GD.RandRange(0.9, 1.1);
                    ((AudioStreamPlayer)GetNode("/root/Scene/Audio/FindCoin")).Play();
                }
                EmitSignal(nameof(CoinCollected), selfValue);
                QueueFree();
            }
			
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
