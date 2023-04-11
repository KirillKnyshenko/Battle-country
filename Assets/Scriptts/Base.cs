using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Base : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;
    [SerializeField] private GameObject _ownerObject;
    private IOwner _iOwner;
    public IOwner iOwner => _iOwner;
    [SerializeField] private PlayerData _data;
    public PlayerData data => _data;
    [SerializeField] private BaseVisual _baseVisual;
    public BaseVisual baseVisual => _baseVisual;
    
    [SerializeField] private float _mass;
    public float mass => _mass;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private float _spawnUnitsBorder;

    public UnityEvent<Vector2> OnDrawLine;
    public UnityEvent OnClearLine;
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;
    public UnityEvent OnMassChanged;

    public void Init(LevelManager levelManager) {
        _levelManager = levelManager;
        
        SetOwner(_ownerObject);
    }

    private void SetOwner(GameObject ownerObject) {
        _ownerObject = ownerObject;
        _iOwner = ownerObject.GetComponent<IOwner>();

        if (_iOwner != null)
        {
            _data = _iOwner.GetData();
                
        }
        else
        {
            Debug.Log("Owner doesn't have a inteface");
        }

        _baseVisual.Init();
    }

    public void SendUnits(GameObject target) {
        if (target == gameObject) return;

        for (int i = 0; i < _mass; i++)
        {
            GameObject newUnit;

            Vector3 unitPos = new Vector3(Random.Range(-_spawnUnitsBorder, _spawnUnitsBorder), Random.Range(-_spawnUnitsBorder, _spawnUnitsBorder)) + transform.position;    

            newUnit = Instantiate(_unitPrefab, unitPos, Quaternion.identity);

            Unit unitSkript = newUnit.GetComponent<Unit>();

            unitSkript.SetTarget(target, _iOwner);
        }

        _mass -= _mass;
        OnMassChanged?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.attachedRigidbody.GetComponent<Unit>();
        
        if (unit != null)
        {
            if (unit.targetObject == gameObject)
            {

                if (unit.iOwner == _iOwner)
                {
                    _mass++;
                }
                else
                {
                    _mass--;

                    if (_mass < 0f)
                    {
                        // Change base owner
                        SetOwner(unit.iOwner.GetData().owner);
                        _mass++;
                    }
                }

                OnMassChanged?.Invoke();
                Destroy(unit.gameObject);
            }
        }
    }
}
