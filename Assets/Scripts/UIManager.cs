using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public Health playerHealth;
    public WaveManager waveManager;

    [Header("UI")]
    public TMP_Text healthText;
    public TMP_Text waveText;

    private void Update()
    {
        if (playerHealth != null && healthText != null)
            healthText.text = $"HP: {playerHealth.CurrentHealth}/{playerHealth.maxHealth}";

        if (waveManager != null && waveText != null)
            waveText.text = $"Wave: {waveManager.CurrentWave}"; 
    }
}