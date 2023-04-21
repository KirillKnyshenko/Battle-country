using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : PlayerCore
{
    [SerializeField] private float _analyzeTime;
    
    public override void Init(LevelManager levelManager) {
        _levelManager = levelManager;
        StartCoroutine(Analyze());
        StartCoroutine(UpdateMass());
    }

    private IEnumerator Analyze() {
        while (true)
        {
            yield return new WaitForSeconds(_analyzeTime);
            
            
            Attack(FindTargetBase());
        }
    }

    private List<Base> FindTargetBase() {
        float totalMass = 0f;

        foreach (var myBase in _bases)
        {
            totalMass = totalMass + myBase.mass;
        }

        List<Base> bases = _levelManager.bases;
        List<Base> targetBases = new List<Base>();

        // Find all enemy bases witch could be taken
        foreach (Base selectedBase in bases)
        {
            if (!_bases.Contains(selectedBase))
            {
                if (totalMass > selectedBase.mass)
                {
                    targetBases.Add(selectedBase);
                }
            }
        }

        if (targetBases != null)
        {
            // Sort fron the nearest to the farest one
            targetBases.Sort((Base x, Base y) => {

                float distanceToX = Vector3.Distance(transform.position, x.transform.position);
                float distanceToY = Vector3.Distance(transform.position, y.transform.position);

                return distanceToX.CompareTo(distanceToY);
            });   

            return targetBases;     
        }

        return null;
    }

    private void Attack(List<Base> targetBases) {
        if (targetBases == null) return;

        List<Base> basesForAttack = new List<Base>();

        foreach (Base targetBase in targetBases)
        {
            float atackMass = 0f;

            foreach (var myBase in _bases)
            {
                atackMass = atackMass + myBase.mass;
                basesForAttack.Add(myBase);

                if (atackMass > targetBase.mass)
                {
                    SendingUnitsToAttack(basesForAttack, targetBase);
                    break;
                }
            }

            basesForAttack.Clear();
        }        
    }

    private void SendingUnitsToAttack(List<Base> basesForAttack, Base targetBase) {
        foreach (var atackBase in basesForAttack)
        {
            atackBase.SendUnits(targetBase.gameObject);
        }
    }
}