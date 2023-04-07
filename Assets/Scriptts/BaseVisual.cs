using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVisual : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private LineRenderer _lineRenderer;
    public LineRenderer LineRenderer => _lineRenderer;
}
