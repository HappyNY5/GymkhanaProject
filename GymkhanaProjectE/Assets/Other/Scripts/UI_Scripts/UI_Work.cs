using UnityEngine;

public class UI_Work : MonoBehaviour
{
    [SerializeField] private UI_Work gameCanvas;
    [SerializeField] private bool isSettingUi = false; 

    [SerializeField] private GameObject allNotGameCanvases;
    private Transform canvasScalerTransform;
    private bool opening = false;
    private bool closing = false;
    private float curAnimTime = 2;
    [SerializeField] private float animTime = 1;
    private float curScale = 0;
    private float openMount = 1;

    void Start()
    {
        canvasScalerTransform = this.transform.GetChild(0);
    }

    void FixedUpdate()
    {
        Debug.Log($"{curAnimTime} / {animTime}");

        if(curAnimTime <= animTime)
        {
            curAnimTime += Time.fixedDeltaTime;
            curScale = curAnimTime/ animTime;
            float newScale = Mathf.Abs(openMount - curScale);
            canvasScalerTransform.localScale = new Vector3(newScale, newScale, 1);
        }else if(isSettingUi & (canvasScalerTransform.localScale.x >= 0.1f) & opening){
            Time.timeScale = 1.05f - curScale;
        }else if(canvasScalerTransform.localScale.x < 0.05f){
            canvasScalerTransform.localScale =  Vector3.zero;
        }
    }

    public void OpenCanvas()
    {
        closing = false;
        opening = true;

        curAnimTime = 0;
        openMount = 0;
        // this.GetComponent<Canvas>().sortingOrder = 1;

        if(isSettingUi)
        {
            Time.timeScale = 1;
            gameCanvas.gameObject.SetActive(false);
        } 
    }

    public void CloseCanvas()
    {
        closing = true;
        opening = false;

        curAnimTime = 0;
        openMount = 1;
        // this.GetComponent<Canvas>().sortingOrder = 0;

        if(isSettingUi)
        {
            Time.timeScale = 1;
            gameCanvas.gameObject.SetActive(true);
        } 
    }

    public void ToMenu()
    {

        //tp car
        //clear lvl
    }
}
