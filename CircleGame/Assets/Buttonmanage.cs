using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public string gameSceneName = "SampleScene"; 

    public void PlayAgain()
    {
        CoinCollector.score = 0; 
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // makes Quit work in the editor
#endif
    }
}