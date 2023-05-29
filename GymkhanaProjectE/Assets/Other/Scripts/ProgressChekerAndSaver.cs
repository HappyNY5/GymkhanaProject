using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressChekerAndSaver : MonoBehaviour
{
    private static bool[] lvlProgress = new bool[10];
    [SerializeField] private Button[] lvlsButtons;
    private static Button[] lvlsButtons1;
    private static int savedBodyIndex = 0;
    private static int savedWheelsIndex = 0;
    private static int savedSmokeIndex = 0;

    private static int savedBodyColorIndex = 0;
    private static int savedWheelsColorIndex = 0;
    private static int savedSmokeColorIndex = 0;

    public enum CarComponents
    {
        Body,
        Wheels,
        Smoke
    }

    void Start()
    {
        LoadLvls();
        lvlsButtons1 = lvlsButtons;
        UpdateLvlButtons();

    }

    public static void UpdateLvlButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            string name = $"lvl {i}";
            lvlsButtons1[i+1].interactable = lvlProgress[i];
            Debug.Log($"{name} {lvlProgress[i]}");
        }
    }

    private void LoadLvls()
    {
        int i = 0;
        foreach(bool lvl in lvlProgress)
        {
            string name = $"lvl {i}";
            lvlProgress[i] = Convert.ToBoolean(PlayerPrefs.GetInt(name));
            i++;
        }
    }

    public static void UpdLvlInformation(int completeLvl)
    {
        lvlProgress[completeLvl] = true;

        Debug.Log($"Lvl info upd: {lvlProgress[0]},{lvlProgress[1]},{lvlProgress[2]},{lvlProgress[3]},{lvlProgress[4]},{lvlProgress[5]},{lvlProgress[6]},{lvlProgress[7]},{lvlProgress[8]},{lvlProgress[9]}");
    }

    public static void UpdCarInformation(CarComponents component, int modelIndex, int colorIndex)
    {
        if(component == CarComponents.Body){
            savedBodyIndex = modelIndex;
            savedBodyColorIndex = colorIndex;
        }else if(component == CarComponents.Wheels){
            savedWheelsIndex = modelIndex;
            savedWheelsColorIndex = colorIndex;
        }else if(component == CarComponents.Smoke){
            savedSmokeIndex = modelIndex;
            savedSmokeColorIndex = colorIndex;
        }

        Debug.Log($"Mesh, color\nBody = {savedBodyIndex}, {savedBodyColorIndex}\nWheels = {savedWheelsIndex}, {savedWheelsColorIndex}\nSmoke = {savedSmokeIndex}, {savedSmokeColorIndex}");
    }

    public static void SaveInfo()
    {
        //save
        PlayerPrefs.SetInt("bodyIndex",savedBodyIndex);
        PlayerPrefs.SetInt("bodycolorIndex",savedBodyColorIndex);

        PlayerPrefs.SetInt("wheelsIndex",savedWheelsIndex);
        PlayerPrefs.SetInt("wheelscolorIndex",savedWheelsColorIndex);

        PlayerPrefs.SetInt("smokeIndex",savedSmokeIndex);
        PlayerPrefs.SetInt("smokecolorIndex",savedSmokeColorIndex);

        int i = 0;
        foreach(bool lvl in lvlProgress)
        {
            string name = $"lvl {i}";
            PlayerPrefs.SetInt(name, Convert.ToInt32(lvl));
            
            i++;
        }

        PlayerPrefs.Save();
        Debug.Log("GameSaved");
    }

    

}
