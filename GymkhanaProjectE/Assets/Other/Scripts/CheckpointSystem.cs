using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> checkpointsPositionsLvl1 = new List<Transform>();
    [SerializeField] private List<Transform> checkpointsPositionsLvl2 = new List<Transform>();
    [SerializeField] private GameObject checkpointObject;
    [SerializeField] private PlayerScore playerScoreScr;

    private static int[] lvlScoreComplete = new int[10]{500, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000}; 

    [Space]
    [Header("--Finish UI--")]
    [SerializeField] private TMP_Text finishScoreText1;
    private static TMP_Text finishScoreText;
    [SerializeField] private Image finishScoreImage1;
    private static Image finishScoreImage;


    [Space]
    [Header("--UI--")]
    [SerializeField] private  TMP_Text checkpointCount_Text1;
    private static TMP_Text checkpointCount_Text;
    [SerializeField] private GameObject finishCanvas1;
    private  static GameObject finishCanvas;

    private List<List<Transform>> levels = new List<List<Transform>>();
    private static Transform parentTransform;
    private static int curCheckpoinNum = 0;
    private static int checkpoinCount = 0;
    private static int curLvl = 0;

    void Start()
    {
        checkpointCount_Text = checkpointCount_Text1;
        playerScoreScr = playerScoreScr.GetComponent<PlayerScore>();

        finishCanvas = finishCanvas1;
        finishScoreText = finishScoreText1;
        finishScoreImage = finishScoreImage1;


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

        curLvl = curLevel;
        Debug.Log("Level BUILDED");
    }

    private void ClearLvl()
    {   
        if(this.transform.childCount != 0)
            for (int i = 0; i < this.transform.childCount - 1; i++)
            {
                Destroy(this.transform.GetChild(i));
            }

        curCheckpoinNum = 0;
        checkpoinCount = 0;
        Debug.Log("Level CLEARED");
    }

    public static void NextCheckpoint()
    {
        if(parentTransform.childCount != 1)
        {
            parentTransform.GetChild(1).gameObject.SetActive(true); //анимация на появление
            //звук прохождения
            //обновление счётчика
            Destroy(parentTransform.GetChild(0).gameObject); 
            curCheckpoinNum++;
        }else{
            //анимация финиша
            finishCanvas.GetComponent<UI_Work>().OpenCanvas();
            Destroy(parentTransform.GetChild(0).gameObject); 
            curCheckpoinNum++; 
            CarControllerV2.isControlEnabled = false;

            //если очков больше чем надо, тогда считать как пройденный
            if(PlayerScore.ReturnPlayerScore() >= lvlScoreComplete[curLvl])
                ProgressChekerAndSaver.UpdLvlInformation(curLvl);

            finishScoreText.text = $"{PlayerScore.ReturnPlayerScore()} / {lvlScoreComplete[curLvl]}";
            finishScoreImage.fillAmount =  (float)PlayerScore.ReturnPlayerScore() / lvlScoreComplete[curLvl];

            PlayerScore.ClearPlayerScore();
            ProgressChekerAndSaver.UpdateLvlButtons();

            Debug.Log("FINISH");
        }
    }

    public static void UpdateCheckpointUI()
    {
        checkpointCount_Text.text = $"{curCheckpoinNum} / {checkpoinCount}";
    }
}
