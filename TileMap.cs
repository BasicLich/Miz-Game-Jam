using Godot;
using System;
using System.Collections.Generic;

public class TileMap : Godot.TileMap
{
	[Export]
	int mapSize = 5;

    
	public List<List<AStarSharp.Node>> tileArray = new List<List<AStarSharp.Node>>(200);

	public AStarSharp.Astar pathfindAstar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int floorLevel = Global.floorLevel;
        Global.state = "game";



		for (int i = 0; i < 200; i++)
		{
			tileArray.Add(new List<AStarSharp.Node>());

			for (int j=0;j< 200;j++)
			{
				tileArray[i].Add(new AStarSharp.Node(new Vector2(i, j), false));
			}
		}

		for (int i=-70;i<200;i++)
		{
			for (int j = -70; j < 200; j++)
			{
				SetCell(i, j, 0);
			}
		}

		//ROOM LAYOUT

		int[,] dungeonRoomGrid = new int[mapSize * 2 + 1, mapSize * 2 + 1];

		int noOfWalkers = (int)Math.Floor(floorLevel / 3f) + 3;
		//GD.Print(noOfWalkers);
		//GD.Print((int)((1f / (floorLevel + 3)) * 100));
		Walker[] walkerArray = new Walker[noOfWalkers];

		for (int i = 0; i < walkerArray.Length; i++) { walkerArray[i] = new Walker(); }

		for(int i=0;i<30;i++)
		{ GD.Print(i+" chance=" +
			((50f / 900f) * i * i - (3000f / 900f) * i + 50)); }

		float chance = ((50f / 900f) * floorLevel * floorLevel - (3000f / 900f) * floorLevel + 50);

		while (true)
		{
			bool flag = true;

			for (int i = 0; i < noOfWalkers; i++)
			{

				if (walkerArray[i].alive)
				{
					flag = false;
					//attempt to kill the walker. chance decreases per floor level
					bool result = walkerArray[i].stillAlive(chance,floorLevel);

					//if it was killed make the current room a treasure room
					if (result)
					{
						walkerArray[i].killX = walkerArray[i].x;
						walkerArray[i].killY = walkerArray[i].y;
					}
					else
					{
						dungeonRoomGrid[walkerArray[i].x + (mapSize - 1), walkerArray[i].y + (mapSize - 1)] = 1;
						walkerArray[i].move(mapSize);
					}

				}

			}
			if (flag)
			{ break; }

		}

		for (int i = 0; i < noOfWalkers; i++)
		{
			dungeonRoomGrid[walkerArray[i].killX + (mapSize - 1), walkerArray[i].killY + (mapSize - 1)] = 2;
		}

		//make the starter room a default room
		dungeonRoomGrid[(mapSize - 1),(mapSize - 1)] = 3;

		for (int i = 0; i < 9; i++)
		{ for (int j = 0; j < 9; j++)
			{
				//GD.PrintRaw(dungeonRoomGrid[i, j]);
			}
			//GD.Print("");
		}

		//POPULATE MAP WITH ROOMS

		RoomGen roomGen = new RoomGen();

		int[][] roomHolder = new int[11][]; //structure to hold data for room, room size 11
		for (int i=0;i< roomHolder.Length;i++)
		{
			roomHolder[i] = new int[11];
		}

		
		for (int i = 0; i < (mapSize * 2) + 1; i++)
		{
			for (int j = 0; j < (mapSize * 2) + 1; j++)
			{

				switch (dungeonRoomGrid[i, j])
				{
					case 0:
						break;
					//main
					case 1:
						roomHolder = roomGen.randomRoom(1);
						updateTileMapWRoom(ref roomHolder, i, j);
						break;
					//treasure
					case 2:
						roomHolder = roomGen.randomRoom(2);
						updateTileMapWRoom(ref roomHolder, i, j);
						break;
					case 3:
						roomHolder = roomGen.randomRoom(3);
						updateTileMapWRoom(ref roomHolder, i, j);
						break;
				}
			}
		}

		//CREATE DOORS

		//For each room check if there is room below it and to the right of it
		for (int i = 0; i < (mapSize * 2) + 1; i++)
		{
			for (int j = 0; j < (mapSize * 2) + 1; j++)
			{
				if (!(dungeonRoomGrid[i, j] == 0))
				{
					

					//carve out door
					if (!(dungeonRoomGrid[i+1, j] == 0))
					{

						//create a door width
						int rand = (int)Math.Round(GD.RandRange(0.5, 5.49));

						for (int k=rand; k<11-rand;k++)
						{
							tileArray[10 + 11 * i][k + 11 * j] = new AStarSharp.Node(new Vector2(10 + 11 * i, k + 11 * j), true);
							tileArray[11 + 11 * i][k + 11 * j] = new AStarSharp.Node(new Vector2(11 + 11 * i, k + 11 * j), true);
							SetCell(10 + 11 * i, k + 11 * j, 1);
							SetCell(11 + 11 * i, k + 11 * j, 1);

						}
					}

					if (!(dungeonRoomGrid[i, j+1] == 0))
					{
						//create a door width
						int rand = (int)Math.Round(GD.RandRange(0.5, 5.49));

						for (int k = rand; k < 11-rand; k++)
						{
							tileArray[k + 11 * i][10 + 11 * j] = new AStarSharp.Node(new Vector2(k + 11 * i, 10+ 11 * j), true);
							tileArray[k + 11 * i][11 + 11 * j] = new AStarSharp.Node(new Vector2(k + 11 * i, 11 + 11 * j), true);

							SetCell(k + 11 * i, 10 + 11 * j, 1);
							SetCell(k + 11 * i, 11 + 11 * j, 1);
						}
					}
				}
			}
		}

