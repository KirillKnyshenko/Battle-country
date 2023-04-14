using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitVisual _unitVisual;
    private PlayerCore _playerCore;
    public PlayerCore playerCore => _playerCore;
    private PlayerData _data;
    public PlayerData data => _data;
    private GameObject _targetObject;
    public GameObject targetObject => _targetObject;
    private Vector2 _targetDir;

    public void SetTarget(GameObject target, PlayerCore playerCore) {
        _playerCore = playerCore;
        _targetObject = target;
        _targetDir = target.transform.position - transform.position;
        _targetDir = _targetDir.normalized;
        _data = _playerCore.GetData();
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
            if (_playerCore != unit.playerCore)
            {
                Destroy(unit.gameObject);
                Destroy(this);
            }   
        }
    }
}
