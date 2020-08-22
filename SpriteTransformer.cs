using Godot;
using System;

public class SpriteTransformer : Node
{
	[Export]
	float spriteFlashTime = 0.05f;
	//[Export]
   // float spriteFlashScale = 1.35f;
	public bool spriteFlashing = false;
	float spriteFlashPercent = 0;
	Vector2 offset;
	//float scale;
	void spriteFlash(float delta)
	{

		if (spriteFlashPercent == 0)
		{
			//scale = ((Sprite)GetParent()).Scale.x;
			//((Sprite)GetParent()).Scale *= spriteFlashScale;
			//((Sprite)GetParent()).Offset /= ((Sprite)GetParent()).Scale / scale;
			((Sprite)GetParent()).Modulate = new Color(10, 10, 10, 10);

			if (GetParent().GetParent().HasNode("Particles"))
			{
				((Particles2D)GetParent().GetParent().FindNode("Particles")).Emitting = true;
			}
		}
		spriteFlashPercent += delta / spriteFlashTime;

		if (spriteFlashPercent > 1)
		{
			//((Sprite)GetParent()).Offset *= ((Sprite)GetParent()).Scale / scale;
			//((Sprite)GetParent()).Scale /= spriteFlashScale;
			if (GetParent().GetParent().HasNode("Particles"))
			{
				((Particles2D)GetParent().GetParent().FindNode("Particles")).Emitting = false;
			}
			spriteFlashPercent = 0;
			spriteFlashing = false;
			((Sprite)GetParent()).Modulate = new Color(1, 1, 1, 1);
		}
	}

	public override void _Process(float delta)
	{
		if (spriteFlashing)
		{
			spriteFlash(delta);
		}
	}
}

