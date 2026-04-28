using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Matrix4x4 mat = gameObject.GetComponent<Camera>().projectionMatrix;
        mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        gameObject.GetComponent<Camera>().projectionMatrix = mat;
    }

}
