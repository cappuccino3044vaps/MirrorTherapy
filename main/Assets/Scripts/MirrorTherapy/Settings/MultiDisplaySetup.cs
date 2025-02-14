using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDisplaySetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connected Displays: " + Display.displays.Length);
        // ディスプレイを有効化
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

}
