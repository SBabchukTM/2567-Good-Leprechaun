using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    public int Id;
    public Button BuyButton;
    public Button UseButton;
    public Image BuyButtonImage;
    public TextMeshProUGUI CostText;
    public int Cost = 0;
    public Color ButtonColor = Color.red;
    public bool IsBought;
    public bool IsUsed;

    private void UpdateUI()
    {
        CostText.text = Cost.ToString();
        BuyButtonImage.color = ButtonColor;
        BuyButton.gameObject.SetActive(!IsBought);
        UseButton.gameObject.SetActive(IsBought && !IsUsed);
    }

    public void Buy()
    {
        BuyButton.gameObject.SetActive(false);
        IsBought = true;
        UpdateUI();
    }

    public void Use()
    {
        UseButton.interactable = false;
        IsUsed = true;
        UpdateUI();
    }
    public void UnUse()
    {
        UseButton.interactable = true;
        IsUsed = false;
        UpdateUI();
    }
}