using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyController : MonoBehaviour
{
    public GameObject hitEffect;
    public float speed = 0.1f;
    public float damage { set; get; }

    float life = 0f;
    float direction = 1f;
    const float LIFETIME = 1.0f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    void Update()
    {
        life += Time.deltaTime;
        if (life >= LIFETIME)
            Destroy(this.gameObject);
    }

    public void setDirection(float x)
    {
        direction = x < 0 ? -1f : 1f;
        this.transform.localScale = new Vector3(-direction, transform.localScale.y, transform.localScale.z);
    }
}
