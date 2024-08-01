using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam_Reciever : MonoBehaviour
{
    private static int INPUT_SIZE = 256;
    private static int FPS = 30;
    // UI
    RawImage rawImage;
    WebCamTexture webCamTexture;
    int selectCamera = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.rawImage = GetComponent<RawImage>();
        this.webCamTexture = new WebCamTexture(INPUT_SIZE,INPUT_SIZE, FPS);
        this.rawImage.texture = this.webCamTexture;
        this.webCamTexture.Play();
    }

    public void ChangeCamera()
    {
        // カメラの取得
        WebCamDevice[] webCamDevice = WebCamTexture.devices;

        // カメラが1個の時は無処理
        if (webCamDevice.Length <= 1) return;

        // カメラの切り替え
        selectCamera++;
        if (selectCamera >= webCamDevice.Length) selectCamera = 0;
        this.webCamTexture.Stop();
        this.webCamTexture = new WebCamTexture(webCamDevice[selectCamera].name,
            INPUT_SIZE, INPUT_SIZE, FPS);
        this.rawImage.texture = this.webCamTexture;
        this.webCamTexture.Play();
    }
}
