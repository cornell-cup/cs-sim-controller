using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // public
    public int port;
    public bool started = false;

    // infos
    public Vector3 lastReceivedUDPPacket;

    public void Start()
    {
        init();
    }

    // init
    private void init()
    {
        print("UDPReceive.init()");

        // define port
        port = 8051;

        // status
        print("Listening on everywhere : " + port);


        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
        started = true;

        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref localEp);
                string text = Encoding.UTF8.GetString(data);
                string[] message = text.Split(new char[] { ',' });
                Vector3 result = new Vector3(float.Parse(message[0]), float.Parse(message[1]), float.Parse(message[2]));
                string playerIP = message[3];

                print(">> " + result + ", " + playerIP);

                lastReceivedUDPPacket = result;


            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // getLatestUDPPacket
    // cleans up the rest
    public Vector3 getLatestUDPPacket()
    {
        return lastReceivedUDPPacket;
    }

    public void OnApplicationQuit()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
            client.Close();
        }
    }

}