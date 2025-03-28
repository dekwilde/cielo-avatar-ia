using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            Matrix4x4 mat = cam.projectionMatrix;
            mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
            cam.projectionMatrix = mat;
            cam.forceIntoRenderTexture = true; // Evita problemas visuais
        }
    }

}
