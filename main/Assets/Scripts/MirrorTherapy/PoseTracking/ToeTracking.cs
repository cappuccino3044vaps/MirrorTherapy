using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UniVRM10;
using Mediapipe.Unity;

namespace MirrorTherapy.PoseTracking
{
public class ToeTracking : MonoBehaviour
{
    public Transform[] Toes =new Transform[2]; 
    public LandmarkManager landmarkManager; 
    private Vector3 LeftToe;
    private Vector3 RightToe;
    public Transform HIP_IS_ORIGIN;
    private void OnEnable()
    {
        if (landmarkManager != null)
        {
            landmarkManager.LandmarkUpdated+=OnLandmarkUpdated;
        }
    }
    private void OnDisable()
    {
        if (landmarkManager != null)
        {
            landmarkManager.LandmarkUpdated-=OnLandmarkUpdated;
        }
    }
    private void OnLandmarkUpdated(List<Vector3> newLandmarks)
    {
        LeftToe=newLandmarks[15];
        RightToe=newLandmarks[16];
    }

    private void Update()
    {
        Toes[0].position=HIP_IS_ORIGIN.position+LeftToe;
        Toes[1].position=HIP_IS_ORIGIN.position+RightToe;
    }
    }
}
