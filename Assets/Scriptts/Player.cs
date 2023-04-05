using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    public GameManager gameManager => _gameManager;
    [SerializeField] private PlayerData _data;
    public PlayerData data => _data;

    public void Init() {
        _data = new PlayerData(1f, 1f, 1f, 1f);
    }
}

[System.Serializable]
public class PlayerData 
{
    public float attack;
    public float defense;
    public float speed;
    public float reproductionTime;

    public PlayerData(float attack, float defense, float speed, float reproductionTime) {
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.reproductionTime = reproductionTime;
    }
}
