using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NickCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickInput;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private PlayerNickName view;

    [SerializeField] private UnityEvent OnNickCreate;

    public void CheckNickName()
    {
        if (nickInput.text.Length < 4) errorText.text = "Ник должно содержать от 4 символов!";
        else if (nickInput.text.Length > 15) errorText.text = "Ник должно содержать до 15 символов!";
        else
        {
            OnNickCreate?.Invoke();
            view.SaveNick(nickInput.text);
        }
    }
}
