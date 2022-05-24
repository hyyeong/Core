using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealEffectController : MonoBehaviour
{
    PlayerController player;
    GameObject playerObject;
    void Start()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player.lifeStealTime < 0)
        {
            Destroy(gameObject);
        }
        transform.position = playerObject.transform.position + new Vector3(0, 2, 0);
    }
}
