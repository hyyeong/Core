using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Move : MonoBehaviour
{
    private MeshRenderer render;

    public float Speed;

    private float offset;

    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        offset += (Time.deltaTime * PlayerMove.BGspeed) * Speed;
        render.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
