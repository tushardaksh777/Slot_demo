using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button spinButton;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI betText;


    public Action<double ,double  , double > OnUpdateUI;

    private void Awake()
    {
        spinButton.onClick.AddListener(OnSpinButtonPressed);
        OnUpdateUI += UpdateUI;
    }

    public void OnSpinButtonPressed()
    {
        GameManager.Instance.OnSpinRequested.Invoke();
        //isActiveWin(false);
    }

    public void UpdateUI(double bal , double win , double bet)
    {
        balanceText.text = "$ " + bal.ToString() + ".00";
        amountText.text = "$ " + win.ToString() + ".00";
        betText.text = "$ " + bet.ToString() + ".00";

        spinButton.interactable = GameManager.Instance.canSpin;
    }

    public void isActiveWin(bool isActive)
    {
        amountText.gameObject.SetActive(isActive);
    }
}
