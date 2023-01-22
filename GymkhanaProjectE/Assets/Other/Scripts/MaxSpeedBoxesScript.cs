using UnityEngine;

public class MaxSpeedBoxesScript : MonoBehaviour
{
    [SerializeField] private int newMaxSpeed;

    void OnTriggerEnter(Collider other)
    {
        if(CarControl.MaxSpeed != newMaxSpeed & other.tag == "Player")
        {
            CarControl.MaxSpeed = newMaxSpeed;
            CarControl.InBreakingZone = true;
            Debug.Log("Max speed updd");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
            CarControl.InBreakingZone = false;
    }
}
