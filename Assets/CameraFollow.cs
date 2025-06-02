using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 baseOffset = new Vector3(0, 5, -10);
    public Vector3 angle = new Vector3(30f, 0f, 0f);
    public const float zoomFactor = 0.8f;
    public const float minZoom = 0.4f;
    public const float maxZoom = 0.9f;
    public const float smoothSpeed = 1.6f;

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = baseOffset;
    }

    void LateUpdate()
    {
        Vector3 targetPos = RunnerManager.Instance.GetGroupCenter();
        int count = Mathf.Max(1, RunnerManager.Instance.runners.Count);
        float zoom = (Mathf.Log10(Mathf.Log(count+2))+0.36f) * zoomFactor;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        Vector3 targetOffset = baseOffset * zoom;

        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * smoothSpeed);

        transform.position = targetPos + currentOffset;
        transform.rotation = Quaternion.Euler(angle);
    }
}
