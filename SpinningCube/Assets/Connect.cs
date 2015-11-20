using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Connect : MonoBehaviour {

    private Button connect;

	// Use this for initialization
	void Start () {
        connect = GetComponent<Button>();
        connect.onClick.AddListener(() =>
        {
            string IP = GameObject.FindGameObjectWithTag("ConnectToIP").GetComponent<Text>().text;
            string name = GameObject.FindGameObjectWithTag("ConnectToName").GetComponent<Text>().text;
            if (IP != "" && name != "") {
                UDPSend2 script = GameObject.FindObjectOfType(typeof(UDPSend2)) as UDPSend2;
                script.init(IP, name);
            }
            else
            {
                print(IP);
                print(name);
                GameObject.FindGameObjectWithTag("ButtonText").GetComponent<Text>().text = "Please fill both fields";
            }
        }
        );
	}
}
