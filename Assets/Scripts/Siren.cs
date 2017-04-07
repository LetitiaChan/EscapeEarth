using UnityEngine;

public class Siren : MonoBehaviour
{
    public float rotateSpeed = 180
        ;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }
}
