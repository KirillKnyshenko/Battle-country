using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    public GameManager gameManager => _gameManager;
    private LevelManager _levelManager;
    [SerializeField] private Transform _parentBar;
    [SerializeField] private GameObject _templeteBar;
    [SerializeField] private TextMeshProUGUI _levelText;
    private List<BarVisual> _bars;

    [SerializeField] private float _timeToHide;
    [SerializeField] private Image _tapToStartBacground;
    [SerializeField] private Transform _tapToStartButton;

    public void Init(GameManager gameManager, int level) {
        _gameManager = gameManager;
        _levelManager = _gameManager.levelManager;
        _bars = new List<BarVisual>();
        _templeteBar.SetActive(false);
        _levelText.text = "LEVEL " + level;

        for (int i = 0; i < _levelManager.players.Count; i++)
        {
            GameObject newBar = Instantiate(_templeteBar, Vector3.zero, Quaternion.identity, _parentBar);

            newBar.SetActive(true);
            newBar.transform.localPosition = Vector3.zero;

            BarVisual barVisual = newBar.GetComponent<BarVisual>();

            RectTransform neighbor = null;
            if (i > 0)
                neighbor = _bars[i - 1].GetComponent<RectTransform>();

            barVisual.Init(_levelManager.players[i].data.color, neighbor);

            _bars.Add(barVisual);
        }

        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI() {
        while (true)
        {
            for (int i = 0; i < _bars.Count; i++)
            {
                float sizeX = 0f;
                if (_levelManager.players[i].playerMass != 0f)
                {
                    sizeX = _levelManager.players[i].playerMass / _levelManager.GetSumMass();
                }

                _bars[i].BarUpdate(sizeX);
            }

            yield return null;
        }
    }

    public void HideTapToStart() {
        _tapToStartBacground.DOFade(0f, _timeToHide);
        _tapToStartButton.DOMoveY(-1000F, _timeToHide);
    }
}
