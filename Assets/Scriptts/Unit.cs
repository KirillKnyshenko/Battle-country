using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private IOwner _iOwner;
    public IOwner iOwner => _iOwner;
    private PlayerData _data;
    private GameObject _targetObject;
    public GameObject targetObject => _targetObject;
    private Vector2 _targetDir;

    public void SetTarget(GameObject target, IOwner iOwner) {
        _iOwner = iOwner;
        _targetObject = target;
        _targetDir = target.transform.position - transform.position;
        _targetDir = _targetDir.normalized;
        _data = _iOwner.GetData();
    }

    private void FixedUpdate() {
        UnitMovement();
    }

    private void UnitMovement() {
        transform.Translate( _targetDir * _data.speed / 40);
    }
}
