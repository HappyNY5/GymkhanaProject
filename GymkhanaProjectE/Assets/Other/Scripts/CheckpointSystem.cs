using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> checkpointsPositions = new List<Transform>();
    [SerializeField] private GameObject checkpointObject;

    private List<List<Transform>> levels = new List<List<Transform>>();
    private static Transform parentTransform;

    void Start()
    {
        parentTransform = this.transform;
//    ДОБАВЛЯТЬ НОВЫЕ УРОВНИ РУЧКАМИ
        levels.Add(checkpointsPositions);  


        BuildLvl(0);
    }

    private void BuildLvl(int curLevel)
    {
        ClearLvl();

        foreach(Transform posForCheckpoint in levels[curLevel])
        {
            GameObject go = Instantiate(checkpointObject, posForCheckpoint.position, Quaternion.Euler(posForCheckpoint.eulerAngles), parentTransform);
            go.SetActive(false);
        }

        this.transform.GetChild(0).gameObject.SetActive(true);

        Debug.Log("Level BUILDED");
    }

    private void ClearLvl()
    {   
        if(this.transform.GetChildCount() != 0)
            for (int i = 0; i < this.transform.GetChildCount() - 1; i++)
            {
                Destroy(this.transform.GetChild(i));
            }

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
        }else{
            //анимация финиша
            Destroy(parentTransform.GetChild(0).gameObject);  
            Debug.Log("FINISH");
        }
    }
}
