using Godot;
using System;
using System.Collections.Generic;
public class EnemyManager : Node
{

	public void moveEnemies(Vector2 dontUse, Vector2 playerLoc)
	{

		foreach (Node2D i in GetChildren())
		{   
			i.Call("choosePath", playerLoc);
		}

		foreach (Node2D i in GetChildren())
		{
			foreach (Node2D j in GetChildren())
			{
				if (i==j)
				{
					continue;
				}
				
                //check to make sure its not trying to move to a tile currently or about to be occupied by an enemy
				if ((Vector2)i.Get("newPos") == ((Vector2)j.Get("newPos")) || (Vector2)i.Get("newPos") == ((Vector2)j.Position))
				{
					goto nested_break;
				}

				
			}

            if ((Vector2)i.Get("newPos")==((MotionModule)GetParent().FindNode("Player").FindNode("Motion")).newPos)
            {
                goto nested_break;
            }

			TileMap x = (TileMap)GetParent().FindNode("TileMap");
			if (x.GetCellv(((Vector2)i.Position / 16) + (Vector2)i.Get("velocity")) == 1)
			{
				i.Call("checkForTimeout", playerLoc);

				if ((bool)i.Get("timeout"))
				{
					i.Set("inMotion", true);
					i.FindNode("Motion").Call("motion", GetProcessDeltaTime(), (Vector2)i.Position, (Vector2)i.Get("velocity"));
				}
			}

		nested_break:;
		}


	}

}
