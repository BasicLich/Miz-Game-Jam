using Godot;
using System;
using System.Collections.Generic;

public class CardAndSpawnManager : Node
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		//generate cards
		List<Card> deck = new List<Card>();

		//suit
		for (int i = 0; i < 4; i++)
		{
			//rank
			for (int j = 2; j < 15; j++)
			{
				//trump logic here todo
				deck.Add(new Card(i, j,false));
			}
		}

		//determine how many monsters of each type

		int floorLevel = Global.floorLevel;

		int playerCards = 26 - floorLevel*3;
		int monsterCards = 52 - playerCards;
		int tempCardTrack = monsterCards;

		int[] monsterNo = new int[3];

		while (tempCardTrack >= monsterCards * (4f / 5f))
		{
			monsterNo[2] += 1;
			tempCardTrack -= 4;
		}
		while (tempCardTrack >= monsterCards * (2f / 5f))
		{
			monsterNo[1] += 1;
			tempCardTrack -= 3;
		}
		while (tempCardTrack > 4)
		{
			monsterNo[0] += 1;
			tempCardTrack -= 2;
		}

		//Add an enemy to use up the remainder of the cards
		monsterNo[tempCardTrack - 2] += 1;
		tempCardTrack -= tempCardTrack;

		Godot.Collections.Array spawners = GetParent().GetNode("../Spawners").GetChildren();

		//spawn the exit door
		while(true)
		{
			int randomIndex = (int)Math.Round(GD.RandRange(-0.5, spawners.Count - 1+0.49));
			Spawner chosenSpawner = ((Spawner)spawners[randomIndex]);
			if (chosenSpawner.treasureRoomSpawner)
			{
				var scene = GD.Load<PackedScene>("res://ExitDoor.tscn");
				var node = scene.Instance();
				((Node2D)node).Position = chosenSpawner.Position;
				GD.Print(chosenSpawner.Position);
				chosenSpawner.EnemySpawned = true;
				GetParent().AddChild(node);
				break;
			}
		}

		//spawn in the monsters

		//for each monster difficulty
		for (int i = 0; i < 3; i++)
		{
			//for each monster of that difficulty
			for (int j = 0; j < monsterNo[i]; j++)
			{
				//choose a random unused spawner
				int randomIndex = (int)Math.Round(GD.RandRange(0, spawners.Count - 1));
				if (!((bool)(((Node)spawners[randomIndex]).Get("EnemySpawned"))))
				{

					List<Card> monsterHand = new List<Card>();
					//give it random cards
					for (int k = 0; k < i + 2; k++)
					{
						int randCardIndex = (int)Math.Round(GD.RandRange(0, deck.Count - 1));
						monsterHand.Add(deck[randCardIndex]);
						deck.RemoveAt(randCardIndex);
					}

					//sort the cards TODO implement trump logic

					monsterHand.Sort((y, x) => x.value.CompareTo(y.value));

					foreach(Card k in monsterHand)
					{
						k.Print();
					}
					GD.Print("");

					((Node)spawners[randomIndex]).Call("spawnEnemy", i, monsterHand);
					continue;
				}
				else
				{
					j--;
					continue;
				}
			}

		}

		//give remainer of the cards to the player
		((Node)GetParent().GetParent().FindNode("Player")).Set("hand", deck);

		//spawn coins
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
