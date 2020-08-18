using Godot;
using System;

public class MotionModule : Node
{

    [Export]
    float moveTime = 0.15f;
    bool motionFlag = false;
    float motionPercentage = 0;
    Vector2 newPos;
    void motion(float delta, Vector2 PrevPos, Vector2 velocity)
    {
        if (!(velocity.x==0&& velocity.y==0)|| motionFlag == true)
        {
            if (motionPercentage >= 1)
            {
                //GD.Print(newPos);
                ((Node2D)GetParent()).Position = newPos;
               // GD.Print(((Node2D)GetParent()).Position);
                motionPercentage = 0;
                motionFlag = false;
                return;
            }
            if (motionPercentage == 0)
            {
                newPos = PrevPos + 16 * velocity;
                if (GetParent().Name == "Player")
                {
                    GetParent().EmitSignal(nameof(Player.PlayerMotion), PrevPos / 16, newPos / 16);
                }
                motionFlag = true;
                //GD.Print(PrevPos);
                //GD.Print(newPos);
                //GD.Print("");
            }
            motionPercentage += delta / moveTime;
            
   
            ((Node2D)GetParent()).Position = (((1 - ease(motionPercentage)) * PrevPos) + (newPos * ease(motionPercentage)));

            ((Sprite)GetParent().FindNode("Sprite")).Position = new Vector2(0, 20 * (motionPercentage * motionPercentage - motionPercentage));
        }
    }

    void idle(float delta, Vector2 Pos)
    {
        int rand = (int)Math.Round(GD.RandRange(0, 3));
        Vector2 velocity=new Vector2(0,0);
        switch (rand)
        {
            case 0:
                velocity.x = 1;
                break;
            case 1:
                velocity.y = 1;
                break;
            case 2:
                velocity.x = -1;
                break;
            case 3:
                velocity.y = -1;
                break;    
        }
        TileMap x = (TileMap)GetParent().GetParent().GetParent().FindNode("TileMap");
        if (x.GetCellv((Pos / 16) + velocity) == 1)
        {
            motion(delta, Pos, velocity);
        }
    }

    float ease(float x)
    {

        return (float)-(Math.Cos(Math.PI * x) - 1) / 2;

    }
}
