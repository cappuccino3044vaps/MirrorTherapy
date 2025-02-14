using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUISetup : MonoBehaviour
{
    public Camera hmdCamera; // HMD用カメラ
    public Camera pcCamera;  // PC用カメラ
    public Canvas hmdCanvas; // HMD用Canvas
    public Canvas pcCanvas;  // PC用Canvas

    void Start()
    {
        // HMD用Canvasのカメラを設定
        if (hmdCanvas != null)
        {
            hmdCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            hmdCanvas.worldCamera = hmdCamera;
        }

        // PC用Canvasのカメラを設定
        if (pcCanvas != null)
        {
            pcCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            pcCanvas.worldCamera = pcCamera;
        }
    }
}
