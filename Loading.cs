using Godot;
using System;

public class Loading : Label
{
	int dots = 1;


	private void _on_Timer_timeout()
	{
		dots++;
		dots = dots % 3;
		//please never write code like this
		switch (dots)
		{
			case 0:
				Text = "Loading.";
				break;
			case 1:
				Text = "Loading..";
				break;
			case 2:
				Text = "Loading...";
				break;
		}
		((Timer)GetNode("Timer")).WaitTime = 0.35f;
		((Timer)GetNode("Timer")).Start();

	}



}

