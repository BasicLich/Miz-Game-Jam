using Godot;
using System;
using System.Collections.Generic;
public class AttackManager : Node
{

	List<Attack> attacks=new List<Attack>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetParent().FindNode("Player").Connect("CardAttack", this , "storeAttack");
		GetParent().FindNode("Enemies").Connect("CardAttack", this , "storeAttack");
	}


	void storeAttack(Attack attack)
	{
		GD.Print(attack);
		attacks.Add(attack);
	}


  public override void _Process(float delta)
  {

       attacks.Clear();
	  
  }
}
