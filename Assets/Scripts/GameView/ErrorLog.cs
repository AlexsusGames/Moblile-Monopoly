using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorLog : MonoBehaviour
{
    [SerializeField] private Image window;
    [SerializeField] private TMP_Text errorText;

    public static ErrorLog instance;

    private void Awake() => instance = this;

    public void ShowError(string message)
    {
        window.gameObject.SetActive(false);
        window.gameObject.SetActive(true);
        errorText.text = message;
    }
}
