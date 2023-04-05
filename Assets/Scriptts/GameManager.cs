using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    public Player player => _player;
    [SerializeField] private LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;
    private void Start()
    {
        _player.Init();
        _levelManager.Init();
    }
}
