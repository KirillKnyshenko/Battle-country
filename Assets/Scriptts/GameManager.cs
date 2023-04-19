using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum State {
        WaitingToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;

    [SerializeField] private LevelManager _levelManager;
    [SerializeField] public LevelManager levelManager => _levelManager;

    private void Start()
    {
        _state = State.WaitingToStart;
        _levelManager.Init();
        StartCoroutine(GameManagerUpdate());
    }

    private IEnumerator GameManagerUpdate() {
        while (true)
        {
            switch (_state)
            {
                case State.WaitingToStart:

                    _state = State.GamePlaying;
                break;
                case State.GamePlaying:

                    _state = State.GameOver;
                break;
                case State.GameOver:

                break;
            }
            yield return null;
            Debug.Log(_state);
        }
    }
}
