using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    public Player player => _player;
    [SerializeField] private AI _ai;
    public AI ai => _ai;
    [SerializeField] private LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;
    private void Start()
    {
        _player.Init();
        _ai.Init();
        _levelManager.Init();
    }
}
