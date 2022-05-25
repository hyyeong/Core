using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AraEffectController : MonoBehaviour
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
        if (player.araTime < 0)
        {
            Destroy(gameObject);
        }
        transform.position = playerObject.transform.position + new Vector3(0, 2.5f, 0);
    }
}
