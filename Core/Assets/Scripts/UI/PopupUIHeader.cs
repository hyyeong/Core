using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupUIHeader : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    // UI창 이동기능
    private RectTransform _parentRect;

    private Vector2 _rectBegin;
    private Vector2 _moveBegin;
    private Vector2 _moveOffset;

    public AudioClip sound;
    AudioSource audio;
    private void Awake()
    {
        _parentRect = transform.parent.GetComponent<RectTransform>();
        audio = gameObject.AddComponent<AudioSource>();
        audio.clip = sound;
        audio.loop = false;
        audio.Play();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _rectBegin = _parentRect.anchoredPosition;
        _moveBegin = eventData.position;
        audio.Play();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        audio.Play();
        _moveOffset = eventData.position - _moveBegin;
        _parentRect.anchoredPosition = _rectBegin + _moveOffset;
    }
}