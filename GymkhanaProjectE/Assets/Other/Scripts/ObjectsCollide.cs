using UnityEngine;
using System.Collections;

public class ObjectsCollide : MonoBehaviour
{
    private PlayerScore playerScore;
    [SerializeField] private uint scoreToPlayer;
    private bool isActive = false;

    void Awake()
    {
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();
    }

    void Start()
    {
        StartCoroutine(ChangeActive());
    }

    IEnumerator ChangeActive()
    {
        yield return new WaitForSecondsRealtime(4);
        isActive = true;
    }

    void OnCollisionEnter(Collision other)
    {
        StartCoroutine(DestroyObj(other));
    }

    IEnumerator DestroyObj(Collision other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Objects") & isActive)
        {
            playerScore.AddScore(scoreToPlayer);
            yield return new WaitForSecondsRealtime(2);

            Destroy(this.gameObject, 3f);
        }
    }
}
