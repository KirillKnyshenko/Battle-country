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
    
    [SerializeField] private int _mass;
    public int mass => _mass;
    [SerializeField] private int _massMax;
    public int massMax => _massMax;

    [SerializeField] private AnimationCurve _spawnSpeedCurve;
    [SerializeField] private float[] _angles;

    [SerializeField] private float _spawnUnitsBatchDelay;
    [SerializeField] private float _spawnCooldown;
    private bool _isSpawnCooldown = true;

    public UnityEvent<Vector2> OnDrawLine;
    public UnityEvent OnClearLine;
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;
    public UnityEvent OnMassUpdate;
    public UnityEvent OnUnitTaken;
    public static UnityEvent OnAnyUnitTaken;
    public UnityEvent OnOwnerChanged;

    private IEnumerator _sendUnits;
    private IEnumerator _spawnUnits;

    public void Init(LevelManager levelManager, int loop) {
        _levelManager = levelManager;
        
        CalculateMass(loop);

        if (_playerCore != null) SetOwner(_playerCore);

        _baseVisual.Init();
        OnMassUpdate?.Invoke();

        _spawnUnits = SpawnUnits();

        _levelManager.gameManager.OnGameStarted.AddListener( () => {
            StartCoroutine(_spawnUnits);
        });
    }

    private void CalculateMass(int loop) {
        if (loop > 0)
        {
            AddMass(levelManager.unitLoopBonus * loop);
            _massMax += levelManager.unitLoopBonus * loop;
        }
    }

    private IEnumerator SpawnUnits() {
        _isSpawnCooldown = true;
        
        while (true)
        {
            if (_isSpawnCooldown)
            {
                yield return new WaitForSeconds(_spawnCooldown);
                _isSpawnCooldown = false;
            }
            
            if (_mass < _massMax && _sendUnits == null) 
            {
                AddMass(1);

                yield return new WaitForSeconds(_spawnSpeedCurve.Evaluate(_mass/_massMax));
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

        StopAllCoroutines();

        _sendUnits = GenerateUnits(target);      
        StartCoroutine(_sendUnits);

        if (levelManager.gameManager.IsTutorialToStart()) 
            levelManager.gameManager.StartGamePlaying();
    }

    public IEnumerator GenerateUnits(GameObject target) {
        int unitsMass = _mass;
        int maxUnitsPerBatch = _angles.Length;
        
        while (unitsMass > 0f)
        {
            _isSpawnCooldown = true;
            if (unitsMass > _mass)
            {
                unitsMass = _mass;
            }

            int unitToGanerate = Mathf.Min(maxUnitsPerBatch, unitsMass);

            for (int i = 0; i < unitToGanerate; i++)
            {
                Unit unitSkript = levelManager.pool.GetFreeElement();

                unitSkript.SetTarget(target, this, _angles[i]);

                RemoveMass(1);
                unitsMass--;
            }

            yield return new WaitForSeconds(_spawnUnitsBatchDelay);
        }

        _sendUnits = null;
        _spawnUnits = SpawnUnits();
        StartCoroutine(_spawnUnits);

        yield return null;
    }

    private void AddMass(int massToAdd) {
        _mass = _mass + massToAdd;

        OnMassUpdate?.Invoke();
    }

    private void RemoveMass(int massToRemove) {
        _mass = _mass - massToRemove;
        _isSpawnCooldown = true;
        OnMassUpdate?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit unit = collision.attachedRigidbody.GetComponent<Unit>();
        
        if (unit != null)
        {
            if (unit.targetObject == gameObject)
            {

                if (unit.playerCore == _playerCore)
                {
                    AddMass(1);
                    _isSpawnCooldown = true;
                }
                else
                {
                    if (_mass == 0)
                    {
                        // Change base owner
                        StopAllCoroutines();
                        SetOwner(unit.playerCore);

                        _sendUnits = null;
                        _spawnUnits = SpawnUnits();
                        StartCoroutine(_spawnUnits);
                    } 
                    else
                    {
                        RemoveMass(1);
                    }

                }

                OnUnitTaken?.Invoke();
                OnAnyUnitTaken?.Invoke();
                unit.gameObject.SetActive(false);
            }
        }
    }
}
