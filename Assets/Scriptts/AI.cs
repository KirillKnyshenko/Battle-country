using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour, IOwner
{
    [SerializeField] private GameManager _gameManager;
    public GameManager gameManager => _gameManager;
    [SerializeField] private float _analyzeTime;
    [SerializeField] private PlayerData _data;
    [SerializeField] private List<Base> _bases;
    public PlayerData data => _data;
    
    public void Init()
    {
        _bases = _gameManager.levelManager.enemyBases;
        StartCoroutine(Analyze());
    }

    public PlayerData GetData() {
        return _data;
    }

    private IEnumerator Analyze() {
        while (true)
        {
            yield return new WaitForSeconds(_analyzeTime);
            
            float totalMass = 0f;

            foreach (var myBase in _bases)
            {
                totalMass += myBase.mass;
            }

            var playerBases = _gameManager.levelManager.playerBases;

            foreach (var playerBase in playerBases)
            {
                if (totalMass > playerBase.mass)
                {
                    Attack(playerBase);
                }
            }
        }
    }

    private void Attack(Base targetBase) {
        foreach (var myBase in _bases)
        {
            myBase.SendUnits(targetBase.gameObject);
        }
    }
}
