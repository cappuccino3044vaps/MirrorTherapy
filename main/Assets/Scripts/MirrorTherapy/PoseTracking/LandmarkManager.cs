using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
//using Mediapipe.Unity.CoordinateSystem;
using Cysharp.Threading.Tasks;

namespace MirrorTherapy.PoseTracking
{
    ///<Summary>
    ///各クラス間の情報を統括するためのクラスです。
    ///</Summary>
    public class LandmarkManager : MonoBehaviour
    {
        private NormalizedLandmarkList landmarks;
        private List<Vector3> landmark_vec=new List<Vector3>();
        public  NormalizedLandmarkList Landmarks=>landmarks;
        [System.Serializable] public enum outputType
        {
            LandmarkOnly,
            LandmarkAndVideo
        }
        [SerializeField] private outputType _outputType=outputType.LandmarkOnly;
        public delegate void OnLandmarkUpdated(List<Vector3> newLandmarks);
        public event OnLandmarkUpdated LandmarkUpdated;

        void Start()
        {
            landmarks=new NormalizedLandmarkList();
        }

        public void UpdateLandmarks(NormalizedLandmarkList newLandmarks)
        {
            landmarks=newLandmarks;
            landmark_vec.Clear();
            for(int i=0;i<landmarks.Landmark.Count;i++)
            {
                if(outputType.LandmarkAndVideo==_outputType)
                {
                    landmarks.Landmark[i].X*=-1;
                    landmarks.Landmark[i].Y*=-1;
                    landmarks.Landmark[i].Z*=-1;
                }
                else{
                    landmarks.Landmark[i].Z*=-1;
                }
                landmark_vec.Add(LandmarkToVector3(landmarks.Landmark[i]));
            }
            LandmarkUpdated?.Invoke(landmark_vec);
        }

        private Vector3 LandmarkToVector3(NormalizedLandmark landmark)
        {
            return new Vector3(landmark.X, landmark.Y, landmark.Z);
        }

        }

}
