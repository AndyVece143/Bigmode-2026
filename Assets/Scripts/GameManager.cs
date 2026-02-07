using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coinAmount;
    public float time;
    public TMP_Text timerText;
    public TMP_Text coinText;
    public Checkpoint activeCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinAmount = 0;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        int seconds = ((int)time % 60);
        int minutes = ((int)time / 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        coinText.text = "Coins: " + coinAmount;
    }

    public void GainCoin()
    {
        coinAmount++;
    }
}