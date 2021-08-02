using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 speed = new Vector3(0,0,5);
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
