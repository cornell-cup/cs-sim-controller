using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class UDPSend2 : MonoBehaviour
{
    // prefs
    public string playerName; // define in GUI
    public string playerIP; // define in init
    public int port;  // define in init
    public bool connected;

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    public string direction = "stopped";

    void Start()
    {
        GameObject.FindGameObjectWithTag("Game").GetComponent<Canvas>().enabled = false;
    }

    // init
    public void init(string targetIP, string name)
    {
        playerName = name;
        GameObject.FindGameObjectWithTag("Game").GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("GUI").GetComponent<Canvas>().enabled = false;
        print("UDPSend.init()");
        port = 8051;
        IPAddress[] addrs = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        foreach (var ip in addrs)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                playerIP = ip.ToString();
            }
        }
        
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(targetIP), port);
        client = new UdpClient();
        connected = true;

        // status
        print("Sending to " + targetIP + " : " + port);
        print("Player IP is " + playerIP);

    }

    public void changeControls()
    {
        print("Clicked");
        if (direction == "stopped" || direction == "backward" || direction == "forward")
        {
            print("Switching to full");
            direction = "none";
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Buttons"))
            {
                g.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            print("Switching from full");
            direction = "stopped";
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Buttons"))
            {
                g.GetComponent<Image>().enabled = true;
            }
        }
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
            else if (direction == "stopped")
            {
                message = Input.acceleration.x + ",0,0,";
            }
            else
            {
                message = Input.acceleration.x + ",0," + Input.acceleration.z + ",";
            }
            message += playerIP + "," + playerName;
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
        if (connected)
        {
            sendString();
        }
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