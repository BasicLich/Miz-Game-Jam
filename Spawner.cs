using Godot;
using System;

public class Spawner : Node2D
{
	public bool EnemySpawned=false;
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void spawnEnemy()
	{
		EnemySpawned=true;
		//todo spawn enemy
	}
	
	public void spawnCoin()
	{
		if(!EnemySpawned)
		{
			if((int)Math.Round(GD.RandRange(0, 1))==0)
			{
				var scene = GD.Load<PackedScene>("res://Coin.tscn");
				var node = scene.Instance();
				((Node2D)node).Position = new Vector2(0, 0);
				if ((int)Math.Round(GD.RandRange(0, 4)) == 0)
				{
					node.Call("setType", 1);
				}
				else { node.Call("setType", 0); }
				AddChild(node);
			}
		}
	}
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}