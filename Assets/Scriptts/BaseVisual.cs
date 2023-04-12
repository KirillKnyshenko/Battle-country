using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BaseVisual : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _selectedSpriteRenderer;
    [SerializeField] private Transform _arrorTransform;
    [SerializeField] private SpriteRenderer _arrorSpriteRenderer;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _selectedVisual;
    public LineRenderer LineRenderer => _lineRenderer;
    [SerializeField] private TextMeshProUGUI _countText;

    public void Init() {
        SetOwnerVisual();

        _base.OnDrawLine.AddListener(DrawLine);
        _base.OnClearLine.AddListener(ClearLine);
        _base.OnSelected.AddListener(ShowSelection);
        _base.OnUnselected.AddListener(HideSelection);
        _base.OnMassChanged.AddListener(UpdateVisual);
        _base.OnOwnerChanged.AddListener(SetOwnerVisual);

        HideSelection();
        UpdateVisual();
    }

    private void SetOwnerVisual() {
        if (_base.iOwner != null)
        {
            _spriteRenderer.color = _base.data.color;
            float alfaColor = .3f;
            Color selectedColor = new Color(_base.data.color.r, _base.data.color.g, _base.data.color.b, alfaColor);
            _selectedSpriteRenderer.color = selectedColor;

            _lineRenderer.material = _base.data.lineMaterial;
            _arrorSpriteRenderer.material = _base.data.lineMaterial;
        }
    }

    private void UpdateVisual() {
        _countText.text = _base.mass.ToString();
    }

    private void DrawLine(Vector2 targetPosition) {
        
        _arrorTransform.position = targetPosition;

        // Set target to the center of arrow for better line visual
        targetPosition = _arrorSpriteRenderer.transform.position;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, targetPosition);
        
        // Rotate arrow
        var dir = targetPosition - (Vector2)transform.position;
        var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        _arrorTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ClearLine() {
        _lineRenderer.SetPosition(1, transform.position);
        
        _arrorTransform.position = (Vector2)transform.position;
    }

    private void ShowSelection() {
        _selectedVisual.SetActive(true);
    }

    private void HideSelection() {
        _selectedVisual.SetActive(false);
    }
}
