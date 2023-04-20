using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private enum State {
        WaitingToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;
    private int _level;
    public int level => _level;

    [SerializeField] private LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;
    
    [SerializeField] private SaveDataSO _saveData;
    [SerializeField] private LevelListSO _levelListSO;

    public UnityEvent OnNextLevel;

    private void Start()
    {
        _state = State.WaitingToStart;
        _levelManager.Init(this);
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

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _state = State.GameOver;
                    }
                break;
                case State.GameOver:
                    NextLevel();
                break;
            }
            yield return null;
        }
    }

    public void NextLevel() {
        OnNextLevel?.Invoke();
        _levelListSO.GetNextLevel();
        _saveData.Save();
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel() {
        SceneManager.LoadScene(_levelListSO.GetCurrentLevelName());
    }

    public bool IsGameOver() {
        return _state == State.GameOver;
    }
}
