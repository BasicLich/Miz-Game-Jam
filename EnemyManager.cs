using Godot;
using System;
using System.Collections.Generic;
public class EnemyManager : Node
{
	[Signal]
	public delegate void CardAttack(Attack attack);
	//TODO in the refactor move much of this behaviour into the enemy class itself and call the methods from here
	public void moveEnemies(Vector2 playerCurrentPos, Vector2 playerNextPos)
	{
		foreach (Enemy i in GetChildren())
		{
			i.velocity = new Vector2(0, 0);
			i.Call("choosePath", playerNextPos);

			if (i.hand.Count == 0)
			{
				
				Vector2 difference = -playerCurrentPos + (i.Position / 16);
				if (Math.Abs(difference.x) > Math.Abs(difference.y))
				{
					i.velocity = new Vector2(difference.x / Math.Abs(difference.x), 0);
				}
				else
				{
					i.velocity = new Vector2(0, difference.y / Math.Abs(difference.y));
				}
				GD.Print(i.Name+" "+i.velocity);
			}
		}

		foreach (Enemy i in GetChildren())
		{

			//check to make sure enemies intended next position is not about to occupied by the player

			if ((playerCurrentPos != playerNextPos) && playerNextPos * 16 == i.Position+(i.velocity*16))
			{
				GD.Print("oops4");
				GD.Print(i.Position);
				GD.Print(i.velocity * 16);
				i.velocity = new Vector2(0, 0);
				goto nested_break;
			}
			//Check to see if player is attacking, if so attack player
			else if ((playerCurrentPos == playerNextPos) &&playerCurrentPos*16==i.Position+(i.velocity*16))
			{
				GD.Print(i.Name + " ATTACK");
				GD.Print(i.velocity * 16);
				EmitSignal(nameof(CardAttack), new Attack(((Enemy)i).hand[0], i.Name, "Player"));
				i.velocity = new Vector2(0, 0);
				((Enemy)i).hand.RemoveAt(0); 
				goto nested_break;
			}

			foreach (Enemy j in GetChildren())
			{
				if (i==j)
				{
					continue;
				}

			   
				Vector2 jNewPos = ((MotionModule)j.FindNode("Motion")).newPos;

				//check to make sure its not trying to move to a tile currently or about to be occupied by an enemy
				if (i.Position+(i.velocity * 16) == j.Position+(j.velocity * 16) || i.Position +(i.velocity * 16) == j.Position)
				{
					GD.Print("oops");
					i.velocity = new Vector2(0, 0);
					goto nested_break;
				}

			}

		   
			TileMap x = (TileMap)GetParent().FindNode("TileMap");
			i.Call("checkForTimeout", playerNextPos);
			if (x.GetCellv((i.Position / 16) + i.velocity) == 1 && (bool)i.Get("timeout"))
			{
			}
			else
			{
				i.velocity = new Vector2(0, 0);
			}


		nested_break:;
            GD.Print(i.Name + " "+i.velocity);
        }

	}

}
