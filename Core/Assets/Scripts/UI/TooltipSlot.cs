using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillToolTip tooltip;
    public string skillName;
    public bool isPassive;
    void Start()
    {
        tooltip = GameObject.Find("SkillToolTip").GetComponent<SkillToolTip>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowToolTip(skillName, isPassive, eventData.position);
    }

    // ���콺 Ŀ���� ���Կ��� ���� �� �ߵ�
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideToolTip();
    }
}
