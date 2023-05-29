using UnityEngine;
using System;
using TMPro;
using UnityEngine.Localization;


public class ChangeLanguageButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    private string[] languages = {"Rus", "Eng"};
    private string curLanguage = "Rus";

    void Start()
    {
        // инициализация языка
        curLanguage = languages[0];
        buttonText.text = curLanguage;
    }
    public void NextLanguage()
    {
        string newCurLanguage = languages[(Array.IndexOf(languages, curLanguage) + 1)%(languages.Length)];
        curLanguage = newCurLanguage;
        buttonText.text = curLanguage;

        LocalizationSettings.AvailableLocales.Locales[i];
        
        Debug.Log($"Cur language = {curLanguage}");
        // return curLanguage;
    }
}
