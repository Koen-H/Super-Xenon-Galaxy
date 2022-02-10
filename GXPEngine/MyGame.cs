using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{
	private Level level;
	private HUD hud;

	public MyGame() : base(1920, 1080, false, true, 960, 540)		// Create a window that's 800x600 and NOT fullscreen
	{
		level = new Level("Map.tmx");
		hud = new HUD();

		AddChild(level);
		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{
        level.Update();
    }

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}