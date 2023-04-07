using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private PlayerData _data;
    private GameObject _targetObject;
    public GameObject targetObject => _targetObject;
    private Vector2 _targetDir;

    public void SetTarget(GameObject target, PlayerData playerData) {
        _targetObject = target;
        _targetDir = target.transform.position - transform.position;
        _targetDir = _targetDir.normalized;
        _data = playerData;
    }

    private void FixedUpdate() {
        UnitMovement();
    }

    private void UnitMovement() {
        transform.Translate( _targetDir * _data.speed / 40);
    }
}
