using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitVisual _unitVisual;
    private IBaseOwner _iOwner;
    public IBaseOwner iOwner => _iOwner;
    private PlayerData _data;
    public PlayerData data => _data;
    private GameObject _targetObject;
    public GameObject targetObject => _targetObject;
    private Vector2 _targetDir;

    public void SetTarget(GameObject target, IBaseOwner iOwner) {
        _iOwner = iOwner;
        _targetObject = target;
        _targetDir = target.transform.position - transform.position;
        _targetDir = _targetDir.normalized;
        _data = _iOwner.GetData();
        _unitVisual.Init();
    }

    private void FixedUpdate() {
        UnitMovement();
    }

    private void UnitMovement() {
        transform.Translate( _targetDir * _data.speed / 40);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.attachedRigidbody.GetComponent<Unit>();
        
        if (unit != null)
        {
            if (_iOwner != unit.iOwner)
            {
                Destroy(unit.gameObject);
                Destroy(this);
            }   
        }
    }
}
