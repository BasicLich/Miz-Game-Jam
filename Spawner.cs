using Godot;
using System;
using System.Collections.Generic;
public class Spawner : Node2D
{
	public bool EnemySpawned=false;
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void spawnEnemy(int enemyDifficulty,List<Card> monsterHand)
	{
		EnemySpawned = true;
		var scene = GD.Load<PackedScene>("res://Enemy.tscn");
		var node = scene.Instance();
		((Node2D)node).Position = Position;
		node.Set("difficulty", enemyDifficulty);
		node.Set("hand", monsterHand);

        foreach (Card i in monsterHand)
        {
            i.Print();
        }
        GD.Print("");
		GetParent().GetParent().FindNode("Enemies").AddChild(node);
	}

	public void spawnCoin()
	{
		if(!EnemySpawned)
		{
			if((int)Math.Round(GD.RandRange(0, 1))==0)
			{
				var scene = GD.Load<PackedScene>("res://Coin.tscn");
				var node = scene.Instance();
				((Node2D)node).Position = Position;
				if ((int)Math.Round(GD.RandRange(0, 4)) == 0)
				{
					node.Call("setType", 1);
				}
				else { node.Call("setType", 0); }
				GetParent().GetParent().FindNode("Player").Connect("PlayerMotion", node, "checkForCollection");
				GetParent().GetParent().FindNode("Coins").AddChild(node);
			}
		}
	}
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
