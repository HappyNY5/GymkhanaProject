using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CheckpointSystem : MonoBehaviour
{
    [Header("-- Levels --")]
    [SerializeField] private List<Transform> checkpointsPositionsLvl1 = new List<Transform>();
    [SerializeField] private List<Transform> objectsPositionsLvl1 = new List<Transform>();
    [SerializeField] private List<Transform> objectsPositionsBarierLvl1 = new List<Transform>();
    [SerializeField] private List<Transform> objectsPositionsPillarLvl1 = new List<Transform>();
    [Space]
    [SerializeField] private List<Transform> checkpointsPositionsLvl2 = new List<Transform>();
    [SerializeField] private List<Transform> objectsPositionsLvl2 = new List<Transform>();
    [SerializeField] private List<Transform> objectsPositionsBarierLvl2 = new List<Transform>();
    [SerializeField] private List<Transform> objectsPositionsPillarLvl2 = new List<Transform>();

    [Space]
    [SerializeField] private GameObject checkpointObject;
    [SerializeField] private GameObject  burrelObject;
    [SerializeField] private GameObject  barrierObject;
    [SerializeField] private GameObject  pillarObject;
    [SerializeField] private PlayerScore playerScoreScr;

    private static int[] lvlScoreComplete = new int[10]{500, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000}; 

    [Space]
    [Header("-- Finish UI --")]
    [SerializeField] private TMP_Text finishScoreText1;
    private static TMP_Text finishScoreText;
    [SerializeField] private Image finishScoreImage1;
    private static Image finishScoreImage;
    [SerializeField] private AudioSource finishSound1;
    private static AudioSource finishSound;


    [Space]
    [Header("-- UI --")]
    [SerializeField] private  TMP_Text checkpointCount_Text1;
    private static TMP_Text checkpointCount_Text;
    [SerializeField] private GameObject finishCanvas1;
    private  static GameObject finishCanvas;

    private List<List<Transform>> levels = new List<List<Transform>>();
    private List<List<Transform>> levelsObjects = new List<List<Transform>>();
    private List<List<Transform>> levelsObjectsBarrier = new List<List<Transform>>();
    private List<List<Transform>> levelsObjectsPillar = new List<List<Transform>>();

    private static Transform parentTransform;
    [SerializeField] private  Transform parentObjectTransform1;
    private static Transform parentObjectTransform;
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

        finishSound = finishSound1;


        parentTransform = this.transform;
        parentObjectTransform = parentObjectTransform1;
//    ДОБАВЛЯТЬ НОВЫЕ УРОВНИ И ОБЪЕКТЫ РУЧКАМИ
        levels.Add(checkpointsPositionsLvl1);  
        levels.Add(checkpointsPositionsLvl2);

        levelsObjects.Add(objectsPositionsLvl1);
        levelsObjects.Add(objectsPositionsLvl2);

        levelsObjectsBarrier.Add(objectsPositionsBarierLvl1);
        levelsObjectsBarrier.Add(objectsPositionsBarierLvl2);

        levelsObjectsPillar.Add(objectsPositionsPillarLvl1);
        levelsObjectsPillar.Add(objectsPositionsPillarLvl2);

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
        // this.transform.GetChild(0).transform.gameObject.SetActive(true);
        checkpointCount_Text.text = $"{curCheckpoinNum} / {checkpoinCount}";

        foreach(Transform posForCheckpoint in levelsObjects[curLevel])
        {
            GameObject go = Instantiate(burrelObject, posForCheckpoint.position, Quaternion.Euler(posForCheckpoint.eulerAngles), parentObjectTransform);
        }
        foreach(Transform posForBarrier in levelsObjectsBarrier[curLevel])
        {
            GameObject go = Instantiate(barrierObject, posForBarrier.position, Quaternion.Euler(posForBarrier.eulerAngles), parentObjectTransform);
            // posForBarrier.gameObject.SetActive(true);
        }
        foreach(Transform posForBarrier in levelsObjectsPillar[curLevel])
        {
            GameObject go = Instantiate(pillarObject, posForBarrier.position, Quaternion.Euler(posForBarrier.eulerAngles), parentObjectTransform);
            // posForBarrier.gameObject.SetActive(true);
        }

        curLvl = curLevel;
        Debug.Log("Level BUILDED");
    }

    private void ClearLvl()
    {   
        if(this.transform.childCount != 0)
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        
        if(parentObjectTransform.transform.childCount != 0)
            for (int i = 0; i < parentObjectTransform.transform.childCount - 1; i++)
            {
                Destroy(parentObjectTransform.transform.GetChild(i).gameObject);
            }

        curCheckpoinNum = 0;
        checkpoinCount = 0;
        Debug.Log("Level CLEARED");
    }

    public static void NextCheckpoint()
    {
        if(curCheckpoinNum+1 != checkpoinCount)
        {
            parentTransform.GetChild(1).gameObject.SetActive(true); //анимация на появление
            //звук прохождения
            //обновление счётчика
            Destroy(parentTransform.GetChild(0).gameObject); 
            curCheckpoinNum++;
        }else{
            //анимация финиша
            Debug.Log("Hi");
            finishCanvas.GetComponent<UI_Work>().OpenCanvas();
            Destroy(parentTransform.GetChild(0).gameObject); 
            curCheckpoinNum++; 
            CarControllerV2.isControlEnabled = false;
            finishSound.Play();

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
