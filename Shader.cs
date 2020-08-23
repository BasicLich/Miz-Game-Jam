using Godot;
using System;

public class Shader : TextureRect
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		((ShaderMaterial)Material).SetShaderParam("palette", Global.palette);
}
	public void update()
	{
		((ShaderMaterial)Material).SetShaderParam("palette", Global.palette);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
