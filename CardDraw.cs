using Godot;
using System;
using System.Collections.Generic;
public class CardDraw : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Texture cards = GD.Load<Texture>("res://Tiles/cards.png");
	Texture back= GD.Load<Texture>("res://Tiles/cardbackdrop.png");
	Texture usedback = GD.Load<Texture>("res://Tiles/usedcardbackdrop.png");
	AtlasTexture atlasTexture = new AtlasTexture();

	
	public override void _Draw()
	{
		List<Card> playerHand = ((Player)GetParent()).hand;
		atlasTexture.Atlas = cards;

		Vector2 screenSize = GetViewport().Size;



		int count = 0;

		DrawTexture(usedback, baseLoc()+new Vector2(0,16*6));
		DrawTexture(back, baseLoc());
		//GD.Print("Draw2" + baseLoc());
		//draw player hand
		foreach (Card i in playerHand)
		{

				atlasTexture.Region = new Rect2((i.rank-2) * 16, i.suit * 16, 16,16);
			Vector2 drawLoc = baseLoc() + CardGUIOffset(count);
				

				DrawTexture(atlasTexture, drawLoc);
				count++;

		}

		//draw used cards

		List<Card> usedCards = ((AttackManager)GetParent().GetParent().GetNode("AttackManager")).usedCards;
		for(int i= 0;i<usedCards.Count;i++)
		{
			if (i < 8)
			{
				atlasTexture.Region = new Rect2((usedCards[i].rank - 2) * 16, usedCards[i].suit * 16, 16, 16);
				Vector2 drawLoc = baseLoc() + new Vector2(i * 16+16, 16 * 7);
				DrawTexture(atlasTexture, drawLoc);
			}
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
