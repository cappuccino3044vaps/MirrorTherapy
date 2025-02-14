using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ViveHandTracking;

namespace MirrorTherapy.HandTracking
{

public class MirrorWrist : MonoBehaviour
{

    // Start is called before the first frame update
    //右手のTransform
    public Transform RightHand;
    //胸のTransform
    public Transform UpperChest;
    // Update is called once per frame
      private async void Start(){
        try 
        {
            while (GestureProvider.Status == GestureStatus.NotStarted)
            {
                await UniTask.Yield();  // 非同期待機（フレームを待つ）
            }
        } 
        catch (System.Exception ex)
        {
            Debug.LogError($"手のトラッキング初期化エラー: {ex.Message}");
        }
      }
    void Update()
    {
        if (RightHand==null || UpperChest==null) {
            Debug.Log("RightHand or UpperChest is null"); 
            return;
        }
        GestureResult result = GestureProvider.RightHand;
        transform.position=SetLeftWrist(RightHand.position,UpperChest);
        if (result == null) {
            return;
        }
        transform.localRotation=SetLeftWristRotation(result.rotation);
    }
    //左手首の位置を計算
    public Vector3 SetLeftWrist(Vector3 rightWrist,Transform upperChest){
            Vector3 planevec=Vector3.Cross(upperChest.forward,Vector3.up).normalized;
            Vector3 closestpoint=rightWrist-Vector3.Dot(rightWrist-upperChest.position,planevec)*planevec;
            Vector3 LeftWrist=2*(closestpoint-rightWrist)+rightWrist;
        return LeftWrist;
    }
    //左手首の回転を計算
    public Quaternion SetLeftWristRotation(Quaternion result){
            Quaternion LeftWristRotation=FlipRotation(result)*Quaternion.Euler(0,90,-90);
        return LeftWristRotation;
    }
    //回転を左右反転する
    public Quaternion FlipRotation(Quaternion rotation){
        return Quaternion.Euler(rotation.eulerAngles.x,-rotation.eulerAngles.y,-rotation.eulerAngles.z);
    }
}
}