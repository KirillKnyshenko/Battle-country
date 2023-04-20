using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCore : MonoBehaviour
{
    [SerializeField] protected LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;
    [SerializeField] protected PlayerData _data;
    public PlayerData data => _data;
    [SerializeField] protected List<Base> _bases;

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
