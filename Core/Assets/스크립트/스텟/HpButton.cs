using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpButton : MonoBehaviour
{
    public void OnClickButton()
    {
        Debug.Log("Button Click");
        if (SP.sp > 0)
        {
            SP.hp += 1;
            SP.sp -= 1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
