using Godot;
using System;

public class CoinEnemyManager : Node
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int floorLevel = (int)GetParent().GetParent().Get("floorLevel");

		int playerCards = 26 - floorLevel-1;
		int monsterCards = 52 - playerCards;
		int tempCardTrack = monsterCards;

		int[] monsterNo = new int[3];

		while (tempCardTrack >= monsterCards * (3f / 4f))
		{
			GD.Print("hard");
			monsterNo[2] +=1;
			tempCardTrack -= 4;
		}
		while (tempCardTrack >= monsterCards * (1f / 3f))
		{
			GD.Print("med");
			monsterNo[1] += 1;
			tempCardTrack -= 3;
		}
		while (tempCardTrack>4)
		{
			GD.Print("easy");
			monsterNo[0] += 1;
			tempCardTrack -= 2;
		}

		//Add an enemy to use up the remainder of the cards
		monsterNo[tempCardTrack - 2] += 1;
		tempCardTrack -= tempCardTrack;

		Godot.Collections.Array spawners= GetParent().GetNode("../Spawners").GetChildren();

		

		//spawn in the monsters
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < monsterNo[i]; j++)
			{
				int randomIndex = (int)Math.Round(GD.RandRange(0, spawners.Count-1));
				if (!((bool)(((Node)spawners[randomIndex]).Get("EnemySpawned"))))
				{
					((Node)spawners[randomIndex]).Call("spawnEnemy",i);
					continue;
				}
				else
				{ j--;
					GD.Print("enemy place fail "+i+" "+j);
					continue;
				}
			}
		}

		foreach (Node N in GetParent().GetNode("../Spawners").GetChildren())
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
