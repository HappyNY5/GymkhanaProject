using UnityEngine;

public class StartLevel : MonoBehaviour
{
    [SerializeField] private CheckpointSystem checkpointSystem;
    [SerializeField] private GameObject allNotGameCanvases;
    [SerializeField] private UI_Work gameCanvas;
    [SerializeField] private UI_Work backCanvas;
    public void StartLVL(int curLvl)
    {
        allNotGameCanvases.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        gameCanvas.OpenCanvas();
        backCanvas.CloseCanvas();
        CarControllerV2.isControlEnabled = true;

        checkpointSystem.BuildLvl(curLvl);
    }
}
