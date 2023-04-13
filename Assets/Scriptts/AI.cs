using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : PlayerCore, IBaseOwner
{
    [SerializeField] private float _analyzeTime;
    [SerializeField] private PlayerData _data;
    [SerializeField] private List<Base> _bases;
    public PlayerData data => _data;
    
    public override void Init(LevelManager levelManager) {
        _levelManager = levelManager;
        StartCoroutine(Analyze());
    }

    private IEnumerator Analyze() {
        while (true)
        {
            yield return new WaitForSeconds(_analyzeTime);
            
            float totalMass = 0f;

            foreach (var myBase in _bases)
            {
                totalMass = totalMass + myBase.mass;
            }

            var bases = _levelManager.bases;

            foreach (var selectedBase in bases)
            {
                if (!_bases.Contains(selectedBase))
                {
                    if (totalMass > selectedBase.mass)
                    {
                        Attack(selectedBase);
                    }
                }
            }
        }
    }

    private void Attack(Base targetBase) {
        float atackMass = 0f;
        List<Base> atackBases = new List<Base>();


        foreach (var myBase in _bases)
        {
            atackMass = atackMass + myBase.mass;

            atackBases.Add(myBase);
            if (atackMass > targetBase.mass)
            {
                continue;
            }
        }

        foreach (var atackBase in atackBases)
        {
            atackBase.SendUnits(targetBase.gameObject);
        }
    }

    public PlayerData GetData() {
        return _data;
    }

    public void AddBase(Base baseArg) {
        _bases.Add(baseArg);
    }

    public void RemoveBase(Base baseArg) {
        _bases.Remove(baseArg);
    }
}