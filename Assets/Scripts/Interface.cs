using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public static Interface instance;

    public GameObject inGamePanel;
    public Text infoText;
    [Space]
    public GameObject winPanel;
    public Text winText;

    private void Awake()
    {
        instance = this;
        infoText.text = "";
    }

    private void Start()
    {
        winPanel.SetActive(false);
    }

    public void ShowText(string text)
    {
        infoText.text = text;
    }

    public void WinMessage(string winner)
    {
        winText.text = winner + " won!";
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

