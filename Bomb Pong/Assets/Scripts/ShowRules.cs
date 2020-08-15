using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class ShowRules : MonoBehaviour
{
    public GameObject rulesPanel;
    public Text rulesPanelTxt;

    public void BtnShowRules()
    {
        Debug.Log("Showing rules...");
        if (rulesPanel!= null)
        {
            ReadRulesFile();
            rulesPanel.SetActive(true);
            rulesPanelTxt.gameObject.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        rulesPanel.SetActive(false);
        rulesPanelTxt.gameObject.SetActive(true);
    }

    //[MenuItem("Tools/Read file")]
    void ReadRulesFile()
    {
        string path = "Assets/Resources/DaBombPong_Rules.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        rulesPanelTxt.text = reader.ReadToEnd();
        reader.Close();
    }
}
