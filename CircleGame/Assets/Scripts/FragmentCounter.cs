using UnityEngine;
using TMPro;

public class FragmentCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GoToGameOver win; 

    void Awake() => CoinCollector.score = 0;

    void Update() => text.text = CoinCollector.score + " / " + (win != null ? win.coinsToWin : 25);
}
