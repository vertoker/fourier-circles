using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraScale : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform tr;
    [SerializeField] private Camera cam;
    private Vector2 _start_point;
    private Vector3 _orig_pos;

    public void OnPointerDown(PointerEventData eventData)
    {
        _start_point = eventData.pointerCurrentRaycast.screenPosition;
        _orig_pos = tr.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 offset = eventData.pointerCurrentRaycast.screenPosition - _start_point;
        tr.position = _orig_pos - offset / Mathf.Abs(100f - cam.orthographicSize * 2f);
    }

    public void ScaleDown()
    {
        if (cam.orthographicSize == 2)
            return;
        cam.orthographicSize -= 1;
    }
    public void ScaleUp()
    {
        cam.orthographicSize += 1;
    }
}
