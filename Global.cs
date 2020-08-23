using Godot;
using System;
using System.Collections.Generic;
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
	static public float palette = 0f;
	static public int highScore;
	static public List<bool> paletteUnlocked=new List<bool>();

	public override void _Ready()
	{
		var saveGame=new File();
		for (int i=0;i<4;i++)
		{
			paletteUnlocked.Add(false);
		}
		if (! saveGame.FileExists("user://savegame.save"))
		{
			saveGame.Open("user://savegame.save", File.ModeFlags.Write);
			saveGame.Store8(0);
			saveGame.Store8(0);
			saveGame.Store8(0);
			saveGame.Store8(0);
			saveGame.Store32(0);
			saveGame.Close();
		}
		saveGame.Open("user://savegame.save", File.ModeFlags.Read);

		for (int i=0;i<4;i++)
		{
			int line = saveGame.Get8();
			if (line==0)
			{
				paletteUnlocked[i] = false;
			}
			else
			{
				paletteUnlocked[i] = true;
			}

			
		}
		//GD.Print(saveGame.GetPosition());
		highScore = (int)saveGame.Get32();
	}

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
