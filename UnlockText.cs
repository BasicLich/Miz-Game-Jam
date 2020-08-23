using Godot;
using System;

public class UnlockText : Label
{
	[Export]
	public string text;
	[Export]
	public int index;
  public override void _Ready()
  {
	  if (Global.paletteUnlocked[index]==true)
	   {
			Text = "";
	   }
	  else
		{
			Text = text;
		}
  }
}
