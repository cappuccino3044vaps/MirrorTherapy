using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveHandTracking;

namespace MirrorTherapy.HandTracking
{

public class VRM_handtracking : MonoBehaviour
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
    "skinned meshrendererをアタッチするためのゲームオブジェクトです"
  )]
  public GameObject Hand = null;
  [Tooltip(
    "対象のオブジェクトが左手か右手かを決定します。"
  )]
  public bool IsLeft=false;
  private Quaternion[] jointRotation;

  void Awake(){
    InitializeModel();
    Hand.SetActive(false);
  }

  IEnumerator Start(){
    while (GestureProvider.Status == GestureStatus.NotStarted) yield return null;
    if (!GestureProvider.HaveSkeleton) this.enabled = false;
  }
    // Update is called once per frame
  void Update()
  {
    GestureResult result = IsLeft ? GestureProvider.LeftHand : GestureProvider.RightHand;
    if (result == null) {
      Hand.SetActive(false);
      return;
    }
    Hand.SetActive(true);

    transform.localPosition = result.points[0];
    transform.localRotation = result.rotation * Quaternion.Euler(initialRotation);

    var parentRotation = transform.parent != null ? transform.parent.rotation : Quaternion.identity;
    int vecIndex = 0;
    int nodeIndex = 1;
    for (int i = 0; i < 5; ++i, nodeIndex += 4, vecIndex += 3) {
      Nodes[nodeIndex].rotation =
          parentRotation * result.rotations[nodeIndex] * jointRotation[vecIndex];
      Nodes[nodeIndex + 1].rotation =
          parentRotation * result.rotations[nodeIndex + 1] * jointRotation[vecIndex + 1];
      Nodes[nodeIndex + 2].rotation =
          parentRotation * result.rotations[nodeIndex + 2] * jointRotation[vecIndex + 2];
    }
  }
#region モデルの軸を決定する
    private void InitializeModel() {
    // find local normal vector in node local axis, assuming all finger nodes have same local axis
    //中指のroot
    var right = FindLocalNormal(Nodes[9]);
    // get initial finger direction and length in local axis
    var thumbright=FindLocalNormal(Nodes[1]);
    jointRotation = new Quaternion[15];
    int vecIndex = 3;
    int nodeIndex = 5;
    var thumbup = Nodes[1].localPosition;
      jointRotation[0] =
          Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(thumbright, thumbup), thumbup));
      thumbup = Nodes[2].localPosition;
      jointRotation[1] =
          Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(thumbright, thumbup), thumbup));
      thumbup = Nodes[3].localPosition;
      jointRotation[2] =
          Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(thumbright, thumbup), thumbup));
    for (int i = 1; i < 5; i++, nodeIndex += 4, vecIndex += 3) {
      var up = Nodes[nodeIndex + 1].localPosition;
      jointRotation[vecIndex] =
          Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(right, up), up));
      up = Nodes[nodeIndex + 2].localPosition;
      jointRotation[vecIndex + 1] =
          Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(right, up), up));
      up = Nodes[nodeIndex + 3].localPosition;
      jointRotation[vecIndex + 2] =
          Quaternion.Inverse(Quaternion.LookRotation(Vector3.Cross(right, up), up));
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
#endregion
 // Links between keypoints, 2*i & 2*i+1 forms a link.
  private static int[] Bones = new int[] {
    2,  3,  3,  4,           // thumb
    5,  6,  6,  7,  7,  8,   // index
    9,  10, 10, 11, 11, 12,  // middle
    13, 14, 14, 15, 15, 16,  // ring
    17, 18, 18, 19, 19, 20,  // pinky
  };
}
}