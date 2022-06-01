using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropContainer : MonoBehaviour
{
    public Image image;
    public float mana;
    public float cooltime = 0f;
    public PlayerController.SkillSet skillfunc;
    void Start()
    {
        gameObject.SetActive(false);
    }
}
