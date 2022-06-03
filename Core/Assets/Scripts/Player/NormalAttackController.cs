using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackController : MonoBehaviour
{
    public GameObject hitEffect;
    public float speed;

    float life = 0f;
    float direction = 1f;
    const float LIFETIME = 3.0f;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //적이 타격된경우
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Map"))
        {
            //적에게 대미지 처리
            Instantiate(hitEffect, this.transform.position, this.transform.rotation);
        }
        Debug.Log("충돌");
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;
        this.transform.Translate(0, -speed * Time.deltaTime, 0);
        if (life >= LIFETIME)
            Destroy(this.gameObject);
    }

    public void setDirection(float x)
    {
        direction = x < 0 ? -1f : 1f;
        this.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }
}
