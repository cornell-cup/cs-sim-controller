using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour
{
    private static int localPort;

    // prefs
    private string IP;  // define in init
    public int port;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // gui
    string strMessage = "";

    // start from unity3d
    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        print("Before init");
        init();
    }

    // init
    public void init()
    {
        print("UDPSend.init()");

        // define
        IP = "192.168.1.90";
        port = 8051;
        
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        // status
        print("Sending to " + IP + " : " + port);
        print("Testing: nc -lu " + IP + " : " + port);

    }

    // sendData
    private void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }


    // endless test
    void Update()
    {
        GetComponent<TextMesh>().text = Convert.ToString(Input.acceleration.x).Substring(0, 4) + ",0," + Convert.ToString(Input.acceleration.z).Substring(0, 4);
        print(Input.acceleration.x + ",0," + Input.acceleration.z);
        sendString(Input.acceleration.x + ",0," + Input.acceleration.z);

    }

    public void OnApplicationQuit()
    {
        client.Close();
        print("UDPClient closed");
    }

}