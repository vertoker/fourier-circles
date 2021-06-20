using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ControllerUI : MonoBehaviour
{
    [SerializeField] private Color active, passive;
    [SerializeField] private Drawer drawer;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject instance;
    private List<CircleGroupPanel> buts = new List<CircleGroupPanel>();
    private int selectID = -1, length = 0;

    [SerializeField] private TMP_InputField time;
    [SerializeField] private Transform show_hide, play_pause;
    [SerializeField] private Transform remove, append;
    [SerializeField] private Transform restart, debug;

    private ButtonPanel _show_hide, _play_pause;
    private ButtonPanel _remove, _append;
    private ButtonPanel _restart, _debug;
    private RectTransform _content_rect;

    public const float PANEL_SIZE = 200;
    private const float START_TIME = 5f;

    private void Start()
    {
        _show_hide = new ButtonPanel(show_hide, ShowHide);
        _play_pause = new ButtonPanel(play_pause, PlayPause);
        _remove = new ButtonPanel(remove, Remove);
        _append = new ButtonPanel(append, Add);
        _restart = new ButtonPanel(restart, () => { Timer.Restart(); drawer.Clear(); });
        _debug = new ButtonPanel(debug, DebugToggle);
        _content_rect = content.GetComponent<RectTransform>();
        time.onValueChanged.AddListener((string str) => drawer.UpdateTime(str));
        drawer.UpdateTime(START_TIME.ToString());
        time.text = START_TIME.ToString();
    }

    private void Add()
    {
        Vector2 pos = new Vector2(0, -PANEL_SIZE / 2 - PANEL_SIZE * length);
        Transform tr = Instantiate(instance, content).transform;
        tr.GetComponent<RectTransform>().anchoredPosition = pos;

        int id = length;
        UnityAction select = () => Select(id),
            up = () => Up(id),
            down = () => Down(id);
        UnityAction<string> speed = (string str) => UpdateValueSpeed(str, id),
            radius = (string str) => UpdateValueRadius(str, id),
            offset = (string str) => UpdateValueOffset(str, id);
        buts.Add(new CircleGroupPanel(tr, select, up, down, speed, radius, offset));
        tr.gameObject.SetActive(true);
        drawer.Add(0, 0, 0);
        length++;
        UpdateLength();
        Timer.EventNextFrame += drawer.ClearAction;
    }
    private void Remove()
    {
        if (length == 0)
            return;
        int id = selectID;
        if (selectID == -1)
            id = length - 1;
        Remove(id);
        selectID = -1;
    }
    private void PlayPause()
    {
        bool play = Timer.PlayPause();
        play_pause.GetChild(0).GetComponent<Image>().enabled = play;
        play_pause.GetChild(1).GetComponent<Image>().enabled = !play;
    }
    private void DebugToggle()
    {
        _debug.ToggleImage(drawer.DebugReverse());
    }
    private void ShowHide()
    {
        settings.SetActive(!settings.activeSelf);
    }

    private void Select(int id)
    {
        if (selectID == -1)
        {
            if (selectID == id)
                return;
            selectID = id;
            buts[selectID].Active = active;
        }
        else if (selectID == id)
        {
            buts[selectID].Active = passive;
            selectID = -1;
        }
        else
        {
            buts[selectID].Active = passive;
            selectID = id;
            buts[selectID].Active = active;
        }
    }
    private void Remove(int id)
    {
        for (int i = id + 1; i < length; i++)
        {
            UpdateGroup(i, i - 1);
            buts[i].MoveUp();
        }
        if (id == selectID)
            Select(selectID);
        else if (id < selectID)
            selectID--;
        buts[id].DestroyBut();
        buts.RemoveAt(id);
        drawer.Remove(id);
        length--;
        UpdateLength();
        Timer.EventNextFrame += drawer.ClearAction;
    }
    private void Up(int id)
    {
        if (id == 0)
            return;
        Select(selectID);
        buts[id].MoveUp();
        buts[id - 1].MoveDown();
        UpdateGroup(id, id - 1);
        UpdateGroup(id - 1, id);
        drawer.Exchange(id - 1);

        CircleGroupPanel temp = buts[id - 1];
        buts[id - 1] = buts[id];
        buts[id] = temp;
    }
    private void Down(int id)
    {
        if (id + 1 == length)
            return;
        Select(selectID);
        buts[id].MoveDown();
        buts[id + 1].MoveUp();
        UpdateGroup(id, id + 1);
        UpdateGroup(id + 1, id);
        drawer.Exchange(id);

        CircleGroupPanel temp = buts[id];
        buts[id] = buts[id + 1];
        buts[id + 1] = temp;
    }

    private void UpdateLength()
    {
        _content_rect.sizeDelta = new Vector2(_content_rect.sizeDelta.x, 200 * length + 100);
    }
    private void UpdateGroup(int i, int index)
    {
        UnityAction select = () => Select(index),
            up = () => Up(index),
            down = () => Down(index);
        UnityAction<string> speed = (string str) => UpdateValueSpeed(str, index),
            radius = (string str) => UpdateValueRadius(str, index),
            offset = (string str) => UpdateValueOffset(str, index);
        buts[i].UpdateGroup(select, up, down, speed, radius, offset);
    }
    private void UpdateValueSpeed(string value, int id)
    {
        if (value == string.Empty)
            FourierSeries.ModifySpeed(id, 0);
        else if (float.TryParse(value.Replace('.', ','), out float num))
            FourierSeries.ModifySpeed(id, num);
        Timer.EventNextFrame += drawer.ClearAction;
    }
    private void UpdateValueRadius(string value, int id)
    {
        if (value == string.Empty)
            FourierSeries.ModifyRadius(id, 0);
        if (float.TryParse(value.Replace('.', ','), out float num))
            FourierSeries.ModifyRadius(id, num);
        Timer.EventNextFrame += drawer.ClearAction;
    }
    private void UpdateValueOffset(string value, int id)
    {
        if (value == string.Empty)
            FourierSeries.ModifyOffset(id, 0);
        if (float.TryParse(value.Replace('.', ','), out float num))
            FourierSeries.ModifyOffset(id, num);
        Timer.EventNextFrame += drawer.ClearAction;
    }
}