using Godot;
using System;
using System.Collections.Generic;
public class Spawner : Node2D
{
	public bool EnemySpawned=false;
	public bool treasureRoomSpawner;


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

		GetParent().GetParent().FindNode("Enemies").AddChild(node);
	}

	public void spawnCoin()
	{
		if(!EnemySpawned)
		{
            int chance;
            if (treasureRoomSpawner)
            { chance = 60; }
            else { chance = 45; }
			if((int)Math.Round(GD.RandRange(0, 100))<chance)
			{
				var scene = GD.Load<PackedScene>("res://Coin.tscn");
				var node = scene.Instance();
				((Node2D)node).Position = Position;

                if (treasureRoomSpawner)
                { chance = 35; }
                else { chance = 25; }

                if (GD.RandRange(0, 100) < chance)
				{
					node.Call("setType", 1);
				}
                else { node.Call("setType", 0); }

                if (GD.RandRange(0, 100) < (7f-Global.floorLevel/3f))
                {
                    node.Call("setType", 2);
                }

                
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
