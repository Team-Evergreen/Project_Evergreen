using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearUI : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text resultText;

    [Header("Buttons")]
    [SerializeField] private Button retryButton;

    private void Awake()
    {
        Hide();

        if (retryButton != null)
            retryButton.onClick.AddListener(RestartScene);
    }

    public void Show(float playTime)
    {
        if (root != null)
            root.SetActive(true);

        if (resultText != null)
            resultText.text = $"Celar Time : {FormatTime(playTime)}";
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }

    private string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}