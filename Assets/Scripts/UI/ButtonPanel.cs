using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

class ButtonPanel
{
    private readonly Button.ButtonClickedEvent _event;
    private readonly Image _image;
    private UnityAction _listener;

    public ButtonPanel(Transform tr, UnityAction act)
    {
        _listener = act;
        _event = tr.GetComponent<Button>().onClick;
        _image = tr.GetChild(0).GetComponent<Image>();
        _event.AddListener(_listener);
    }

    public void Update(UnityAction act)
    {
        _event.RemoveListener(_listener);
        _listener = act;
        _event.AddListener(_listener);
    }

    public void ToggleImage(bool value)
    {
        _image.enabled = value;
    }
}