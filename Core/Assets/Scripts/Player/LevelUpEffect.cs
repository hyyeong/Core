using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEffect : MonoBehaviour
{

    float life = 0.0f;
    public float lifeTime;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
        life += Time.deltaTime;

        if (life > lifeTime)
        {
            Destroy(gameObject);
        }

    }
}
