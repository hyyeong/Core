using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffectController : MonoBehaviour
{
    float time = 0.0f;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
