using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LightningArrowSkillUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    public Button btn;
    public PlayerController.SkillSet skillfunc; //��ų�� ���� delegate

    // ��ų ����
    public float mana = 0;
    public float cooltime = 0f;
    bool isDragging = false;
    public PlayerController player;

    void Start()
    {
        skillImage = btn.image;
        skillfunc = player.SkillLightningArrow;

    }

    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!btn.IsInteractable())
        {
            // ������ ���
            dragAndDropContainer.gameObject.SetActive(true);
            dragAndDropContainer.mana = mana;
            dragAndDropContainer.image.sprite = skillImage.sprite;
            dragAndDropContainer.cooltime = cooltime;
            dragAndDropContainer.skillfunc = skillfunc;
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            dragAndDropContainer.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!btn.IsInteractable())
        {
            isDragging = false;
            // ����
            dragAndDropContainer.image.sprite = null;
            dragAndDropContainer.transform.position = new Vector3(0, 0, 0);
            dragAndDropContainer.mana = 0;
            dragAndDropContainer.skillfunc = null;
            dragAndDropContainer.gameObject.SetActive(false);
        }
    }
}