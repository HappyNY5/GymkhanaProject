using UnityEngine;

public class WheelsPositions : MonoBehaviour
{
    [SerializeField] private Vector3[] wheelsPosition = new Vector3[4]{Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
    [SerializeField] public float WheelsRadius{get;} = 2.5f;

    public Vector3[] ReturnWheelsPosition()
    {
        return wheelsPosition;
    }
}
