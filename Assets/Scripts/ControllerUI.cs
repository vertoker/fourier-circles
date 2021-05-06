using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ControllerUI : MonoBehaviour
{
    public Color active, passive;
    public Arrows arrows;
    public Transform content;
    public GameObject settings;
    public MainProcess process;
    public GameObject instance;
    private List<ButCircle> buts = new List<ButCircle>();
    private int selectID = -1, length = 0;

    public TMP_InputField time;
    public Transform show_hideTR, play_pauseTR;
    public Transform removeTR, appendTR;
    public Transform restartTR, debugTR;

    private But show_hide, play_pause;
    private But remove, append;
    private But restart, debug;
    private RectTransform contentRect;

    private void Start()
    {
        show_hide = new But(show_hideTR, ShowHide);
        play_pause = new But(play_pauseTR, PlayPause);
        remove = new But(removeTR, Remove);
        append = new But(appendTR, Add);
        restart = new But(restartTR, Restart);
        debug = new But(debugTR, DebugToggle);
        contentRect = content.GetComponent<RectTransform>();
        time.onValueChanged.AddListener((string str) => arrows.process.UpdateTime(str));
    }

    public void Add()
    {
        Vector2 pos = new Vector2(0, -100 - 200 * length);
        Transform tr = Instantiate(instance, content).transform;
        tr.GetComponent<RectTransform>().anchoredPosition = pos;

        int id = length;
        UnityAction select = () => Select(id),
            remove = () => Remove(id),
            up = () => Up(id),
            down = () => Down(id);
        UnityAction<string> s = (string str) => UpdateValueSpeed(str, id),
            k = (string str) => UpdateValueRadius(str, id),
            a = (string str) => UpdateValueAngle(str, id);
        buts.Add(new ButCircle(tr, select, remove, up, down, s, k, a));
        arrows.Add(0, 0, 0);
        tr.gameObject.SetActive(true);
        length++;
        UpdateLength();
    }
    public void Remove()
    {
        if (length == 0)
            return;
        int id = selectID;
        if (selectID == -1)
            id = length - 1;
        Remove(id);
        selectID = -1;
    }
    public void PlayPause()
    {
        if (process.PlayPause())
        {
            play_pauseTR.GetChild(1).GetComponent<Image>().enabled = false;
            play_pauseTR.GetChild(0).GetComponent<Image>().enabled = true;
        }
        else
        {
            play_pauseTR.GetChild(0).GetComponent<Image>().enabled = false;
            play_pauseTR.GetChild(1).GetComponent<Image>().enabled = true;
        }
    }
    public void DebugToggle()
    {
        debug.ToggleImage(arrows.DebugReverse());
    }
    public void Restart()
    {
        process.StartDraw();
    }
    public void ShowHide()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void Select(int id)
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
    public void Remove(int id)
    {
        for (int i = id + 1; i < length; i++)
        {
            UpdateID(i, i - 1);
            buts[i].MoveUp();
        }
        if (id == selectID)
            Select(selectID);
        else if (id < selectID)
            selectID--;
        buts[id].DestroyBut();
        buts.RemoveAt(id);
        arrows.Remove(id);
        length--;
        UpdateLength();
    }
    public void Up(int id)
    {
        if (id == 0)
            return;
        Select(selectID);
        buts[id].MoveUp();
        buts[id - 1].MoveDown();
        UpdateID(id, id - 1);
        UpdateID(id - 1, id);
        arrows.ChangePos(id - 1);

        ButCircle temp = buts[id - 1];
        buts[id - 1] = buts[id];
        buts[id] = temp;
    }
    public void Down(int id)
    {
        if (id + 1 == length)
            return;
        Select(selectID);
        buts[id].MoveDown();
        buts[id + 1].MoveUp();
        UpdateID(id, id + 1);
        UpdateID(id + 1, id);
        arrows.ChangePos(id);

        ButCircle temp = buts[id];
        buts[id] = buts[id + 1];
        buts[id + 1] = temp;
    }

    public void UpdateLength()
    {
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 200 * length + 100);
    }
    public void UpdateID(int i, int index)
    {
        UnityAction select = () => Select(index),
            remove = () => Remove(index),
            up = () => Up(index),
            down = () => Down(index);
        UnityAction<string> s = (string str) => UpdateValueSpeed(str, index),
            k = (string str) => UpdateValueRadius(str, index),
            a = (string str) => UpdateValueAngle(str, index);
        buts[i].ChangeID(select, remove, up, down, s, k, a);
    }
    public void UpdateValueSpeed(string value, int id)
    {
        if (value == string.Empty)
            arrows.ModSpeed(id, 0);
        else if (float.TryParse(value.Replace('.', ','), out float num))
            arrows.ModSpeed(id, num);
    }
    public void UpdateValueRadius(string value, int id)
    {
        if (value == string.Empty)
            arrows.ModCoefficient(id, 0);
        if (float.TryParse(value.Replace('.', ','), out float num))
            arrows.ModCoefficient(id, num);
    }
    public void UpdateValueAngle(string value, int id)
    {
        if (value == string.Empty)
            arrows.ModAngle(id, 0);
        if (float.TryParse(value.Replace('.', ','), out float num))
            arrows.ModAngle(id, num);
    }
}

