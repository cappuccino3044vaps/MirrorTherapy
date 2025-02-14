using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ViveHandTracking;

namespace MirrorTherapy.HandTracking
{
public class VRM_handtracking_mirror : MonoBehaviour
{
  [Tooltip(
    "手指の初期回転を定義するものです"
      )]
  public Vector3 initialRotation=Vector3.zero;
  [Tooltip(
    "各関節の座標・回転情報を格納します"
  )]
  public Transform[] Nodes =new Transform[21];
  [Tooltip(
    "ミラーリングするためのノードを格納します"
    )]
  public Transform[] MirrorNodes =new Transform[21];
  [Tooltip(
    "skinned meshrendererをアタッチするためのゲームオブジェクトです"
  )]
  public GameObject Hand = null;
  [Tooltip(
    "対象のオブジェクトが左手か右手かを決定します。"
  )]
  public bool IsLeft=false;
  private Quaternion[] jointRotation;
  private Quaternion[] Mirror_jointRotation;
  public Transform MirrorTag=null;

  private async void Start(){
  try 
  {
    while (GestureProvider.Status == GestureStatus.NotStarted)
    {
        await UniTask.Yield();  // 非同期待機（フレームを待つ）
    }

    if (!GestureProvider.HaveSkeleton)
    {
        this.enabled = false;
    }
  } 
  catch (System.Exception ex)
  {
    Debug.LogError($"手のトラッキング初期化エラー: {ex.Message}");
  }
  }
  void Awake(){
    InitializeModel();
    Hand.SetActive(false);
  } // Update is called once per frame
  void Update()
  {
    GestureResult result = IsLeft ? GestureProvider.LeftHand : GestureProvider.RightHand;
    if (result == null) {
      Hand.SetActive(false);
      return;
    }
    Hand.SetActive(true);

    //transform.localPosition = result.points[0];
    Nodes[0].localRotation = result.rotation * Quaternion.Euler(initialRotation);
    MirrorNodes[0].localRotation = result.rotation * Quaternion.Euler(0,90,90);
    var parentRotation = Nodes[0].parent != null ? Nodes[0].parent.rotation : Quaternion.identity;
    var parentMirrorRotation = MirrorNodes[0].parent != null ? MirrorNodes[0].parent.rotation : Quaternion.identity;

    int vecIndex = 0;
    int nodeIndex = 1;

    var MirrorRotation=Quaternion.identity;
    for (int i = 0; i < 5; ++i, nodeIndex += 4, vecIndex += 3) {
      
      Nodes[nodeIndex].rotation =
          parentRotation * result.rotations[nodeIndex] * jointRotation[vecIndex];

      MirrorNodes[nodeIndex].rotation =
          parentMirrorRotation * result.rotations[nodeIndex] * Mirror_jointRotation[vecIndex];

      Nodes[nodeIndex + 1].rotation =
          parentRotation* result.rotations[nodeIndex + 1] * jointRotation[vecIndex + 1];

      MirrorNodes[nodeIndex + 1].rotation =
          parentMirrorRotation * result.rotations[nodeIndex + 1] * Mirror_jointRotation[vecIndex+1];

      Nodes[nodeIndex + 2].rotation =
          parentRotation * result.rotations[nodeIndex + 2] * jointRotation[vecIndex + 2];

      MirrorNodes[nodeIndex + 2].rotation =
          parentMirrorRotation * result.rotations[nodeIndex + 2] * Mirror_jointRotation[vecIndex+2];

    }
  }

#region モデルの軸を決定する
    private void InitializeModel() {

    var right = FindLocalNormal(Nodes[9]);
    var thumbright=FindLocalNormal(Nodes[1]);

    jointRotation = new Quaternion[15];
    Mirror_jointRotation = new Quaternion[15];

    for (int i = 0; i < 3; i++){
      var thumbup = Nodes[i + 2].localPosition;
      jointRotation[i] = CaliculatejointRotation(thumbright,thumbup);
      thumbup = MirrorNodes[i + 2].localPosition;
      Mirror_jointRotation[i] = FlipEulerRotation(CaliculatejointRotation(thumbright,thumbup));
    }
    int vecIndex = 3;
    int nodeIndex = 5;
    //i=指の本数, j=関節の数
    for (int i = 1; i < 5; i++, nodeIndex += 4, vecIndex += 3) {
      for (int j = 0; j < 3; j++){
        var up = Nodes[nodeIndex + j + 1].localPosition;
          jointRotation[vecIndex + j] = CaliculatejointRotation(right,up);
        up = MirrorNodes[nodeIndex + j + 1].localPosition;
          Mirror_jointRotation[vecIndex + j] = FlipEulerRotation(CaliculatejointRotation(right,up));
      }
    }      
  }
  private Vector3 FindLocalNormal(Transform node) {
    var rotation = node.rotation;
    if (transform.parent != null)
      rotation = Quaternion.Inverse(transform.parent.rotation) * rotation;
    var axis = Vector3.zero;
    var minDistance = 0f;
    var dot = Vector3.Dot(rotation * Vector3.forward, Vector3.right);
    if (dot > minDistance) {
      minDistance = dot;
      axis = Vector3.forward;
    } else if (-dot > minDistance) {
      minDistance = -dot;
      axis = Vector3.back;
    }

    dot = Vector3.Dot(rotation * Vector3.right, Vector3.right);
    if (dot > minDistance) {
      minDistance = dot;
      axis = Vector3.right;
    } else if (-dot > minDistance) {
      minDistance = -dot;
      axis = Vector3.left;
    }

    dot = Vector3.Dot(rotation * Vector3.up, Vector3.right);
    if (dot > minDistance) {
      minDistance = dot;
      axis = Vector3.up;
    } else if (-dot > minDistance) {
      minDistance = -dot;
      axis = Vector3.down;
    }
    return axis;
  }
  private Vector3 MirrorFindLocalNormal(Transform node) {
    var rotation = node.rotation;
    if (transform.parent != null)
      rotation = Quaternion.Inverse(transform.parent.rotation) * rotation;
    var axis = Vector3.zero;
    var minDistance = 0f;
    var dot = Vector3.Dot(rotation * Vector3.forward, Vector3.left);
    if (dot > minDistance) {
      minDistance = dot;
      axis = Vector3.forward;
    } else if (-dot > minDistance) {
      minDistance = -dot;
      axis = Vector3.back;
    }

    dot = Vector3.Dot(rotation * Vector3.left, Vector3.left);
    if (dot > minDistance) {
      minDistance = dot;
      axis = Vector3.left;
    } else if (-dot > minDistance) {
      minDistance = -dot;
      axis = Vector3.left;
    }

    dot = Vector3.Dot(rotation * Vector3.up, Vector3.right);
    if (dot > minDistance) {
      minDistance = dot;
      axis = Vector3.up;
    } else if (-dot > minDistance) {
      minDistance = -dot;
      axis = Vector3.down;
    }
    return axis;
  }
#endregion

private Quaternion CaliculatejointRotation(Vector3 right,Vector3 up){
  return Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(right, up), up));
}

private Quaternion FlipEulerRotation(Quaternion rotation){
  return Quaternion.Euler(-rotation.eulerAngles);
}

private static int[] Bones = new int[] {
    2,  3,  3,  4,           // thumb
    5,  6,  6,  7,  7,  8,   // index
    9,  10, 10, 11, 11, 12,  // middle
    13, 14, 14, 15, 15, 16,  // ring
    17, 18, 18, 19, 19, 20,  // pinky
  };
}
}