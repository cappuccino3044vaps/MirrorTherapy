using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

//Openのとこ後で治す
//rs232c使うときEdit-Player-Netframeworkになっているか確認
namespace MirrorTherapy.DAConverter
{
public class Rs232c : MonoBehaviour
{

    private SerialPort serialPort;
    [SerializeField] private string portName = "COM3";

    // Start is called before the first frame update
    void Start()
    {
        
        if(serialPort != null)
        {
            serialPort.Close();
        }
        if(serialPort == null)
        {
            Debug.Log("start");
            serialPort = new SerialPort(portName, 9600);
            //ここも後で消す
            /*
            try
            {
                serialPort.Open();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to open serial port: " + ex.Message);
            }
            */
        }

    }

    public void SendToStart()
    {
        //後で治す
        serialPort.Open();
        Debug.Log("send to raspberry pi \"High\"");
        string dataToSend = "High";
        try
        {
            serialPort.WriteLine(dataToSend);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to write to serial port: " + ex.Message);
        }
        finally
        {
            serialPort.Close();
        }
    }
    public void SendToEnd()
    {
        Debug.Log("owari");
        string dataToSend = "Finish";
        try
        {
            serialPort.WriteLine(dataToSend);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to write to serial port: " + ex.Message);
        }
        finally
        {
            serialPort.Close();
        }
    }
}
}