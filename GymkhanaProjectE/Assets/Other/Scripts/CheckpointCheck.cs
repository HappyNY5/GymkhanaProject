using UnityEngine;

public class CheckpointCheck : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
   void OnTriggerEnter(Collider other)
   {
        if(other.tag == "Player")
        {
            sound.Play();
            CheckpointSystem.NextCheckpoint();
            CheckpointSystem.UpdateCheckpointUI();
        }
   }
}
