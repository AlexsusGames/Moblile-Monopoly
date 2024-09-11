using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationSercive : MonoBehaviour
{
    [SerializeField] private GameObject tab;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text info;
    [SerializeField] private TMP_Text buttonAcceptText;
    [SerializeField] private TMP_Text buttonDenyText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button cancelButton;

    public void Open(NotificationData data, UnityAction acceptAction = null, UnityAction cancelAction = null, bool isAutoQuit = false)
    {
        title.text = data.Title;
        info.text = data.Message;
        buttonAcceptText.text = data.FirstAction;
        buttonDenyText.text = data.SecondAction;
        AddListeners(acceptAction, cancelAction, isAutoQuit);
        tab.SetActive(true);
    }
    private void AddListeners(UnityAction acceptAction = null, UnityAction cancelAction = null, bool isAutoQuit = false)
    {
        acceptButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        if(acceptAction != null)
        {
            acceptButton.onClick.AddListener(acceptAction);
            if (!isAutoQuit)
            {
                acceptButton.onClick.AddListener(() => tab.SetActive(false));
            }
        }
        if(cancelAction != null)
        {
            cancelButton.onClick.AddListener(cancelAction);
            cancelButton.onClick.AddListener(() => tab.SetActive(false));
        }
        acceptButton.interactable = acceptAction != null;
        cancelButton.gameObject.SetActive(cancelAction != null);
    }
    public void CloseTab()
    {
        tab.SetActive(false);
    }
}
