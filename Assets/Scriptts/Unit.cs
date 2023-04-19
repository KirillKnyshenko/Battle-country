using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Unit : MonoBehaviour
{
    [SerializeField] private UnitVisual _unitVisual;
    [SerializeField] private float _unitSpeed;
    [SerializeField] private float _spreadSize;
    
    private PlayerCore _playerCore;
    public PlayerCore playerCore => _playerCore;

    private PlayerData _data;
    public PlayerData data => _data;

    private Base _baseParent;

    private GameObject _targetObject;
    public GameObject targetObject => _targetObject;
    
    private Vector3 _targetPos;
    private Vector3 _angelDir;
    private Vector3 _offsetDir;

    public void SetTarget(GameObject target, Base baseParent, float angle) {
        _baseParent = baseParent;
        _playerCore = _baseParent.playerCore;

        _data = _playerCore.GetData();
        _unitVisual.Init();

        _targetObject = target;
        _targetPos = target.transform.position;

        _angelDir = (Quaternion.Euler(0f, 0f, angle) * (_targetPos - transform.position)).normalized;
        _offsetDir = (_targetPos - _baseParent.transform.position).normalized;
    }

    private void FixedUpdate() {
        UnitMovement();
    }

    private void UnitMovement() {
        float offsetFinalTarget = Vector3.Distance(_targetPos, transform.position);
        float offsetAngle = Vector3.Distance(_baseParent.transform.position, transform.position);
        
        if (offsetAngle < _spreadSize)
        {
            // Move from parent base 
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _angelDir, Time.fixedDeltaTime * _unitSpeed);
        }
        else if (offsetFinalTarget < _spreadSize)
        {
            // Move to target base 
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.fixedDeltaTime * _unitSpeed);
        }
        else
        {
            // Move to target base with offset
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _offsetDir, Time.fixedDeltaTime * _unitSpeed);        
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.attachedRigidbody.GetComponent<Unit>();
        
        if (unit != null)
        {
            if (_playerCore != unit.playerCore)
            {
                Destroy(unit.gameObject);
                Destroy(this);
            }   
        }
    }
}
