using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cam;

    void Start() => cam = Camera.main;

    void LateUpdate()
    {
        if (cam)
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}