		pathfindAstar = new AStarSharp.Astar(tileArray);

		UpdateBitmaskRegion();
		for (int i = 0; i < 200; i++)
		{
			for (int j = 0; j < 200; j++)
			{
				if (tileArray[i][j].Walkable)
				{// GD.PrintRaw(0); 
				}
				else {//GD.PrintRaw(1); 
					}
			}
			//GD.Print("");
		}
		var scene = GD.Load<PackedScene>("res://CardAndSpawnManager.tscn");

		var node = scene.Instance();
		AddChild(node);
	}

	void updateTileMapWRoom(ref int[][] roomHolder,int roomX, int roomY)
	{
		//ROTATION

		int rand = (int)Math.Round(GD.RandRange(-0.5, 3.49));

		int x;
		int y;
		

		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				switch (rand)
				{
					case 0:
						x = i;
						y = j;
						break;
					case 1:
						x = j;
						y = 10-i;
						break;
					case 2:
						x = 10-j;
						y = 10-i;
						break;
					case 3:
						x = 10 - j;
						y = i;
						break;
					default:
						x = i;
						y = j;
						break;
				}


				if (roomHolder[x][y] != 1)
				{

					SetCell(i + 11 * roomX, j + 11 * roomY, 1);
					tileArray[i + 11 * roomX][j + 11 * roomY] = new AStarSharp.Node(new Vector2(i + 11 * roomX,j + 11 * roomY), true);

					if (roomHolder[x][y] == 2 || roomHolder[x][y]==3)
					{
						var scene = GD.Load<PackedScene>("res://Spawner.tscn");
						var node = scene.Instance();
						if (roomHolder[x][y]==2)
						{
							((Spawner)node).treasureRoomSpawner = false;
						}
						else
						{
							((Spawner)node).treasureRoomSpawner = true;
						}
						((Node2D)node).Position = new Vector2(16*(i + 11 * roomX), 16*(j + 11 * roomY));
						GetNode("../Spawners").AddChild(node);
					}
				}
			}
		}
	}

	Vector2 pathfind(Vector2 myPos,Vector2 otherPos)
	{
		Stack<AStarSharp.Node> temp=pathfindAstar.FindPath(new Vector2(myPos.x/16,myPos.y/16), new Vector2(otherPos.x,otherPos.y));

		return temp.Pop().Position;
	}

}

public class RoomGen
{
	public RoomGen()
	{
		mainRooms[0] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		mainRooms[1] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,2,0,1},
			new int[]{ 1,0,1,1,0,0,0,1,1,0,1},
			new int[]{ 1,0,1,1,0,0,0,1,1,0,1},
			new int[]{ 1,0,1,1,0,0,0,1,1,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,1,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,1,1},
			new int[]{ 1,0,0,2,0,0,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		mainRooms[2] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,2,0,1},
			new int[]{ 1,0,1,1,1,1,1,1,1,0,1},
			new int[]{ 1,0,1,1,1,1,1,1,1,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,1,1,1,1,1,1,1,0,1},
			new int[]{ 1,0,1,1,1,1,1,1,1,0,1},
			new int[]{ 1,0,0,2,0,0,0,0,0,0,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		

		mainRooms[3] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,2,0,1},
			new int[]{ 1,0,0,1,1,1,1,0,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,0,0,0,1},
			new int[]{ 1,0,0,1,1,2,0,0,0,0,1},
			new int[]{ 1,0,0,1,1,0,0,0,0,0,1},
			new int[]{ 1,0,0,1,1,0,0,1,1,1,1},
			new int[]{ 1,0,2,0,0,0,0,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		mainRooms[4] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,1,1,1,0,0,0,1,1,1,1},
			new int[]{ 1,1,1,1,0,2,0,1,1,1,1},
			new int[]{ 1,1,1,1,0,0,0,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,1,1,1,0,0,0,1,1,1,1},
			new int[]{ 1,1,1,1,0,0,0,1,1,1,1},
			new int[]{ 1,1,1,1,0,2,0,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
		};

