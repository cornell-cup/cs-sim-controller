using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

    public void OnMouseDown()
    {
        print("Brah\n\n\n\n\n");
        if (Application.loadedLevelName == "FullGyro")
        {
            Application.LoadLevel("GyroButtons");
        }
        if (Application.loadedLevelName == "GyroButtons")
        {
            Application.LoadLevel("FullGyro");
        }
    }
}
