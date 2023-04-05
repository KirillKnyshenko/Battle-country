using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private PlayerData _data;
    private GameObject _targetObject;
    private Vector2 _targetDir;

    public void SetTarget(GameObject target) {
        _targetObject = target;
        _targetDir = target.transform.position - transform.position;
        _targetDir = _targetDir.normalized;
    }

    private void FixedUpdate() {
        UnitMovement();
    }

    private void UnitMovement() {
        transform.Translate( _targetDir * _data.speed / 40);
    }
}
