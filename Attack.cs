using Godot;
using System;

public class Attack : Node
{
	public Card card; public string attackerName; public string attackeeName;

	public Attack(Card c, string attacker, string attackee)
	{
		card = c;
		attackerName = attacker;
		attackeeName = attackee;
	}
}
