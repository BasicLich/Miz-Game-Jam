using Godot;
using System;

public class Sound : AudioStreamPlayer
{
	[Export]
	public string type;
	[Export]
	public float volumeScale = 1;
	public override void _Ready()
	{
		if (type == "music")
			
		{
			VolumeDb = GD.Linear2Db(Global.musicVol* volumeScale);
		}
		else
		{
			VolumeDb = GD.Linear2Db(Global.sfxVol* volumeScale);
		}
	}


	public override void _Process(float delta)
 {
	if(type=="music" && Global.musicVol!=GD.Db2Linear(VolumeDb)
			)
		{
			VolumeDb = GD.Linear2Db(Global.musicVol* volumeScale);
		}
		if (type == "sfx" && Global.sfxVol != GD.Db2Linear(VolumeDb)
				)
		{
			VolumeDb = GD.Linear2Db(Global.sfxVol* volumeScale);
		}
	}
}
