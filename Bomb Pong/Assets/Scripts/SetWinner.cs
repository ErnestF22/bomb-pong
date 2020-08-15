using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetWinner : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        string text = PlayerPrefs.GetString("winnerTxt");
        this.gameObject.GetComponent<Text>().text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
