using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    public GameManager gameManager => _gameManager;
    [SerializeField] private List<Base> _bases;
    public List<Base> bases => _bases;
    [SerializeField] private List<PlayerCore> _players;
    
    public void Init() {
        foreach (var player in _players)
        {
            player.Init(this);
        }

        foreach (var myBase in _bases)
        {
            myBase.Init(this);
        }
    }
}
