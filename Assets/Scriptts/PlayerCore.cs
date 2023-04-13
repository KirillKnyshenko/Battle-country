using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCore : MonoBehaviour
{
    [SerializeField] protected LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;

    public abstract void Init(LevelManager levelManager);
}

[System.Serializable]
public class PlayerData 
{
    public GameObject owner;
    public float attack;
    public float defense;
    public float speed;
    public float reproductionTime;
    public Color color;
    public Gradient fieldColor;
    public Material lineMaterial;
}
