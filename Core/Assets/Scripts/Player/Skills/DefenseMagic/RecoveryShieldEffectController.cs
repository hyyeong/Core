using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryShieldEffectController : MonoBehaviour
{
    PlayerController player;
    GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.recoverySheildTime < 0)
        {
            Destroy(gameObject);
        }
        transform.position = playerObject.transform.position + new Vector3(0, 2, 0);
    }
}
