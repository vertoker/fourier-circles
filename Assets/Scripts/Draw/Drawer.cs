using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private Transform instance;
    [SerializeField] private Transform parent;
    private List<Arrow> arrows = new List<Arrow>();
    [SerializeField] private Transform final;
    [SerializeField] private TrailRenderer trail;
    private bool debug = false;
    private int length = 0;

    private void Start()
    {
        Clean();//Invoke it because it's bug I guess
        Timer.EventUpdate += ArrowsUpdate;
    }

    public bool DebugReverse()
    {
        debug = !debug;
        parent.gameObject.SetActive(debug);
        return debug;
    }

    public void UpdateTime(string value)
    {
        if (float.TryParse(value, out float num))
        {
            Timer.TimeModify = num;
            trail.time = num + 0.1f;
            Clear();
        }
    }

    public void Add(double s, double k, double a)
    {
        FourierSeries.Add(s, k, a);
        arrows.Add(new Arrow(instance, parent));
        length++;
    }

    public void Exchange(int id)
    {
        FourierSeries.Exchange(id);
        Arrow temp = arrows[id];
        arrows[id] = arrows[id + 1];
        arrows[id + 1] = temp;
    }

    public void Remove(int id)
    {
        FourierSeries.Remove(id);
        arrows[id].DestroyArrow();
        arrows.RemoveAt(id);
        length--;
    }
    public void Clean()
    {
        FourierSeries.Clean();
        for (int i = 0; i < length; i++)
            arrows[i].DestroyArrow();
        arrows.Clear();
        Clear();
    }
    public void Clear()
    {
        trail.Clear();
    }
    public UnityAction ClearAction => Clear;

    public void ArrowsUpdate(double time)
    {
        if (debug)
            final.position = FourierSeries.GetPosition(time, ref arrows);
        else
            final.position = FourierSeries.GetPosition(time);
    }
}