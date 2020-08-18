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

		Vector2 screenSize=GetViewport().Size;
        GD.Print(screenSize);
        GD.Print(((Node2D)GetParent()).Position);
        GD.Print((((Node2D)GetParent()).Position)-screenSize/2);
		int count = 0;

        Vector2 baseLoc = new Vector2((-screenSize.x / 4) + (screenSize.x / 2 - 16 * 8) - 32-10, 10 + (-screenSize.y / 4));

        DrawTexture(back, baseLoc);

        foreach (Card i in playerHand)
		{

				atlasTexture.Region = new Rect2((i.rank-2) * 16, i.suit * 16, 16,16);
                Vector2 drawLoc = baseLoc+ new Vector2( ((count % 8 + 1) * 16), ((count / 8+1) * 16));
                

                DrawTexture(atlasTexture, drawLoc);
				count++;

		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
