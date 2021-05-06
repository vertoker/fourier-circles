using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierMath
{
    private List<double> secs, kofs, angs;
    private int length;

    public FourierMath()
    {
        secs = new List<double>();
        kofs = new List<double>();
        angs = new List<double>();
        length = 0;
    }

    public FourierMath(List<double> s, List<double> k, List<double> a)
    {
        length = s.Count;
        secs = s;
        kofs = k;
        angs = a;
    }

    public FourierMath(double[] s, double[] k, double[] a)
    {
        length = s.Length;
        for (int i = 0; i < length; i++)
        {
            secs.Add(s[i]);
            kofs.Add(k[i]);
            angs.Add(a[i]);
        }
    }

    public void Add(double s, double k, double a)
    {
        secs.Add(s);
        kofs.Add(k);
        angs.Add(a);
        length++;
    }

    public void ModSpeed(int id, double s)
    {
        secs[id] = s;
    }
    public void ModCoefficient(int id, double k)
    {
        kofs[id] = k;
    }
    public void ModAngle(int id, double a)
    {
        angs[id] = a;
    }

    /// <param name="id">0 <= id < length - 1</param>
    /// <returns></returns>
    public void ChangePos(int id)
    {
        double temp = secs[id];
        secs[id] = secs[id + 1];
        secs[id + 1] = temp;
        temp = kofs[id];
        kofs[id] = kofs[id + 1];
        kofs[id + 1] = temp;
        temp = angs[id];
        angs[id] = angs[id + 1];
        angs[id + 1] = temp;
    }

    public void Remove(int id)
    {
        secs.RemoveAt(id);
        kofs.RemoveAt(id);
        angs.RemoveAt(id);
        length--;
    }

    public void Clear()
    {
        secs = new List<double>();
        kofs = new List<double>();
        angs = new List<double>();
        length = 0;
    }

    /// <summary>
    /// Main calculate function for Fourier Circles
    /// </summary>
    /// <param name="s">Speed of rotating</param>
    /// <param name="t">Timer (from 0 to 1)</param>
    /// <param name="k">Coiffecent or size of circle</param>
    /// <param name="a">Start angle (from 0 to 1)</param>
    /// <returns></returns>
    public const double PI2ANGLE = 57.2957795130823209d;
    public static void Calculate(ref double x, ref double y, double s, double k, double a, double t)
    {
        double p = (s * t + a) * 2 * Math.PI;
        //Debug.Log(s.ToString() + " " + k.ToString() + " " + a.ToString() + " " + p.ToString());
        x += k * Math.Cos(p);
        y += k * Math.Sin(p);
    }
    public static void ArrowTransform(double xs, double ys, double xe, double ye, ref Transform tr)
    {
        float posX = (float)(xe - xs), posY = (float)(ye - ys);
        float angle = (float)(Math.Atan2(posY, posX) * PI2ANGLE) - 90f;
        float sca = (float)Math.Sqrt(posX * posX + posY * posY);
        float middlePosX = (float)xs + posX / 2f;
        float middlePosY = (float)ys + posY / 2f;

        tr.localPosition = new Vector3(middlePosX, middlePosY, 0);
        tr.localEulerAngles = new Vector3(0, 0, angle);
        tr.GetComponent<SpriteRenderer>().size = new Vector2(1, sca);
    }

    public Vector2 GetVectorTransforms(double t, ref List<Transform> arrows)
    {
        if (arrows.Count == 0 || length == 0)
            return Vector2.zero;

        double xs = 0, ys = 0;
        for (int i = 0; i < length; i++)
        {
            double xe = xs, ye = ys;
            Transform tr = arrows[i];
            Calculate(ref xe, ref ye, secs[i], kofs[i], angs[i], t);
            ArrowTransform(xs, ys, xe, ye, ref tr);
            xs = xe; ys = ye;
        }
        return new Vector2((float)xs, (float)ys);
    }

    public Vector2 GetPosition(double t)
    {
        if (length == 0)
            return Vector2.zero;
        double x = 0, y = 0;
        for (int i = 0; i < length; i++)
        {
            Calculate(ref x, ref y, secs[i], kofs[i], angs[i], t);
        }
        return new Vector2((float)x, (float)y);
    }
}