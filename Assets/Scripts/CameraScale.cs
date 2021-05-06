using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraScale : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Transform tr;
    public Camera cam;
    public RectTransform view;
    private Vector2 startPoint;
    private Vector3 origPos;

    public void Start()
    {
        view.sizeDelta = new Vector2(view.sizeDelta.x, 540f / cam.aspect);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.pointerCurrentRaycast.screenPosition;
        origPos = tr.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 offset = eventData.pointerCurrentRaycast.screenPosition - startPoint;
        tr.position = origPos - offset / Mathf.Abs(100f - cam.orthographicSize * 2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

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
