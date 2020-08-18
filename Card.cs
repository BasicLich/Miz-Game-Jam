using Godot;
using System;

public class Card : Node
{
    public Card(int s,int r)
    {
        suit = s;
        rank = r;
    }

    public void Print()
    {
        GD.Print("Suit: " + suit + " Rank: " + rank);
    }

    public int suit;
    public int rank;
}
