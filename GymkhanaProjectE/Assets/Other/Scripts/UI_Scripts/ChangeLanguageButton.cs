using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;



public class ChangeLanguageButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    private string[] languages = {"Eng", "Rus"};
    private int curLanguageINdex = 0;

    void Start()
    {        
        buttonText.text = languages[curLanguageINdex];
    }
    public void NextLanguage()
    {
        curLanguageINdex =  (curLanguageINdex + 1)%(languages.Length);
        buttonText.text = languages[curLanguageINdex];

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[curLanguageINdex];

        Debug.Log($"Cur language = {curLanguageINdex}");
    }
}
