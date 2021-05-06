using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
    public GameObject instance;
    public Transform parent;
    public List<Transform> arrows;
    public Transform final;
    public TrailRenderer trail;
    public MainProcess process;
    public FourierMath math = new FourierMath();
    private bool updated = true, debug = false;
    private int length = 0;

    public bool DebugReverse()
    {
        debug = !debug;
        parent.gameObject.SetActive(debug);
        return debug;
    }

    public void Add(double s, double k, double a)
    {
        math.Add(s, k, a);
        arrows.Add(Instantiate(instance, parent).transform);
        length++;
        trail.Clear();
    }

    public void ModSpeed(int id, double s)
    {
        math.ModSpeed(id, s);
        trail.Clear();
    }
    public void ModCoefficient(int id, double k)
    {
        math.ModCoefficient(id, k);
        trail.Clear();
    }
    public void ModAngle(int id, double a)
    {
        math.ModAngle(id, a);
        trail.Clear();
    }

    public void ChangePos(int id)
    {
        Transform temp = arrows[id];
        arrows[id] = arrows[id + 1];
        arrows[id + 1] = temp;
        math.ChangePos(id);
    }


    public void Remove(int id)
    {
        math.Remove(id);
        Destroy(arrows[id].gameObject);
        arrows.RemoveAt(id);
        length--;
        trail.Clear();
    }

    public void Clear()
    {
        math.Clear();
        for (int i = 0; i < length; i++)
        {
            Destroy(arrows[i].gameObject);
        }
        arrows.Clear();
        trail.Clear();
    }

    public void UpdateFourier(double t)
    {
        if (updated)
        {
            if (debug)
                final.position = math.GetVectorTransforms(t, ref arrows);
            else
                final.position = math.GetPosition(t);
        }
    }
}