using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    public GameManager gameManager => _gameManager;
    [SerializeField] private List<Base> _bases;
    public List<Base> bases => _bases;
    [SerializeField] private List<Base> _playerBases;
    public List<Base> playerBases => _playerBases;
    [SerializeField] private List<Base> _enemyBases;
    public List<Base> enemyBases => _enemyBases;
    
    public void Init() {
        foreach (var myBase in _bases)
        {
            myBase.Init(this);
        }
    }
}
