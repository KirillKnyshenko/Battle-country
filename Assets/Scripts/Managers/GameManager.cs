using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private enum State {
        TapToStart,
        TutorialToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;
    private int _level;
    public int level => _level;

    [SerializeField] private LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;

    [SerializeField] private UIManager _UIManager;
    public UIManager UIManager => _UIManager;

    [SerializeField] private SaveDataSO _saveData;
    [SerializeField] private LevelListSO _levelListSO;
    
    public UnityEvent OnNextLevel;
    public UnityEvent OnTapToStart;
    public UnityEvent OnTutorialToStart;
    public UnityEvent OnGameStarted;
    public UnityEvent OnGameOver;

    private void Start()
    {
        _state = State.TapToStart;
        _levelManager.Init(this);

        int currentLevel = _saveData.level + (_levelListSO.levels.Count * (_saveData.loop - 1));

        _level = currentLevel;

        _UIManager.Init(this, _level);
        StartCoroutine(GameManagerUpdate());
    }

    private IEnumerator GameManagerUpdate() {
        while (true)
        {
            switch (_state)
            {
                case State.TapToStart:

                break;
                case State.TutorialToStart:
                    
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

    public void StartTutorialToStart() {
        _state = State.TutorialToStart;
        OnTutorialToStart?.Invoke();
    }

    public void StartGamePlaying() {
        _state = State.GamePlaying;
        OnGameStarted?.Invoke();
    }

    public void LoadCurrentLevel() {
        SceneManager.LoadScene(_levelListSO.GetCurrentLevelName());
    }

    public bool IsTutorialToStart() {
        return _state == State.TutorialToStart;
    }

    public bool IsGamePlaying() {
        return _state == State.GamePlaying;
    }

    public bool IsGameOver() {
        return _state == State.GameOver;
    }
}
