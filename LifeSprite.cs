using Godot;
using System;

public class LifeSprite : Sprite
{
	[Export]
	public int value=0;
	Texture fulltex = GD.Load<Texture>("res://Tiles/heartfull.png");
	Texture emptytex= GD.Load<Texture>("res://Tiles/heartempty.png");
	public override void _Ready ()
	{
		value = Int32.Parse(Name);

		GetNode("/root/Scene/Player").Connect("HealthChange", this, "update");
		update(5);
	}


	public void update(int health)
  {
		Texture oldtex = Texture;
	  if (health>=value)
		{
			Texture = fulltex;
		}
	  else
		{ Texture = emptytex; }

	  if (oldtex!=Texture)
		{
			((SpriteTransformer)FindNode("SpriteTransformer")).spriteFlashing = true;
		}
  }
}
