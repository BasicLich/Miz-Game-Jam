using Godot;
using System;

public class SpriteTransformer : Node
{
	[Export]
	float spriteFlashTime = 0.1f;
	public bool spriteFlashing = false;
	float spriteFlashPercent = 0;

	void spriteFlash(float delta)
	{
		float intensity = 10;

		((Sprite)GetParent()).Modulate = new Color(intensity, intensity, intensity, intensity);
		spriteFlashPercent += delta / spriteFlashTime;

		if (spriteFlashPercent > 1)
		{
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

