using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private SpriteRenderer _spriteRenderer;

     public void Init() {
        _spriteRenderer.color = _unit.data.color;
    }
}
