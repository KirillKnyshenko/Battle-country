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
    public UnityEvent OnOwnerChanged;

    private IEnumerator baseAction;

    public void Init(LevelManager levelManager) {
        _levelManager = levelManager;
        
        if (_playerCore != null) SetOwner(_playerCore);

        _baseVisual.Init();
        OnMassUpdate?.Invoke();
        
        _levelManager.gameManager.OnGameStarted.AddListener( () => {
            StopAllCoroutines();  
            baseAction = SpawnUnits();
         
            StartCoroutine(baseAction);
        });
    }

    private IEnumerator SpawnUnits() {
        while (true)
        {
            if (_isSpawnCooldown)
            {
                yield return new WaitForSeconds(_spawnCooldown);
                _isSpawnCooldown = false;
            }

            if (_mass < _massMax) 
            {
                AddMass(1f);

                yield return new WaitForSeconds(_spawnSpeedCurve.Evaluate(_mass/_massMax));
            }

            yield return null;
        };
    }

    private void SetOwner(PlayerCore playerCore) {
        if (_playerCore != null) _playerCore.RemoveBase(this);

        StopAllCoroutines();  

        _playerCore = playerCore;

        if (_playerCore != null)
        {
            _data = _playerCore.GetData();

            _playerCore.AddBase(this);

            OnMassUpdate?.Invoke();
            OnOwnerChanged?.Invoke();
        }
        else
        {
            Debug.LogError("PlayerCore was not found");
        }
        
        if (!levelManager.gameManager.IsGamePlaying()) return;

        baseAction = SpawnUnits();      
        StartCoroutine(baseAction);
        _isSpawnCooldown = true;
    }

    public void SendUnits(GameObject target) {
        if (target == gameObject) return;

        StopAllCoroutines();

        baseAction = GenerateUnits(target);      
        StartCoroutine(baseAction);
    }

    public IEnumerator GenerateUnits(GameObject target) {
        float unitsMass = _mass;
        float maxUnitsPerBatch = _angles.Length;
        
        while (unitsMass > 0f)
        {
            if (unitsMass > _mass)
            {
                unitsMass = _mass;
            }

            float unitToGanerate = Mathf.Min(maxUnitsPerBatch, unitsMass);

            for (int i = 0; i < unitToGanerate; i++)
            {
                Unit unitSkript = levelManager.pool.GetFreeElement();

                unitSkript.SetTarget(target, this, _angles[i]);

                RemoveMass(1f);
                unitsMass--;
            }

            yield return new WaitForSeconds(_spawnUnitsBatchDelay);
        }

        baseAction = SpawnUnits();
        _isSpawnCooldown = true;
        StartCoroutine(baseAction);

        yield return null;
    }

    private void AddMass(float massToAdd) {
        _mass = _mass + massToAdd;

        OnMassUpdate?.Invoke();
    }

    private void RemoveMass(float massToRemove) {
        _mass = _mass - massToRemove;

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
                        _isSpawnCooldown = true;
                    }

                }

                OnUnitTaken?.Invoke();
                unit.gameObject.SetActive(false);
            }
        }
    }
}
