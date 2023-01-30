using UnityEngine;
using TMPro;

public class SDKusing : MonoBehaviour
{
    [SerializeField] private TMP_Text deviceText;
    private bool isAndro = false;

    private void Start() 
    {
        deviceText.text = $"Device = PC";
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                deviceText.text = $"Device = andro";
                isAndro = true;
            }
        }
    }
}
