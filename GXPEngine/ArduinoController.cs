using System.IO.Ports;
using System;
using GXPEngine;
using System.Collections.Generic;

public class ArduinoController
{
    static SerialPort port = null;
    private String[] Ports = SerialPort.GetPortNames();
    private bool found = false;
    private String _message;
    private static int _parameterSize = 8;//number of parameters, i think 8 is plenty
    private String[] _parameters = new String[_parameterSize];

    private List<LightAnimationData> lightAnimation = new List<LightAnimationData>();
    public Boolean playLightAnimation = true;
    private int currentLightAnimation = 0;
    private float lightAnimationInterval;

    public float analogRotation;
    public float analogForce;


    public ArduinoController()
    {
        Console.WriteLine("Connecting with controller...");
        
        CreateLightAnimation();
        Initialise();
        SendString("LED_SPACE_ON");
        SendString("COLORS_OFF");
        Console.WriteLine("Done.");
    }

    private void Initialise()
    {
        //initialize parameter array
        Search();
    }

    private void Search()
    {
        port = null;

        for (int i = 0; i < _parameterSize; i++)
        {
            _parameters[i] = "0";
        }

        while (!found)
        {
            Ports = SerialPort.GetPortNames();
            foreach (String portName in Ports)
            {
                port = new SerialPort(portName);

                port.BaudRate = 9600;
                port.ReadTimeout = 1000000;

                port.RtsEnable = false;
                port.DtrEnable = false;

                if (port.IsOpen)
                {
                    port.Close();
                    try { port.Open(); }
                    catch (System.IO.IOException e) { continue; }
                }
                else
                {
                    try { port.Open(); }
                    catch (System.IO.IOException e) { continue; }
                }

                port.DiscardOutBuffer();
                port.DiscardInBuffer();

                SendString("CONNECT CONTROLLER");

                String Accept = null;
                bool accepted = false;

                while (!accepted)
                    try
                    {
                        Accept = port.ReadLine();
                        accepted = true;
                    }
                    catch (TimeoutException e)
                    {
                        Console.WriteLine("rip");
                    }

                Console.WriteLine(Accept);
                if (Accept.Contains("CONNECTED"))
                {
                    found = true;
                    port.Write("CONTROLLER SUCCESFULLY CONNECTED");
                    break;
                }

                port.DiscardInBuffer();
            }
        }

        port.DiscardInBuffer();

        System.Console.WriteLine("Initializing port");
    }

    public String GetMessage()
    {
        getString();
        return _message;
    }

    /// <summary>
    /// Returns a string representing the specific parameter from the Arduino
    /// </summary>
    /// <param name="parameter">
    /// Specify which parameter to return</param>
    /// <returns>String</returns>
    public String GetStringParameter(int parameter)
    {
        getString();
        return _parameters[parameter];
    }

    /// <summary>
    /// Returns an integer representing the specific parameter from the Arduino
    /// </summary>
    /// <param name="parameter">
    /// Specify which parameter to return</param>
    /// <returns>Int</returns>
    public int GetIntParameter(int parameter)
    {
        getString();
        return Int32.Parse(_parameters[parameter]);
    }

    /// <summary>
    /// Returns a float representing the specific parameter from the Arduino
    /// </summary>
    /// <param name="parameter">
    /// Specify which parameter to return</param>
    /// <returns>float</returns>
    public float GetFloatParameter(int parameter)
    {
        getString();
        return float.Parse(_parameters[parameter]);
    }

    public void SendString(string send)//Send a message to the controller
    {
        if (port.IsOpen)
            try
            {
                port.Write(send);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("is non existent");
                found = false;
                Search();
                SendString(send);
            }
        else
        {
            Console.WriteLine("is closed");
            found = false;
            Search();
            SendString(send);
        }
    }

    private void getString()
    {
        SendString("SEND");

        try { _message = port.ReadLine(); }
        catch (System.IO.IOException e) { _message = ""; }

        _message.Trim();
        _parameters = _message.Split(' ');
    }

    public void AnalogStick()
    {
        double xPos = Math.Ceiling(GetFloatParameter(0) / 10);
        double yPos = Math.Ceiling(GetFloatParameter(1) / 10) * -1;
        if (xPos == 0) xPos = 1;
        if (yPos == 0) yPos = 1;
        double force = Math.Sqrt((xPos * xPos) + (yPos * yPos));

        if (force > 50)//Easy fix
        {
            force = 50;
        }
        analogForce = (float)force;
        if (analogForce > 10)
        {
            analogRotation = (float)Math.Ceiling(((Math.PI + Math.Atan2(yPos, -xPos)) * 180 / Math.PI));

            //Console.WriteLine(xPos + ", " + yPos + ", " + analogRotation);


        }
    }

