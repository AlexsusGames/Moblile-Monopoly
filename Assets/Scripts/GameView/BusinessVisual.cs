using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public abstract class BusinessVisual : MonoBehaviour
{
    [SerializeField] private Image logoImage;
    [SerializeField] protected TMP_Text priceText;
    [SerializeField] private Color businessColor;
    [SerializeField] protected Image tabColor;
    [SerializeField] private Image pawnBlock;
    [SerializeField] private TMP_Text pawnTermText;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public BusinessConfig BusinessConfig { set; protected get; }
    public Business BusinesData { set; get; }

    public void SetPawn(int term)
    {
        if (term > 0)
        {
            pawnBlock.gameObject.SetActive(true);
            pawnTermText.text = term.ToString();
        }
        else pawnBlock.gameObject.SetActive(false);
    }
    public void Setup(Business business, BusinessInfoPanel info)
    {
        BusinessConfig = business.GetConfig();
        button.onClick.AddListener(() => info.OpenInfoPanel(business, businessColor));
        SetStandart();
    }
    public void SetStandart()
    {
        logoImage.sprite = BusinessConfig.businessLogo;
        priceText.text = $"{BusinessConfig.BusinessPrice}$";
        tabColor.color = Color.white;
        SetPawn(0);
    }
    public virtual void UpdateOutcome(int newOutcome)
    {
        priceText.text = $"{newOutcome}$";
    }
    public abstract void OnBusinessLevelChange(PlayerData playerColor, int level = 0);
}