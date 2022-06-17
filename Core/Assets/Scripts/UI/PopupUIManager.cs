using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    /***********************************************************************
    *                               Public Fields
    ***********************************************************************/
    public PopupUI _skillPopup;

    public AudioClip sound;
    [Space]
    public KeyCode _escapeKey = KeyCode.Escape;
    public KeyCode _skillKey = KeyCode.K;

    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    /// <summary> �ǽð� �˾� ���� ��ũ�� ����Ʈ </summary>
    private LinkedList<PopupUI> _activePopupLList;

    /// <summary> ��ü �˾� ��� </summary>
    private List<PopupUI> _allPopupList;
    private AudioSource audio;

    /***********************************************************************
    *                               Unity Callbacks
    ***********************************************************************/
    private void Awake()
    {
        _activePopupLList = new LinkedList<PopupUI>();
        Init();
        InitCloseAll();
        audio = gameObject.AddComponent<AudioSource>();
        audio.clip = sound;
        audio.loop = false;
        audio.Play();
    }

    private void Update()
    {
        // ESC ���� ��� ��ũ�帮��Ʈ�� First �ݱ�
        if (Input.GetKeyDown(_escapeKey))
        {
            if (_activePopupLList.Count > 0)
            {
                ClosePopup(_activePopupLList.First.Value);
                audio.Play();
            }
        }

        // ����Ű ����
        ToggleKeyDownAction(_skillKey, _skillPopup);
    }

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    private void Init()
    {
        // 1. ����Ʈ �ʱ�ȭ
        _allPopupList = new List<PopupUI>()
        {
             _skillPopup
        };

        // 2. ��� �˾��� �̺�Ʈ ���
        foreach (var popup in _allPopupList)
        {
            // ��� ��Ŀ�� �̺�Ʈ
            popup.OnFocus += () =>
            {
                _activePopupLList.Remove(popup);
                _activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            // �ݱ� ��ư �̺�Ʈ
            popup._closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
    }

    /// <summary> ���� �� ��� �˾� �ݱ� </summary>
    private void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }

    /// <summary> ����Ű �Է¿� ���� �˾� ���ų� �ݱ� </summary>
    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleOpenClosePopup(popup);

            audio.Play();
        }
    }

    /// <summary> �˾��� ����(opened/closed)�� ���� ���ų� �ݱ� </summary>
    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if (!popup.gameObject.activeSelf) OpenPopup(popup);
        else ClosePopup(popup);
        audio.Play();
    }

    /// <summary> �˾��� ���� ��ũ�帮��Ʈ�� ��ܿ� �߰� </summary>
    private void OpenPopup(PopupUI popup)
    {
        _activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }

    /// <summary> �˾��� �ݰ� ��ũ�帮��Ʈ���� ���� </summary>
    private void ClosePopup(PopupUI popup)
    {
        _activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }

    /// <summary> ��ũ�帮��Ʈ �� ��� �˾��� �ڽ� ���� ���ġ </summary>
    private void RefreshAllPopupDepth()
    {
        foreach (var popup in _activePopupLList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }
}