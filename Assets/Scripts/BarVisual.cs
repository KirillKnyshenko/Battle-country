using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarVisual : MonoBehaviour
{
    [SerializeField] private float _barSpeed;
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _naighborRectTransform;

    public void Init(Color color, RectTransform naighborRectTransform = null) {
        _image.color = color;
        _naighborRectTransform = naighborRectTransform;
        transform.localScale = new Vector3(0f, 1f, 1f);
    }

    public void BarUpdate(float x) {
        if (_naighborRectTransform != null)
            transform.localPosition = new Vector3(_naighborRectTransform.transform.localPosition.x + _naighborRectTransform.sizeDelta.x * _naighborRectTransform.transform.localScale.x, 0f, 0f);

        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, x, Time.deltaTime * _barSpeed), 1f, 1f);
    }
}
