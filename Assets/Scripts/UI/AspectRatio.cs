using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    [SerializeField] private RectTransform view;
    [SerializeField] private Camera cam;

    private void Start()
    {
        view.sizeDelta = new Vector2(view.sizeDelta.x, 540f / cam.aspect);
    }
}
