using UnityEngine;

public class MaxSpeedBoxesScript : MonoBehaviour
{
    [SerializeField] private int newMaxSpeed;
    [SerializeField] private bool startDriftZone = false;


    void OnTriggerEnter(Collider other)
    {
        if(CarControl.MaxSpeed != newMaxSpeed & other.tag == "Player")
        {
            CarControl.MaxSpeed = newMaxSpeed;

            CarControl.DriftMode(startDriftZone);

            Debug.Log($"Max speed = {newMaxSpeed} \n Drift mode = {startDriftZone}");
        }
    }
}
