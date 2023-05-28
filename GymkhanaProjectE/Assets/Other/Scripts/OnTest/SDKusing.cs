using UnityEngine;

public class SDKusing : MonoBehaviour
{
    [SerializeField] private GameObject AndoInput;
    public static bool isAndro = false;


    void Update()
    {
        if(!isAndro)
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    isAndro = true;
                    AndoInput.active = true;
                }
            }
    }
}
