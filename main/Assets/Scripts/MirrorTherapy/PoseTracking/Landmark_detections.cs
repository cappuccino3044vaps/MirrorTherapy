using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.CoordinateSystem;
using Stopwatch = System.Diagnostics.Stopwatch;
using Mediapipe.Unity;
using Mediapipe;
using System.Threading.Tasks;
namespace MirrorTherapy.PoseTracking
{
///<Summary>
/// MediaPipeApiとデータの受け渡しをするクラスです。
///</Summary>
public class Landmark_detections : MonoBehaviour
{

    [Tooltip("使用するMediaPipeApi_configファイルをセットしてください")]
    [SerializeField] private TextAsset _configAsset;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _fps;

    protected CalculatorGraph _graph;
    private OutputStream<NormalizedLandmarkList> _PoseLandmarksStream;
    private IResourceManager _resourceManager;
    private WebCamTexture _webCamTexture;

    private Texture2D _inputTexture;
    private Color32[] _inputPixelData;

    [SerializeField]
    private LandmarkManager landmarkManager = null;
    
    #region 各種フォーマットの準備・確認
    private async UniTaskVoid Start()
    {

      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }

       var webCamDevice = WebCamTexture.devices[0];
      _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
      _webCamTexture.Play();
      
      await UniTask.WaitUntil(() => _webCamTexture.width > 16);

      _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
      _inputPixelData = new Color32[_width * _height];

      _resourceManager = new LocalResourceManager();
      await _resourceManager.PrepareAssetAsync("pose_detection.bytes");
      await _resourceManager.PrepareAssetAsync("pose_landmark_full.bytes");
      
      var stopwatch = new Stopwatch();
      _graph = new CalculatorGraph(_configAsset.text);
      //_PoseLandmarksStream = new OutputStream<NormalizedLandmarkList>(_graph, "pose_landmarks");
      _PoseLandmarksStream = new OutputStream<NormalizedLandmarkList>(_graph, "pose_world_landmarks");
    
      _PoseLandmarksStream.StartPolling();
      _graph.StartRun();
      stopwatch.Start();
    
      await ProcessLandmarksAsync(stopwatch);
    }
    #endregion
    #region リアルタイム処理
    private async UniTask ProcessLandmarksAsync(Stopwatch stopwatch)
    {
    while(true)
    {
        _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
        var imageFrame=new ImageFrame(ImageFormat.Types.Format.Srgba,_width,_height,_width*4,_inputTexture.GetRawTextureData<byte>());
        var currentTimestamp = stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000);
        _graph.AddPacketToInputStream("input_video", Packet.CreateImageFrameAt(imageFrame, currentTimestamp));
    
        var task=_PoseLandmarksStream.WaitNextAsync();
        await UniTask.WaitUntil(() => task.IsCompleted);

        if (!task.Result.ok)
        {
          //Debug.LogError($"task1.Result.ok: {task1.Result.ok}, task2.Result.ok: {task2.Result.ok}");
          throw new System.Exception("Something went wrong");
        }
        var PoseLandmarksPacket = task.Result.packet;
        if(PoseLandmarksPacket!=null){
            var PoseLandmarks=PoseLandmarksPacket.Get(NormalizedLandmarkList.Parser);
            await UniTask.RunOnThreadPool(() => {
            landmarkManager.UpdateLandmarks(PoseLandmarks);
            });
        }
        else{
            Debug.Log("Don't detection");
        }
      }
    }
    #endregion
    #region Playモード終了時の処理
    private void OnDestroy(){

        if(_webCamTexture!=null){
            _webCamTexture.Stop();
        }
        _PoseLandmarksStream?.Dispose();
        _PoseLandmarksStream=null;
        if (_graph != null)
        {
        try
        {
          _graph.CloseInputStream("input_video");
          _graph.WaitUntilDone();
        }
        finally
        {
          _graph.Dispose();
          _graph = null;
        }
      }
    }
    #endregion
}
}