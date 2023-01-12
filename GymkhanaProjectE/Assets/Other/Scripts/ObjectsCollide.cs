using UnityEngine;
using System.Collections;

public class ObjectsCollide : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        StartCoroutine(DestroyObj(other));
    }

    IEnumerator DestroyObj(Collision other)
    {
        yield return new WaitForSecondsRealtime(2);

        
        if (other.gameObject.tag == "Player")
        {
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<CapsuleCollider>().enabled = false;

            Destroy(this, 3f);
        }
    }
}
