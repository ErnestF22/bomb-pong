using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Playing Scene");
    }

    public void ChangeGameMode()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
