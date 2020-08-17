using Godot;
using System;

public class Coin : Node2D
{
	int CoinType;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void setType(int type)
	{
		CoinType = type;
		if (type == 0)
		{
			var coin_tex = GD.Load<Texture>("res://Tiles/coin.png");
			((Sprite)GetNode("Sprite")).Texture=coin_tex;
		}
		else if (type==1)
		{
			var coin_tex = GD.Load<Texture>("res://Tiles/gem.png");
			((Sprite)GetNode("Sprite")).Texture = coin_tex;
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
