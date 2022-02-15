using System.IO.Ports;
using System;
using GXPEngine;

public class ArduinoController
{
    static SerialPort port = null;
    private String[] Ports = SerialPort.GetPortNames();
    private bool found = false;
    private String _message;
    private static int _parameterSize = 8;//number of parameters, i think 8 is plenty
    private String[] _parameters = new String[_parameterSize];

    public float analogRotation;
    public float analogForce;


    public ArduinoController()
    {
        Console.WriteLine("Connecting with controller...");
        Initialise();
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
        double yPos = Math.Ceiling(GetFloatParameter(1) / 10)*-1;
        if (xPos == 0) xPos = 1;
        if (yPos == 0) yPos = 1;
        double force = Math.Sqrt((xPos * xPos) + (yPos * yPos));
       
        if(force > 50)//Easy fix
        {
            force = 50;
        }
        analogForce = (float)force;
        if (analogForce > 10)
        {
            analogRotation = (float) Math.Ceiling(((Math.PI + Math.Atan2(yPos ,- xPos)) * 180 / Math.PI));
            analogRotation += 90;
            //Console.WriteLine(xPos + ", " + yPos + ", " + analogRotation);


        }
    }


    ~ArduinoController()
    {
        port.Close();
    }
}