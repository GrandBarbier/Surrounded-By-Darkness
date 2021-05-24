using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTextLanguage : MonoBehaviour
{
    public int id;
    public TextMeshProUGUI text;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        SetText();
        LanguageSystem.setLanguageEvent += SetText;
    }
    
    void Update()
    {
        
    }

    public void SetText()
    {
        text.text = LanguageSystem.outputData[id];
    }
}
