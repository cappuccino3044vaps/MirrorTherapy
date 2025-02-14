using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace MirrorTherapy.DAconverter
{
public class Rs232c_test : MonoBehaviour
{
    SerialPort serialPort;
    float count;
    int i=1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        serialPort = new SerialPort("COM3", 9600);
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if(serialPort.IsOpen)
        {
            if(count >= 5*i && count < 30)
            {
                Debug.Log("sousin");
                string dataToSend = "High";
                serialPort.WriteLine(dataToSend);
                serialPort.Close();
                i+=1;
            }
            else if(count >= 30)
            {
                Debug.Log("owari");
                string dataToSend = "Finish";
                serialPort.WriteLine(dataToSend);
                serialPort.Close();
            }
        }

        else
        {
            serialPort.Open();
        }
    }
}
}