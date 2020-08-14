using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLvl : MonoBehaviour
{
    private int easyCountdown = 25;
    private int intermCountdown = 15;
    private int hardCountdown = 10;

    public void BtnEasyLvl(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);
        PlayerPrefs.SetInt("countdown", easyCountdown);
        SceneManager.LoadScene(sceneName);
    }

    public void BtnMidLvl(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);
        PlayerPrefs.SetInt("countdown", intermCountdown);
        SceneManager.LoadScene(sceneName);
    }

    public void BtnHardLvl(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);
        PlayerPrefs.SetInt("countdown", hardCountdown);
        SceneManager.LoadScene(sceneName);
    }
}
