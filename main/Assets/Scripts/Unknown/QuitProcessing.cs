using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ViveHandTracking {
public class ON : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GestureProvider.Current.enabled = true;  // enable the script to start the detection
    }

    // Update is called once per frame
        private void OnApplicationQuit()
        {
            GestureProvider.Current.enabled = false;  // enable the script to start the detection
        }
    }

}
