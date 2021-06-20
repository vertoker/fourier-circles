using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FourierSeries
{
    private static List<double> _speed = new List<double>();// of rotating
    private static List<double> _radius = new List<double>();// of circle
    private static List<double> _offset = new List<double>();// of start angle
    private static int _count = 0;// of circles

    /// <summary>
    /// Add vector
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="radius"></param>
    /// <param name="offset"></param>
    public static void Add(double speed, double radius, double offset)
    {
        _speed.Add(speed);
        _radius.Add(radius);
        _offset.Add(offset);
        _count++;
    }

    public static void ModifySpeed(int id, double speed)
    {
        _speed[id] = speed;
    }
    public static void ModifyRadius(int id, double radius)
    {
        _radius[id] = radius;
    }
    public static void ModifyOffset(int id, double offset)
    {
        _offset[id] = offset;
    }

    /// <summary>
    /// Exchange vectors
    /// </summary>
    /// <param name="id">0 <= id < length - 1</param>
    public static void Exchange(int id)
    {
        if (id < 0 || _count - 2 < id)
            return;

        double temp = _speed[id];
        _speed[id] = _speed[id + 1];
        _speed[id + 1] = temp;
        temp = _radius[id];
        _radius[id] = _radius[id + 1];
        _radius[id + 1] = temp;
        temp = _offset[id];
        _offset[id] = _offset[id + 1];
        _offset[id + 1] = temp;
    }
    /// <summary>
    /// Add vector
    /// </summary>
    /// <param name="id"></param>
    public static void Remove(int id)
    {
        if (_count > 0)
        {
            _speed.RemoveAt(id);
            _radius.RemoveAt(id);
            _offset.RemoveAt(id);
            _count--;
        }
    }
    public static void Clean()
    {
        _speed = new List<double>();
        _radius = new List<double>();
        _offset = new List<double>();
        _count = 0;
    }

    /// <summary>
    /// Main calculate function for Fourier Circles
    /// </summary>
    /// <param name="x"><param name="y">Position parameter, which being a start parameter and return next modified position</param></param>
    /// <param name="speed">Speed of rotating</param>
    /// <param name="radius">Size of circle</param>
    /// <param name="offset">Start angle of rotating</param>
    /// <param name="time">Timer (from 0 to 1)</param>
    /// <returns></returns>
    public static void Calculate(ref double x, ref double y, double speed, double radius, double offset, double time)
    {
        double angle = (speed * time + offset) * 2 * Math.PI;
        //Debug.Log(string.Join(" ", speed, radius, offset, angle, time));
        x += radius * Math.Cos(angle);
        y += radius * Math.Sin(angle);
    }

    public static Vector2 GetPosition(double t, ref List<Arrow> arrows)
    {
        if (arrows.Count == 0 || _count == 0)
            return Vector2.zero;

        double xs = 0, ys = 0;
        for (int i = 0; i < _count; i++)
        {
            double xe = xs, ye = ys;
            Calculate(ref xe, ref ye, _speed[i], _radius[i], _offset[i], t);
            arrows[i].Transformation(xs, ys, xe, ye);
            xs = xe; ys = ye;
        }
        return new Vector2((float)xs, (float)ys);
    }
    public static Vector2 GetPosition(double t)
    {
        if (_count == 0)
            return Vector2.zero;
        double x = 0, y = 0;
        for (int i = 0; i < _count; i++)
        {
            Calculate(ref x, ref y, _speed[i], _radius[i], _offset[i], t);
        }
        return new Vector2((float)x, (float)y);
    }
}