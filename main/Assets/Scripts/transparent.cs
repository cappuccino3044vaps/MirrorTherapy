using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 // Start is called before the first frame update


public class transparent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool is_renderer=false;  

    void Start()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        var color = renderer.material.color;
        // 非表示
        renderer.material.color = new Color(color.r, color.g, color.b, 0f);
        // 非表示
        gameObject.layer = LayerMask.NameToLayer("Hidden");
        // 表示
        //gameObject.layer = LayerMask.NameToLayer("Default");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
