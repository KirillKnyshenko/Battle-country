using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class PlayerCore : MonoBehaviour
{
    [SerializeField] protected LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;
    [SerializeField] protected PlayerData _data;
    public PlayerData data => _data;
    [SerializeField] protected List<Base> _bases;
    public List<Base> bases => _bases;
    [SerializeField] protected float _playerMass;
    public float playerMass => _playerMass;
    [SerializeField] protected float _unitMass;
    public float unitMass => _unitMass;
    public abstract void Init(LevelManager levelManager);

    public PlayerData GetData() {
        return _data;
    }

    public void AddBase(Base baseArg) {
        _bases.Add(baseArg);
    }

    public void RemoveBase(Base baseArg) {
        _bases.Remove(baseArg);
    }

    protected IEnumerator UpdateMass() {
        while (true)
        {
            _playerMass = 0;
            foreach (var myBase in _bases)
            {
                _playerMass += myBase.mass;
            }

            _playerMass += _unitMass;

            yield return null;
        }
    }

    public void AddUnitMass() {
        _unitMass++;
    }

    public void RemoveUnitMass() {
        _unitMass--;
    }
}

[System.Serializable]
public class PlayerData 
{
    public float attack;
    public float defense;
    public float speed;
    public float reproductionTime;
    public Color color;
    public Gradient fieldColor;
    public Material lineMaterial;
}
