using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

    public void OnMouseDown()
    {
        string direction = GameObject.FindGameObjectWithTag("Game").GetComponent<UDPSend2>().direction;
        //if (Application.loadedLevelName == "FullGyro")
        //{
        //    Application.LoadLevel("GyroButtons");
        //}
        //if (Application.loadedLevelName == "GyroButtons")
        //{
        //    Application.LoadLevel("FullGyro");
        //}
    }
}
