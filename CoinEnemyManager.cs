using Godot;
using System;

public class CoinEnemyManager : Node
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	  foreach(Node N in GetParent().GetNode("../Spawners").GetChildren())
		{
			N.Call("spawnCoin");
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
