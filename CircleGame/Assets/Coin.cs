using UnityEngine;
using TMPro; 

public class CoinCollector : MonoBehaviour
{

    public TextMeshProUGUI coinText;

    public static int score = 0;

    private bool collected = false;

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            score += 1;
            UpdateUI();
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayNextCoinSound();
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "Fragments Collected: " + score;
    }

}