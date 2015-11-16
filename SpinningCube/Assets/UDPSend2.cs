using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend2 : MonoBehaviour
{
    // prefs
    private string IP;  // define in GUI
    public string playerName; // define in GUI
    public string playerIP; // define in init
    public int port;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    private string direction = "stopped";

    public void Start()
    {
        init();
    }


    // init
    public void init()
    {
        print("UDPSend.init()");
        port = 8051;
        IP = "127.0.0.1";
        IPAddress[] addrs = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        foreach (var ip in addrs)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                playerIP = ip.ToString();
            }
        }
        
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
        
        // status
        print("Sending to " + IP + " : " + port);
        print("Player IP is " + playerIP);

    }

    // sendData
    public void sendString()
    {
        try
        {
            string message;
            if (direction == "forward")
            {
                message = Input.acceleration.x + ",0,-1,";
            }
            else if (direction == "backward")
            {
                message = Input.acceleration.x + ",0,1,";
            }
            else
            {
                message = Input.acceleration.x + ",0,0,";
            }
            message += playerIP;
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