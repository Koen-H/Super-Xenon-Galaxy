using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

public class MyGame : Game
{
	private GameManager gameManager;
    public ArduinoController gameController;
    private Boolean useController = false;//DISABLE FOR KEYBOARD


    public MyGame() : base(1920, 1080, false, false, 960, 540)		// Create a window that's 800x600 and NOT fullscreen
	{
        if (useController)
        {
            gameController = new ArduinoController();
            //gameController.SendString("LED_SPACE_ON");
        }
        string[] lines =
        {
            "First line", "Second line", "Third line"
        };

        File.WriteAllLines("Leaderboard", lines);
        gameManager = new GameManager();
		AddChild(gameManager);
		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{

		gameManager.Update();
        if (gameController != null)
        {
                //gameController.Update();
                //gameController.AnalogStick();
        }

        //Console.WriteLine(GetDiagnostics());
    }


    static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
        
    }
}