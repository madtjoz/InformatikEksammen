using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    public void SetMoney(float amount)
    {
        moneyText.text = "Penge: " + amount.ToString("0");
    }
}