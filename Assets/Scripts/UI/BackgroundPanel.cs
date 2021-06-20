using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

class BackgroundPanel
{
    private readonly Image _image;
    private readonly Button.ButtonClickedEvent _event;
    private UnityAction _listener;

    public BackgroundPanel(Transform tr, UnityAction act)
    {
        _listener = act;
        _image = tr.GetComponent<Image>();
        _event = tr.GetComponent<Button>().onClick;
        _event.AddListener(_listener);
    }

    public void Update(UnityAction act)
    {
        _event.RemoveListener(_listener);
        _listener = act;
        _event.AddListener(_listener);
    }

    public Color Active
    {
        get => _image.color;
        set => _image.color = value;
    }
}