public class But
{
    private Button.ButtonClickedEvent but;
    private RectTransform tr;
    private RectTransform im;
    private UnityAction listener;

    public But(Transform tr, UnityAction act)
    {
        listener = act;
        this.tr = tr.GetComponent<RectTransform>();
        but = tr.GetComponent<Button>().onClick;
        im = tr.GetChild(0).GetComponent<RectTransform>();
        but.AddListener(listener);
    }

    public void ChangeID(UnityAction act)
    {
        but.RemoveListener(listener);
        listener = act;
        but.AddListener(listener);
    }

    public void ToggleImage(bool value)
    {
        im.GetComponent<Image>().enabled = value;
    }
}

public class ButCircle
{
    private RectTransform tr;
    private Image selectImage;
    private Button selectBut;
    private But deleteBut, upBut, downBut;
    private TMP_InputField speedInput, radiusInput, angleInput;
    private Button.ButtonClickedEvent but;
    private TMP_InputField.OnChangeEvent speedEvent, radiusEvent, angleEvent;
    private UnityAction<string> listenerSpeed, listenerRadius, listenerAngle;
    private UnityAction listenerSelect;

    public ButCircle(Transform tr, UnityAction select, UnityAction delete, UnityAction up, UnityAction down, UnityAction<string> speed, UnityAction<string> radius, UnityAction<string> angle)
    {
        this.tr = tr.GetComponent<RectTransform>();
        selectImage = tr.GetComponent<Image>();
        selectBut = tr.GetComponent<Button>();
        but = selectBut.onClick;
        but.AddListener(select);
        listenerSelect = select;

        deleteBut = new But(tr.GetChild(0), delete);
        upBut = new But(tr.GetChild(1), up);
        downBut = new But(tr.GetChild(2), down);

        speedInput = tr.GetChild(3).GetComponent<TMP_InputField>();
        radiusInput = tr.GetChild(4).GetComponent<TMP_InputField>();
        angleInput = tr.GetChild(5).GetComponent<TMP_InputField>();
        speedEvent = speedInput.onValueChanged;
        radiusEvent = radiusInput.onValueChanged;
        angleEvent = angleInput.onValueChanged;
        speedEvent.AddListener(speed);
        radiusEvent.AddListener(radius);
        angleEvent.AddListener(angle);
        listenerSpeed = speed;
        listenerRadius = radius;
        listenerAngle = angle;
    }

    public void DestroyBut()
    {
        Object.Destroy(tr.gameObject);
    }

    public void ChangeID(UnityAction select, UnityAction delete, UnityAction up, UnityAction down, UnityAction<string> speed, UnityAction<string> radius, UnityAction<string> angle)
    {
        but.RemoveListener(listenerSelect);
        speedEvent.RemoveListener(listenerSpeed);
        radiusEvent.RemoveListener(listenerRadius);
        angleEvent.RemoveListener(listenerAngle);
        listenerSelect = select;
        listenerSpeed = speed;
        listenerRadius = radius;
        listenerAngle = angle;
        but.AddListener(listenerSelect);
        speedEvent.AddListener(listenerSpeed);
        radiusEvent.AddListener(listenerRadius);
        angleEvent.AddListener(listenerAngle);

        deleteBut.ChangeID(delete);
        upBut.ChangeID(up);
        downBut.ChangeID(down);
    }
    public void MoveUp()
    {
        tr.anchoredPosition = new Vector2(0, tr.anchoredPosition.y + 200);
    }
    public void MoveDown()
    {
        tr.anchoredPosition = new Vector2(0, tr.anchoredPosition.y - 200);
    }
    public Color Active
    {
        get
        {
            return selectImage.color;
        }
        set
        {
            selectImage.color = value;
        }
    }
}