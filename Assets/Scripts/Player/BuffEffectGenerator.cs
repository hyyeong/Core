using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectGenerator : MonoBehaviour
{
    //Effects
    public GameObject lifeStealEffect;
    public GameObject recoveryShieldEffect;
    public GameObject araEffect;
    public GameObject concentrationEffect;
    public GameObject recyclingEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Generate
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
    public void AraEffect(Transform playerPos)
    {
        Vector3 pos = playerPos.position + new Vector3(0, 2, 0);
        Instantiate(araEffect, pos, playerPos.rotation);
    }
    public void ConcentrationEffect(Transform playerPos)
    {
        Vector3 pos = playerPos.position + new Vector3(0, 2, 0);
        Instantiate(concentrationEffect, pos, playerPos.rotation);
    }
    public void RecyclingEffect(Transform playerPos)
    {
        Vector3 pos = playerPos.position + new Vector3(0, 2, 0);
        Instantiate(recyclingEffect, pos, playerPos.rotation);
    }
}
