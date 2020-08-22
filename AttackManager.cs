using Godot;
using System;
using System.Collections.Generic;
public class AttackManager : Node2D
{
	[Signal]
	public delegate void DamagePlayer(int amount);
	[Signal]
	public delegate void DamageEnemy(string name);

	public List<Card> usedCards = new List<Card>();
	List<Attack> attacks=new List<Attack>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetParent().FindNode("Player").Connect("CardAttack", this , "storeAttack");
		GetParent().FindNode("Enemies").Connect("CardAttack", this , "storeAttack");
		Connect("DamagePlayer", GetParent().FindNode("Player"), "takeDamage");
		Connect("DamageEnemy", GetParent().FindNode("Enemies"), "takeDamage");
	}


	void storeAttack(Attack attack)
	{
		GD.Print(attack);
		attacks.Add(attack);
	}


  public override void _Process(float delta)
  {
		if (attacks.Count > 0)
		{

            //if the player is attacking ie the player has cards
            if (attacks[attacks.Count - 1].attackerName == "Player")
            {
                int playerIndex = attacks.Count - 1;
                //sentinel value
                int opponentIndex = 100;
                for (int i = 0; i < attacks.Count - 1; i++)
                {
                    if (attacks[i].attackerName == attacks[attacks.Count - 1].attackeeName)
                    {
                        opponentIndex = i;
                        break;
                    }
                }

                if (opponentIndex == 100)
                {
                    EmitSignal(nameof(DamageEnemy), attacks[playerIndex].attackeeName);
                }
                else
                {
                    if (attacks[playerIndex].card.value > attacks[opponentIndex].card.value)
                    {
                        EmitSignal(nameof(DamageEnemy), attacks[opponentIndex].attackerName);
                    }
                    else if (attacks[playerIndex].card.value < attacks[opponentIndex].card.value)
                    {
                        EmitSignal(nameof(DamagePlayer), 1);
                    }
                    else
                    {
                        ((AudioStreamPlayer)GetNode("/root/Scene/Audio/Draw")).PitchScale = (float)GD.RandRange(0.9, 1.1);
                        ((AudioStreamPlayer)GetNode("/root/Scene/Audio/Draw")).Play();
                    }

                }


                //Render any extra attacks against the player
                if (attacks.Count > 2)
                {
                    EmitSignal(nameof(DamagePlayer), attacks.Count - 2);
                }
            }
            //player has no cards, damage them for the number of attackers attacking the player
            else
            {
                EmitSignal(nameof(DamagePlayer), attacks.Count);
            }

			foreach (Attack i in attacks)
			{
				usedCards.Insert(0,i.card);
			}

			((Node2D)GetParent().FindNode("Player").FindNode("CardDraw")).Update();

			attacks.Clear();
		}
  }
}
