using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject TapToStart = null;
    [SerializeField] private GameStateManager m_manager = null;

    [Header("Score Board")]
    [SerializeField] private GameObject ScorePanel = null;
    [SerializeField] private Text HighScore = null;
    [SerializeField] private RectTransform forwardButton = null;
    [SerializeField] private RectTransform backButton = null;
    [SerializeField] private TextMeshProUGUI currentScore = null;
    [SerializeField] private TextMeshProUGUI bestScore = null;
    [SerializeField] private TextMeshProUGUI coins = null;
    [SerializeField] private Sprite[] medalIcons = null;
    [SerializeField] private Image medal = null;

    void Start()
    {
        //Seperate UI behaviors into this file (GameStateManager)
        //Subscribe to events
        GameStateManager.StartGame += OnStartGame;
        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.PlayerLanded += OnPlayerLanded;
        GameStateManager.RestartGame += OnRestartGame;
        ScorePanel.SetActive(false);
        TapToStart.SetActive(true);
    }

    public void ResetButtons()
    {
        forwardButton.localScale = Vector3.one;
        backButton.localScale = Vector3.one;
    }

    public void FlipButton()
    {
        forwardButton.localScale = Utils.FlipLocalScale(forwardButton.localScale);
        backButton.localScale = Utils.FlipLocalScale(backButton.localScale);
    }

    private void OnDestroy()
    {
        GameStateManager.StartGame -= OnStartGame;
        GameStateManager.PlayerDied -= OnPlayerDied;
        GameStateManager.PlayerLanded -= OnPlayerLanded;
        GameStateManager.RestartGame -= OnRestartGame;
    }

    private void OnStartGame()
    {
        TapToStart.SetActive(false);
        SetText();
    }

    private void OnRestartGame()
    {
        ScorePanel.SetActive(false);
        TapToStart.SetActive(true);
        ResetButtons();
    }

    private void OnPlayerLanded()
    {
        SetText();
    }

    private void SetText()
    {
        HighScore.text = $"High Score: {m_manager.GetHeight()}";
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        TapToStart.SetActive(false);
        var score = e.Score.ToString();
        currentScore.text = score;
        coins.text = e.Coins.ToString();
        var best = SaveManager.Instance.GetHighScore();
        if(e.Score > best)
        {
            bestScore.text = score;
            SaveManager.Instance.UpdateScore(e.Score);
        }
        else
        {
            bestScore.text = best.ToString();
        }
        Sprite medalImage = GetMedalImage(e.Score);
        medal.sprite = medalImage;
        ScorePanel.SetActive(true);
    }

    private Sprite GetMedalImage(int score)
    {
        if (score >= 200)
            return medalIcons[0];
        else if (score >= 100)
            return medalIcons[1];
        else if (score >= 50)
            return medalIcons[2];
        else if (score >= 25)
            return medalIcons[3];
        else
            return medalIcons[4];
    }

    public void Share()
    {
        StartCoroutine("TakeScreenshotAndShare");
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();
        var shareMessage = "I just scored " + currentScore.text + " points in Sky Climbers! Can you get higher?";

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Sky Climbers").SetText(shareMessage)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
