using Godot;
using System;

 public class Global : Node
{
   static public int floorLevel=0;
   static public int difficulty=0;
   static public int score = 0;
   static public bool win;
	static public string state;
	static public string stateprev;
	static public float musicVol=0.5f;
	static public float sfxVol=0.5f;
	static public string playsoundname;
	public override void _Process(float delta)
	{
		if (state=="game" && state != stateprev)
		{
			((AudioStreamPlayer)GetNode("MenuMusic")).Stop();
			((AudioStreamPlayer)FindNode("GameMusic")).Play();
		}
		if (state == "menu" && state != stateprev)
		{
			((AudioStreamPlayer)GetNode("GameMusic")).Stop();
			((AudioStreamPlayer)GetNode("MenuMusic")).Play();
		}
		if (playsoundname!=null)
		{
			((AudioStreamPlayer)FindNode(playsoundname)).Play();
		}
		playsoundname = null;
		stateprev = state;
	}

}
