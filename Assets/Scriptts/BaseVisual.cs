using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVisual : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _selectedVisual;
    public LineRenderer LineRenderer => _lineRenderer;

    public void Init() {
        _base.OnDrawLine.AddListener(DrawLine);
        _base.OnClearLine.AddListener(ClearLine);
        _base.OnSelected.AddListener(ShowSelection);
        _base.OnUnselected.AddListener(HideSelection);

        HideSelection();
    }

    private void DrawLine(Vector2 targetPosition) {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, targetPosition);
    }

    private void ClearLine() {
        _lineRenderer.SetPosition(1, transform.position);
    }

    private void ShowSelection() {
        _selectedVisual.SetActive(true);
    }

    private void HideSelection() {
        _selectedVisual.SetActive(false);
    }
}
