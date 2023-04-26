using UnityEngine;

public class CheckpointCheck : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
   {
        if(other.tag == "Player")
        {
            CheckpointSystem.NextCheckpoint();
            CheckpointSystem.UpdateCheckpointUI();
        }
   }
}
