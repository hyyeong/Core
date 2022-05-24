using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectGenerator : MonoBehaviour
{
    //Effects
    public GameObject lifeStealEffect;
    public GameObject recoveryShieldEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LifeStealEffect(Transform playerPos)
    {
        Vector3 pos = playerPos.position + new Vector3(0,2,0);
        Instantiate(lifeStealEffect, pos, playerPos.rotation);
    }
    public void RecoveryShieldEffect(Transform playerPos)
    {
        Vector3 pos = playerPos.position + new Vector3(0, 2, 0);
        Instantiate(recoveryShieldEffect, pos, playerPos.rotation);
    }
}
