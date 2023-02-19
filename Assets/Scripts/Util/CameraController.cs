using UnityEngine;
using DG.Tweening;

/// <summary>
/// 카메라 연출을 담당하는 class
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform thisTransform;
    public Transform camTransform;
    public Camera cam;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        originOrthographic = cam.orthographicSize;
    }

    public void SetCameraPosition(Vector3 nothing)
    {
        thisTransform.DOKill();
        thisTransform.position = nothing;
    }

    float originOrthographic = 0;
    [System.NonSerialized]
    public float targetZoomValue = 1;

    public void SetZoom(float zoomValue)
    {
        targetZoomValue = zoomValue;
        cam.orthographicSize = originOrthographic/zoomValue;
    }

    public Tween DoSetZoom(float zoomValue, float dur, Ease ease = Ease.OutQuad)
    {
        targetZoomValue = zoomValue;
        return cam.DOOrthoSize(originOrthographic / zoomValue, dur).SetEase(ease);
    }
}
