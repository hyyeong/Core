using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HealSkillUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    public Button btn;
    bool isDragging = false;
    void Start()
    {
        skillImage = btn.image;
    }

    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragAndDropContainer.gameObject.SetActive(true);
        // Set Data 
        dragAndDropContainer.image.sprite = skillImage.sprite;
        isDragging = true;
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
        isDragging = false;
        // Reset Contatiner
        dragAndDropContainer.image.sprite = null;
        dragAndDropContainer.transform.position = new Vector3(0, 0, 0);
        dragAndDropContainer.gameObject.SetActive(false);
    }
    // 드롭 오브젝트에서 발생
    /*public void OnDrop(PointerEventData eventData)
    {
        if (dragAndDropContainer.image.sprite != null)
        {
            Sprite tempSprite = skillImage.sprite;

            dragAndDropContainer.image.sprite = tempSprite;
        }
        else
        {
            dragAndDropContainer.image.sprite = null;
        }
    }*/
}
