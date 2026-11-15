using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToGameOver : MonoBehaviour
{
    public int coinsToWin = 25;
    public string gameOverSceneName = "GameOverWin"; 

    private bool hasLoaded = false;

    private void Update()
    {
        if (!hasLoaded && CoinCollector.score >= coinsToWin)
        {
            hasLoaded = true;
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}