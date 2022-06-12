using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yellowOrb : MonoBehaviour
{
    float time = 0;
    GameObject player;

    void Start()
    {
        this.player = GameObject.Find("player");
    }

    void Update()
    {
        if (time < 0.8f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, time);
        }
        else
        {
            transform.Translate(0, -0.1f, 0);
        }
        
        time += Time.deltaTime;
    }

    public void OnTriggerEnter2D()
    {
        Destroy(gameObject);
    }
}
