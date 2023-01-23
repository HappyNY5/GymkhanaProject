using UnityEngine;

public class MaxSpeedBoxesScript : MonoBehaviour
{
    [SerializeField] private int newMaxSpeed;
    [SerializeField] private bool breakFlag = true;


    void OnTriggerEnter(Collider other)
    {
        if(CarControl.MaxSpeed != newMaxSpeed & other.tag == "Player")
        {
            CarControl.MaxSpeed = newMaxSpeed;
            Debug.Log("Max speed updd");
            
            if (breakFlag)
                CarControl.InBreakingZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
            CarControl.InBreakingZone = false;
    }
}
