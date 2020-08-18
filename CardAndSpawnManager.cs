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
                deck.Add(new Card(i, j));
            }
        }

        //determine how many monsters of each type

        int floorLevel = (int)GetParent().GetParent().Get("floorLevel");

        int playerCards = 26 - (floorLevel - 1);
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

        //spawn in the monsters
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < monsterNo[i]; j++)
            {
                int randomIndex = (int)Math.Round(GD.RandRange(0, spawners.Count - 1));
                if (!((bool)(((Node)spawners[randomIndex]).Get("EnemySpawned"))))
                {
                    List<Card> monsterHand = new List<Card>();
                    for (int k = 0; k < i + 2; k++)
                    {
                        int randCardIndex = (int)Math.Round(GD.RandRange(0, deck.Count - 1));
                        monsterHand.Add(deck[randCardIndex]);
                        deck.RemoveAt(randCardIndex);

                    }
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
