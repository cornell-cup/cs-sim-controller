using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend2 : MonoBehaviour
{
    private static int localPort;

    // prefs
    private string IP;  // define in init
    public int port;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    IPEndPoint groupEndPoint;
    UdpClient client;

    private string direction = "stopped";

    // start from unity3d
    public void Start()
    {
        print("Before init");
        init();
    }

    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define
        IP = "192.168.1.90";
        port = 8051;

        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        groupEndPoint = new IPEndPoint(IPAddress.Broadcast, port);

        client = new UdpClient();

        IPAddress multicastaddress = IPAddress.Parse("224.0.0.1");
        client.JoinMulticastGroup(multicastaddress);
        remoteEndPoint = new IPEndPoint(multicastaddress, port);
        

        // status
        print("Sending to " + IP + " : " + port);
        print("Testing: nc -lu " + IP + " : " + port);

    }

    // sendData
    public void sendString()
    {
        try
        {
            string message;
            if (direction == "forward")
            {
                message = Input.acceleration.x + ",0,-1";
            }
            else if (direction == "backward")
            {
                message = Input.acceleration.x + ",0,1";
            }
            else
            {
                message = Input.acceleration.x + ",0,0";
            }
            byte[] data = Encoding.UTF8.GetBytes(message);
            print("Sending " + message);
            client.Send(data, data.Length, remoteEndPoint); 
            //}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    void Update()
    {
        sendString();
    }

    public void setDirection(string dir)
    {
        direction = dir;
    }

    public void OnApplicationQuit()
    {
        client.Close();
        print("UDPClient closed");
    }

}