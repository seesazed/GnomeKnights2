using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public PlayerCurrency playerCurrency;
    public TextMeshProUGUI moneyText;

    private int displayedMoney;
    private Coroutine animationCoroutine;

    void Start()
    {
        if (playerCurrency != null)
        {
            displayedMoney = playerCurrency.CurrentMoney;
            UpdateMoneyText();
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerCurrency = player.GetComponent<PlayerCurrency>();
                if (playerCurrency != null)
                {
                    displayedMoney = playerCurrency.CurrentMoney;
                    UpdateMoneyText();
                }
            }
        }
    }

    void Update()
    {
        if (playerCurrency == null || moneyText == null) return;

        if (displayedMoney != playerCurrency.CurrentMoney)
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(AnimateMoneyChange(playerCurrency.CurrentMoney));
        }
    }

    private IEnumerator AnimateMoneyChange(int targetMoney)
    {
        int startMoney = displayedMoney;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            displayedMoney = (int)Mathf.Lerp(startMoney, targetMoney, elapsed / duration);
            UpdateMoneyText();
            yield return null;
        }

        displayedMoney = targetMoney;
        UpdateMoneyText();
        animationCoroutine = null;
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "Duobloons: " + displayedMoney.ToString();
        }
    }
}