using TMPro;
using UnityEngine;

public class PlayTimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    private float playTime;
    private bool isRunning;
    public float CurrentTime => playTime;

    private void Update()
    {
        if (!isRunning)
            return;

        playTime += Time.deltaTime;
        UpdateUI();
    }

    public void StartTimer()
    {
        playTime = 0f;
        isRunning = true;
        UpdateUI();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateUI()
    {
        int totalSeconds = Mathf.FloorToInt(playTime);

        int hours = totalSeconds / 3600;
        int minutes = totalSeconds % 3600 / 60;
        int seconds = totalSeconds % 60;

        timeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}