using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltinController : MonoBehaviour
{
    public GameObject hitEffect;
    public float speed = 0.1f;
    public float damage { set; get; }

    float life = 0f;
    float direction = 1f;
    const float LIFETIME = 7.0f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //적이 타격된경우
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //적에게 대미지 처리
            Vector3 hitPos = transform.position + new Vector3(0, 0, 0);
            Instantiate(hitEffect, hitPos, this.transform.rotation);
        }
        Debug.Log("충돌");
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        life += Time.deltaTime;
        this.transform.Translate(direction * speed * Time.deltaTime, 0, 0);
        if (life >= LIFETIME)
            Destroy(this.gameObject);
    }

    public void setDirection(float x)
    {
        direction = x < 0 ? -1f : 1f;
        this.transform.localScale = new Vector3(-direction, transform.localScale.y, transform.localScale.z);
    }
}
