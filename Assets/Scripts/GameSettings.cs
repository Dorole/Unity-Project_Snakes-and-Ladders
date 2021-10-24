using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public Toggle greenCpu, greenHuman;
    public Toggle redCpu, redHuman;
    public Toggle blueCpu, blueHuman, blueNone;
    public Toggle yellowCpu, yellowHuman, yellowNone;

    public void ReadToggle()
    {
        //green 
        if (greenCpu.isOn)
            SaveSettings.players[0] = "CPU";
        else if (greenHuman.isOn)
            SaveSettings.players[0] = "HUMAN";

        //red
        if (redCpu.isOn)
            SaveSettings.players[1] = "CPU";
        else if (redHuman.isOn)
            SaveSettings.players[1] = "HUMAN";

        //blue
        if (blueCpu.isOn)
            SaveSettings.players[2] = "CPU";
        else if (blueHuman.isOn)
            SaveSettings.players[2] = "HUMAN";
        else if (blueNone.isOn)
            SaveSettings.players[2] = "NONE";

        //yellow
        if (yellowCpu.isOn)
            SaveSettings.players[3] = "CPU";
        else if (yellowHuman.isOn)
            SaveSettings.players[3] = "HUMAN";
        else if (yellowNone.isOn)
            SaveSettings.players[3] = "NONE";
    }

    public void StartGame()
    {
        ReadToggle();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}


public static class SaveSettings
{
    public static string[] players = new string[4];
}
