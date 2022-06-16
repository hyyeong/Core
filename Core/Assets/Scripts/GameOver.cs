using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    float time = 0;

    void Update()
    {
        if (time < 3f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, time / 3);
        }
        else if (time > 5f && Input.anyKeyDown)
        {
            Application.Quit();
        }
        time += Time.deltaTime;
    }
}
