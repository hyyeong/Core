using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShieldController : MonoBehaviour
{
    public GameObject physics;
    public GameObject player;
    public float speed = 0.1f;
    public float damage { set; get; }
    float angle = 0f;
    float life = 0f;
    float direction = 1f;
    const float LIFETIME = 30.0f;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    void Update()
    {
        life += Time.deltaTime;
        angle -= Time.deltaTime * 114.2857f; // 회전이 3.15초당 360도 1초당 약 114.2857만큼 회전하면 물리객체가 파티클이랑 같이움직임
        physics.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));
        transform.position = player.transform.position;

        if (life >= LIFETIME)
            Destroy(this.gameObject);
    }

    public void setDirection(float x)
    {
        direction = x < 0 ? -1f : 1f;
        this.transform.localScale = new Vector3(-direction, transform.localScale.y, transform.localScale.z);
    }
}
