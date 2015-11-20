using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // public
    public int port;
    private IPEndPoint localEp;
    private bool started = false;
    private Texture2D tex;

    // infos
    public byte[] lastReceivedUDPPacket;

    public void Start()
    {
        init();
    }

    // init
    private void init()
    {
        print("UDPReceive.init()");

        // define port
        port = 8050;

        // status
        print("Listening on everywhere : " + port);

        client = new UdpClient(port);
        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);

        tex = new Texture2D(100, 50);


        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref localEp);
                started = true;
                lastReceivedUDPPacket = data;          
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private byte[] getLast()
    {
        return lastReceivedUDPPacket;
    }

    public void Update()
    {
        if (started)
        {
            tex.LoadImage(getLast());
            GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
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