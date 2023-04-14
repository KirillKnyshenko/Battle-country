using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Base : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    public LevelManager levelManager => _levelManager;

    [SerializeField] private PlayerCore _playerCore;
    public PlayerCore playerCore => _playerCore;

    [SerializeField] private PlayerData _data;
    public PlayerData data => _data;

    [SerializeField] private BaseVisual _baseVisual;
    public BaseVisual baseVisual => _baseVisual;
    
    [SerializeField] private float _mass;
    public float mass => _mass;
    [SerializeField] private float _massMax;
    public float massMax => _massMax;

    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private float _spawnUnitsBorder;
    

    public UnityEvent<Vector2> OnDrawLine;
    public UnityEvent OnClearLine;
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;
    public UnityEvent OnMassChanged;
    public UnityEvent OnUnitTaken;
    public UnityEvent OnOwnerChanged;

    public void Init(LevelManager levelManager) {
        _levelManager = levelManager;
        
        if (_playerCore != null) SetOwner(_playerCore);

        _baseVisual.Init();

        StartCoroutine(BaseUpdate());
    }

    private IEnumerator BaseUpdate() {
        while (true)
        {
            if (_playerCore != null && _mass < _massMax) 
            {
                AddMass(1f);
                yield return new WaitForSeconds(_data.reproductionTime);
            }

            yield return null;
        };
    }

    private void SetOwner(PlayerCore playerCore) {
        if (_playerCore != null) _playerCore.RemoveBase(this);

        _playerCore = playerCore;

        if (_playerCore != null)
        {
            _data = _playerCore.GetData();

            _playerCore.AddBase(this);

            OnOwnerChanged?.Invoke();
        }
        else
        {
            Debug.LogError("PlayerCore was not found");
        }
    }

    public void SendUnits(GameObject target) {
        if (target == gameObject) return;

        for (int i = 0; i < _mass; i++)
        {
            GameObject newUnit;

            Vector3 unitPos = new Vector3(Random.Range(-_spawnUnitsBorder, _spawnUnitsBorder), Random.Range(-_spawnUnitsBorder, _spawnUnitsBorder)) + transform.position;    

            newUnit = Instantiate(_unitPrefab, unitPos, Quaternion.identity);

            Unit unitSkript = newUnit.GetComponent<Unit>();

            unitSkript.SetTarget(target, _playerCore);
        }

        RemoveMass(_mass);
    }

    private void AddMass(float massToAdd) {
        _mass = _mass + massToAdd;
        OnMassChanged?.Invoke();
    }

    private void RemoveMass(float massToRemove) {
        _mass = _mass - massToRemove;
        OnMassChanged?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.attachedRigidbody.GetComponent<Unit>();
        
        if (unit != null)
        {
            if (unit.targetObject == gameObject)
            {

                if (unit.playerCore == _playerCore)
                {
                    AddMass(1f);
                }
                else
                {
                    if (_mass == 0f)
                    {
                        // Change base owner
                        SetOwner(unit.playerCore);
                    } 
                    else
                    {
                        RemoveMass(1f);
                    }

                }

                OnUnitTaken?.Invoke();
                Destroy(unit.gameObject);
            }
        }
    }
}
