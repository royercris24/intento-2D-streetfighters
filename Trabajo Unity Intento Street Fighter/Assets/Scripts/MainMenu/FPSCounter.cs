using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < _counterFPS.Length; i++)
        {
            _counterFPS[i] = 60;
        }
    }

    private void Update()
    {
        _counter++;
        _timer += Time.deltaTime;

        if (_timer > 1f)
        {
            _timer -= 1f;

            _currentFPS = _counter;

            _counter = 0;

            _counterFPS[_index] = _currentFPS;
            _index = ++_index % _counterFPS.Length;

            for (int i = 0; i < _counterFPS.Length; i++)
            {
                _avarageFPS += _counterFPS[i];
            }

            _avarageFPS /= _counterFPS.Length;


            _text.text = _currentFPS + " / " + _avarageFPS.ToString("00.00");
        }
    }

    private const int TotalSeconds = 60;

    [SerializeField]
    private Text _text = null;

    private int _counter;

    private float _timer;

    private int _currentFPS;
    private float _avarageFPS;
    private int[] _counterFPS = new int[TotalSeconds];
    private int _index;
}