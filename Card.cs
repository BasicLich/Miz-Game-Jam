using Godot;
using System;

public class Card : Node
{
    public Card(int s,int r,bool trump)
    {
        int trumpBoost = 0;
        suit = s;
        rank = r;
        if (trump)
        {
            trumpBoost = 1000;
        }
        value = r + trumpBoost;
    }

    public void Print()
    {
        GD.Print("Suit: " + suit + " Rank: " + rank+ " Value:"+ value);
    }

    public int value;
    public int suit;
    public int rank;
}
