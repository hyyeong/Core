using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    GameObject player;
    public float cameraSpeed = 2.0f;
    float height;
    float width;

    public Vector2 mapSize;
    public Vector2 center;
    public Vector3 cameraPosition;
    public Vector3 playerY;

    //https://velog.io/@cedongne/ 웹사이트 참조
    void Start()
    {
        this.player = GameObject.Find("Player");
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        if (player != null)
        {
            //Camera 조절
            Vector3 playerPos = this.player.transform.position;
            // 카메라 좌표 설정
            transform.position = Vector3.Lerp(transform.position,
                                           playerPos + cameraPosition + playerY,
                                           Time.deltaTime * cameraSpeed);
            // 최대와 최소를 제한하기
            float lx = mapSize.x - width;
            float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

            float ly = mapSize.y - height;
            float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
