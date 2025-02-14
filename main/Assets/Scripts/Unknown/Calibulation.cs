/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibulation : MonoBehaviour
{
    [Tooltip(
        "VIVEhand_trackingのモデルをアタッチして下さい"
    )]
    public Transform User_Hand_L;
    public Transform User_Hand_R;
    [Tooltip(
        "アバタとして使用するモデルをアタッチして下さい"
    )]
    public GameObject Avatar;
    [Tooltip(
        "使用するモデルのHandオブジェクトをアタッチして下さい"
    )]
    public Transform Avatar_Hand_L;
    public Transform Avatar_Hand_R;
    void Start()
    {
        var Avatar_Hand_Diff=Avatar_Hand_R.localPosition.x-Avatar_Hand_L.transform.localPosition.x;
    }

    void Update()
    {
        var User_Hand_Diff=Hand_R.localPosition.x-Hand_L.transform.localPosition;
    }
}
*/