		mainRooms[5] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,2,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,2,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,2,0,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
		};

		mainRooms[6] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,1,1,1,0,0,0,0,0,0,1},
			new int[]{ 1,1,1,1,0,0,0,2,0,0,1},
			new int[]{ 1,1,1,1,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,2,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
		};

		mainRooms[7] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,0,0,0,1,1,1},
			new int[]{ 1,1,1,1,1,0,0,0,1,1,1},
			new int[]{ 1,1,1,1,1,0,0,0,0,0,1},
			new int[]{ 1,0,2,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,2,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,2,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
		};

		mainRooms[8] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,0,0,0,0,0,1},
			new int[]{ 1,1,1,1,1,0,2,0,0,0,1},
			new int[]{ 1,1,1,1,1,1,1,1,0,0,1},
			new int[]{ 1,0,1,1,1,1,1,1,0,0,1},
			new int[]{ 1,0,1,1,0,0,0,0,0,2,1},
			new int[]{ 1,2,1,1,0,0,0,0,0,0,1},
			new int[]{ 1,0,1,1,0,0,0,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
		};

		mainRooms[9] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,2,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,0,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,0,2,0,1},
			new int[]{ 1,0,0,1,1,1,1,0,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,2,0,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,1,1,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
		};

		mainRooms[10] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,1,1,0,0,0,0,0,0,0,1},
			new int[]{ 1,1,1,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,1,1,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		mainRooms[11] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,1,1,1,0,0,0,1},
			new int[]{ 1,0,2,0,1,1,1,0,0,0,1},
			new int[]{ 1,0,0,0,1,1,1,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,2,0,0,0,0,1},
			new int[]{ 1,0,0,0,0,0,0,0,0,0,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		treasureRooms[0] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,0,0,0,0,3,0,0,0,0,1},
			new int[]{ 1,0,3,0,0,0,0,0,0,0,1},
			new int[]{ 1,0,0,1,1,0,1,1,3,0,1},
			new int[]{ 1,0,0,1,1,0,1,1,0,0,1},
			new int[]{ 1,3,0,0,0,3,0,0,0,3,1},
			new int[]{ 1,0,0,1,1,0,1,1,0,0,1},
			new int[]{ 1,0,0,1,1,0,1,1,0,0,1},
			new int[]{ 1,0,3,0,0,0,0,0,3,0,1},
			new int[]{ 1,0,0,0,0,3,0,0,0,0,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		treasureRooms[1] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,3,0,0,0,3,0,0,0,3,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,0,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,0,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,0,1},
			new int[]{ 1,3,0,0,0,3,0,0,0,3,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,0,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,0,1},
			new int[]{ 1,0,1,1,1,0,1,1,1,0,1},
			new int[]{ 1,3,0,0,0,3,0,0,0,3,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};

		treasureRooms[2] = new int[][]{
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1},
			new int[]{ 1,3,0,0,0,0,0,0,0,3,1},
			new int[]{ 1,0,0,0,0,3,0,0,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,1,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,1,0,0,1},
			new int[]{ 1,0,3,1,1,3,0,0,3,0,1},
			new int[]{ 1,0,0,1,1,1,1,1,0,0,1},
			new int[]{ 1,0,0,1,1,1,1,1,0,0,1},
			new int[]{ 1,0,0,0,0,3,0,0,0,0,1},
			new int[]{ 1,3,0,0,0,0,0,0,0,3,1},
			new int[]{ 1,1,1,1,1,1,1,1,1,1,1}
		};
	}

	public int[][] randomRoom(int roomType)
	{
		int rand;

		switch (roomType)
		{
			case 1:
				rand= (int)Math.Round(GD.RandRange(-0.5, 11.49));
				//GD.Print("room" + rand);
				return mainRooms[rand];

			case 2:
				rand = (int)Math.Round(GD.RandRange(-0.5, 2.49));
				return treasureRooms[rand];

			case 3:
				return mainRooms[0];
			default:
				return mainRooms[0];
		}
	}

	int[][][] mainRooms = new int[12][][];
	
	int[][][] treasureRooms = new int[4][][];

}


public class Walker
{
	[Export]
	int minimumLife = 3;
	int lifeLength = 0;
	public int x=0;
	public int y=0;
	public int killX;
	public int killY;
	public bool alive=true;
	public void move(int mapSize)
	{
		Restart:
		GD.Randomize();
		int rand=(int)Math.Round(GD.RandRange(-0.5f,3.49f));
		//GD.Print("direction: "+rand);
		//disgusting code, clean later if there's time
		switch(rand)
		{
			case 0:
				if (x+1>=mapSize)
				{
					goto Restart;
				}
				x++;
				break;
			case 1:
				if (x-1<=mapSize-(2*mapSize))
				{
					goto Restart;
				}
				x--;
				break;
			case 2:
				if (y+1>=mapSize)
				{
					goto Restart;
				}
				y++;
				break;
			case 3:
				if (y-1<=mapSize-(2*mapSize))
				{
					goto Restart;
				}
				y--;
				break;	
		}
	}

	public bool stillAlive(float chance, int floorLevel)
	{
		lifeLength += 1;
		if (lifeLength > minimumLife)
		{
			double rand = GD.RandRange(-0.5, 100.49);
			GD.Print("chance:" + (chance + ((lifeLength - minimumLife-1) * Math.Max(0, 30 - floorLevel*2.6f))));
			if (rand < (chance + ((lifeLength - minimumLife - 1) * Math.Max(0, 30 - (floorLevel * 2)))))
			{
				//GD.Print("kill location:" + x + " " + y);
				alive = false;
				return true;
			}
		}
		return false;
	}
	
	
}



