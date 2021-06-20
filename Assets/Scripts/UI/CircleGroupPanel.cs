using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CircleGroupPanel
{
    private readonly RectTransform _tr;
    private readonly BackgroundPanel _background_button;
    private readonly ButtonPanel _up_button, _down_button;
    private readonly InputFieldPanel _speed_field, _radius_field, _offset_field;

    public CircleGroupPanel(Transform tr, UnityAction select, UnityAction up, UnityAction down, UnityAction<string> speed, UnityAction<string> radius, UnityAction<string> offset)
    {
        _tr = tr.GetComponent<RectTransform>();

        _background_button = new BackgroundPanel(tr, select);
        _up_button = new ButtonPanel(tr.GetChild(0), up);
        _down_button = new ButtonPanel(tr.GetChild(1), down);

        _speed_field = new InputFieldPanel(tr.GetChild(2), speed);
        _radius_field = new InputFieldPanel(tr.GetChild(3), radius);
        _offset_field = new InputFieldPanel(tr.GetChild(4), offset);
    }

    public void DestroyBut()
    {
        Object.Destroy(_tr.gameObject);
    }

    public void UpdateGroup(UnityAction select, UnityAction up, UnityAction down, UnityAction<string> speed, UnityAction<string> radius, UnityAction<string> offset)
    {
        _background_button.Update(select);
        _up_button.Update(up);
        _down_button.Update(down);

        _speed_field.Update(speed);
        _radius_field.Update(radius);
        _offset_field.Update(offset);
    }
    public void MoveUp()
    {
        _tr.anchoredPosition = new Vector2(0, _tr.anchoredPosition.y + ControllerUI.PANEL_SIZE);
    }
    public void MoveDown()
    {
        _tr.anchoredPosition = new Vector2(0, _tr.anchoredPosition.y - ControllerUI.PANEL_SIZE);
    }
    public Color Active
    {
        get => _background_button.Active;
        set => _background_button.Active = value;
    }
}
