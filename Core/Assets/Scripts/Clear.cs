using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        for (int i = 0; i < Canvas.transform.childCount; i++)
        {
            if (Canvas.transform.GetChild(i).CompareTag("hpbar"))
            {
                Destroy(Canvas.transform.GetChild(i));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
