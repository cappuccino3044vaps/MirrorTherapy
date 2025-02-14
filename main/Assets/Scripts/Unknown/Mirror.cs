using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.localScale=new Vector3(-1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
