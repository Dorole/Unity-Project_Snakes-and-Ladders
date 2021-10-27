using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public static Interface instance;

    public GameObject inGamePanel;
    public GameObject infoPanel;
    public Text infoText;

    [Space]
    public GameObject winPanel;
    public Text winText;

    [Space]
    public Button fwdButton;
    public Button backButton;

    [HideInInspector] public bool fwdButtonPressed;
    [HideInInspector] public bool backButtonPressed;

    private void Awake()
    {
        instance = this;
        infoText.text = "";
    }

    private void Start()
    {
        winPanel.SetActive(false);
        fwdButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void MoveForward()
    {
        fwdButtonPressed = true;
        backButtonPressed = false;
    }

    public void MoveBack()
    {
        backButtonPressed = true;
        fwdButtonPressed = false;
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

    public void EndScreens()
    {
        winPanel.SetActive(true);
        inGamePanel.SetActive(false);
        infoPanel.SetActive(false);
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

