using Godot;
using System;

 public class Global : Node
{
   static public int floorLevel=0;
   static public int difficulty=0;
   static public int score = 0;
   static public bool win;
	static public bool ingame = false;
	bool prevframeingame;

	public override void _Process(float delta)
	{
		if (ingame && ingame!=prevframeingame)
		{
			((AudioStreamPlayer)FindNode("Music")).Play();
		}
		if (!ingame && ingame != prevframeingame)
		{
			((AudioStreamPlayer)GetNode("Music")).Stop();
		}
		prevframeingame = ingame;
	}
}
