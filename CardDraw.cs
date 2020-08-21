using Godot;
using System;
using System.Collections.Generic;
public class CardDraw : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Texture texture = GD.Load<Texture>("res://Tiles/cards.png");
	Texture back= GD.Load<Texture>("res://Tiles/cardbackdrop.png");
	AtlasTexture atlasTexture = new AtlasTexture();

	
	public override void _Draw()
	{
		List<Card> playerHand = ((Player)GetParent()).hand;
		atlasTexture.Atlas = texture;

		

		int count = 0;


		DrawTexture(back, baseLoc());

		foreach (Card i in playerHand)
		{

				atlasTexture.Region = new Rect2((i.rank-2) * 16, i.suit * 16, 16,16);
			Vector2 drawLoc = baseLoc() + CardGUIOffset(count);
				

				DrawTexture(atlasTexture, drawLoc);
				count++;

		}
	}

	public Vector2 CardGUIOffset(int offsetIndex)
	{
		return new Vector2(((offsetIndex % 8 + 1) * 16), ((offsetIndex / 8 + 1) * 16));
	}

	public Vector2 baseLoc()
	{
		Vector2 screenSize = GetViewport().Size;
		return new Vector2((-screenSize.x / 4) + (screenSize.x / 2 - 16 * 8) - 32 - 10, 10 + (-screenSize.y / 4));
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
