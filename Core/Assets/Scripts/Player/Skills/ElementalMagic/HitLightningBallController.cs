using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLightningBallController : MonoBehaviour
{
    float life = 0.0f;
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;

        if (life > lifeTime)
        {
            Destroy(gameObject);
        }

    }
}
