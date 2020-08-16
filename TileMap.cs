using Godot;
using System;

public class TileMap : Godot.TileMap
{
	[Export]
	public int floorLevel=1;
	[Export]
	int mapSize=5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		

		int[,] dungeonRoomGrid = new int[mapSize*2+1,mapSize*2+1];
		
		int noOfWalkers=(int)Math.Floor(floorLevel /3f)+3;
		GD.Print(noOfWalkers);
		GD.Print((int)((1f/(floorLevel+3))*100));
		Walker[] walkerArray=new Walker[noOfWalkers];
		
		for (int i = 0; i < walkerArray.Length; i++) { walkerArray[i] = new Walker(); }

		while(true)
		{
		bool flag=true;	
		
		for(int i=0; i<noOfWalkers;i++)
		{
			
			if(walkerArray[i].alive)
			{
				flag=false;
				//attempt to kill the walker. chance decreases per floor level
				bool result=walkerArray[i].stillAlive((int)((1f/(floorLevel+2))*100));
				
				//if it was killed make the current room a treasure room
				if(result)
				{
					walkerArray[i].killX=walkerArray[i].x;
					walkerArray[i].killY=walkerArray[i].y;
				}
				else
				{
					dungeonRoomGrid[walkerArray[i].x+(mapSize-1), walkerArray[i].y+ (mapSize-1)] =1;
					walkerArray[i].move(mapSize);
				}

			}
			
		}
		if(flag)
		{break;}

		}
		
		for (int i=0;i<noOfWalkers;i++)
		{
			dungeonRoomGrid[walkerArray[i].killX + (mapSize - 1), walkerArray[i].killY + (mapSize - 1)] = 2;
		}
		
		for(int i=0;i<9;i++)
		{for(int j=0;j<9;j++)
		{
			GD.PrintRaw(dungeonRoomGrid[i,j]);
		}
			GD.Print("");
		}
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

public class Walker
{
	
	public int x=0;
	public int y=0;
	public int killX;
	public int killY;
	public bool alive=true;
	public void move(int mapSize)
	{
		Restart:
		GD.Randomize();
		int rand=(int)Math.Round(GD.RandRange(0,3));
		GD.Print("direction: "+rand);
		//disgusting code, clean later if there's time
		switch(rand)
		{
			case 0:
				if (x+1>mapSize)
				{
					goto Restart;
				}
				x++;
				break;
			case 1:
				if (x-1<mapSize-(2*mapSize))
				{
					goto Restart;
				}
				x--;
				break;
			case 2:
				if (y+1>mapSize)
				{
					goto Restart;
				}
				y++;
				break;
			case 3:
				if (y-1<mapSize-(2*mapSize))
				{
					goto Restart;
				}
				y--;
				break;	
		}
	}
	
	public bool stillAlive(int chance)
	{
		int rand=(int) GD.RandRange(0,100);
		GD.Print("chance:" + rand);
		if (chance>rand)
		{
			GD.Print("kill location:" + x +" "+ y);
			alive=false;
			return true;
		}
		return false;
	}
	
}
