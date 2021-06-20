using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Timer : MonoBehaviour
{
    [Header("Counter")]
    private double _from = 0;
    private double _to = 1;
    private bool _repeat = true;
    private float _time = 1;

#if UNITY_EDITOR
    [Range(0, 1)]
    [SerializeField] private double _counter;
#endif

    private UnityEvent<double> _event_update = new UnityEvent<double>();
    private UnityEvent _event_frame = new UnityEvent();

    private bool _event_frame_next = false;
    private Coroutine _update;
    private static Timer _instance;

    /// <summary>
    /// Event, which invoke in every timer update
    /// </summary>
    public static event UnityAction<double> EventUpdate
    {
        add => _instance._event_update.AddListener(value);
        remove => _instance._event_update.RemoveListener(value);
    }
    /// <summary>
    /// Add event, which invoke in next timer frame and clear yourself
    /// </summary>
    public static event UnityAction EventNextFrame
    {
        add { _instance._event_frame.AddListener(value); _instance._event_frame_next = true; }
        remove { _instance._event_frame.RemoveListener(value); _instance._event_frame_next = false; }
    }
    /// <summary>
    /// Modified time to complete all cycle in timer
    /// </summary>
    public static float TimeModify { set => _instance._time = value; }

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        Pause();
    }

    /// <summary>
    /// Invoke play or pause
    /// </summary>
    /// <returns></returns>
    public static bool PlayPause()
    {
        if (Time.timeScale == 1)
            Pause();
        else
            Play();
        return Time.timeScale == 1;
    }
    /// <summary>
    /// Restart timer with start
    /// </summary>
    /// <returns></returns>
    public static void Restart()
    {
        if (_instance._update != null)
        {
            _instance.StopCoroutine(_instance._update);
            _instance._update = null;
        }
        _instance._update = _instance.StartCoroutine(_instance.UpdateCounter());;
    }
    /// <summary>
    /// Continue game
    /// </summary>
    public static void Play()
    {
        Time.timeScale = 1;
        if (_instance._update == null)
            _instance._update = _instance.StartCoroutine(_instance.UpdateCounter());
    }
    /// <summary>
    /// Pause game
    /// </summary>
    public static void Pause()
    {
        Time.timeScale = 0;
    }

    private IEnumerator UpdateCounter()
    {
        for (double i = _from; i < _to; i += Time.deltaTime * (_to - _from) / _time * Time.timeScale)
        {
#if UNITY_EDITOR
            _counter = i;
#endif
            _event_update.Invoke(i);
            if (_event_frame_next)
            {
                _event_frame.Invoke();
                _event_frame.RemoveAllListeners();
                _event_frame_next = false;
            }
            yield return null;
        }
        if (_repeat)
            _update = StartCoroutine(UpdateCounter());
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Timer))]
public class TimerEditor : Editor
{
    override public void OnInspectorGUI()
    {
        //Timer main = (Timer)target;
        if (GUILayout.Button("Restart"))
            Timer.Restart();
        if (GUILayout.Button("Play"))
            Timer.Play();
        if (GUILayout.Button("Pause"))
            Timer.Pause();
        DrawDefaultInspector();
    }
}
#endif