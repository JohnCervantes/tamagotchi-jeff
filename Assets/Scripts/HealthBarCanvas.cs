using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBarCanvas : MonoBehaviour
{
    [Header("UI Reference")]
    public Image healthBarFill;

    [Header("Testing Settings")]
    public float maxHealth = 100f;
    public float decayPerSecond = 5f; // Now loses 5 points every single second

    private float currentHealth;

    void Start()
    {
        LoadHealthState();
    }

    void Update()
    {
        // 1. Rapid decay while playing
        ApplyDecay(Time.deltaTime);
        UpdateUI();
    }

    void LoadHealthState()
    {
        currentHealth = PlayerPrefs.GetFloat("PetHealth", maxHealth);

        if (PlayerPrefs.HasKey("LastSaveTime"))
        {
            string lastSaveStr = PlayerPrefs.GetString("LastSaveTime");
            DateTime lastSaveTime = DateTime.Parse(lastSaveStr);

            double secondsOffline = (DateTime.Now - lastSaveTime).TotalSeconds;

            // 2. Apply the fast decay for time spent away
            ApplyDecay((float)secondsOffline);
        }

        UpdateUI();
    }

    void ApplyDecay(float seconds)
    {
        // Removed the "/ 3600" so it decays exactly by the decayPerSecond value
        currentHealth -= decayPerSecond * seconds;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;

        // Make sure health doesn't go over the maximum
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();
        SaveHealth(); // Save immediately so the progress isn't lost

        Debug.Log("Pet fed! New health: " + currentHealth);
    }

    public void SaveHealth()
    {
        PlayerPrefs.SetFloat("PetHealth", currentHealth);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }



    private void OnApplicationQuit() => SaveHealth();
    private void OnApplicationPause(bool pause) { if (pause) SaveHealth(); }
}