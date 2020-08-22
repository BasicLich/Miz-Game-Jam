using Godot;
using System;

public class Sound : AudioStreamPlayer
{
	[Export]
	public string type;

	public override void _Ready()
	{
		if (type == "music")
			
		{
			VolumeDb = GD.Linear2Db(Global.musicVol);
		}
		else
		{
			VolumeDb = GD.Linear2Db(Global.sfxVol);
		}
	}


	public override void _Process(float delta)
 {
	if(type=="music" && Global.musicVol!=GD.Db2Linear(VolumeDb)
			)
		{
			VolumeDb = GD.Linear2Db(Global.musicVol);
		}
		if (type == "sfx" && Global.sfxVol != GD.Db2Linear(VolumeDb)
				)
		{
			VolumeDb = GD.Linear2Db(Global.sfxVol);
		}
	}
}
