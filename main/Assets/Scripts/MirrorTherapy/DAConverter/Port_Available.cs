using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace MirrorTherapy.DAConverter
{
public class Port_Available : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log("Available ports:");
        foreach (string port in ports)
        {
            Debug.Log(port);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}