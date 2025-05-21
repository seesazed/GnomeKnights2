using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public PlayerCurrency playerCurrency;
    public PlayerHealth playerHealth;
    public PlayerStats playerStats;

    [Header("Upgrade Costs")]
    public int maxHealthCost = 75;
    public int speedCost = 60;
    public int attackSpeedCost = 80;
    public int rangeCost = 70;
    public int damageCost = 90;

    [Header("Upgrade Amounts")]
    public int healthIncreaseAmount = 20;
    public float speedIncrease = 0.5f;
    public float attackSpeedIncrease = 0.1f;
    public float rangeIncrease = 1f;
    public int damageIncrease = 5;

    [Header("UI Elements")]
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI speedCostText;
    public TextMeshProUGUI attackSpeedCostText;
    public TextMeshProUGUI rangeCostText;
    public TextMeshProUGUI damageCostText;

    void Start()
    {
        UpdateUpgradeTexts();
    }

    void UpdateUpgradeTexts()
    {
        if (healthCostText) healthCostText.text = "$" + maxHealthCost;
        if (speedCostText) speedCostText.text = "$" + speedCost;
        if (attackSpeedCostText) attackSpeedCostText.text = "$" + attackSpeedCost;
        if (rangeCostText) rangeCostText.text = "$" + rangeCost;
        if (damageCostText) damageCostText.text = "$" + damageCost;
    }

    public void BuyMaxHealth()
    {
        if (playerCurrency.SpendMoney(maxHealthCost))
        {
            playerHealth.maxHealth += healthIncreaseAmount;
            playerHealth.currentHealth += healthIncreaseAmount;
            Debug.Log("Max health increased!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade max health.");
        }
    }

    public void BuySpeed()
    {
        if (playerCurrency.SpendMoney(speedCost))
        {
            playerStats.moveSpeed += speedIncrease;
            Debug.Log("Speed increased!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade speed.");
        }
    }

    public void BuyAttackSpeed()
    {
        if (playerCurrency.SpendMoney(attackSpeedCost))
        {
            playerStats.attackCooldown -= attackSpeedIncrease;
            playerStats.attackCooldown = Mathf.Max(0.1f, playerStats.attackCooldown);
            Debug.Log("Attack speed increased!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade attack speed.");
        }
    }

    public void BuyRange()
    {
        if (playerCurrency.SpendMoney(rangeCost))
        {
            playerStats.attackRange += rangeIncrease;
            Debug.Log("Attack range increased!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade range.");
        }
    }

    public void BuyDamage()
    {
        if (playerCurrency.SpendMoney(damageCost))
        {
            playerStats.attackDamage += damageIncrease;
            Debug.Log("Attack damage increased!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade damage.");
        }
    }
    public GameObject shopUI; // Assign this in the inspector
    public MonoBehaviour cameraLookScript; // Reference to your look/movement script

    public void CloseShop()
    {
        shopUI.SetActive(false);

        // Re-enable camera / controls
        if (cameraLookScript != null)
            cameraLookScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
