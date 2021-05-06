using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainProcess : MonoBehaviour
{
    [Header("Counter")]
    public double from = 0;
    public double to = 1;
    public bool repeat = true;
    public float time = 1;
#if UNITY_EDITOR
    [Range(0, 1)]
    [SerializeField] private double counter;
#endif
    private Coroutine updater;
    public Arrows arrows;

    private void Start()
    {
#if UNITY_EDITOR
        counter = from;
#endif
        StopDraw();
    }
    public bool PlayPause()
    {
        if (Time.timeScale == 1)
            StopDraw();
        else
            ContinueDraw();
        return Time.timeScale == 1;
    }
    public void StartDraw()
    {
        if (updater != null)
        {
            StopCoroutine(updater);
            updater = null;
        }
#if UNITY_EDITOR
        counter = from;
#endif
        updater = StartCoroutine(UpdateCounter());
        arrows.trail.Clear();
    }
    public void ContinueDraw()
    {
        Time.timeScale = 1;
        if (updater == null)
            updater = StartCoroutine(UpdateCounter());
    }
    public void StopDraw()
    {
        Time.timeScale = 0;
    }

    private IEnumerator UpdateCounter()
    {
        for (double i = from; i < to; i += Time.deltaTime * (to - from) / time * Time.timeScale)
        {
#if UNITY_EDITOR
            counter = i;
#endif
            arrows.UpdateFourier(i);
            yield return null;
        }
        if (repeat)
        {
            //arrows.trail.emitting = false;
            updater = StartCoroutine(UpdateCounter());
        }
    }

    public void UpdateTime(string value)
    {
        if (float.TryParse(value, out float num))
        {
            time = num;
            arrows.trail.time = time + 1f;
            arrows.trail.Clear();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MainProcess))]
public class ColliderCreatorEditor : Editor
{
    override public void OnInspectorGUI()
    {
        MainProcess main = (MainProcess)target;
        if (GUILayout.Button("Start"))
        {
            main.StartDraw();
        }
        if (GUILayout.Button("Continue"))
        {
            main.ContinueDraw();
        }
        if (GUILayout.Button("Stop"))
        {
            main.StopDraw();
        }
        DrawDefaultInspector();
    }
}
#endif