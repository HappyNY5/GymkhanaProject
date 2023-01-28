using UnityEngine;
using System.Collections.Generic;

public class WheelsPositions : MonoBehaviour
{
    public static Dictionary<Mesh, Vector3[]>  wheelsPositions = new Dictionary<Mesh, Vector3[]>();
    public CarControllerV2 car;

    void Start()
    {
        ReadAllWheelPos();
    }


    public Vector3[] ReturnWheelsPosition(Mesh curBodyMesh)
    {
        if(curBodyMesh != null)
        {
            Vector3[] vec;
            wheelsPositions.TryGetValue(curBodyMesh, out vec);
            return vec;
        }
        else
        {
            Debug.Log($"{curBodyMesh.name} wheels position index NOT find");
            return null;
        }
    }


    public void ReadAllWheelPos()
    {
        for (int a = 0; a < (this.transform.GetChildCount()); a++)
        {
            Vector3[] _wp = new Vector3[4]{Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};

            for (int i = 0; i < 4; i++)
            {
                _wp[i] = transform.GetChild(a).transform.GetChild(i).position;
                Destroy(transform.GetChild(a).transform.GetChild(i).gameObject);
                Debug.Log(this.transform.GetChildCount());
            }


            wheelsPositions.Add(car.bodyModelsMesh[a], _wp);
            Destroy(this.transform.GetChild(a).gameObject);
            Debug.Log($"WHeel pos {a} added");
        }
    }

}
