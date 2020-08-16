using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ServeCarefully : MonoBehaviour
{
    public void BtnServeCarefully()
    {
        Debug.Log("Serving carefully for starting new rally");
        SceneManager.LoadScene(GameLogic.initScene.name);
    }
}
