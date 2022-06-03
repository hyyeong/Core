using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAlertController : MonoBehaviour
{
    public float time = 0f;
    public float lifetime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > lifetime || Input.GetMouseButton(0)) // Ŭ��Ȥ�� �ð��ʰ��� �����
            gameObject.SetActive(false);
    }
}
