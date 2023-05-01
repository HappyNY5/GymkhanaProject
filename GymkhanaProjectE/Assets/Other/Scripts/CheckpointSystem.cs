using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> checkpointsPositionsLvl1 = new List<Transform>();
    [SerializeField] private List<Transform> checkpointsPositionsLvl2 = new List<Transform>();
    [SerializeField] private GameObject checkpointObject;
    [Space]
    [Header("UI")]
    [SerializeField] private  TMP_Text checkpointCount_Text1;
    [SerializeField] private static TMP_Text checkpointCount_Text;

    private List<List<Transform>> levels = new List<List<Transform>>();
    private static Transform parentTransform;
    private static int curCheckpoinNum = 0;
    private static int checkpoinCount = 0;

    void Start()
    {
        checkpointCount_Text = checkpointCount_Text1;

        parentTransform = this.transform;
//    ДОБАВЛЯТЬ НОВЫЕ УРОВНИ РУЧКАМИ
        levels.Add(checkpointsPositionsLvl1);  
        levels.Add(checkpointsPositionsLvl2);


        // BuildLvl(0);
    }

    public void BuildLvl(int curLevel)
    {
        ClearLvl();

        foreach(Transform posForCheckpoint in levels[curLevel])
        {
            GameObject go = Instantiate(checkpointObject, posForCheckpoint.position, Quaternion.Euler(posForCheckpoint.eulerAngles), parentTransform);
            go.SetActive(false);
            checkpoinCount++;
        }

        this.transform.GetChild(0).gameObject.SetActive(true);
        checkpointCount_Text.text = $"{curCheckpoinNum} / {checkpoinCount}";
        Debug.Log("Level BUILDED");
    }

    private void ClearLvl()
    {   
        if(this.transform.GetChildCount() != 0)
            for (int i = 0; i < this.transform.GetChildCount() - 1; i++)
            {
                Destroy(this.transform.GetChild(i));
            }

        curCheckpoinNum = 0;
        checkpoinCount = 0;
        Debug.Log("Level CLEARED");
    }

    public static void NextCheckpoint()
    {
        if(parentTransform.GetChildCount() != 1)
        {
            parentTransform.GetChild(1).gameObject.SetActive(true); //анимация на появление
            //звук прохождения
            //обновление счётчика
            Destroy(parentTransform.GetChild(0).gameObject); 
            curCheckpoinNum++;
        }else{
            //анимация финиша
            Destroy(parentTransform.GetChild(0).gameObject); 
            curCheckpoinNum++; 
            CarControllerV2.isControlEnabled = false;
            Debug.Log("FINISH");
        }
    }

    public static void UpdateCheckpointUI()
    {
        checkpointCount_Text.text = $"{curCheckpoinNum} / {checkpoinCount}";
    }
}
