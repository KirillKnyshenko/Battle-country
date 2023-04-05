using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Base : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private PlayerData _data;
    public PlayerData data => _data;
    [SerializeField] private float _mass;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private float _spawnUnitsBorder;
    public void Init(LevelManager levelManager, PlayerData data) {
        _levelManager = levelManager;
        _data = data;
    }

    private void SendUnits(GameObject target) {
        if (target == gameObject) return;

        for (int i = 0; i < _mass; i++)
        {
            GameObject newUnit;

            Vector3 unitPos = new Vector3(Random.Range(-_spawnUnitsBorder, _spawnUnitsBorder), Random.Range(-_spawnUnitsBorder, _spawnUnitsBorder)) + transform.position;    

            newUnit = Instantiate(_unitPrefab, unitPos, Quaternion.identity);

            Unit unitSkript = newUnit.GetComponent<Unit>();

            unitSkript.SetTarget(target);
        }
    }
}