    public void ChangeLight(ObjectColor currentColor)
    {
        SendString("COLORS_OFF");
        switch (currentColor)
        {

            case ObjectColor.CYAN:
                {
                    SendString("LED_ONE_ON");
                    break;
                }
            case ObjectColor.ORANGE:
                {
                    SendString("LED_TWO_ON");
                    break;
                }
            case ObjectColor.PINK:
                {
                    SendString("LED_THREE_ON");
                    break;
                }
            case ObjectColor.PURPLE:
                {
                    SendString("LED_FOUR_ON");
                    break;
                }
        }
    }

    public void CreateLightAnimation() { 
    
        // cyan. orange, pink, purple, white, delay after the previous one happend;
        lightAnimation.Add(new LightAnimationData( false, false, false, false, false, 0 ));
        lightAnimation.Add(new LightAnimationData(true, false, false, false, false, 500));
        lightAnimation.Add(new LightAnimationData(false, false, true, false, false, 500));
        lightAnimation.Add(new LightAnimationData( false, false, false, true, false, 500 ));
        lightAnimation.Add(new LightAnimationData(false, true, false, false, false, 500)); // lightAnimation.Add(new int[] { 0, 1, 0, 0, 0, 2000 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, true, 500)); //lightAnimation.Add(new int[] { 0, 0, 0, 0, 1, 2500 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, false, 500)); //lightAnimation.Add(new int[] { 0, 0, 0, 0, 0, 3000 });
        lightAnimation.Add(new LightAnimationData(true, true, true, true, true, 500)); //lightAnimation.Add(new int[] { 1, 1, 1, 1, 1, 3500 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, false, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 0, 4000 });
        lightAnimation.Add(new LightAnimationData(true, true, true, true, true, 500));//lightAnimation.Add(new int[] { 1, 1, 1, 1, 1, 4500 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, false, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 0, 5000 });
        lightAnimation.Add(new LightAnimationData(true, false, false, true, false, 500));//lightAnimation.Add(new int[] { 1, 0, 0, 1, 0, 5500 });
        lightAnimation.Add(new LightAnimationData(false, true, true, false, false, 500));//lightAnimation.Add(new int[] { 0, 1, 1, 0, 0, 6000 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, true, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 1, 6500 });
        lightAnimation.Add(new LightAnimationData(true, false, true, false, false, 500));//lightAnimation.Add(new int[] { 1, 0, 1, 0, 0, 7000 });
        lightAnimation.Add(new LightAnimationData(false, true, false, true, false, 500));//lightAnimation.Add(new int[] { 0, 1, 0, 1, 0, 7500 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, true, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 1, 8000 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, false, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 0, 8500 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, true, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 1, 9000 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, false, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 0, 9500 });
        lightAnimation.Add(new LightAnimationData(false, false, false, false, true, 500));//lightAnimation.Add(new int[] { 0, 0, 0, 0, 1, 10000 });
        Console.WriteLine(lightAnimation.Count);
    }
    

    public void Update()
    {
        if (Time.time > lightAnimationInterval && playLightAnimation) {//Change to next light layout
            LightAnimationData currentData = lightAnimation[currentLightAnimation++];
            SetLights(currentData);
            lightAnimationInterval = currentData.delay + Time.time;
            if (currentLightAnimation == lightAnimation.Count) currentLightAnimation = 0;
        }
        
    }

    private void SetLights(LightAnimationData lightData)
    {
        SendString("COLORS_OFF");
        //

        if (lightData.cyan)
        {
            SendString("LED_ONE_ON");
        }
        if (lightData.orange)
        {
            SendString("LED_TWO_ON");
        }
        if (lightData.pink)
        {
            SendString("LED_THREE_ON");
        }
        if (lightData.purple)
        {
            SendString("LED_FOUR_ON");
        }
        if (lightData.space)
        {
            SendString("LED_SPACE_ON");
        }
        else
        {
            SendString("LED_SPACE_OFF");
        }
    }

    ~ArduinoController()
    {
        port.Close();
    }
}