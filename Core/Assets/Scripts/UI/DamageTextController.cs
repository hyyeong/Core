using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageTextController : MonoBehaviour
{
    float life = 0.0f;
    public float lifeTime;
    public Vector3 initTransform;
    Text text;
    void Start()
    {
        this.transform.SetAsFirstSibling();
        text = GetComponent<Text>();
    }
    void Update()
    {
        life += Time.deltaTime;
        transform.position = Camera.main.WorldToScreenPoint(initTransform);
        text.color = new Color(119, 0, 137,1 - life / lifeTime);
        initTransform.y = initTransform.y + Time.deltaTime*2;
        if (life > lifeTime)
        {
            Destroy(gameObject);
        }

    }
}
