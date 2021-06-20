using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

class InputFieldPanel
{
    private readonly TMP_InputField _input;
    private readonly TMP_InputField.OnChangeEvent _event;
    private UnityAction<string> _listener;

    public InputFieldPanel(Transform tr, UnityAction<string> act)
    {
        _listener = act;
        _input = tr.GetComponent<TMP_InputField>();
        _event = _input.onValueChanged;
        _event.AddListener(_listener);
    }

    public void Update(UnityAction<string> act)
    {
        _event.RemoveListener(_listener);
        _listener = act;
        _event.AddListener(_listener);
    }
}
