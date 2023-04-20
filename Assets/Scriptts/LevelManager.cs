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

    [SerializeField] private int _poolCount;
    private bool _autoExpand = true;
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Transform _poolContainer;
    
    private PoolMono<Unit> _pool;
    public PoolMono<Unit> pool => _pool;

    public void Init() {
        foreach (var player in _players)
        {
            player.Init(this);
        }

        foreach (var myBase in _bases)
        {
            myBase.Init(this);
        }

        this._pool = new PoolMono<Unit>(_unitPrefab, _poolCount, _poolContainer);
        this._pool.autoExpand = _autoExpand;
    }

    private void LoadLevel() {

    }
}
