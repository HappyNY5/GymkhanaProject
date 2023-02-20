using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Work : MonoBehaviour
{
    private Transform canvasScalerTransform;
    private float curAnimTime = 2;
    private float animTime = 1;
    private float curScale = 0;
    private int openMount = 1;

    void Start()
    {
        canvasScalerTransform = this.transform.GetChild(0);
    }

    void FixedUpdate()
    {
        if(curAnimTime <= animTime)
        {
            curAnimTime += Time.fixedDeltaTime;
            curScale = curAnimTime/ animTime;
            float newScale = Mathf.Abs(openMount - curScale);
            canvasScalerTransform.localScale = new Vector3(newScale, newScale, 1);
        }
    }

    public void OpenCanvas()
    {
        curAnimTime = 0;
        openMount = 0;
        this.GetComponent<Canvas>().sortingOrder = 1;
    }

    public void CloseCanvas()
    {
        curAnimTime = 0;
        openMount = 1;
        this.GetComponent<Canvas>().sortingOrder = 0;
    }
}
