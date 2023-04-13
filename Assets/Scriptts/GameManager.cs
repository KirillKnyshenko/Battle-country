using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] public LevelManager levelManager => _levelManager;

    private void Start()
    {
        _levelManager.Init();
    }
}
