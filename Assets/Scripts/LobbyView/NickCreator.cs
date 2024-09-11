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
        if (nickInput.text.Length < 4) errorText.text = "��� ������ ��������� �� 4 ��������!";
        else if (nickInput.text.Length > 15) errorText.text = "��� ������ ��������� �� 15 ��������!";
        else
        {
            OnNickCreate?.Invoke();
            view.SaveNick(nickInput.text);
        }
    }
}
