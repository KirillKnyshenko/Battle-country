using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static int IS_WIN = Animator.StringToHash("isWin");
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
    [SerializeField] private TutorialUI _tutorialUI;

    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private GameObject _winUI;
    [SerializeField] private float _winUISpeed;
    [SerializeField] private Image _winBackground;
    [SerializeField] private Transform _winLabelTF;
    [SerializeField] private Transform _winButtonTF;
    [SerializeField] private Transform _winAreaTF;

    [SerializeField] private GameObject _loseUI;
    [SerializeField] private float _loseUISpeed;
    [SerializeField] private Image _loseBackground;
    [SerializeField] private Transform _loseLabelTF;
    [SerializeField] private Transform _loseButtonTF;

    [SerializeField] private Transform _ButtonTargetTF;
    [SerializeField] private Animator _winUIAnimator;

    [SerializeField] private GameObject _soundLine;
    [SerializeField] private TextMeshProUGUI _soundText;
    [SerializeField] private GameObject _musicLine;
    [SerializeField] private TextMeshProUGUI _musicText;

    public void Init(GameManager gameManager, int level) {
        _gameManager = gameManager;
        _levelManager = _gameManager.levelManager;

        _templeteBar.SetActive(false);
        _winUI.SetActive(false);
        _loseUI.SetActive(false);
        _settingsPanel.SetActive(false);

        _levelText.text = "LEVEL " + level;

        _bars = new List<BarVisual>();
        BarInit();

        _tutorialUI.Init(this);

        _gameManager.OnLose.AddListener(LoseUI);
        _gameManager.OnWin.AddListener(WinUI);
        _gameManager.OnTutorialToStart.AddListener(() => {
            StartCoroutine(UpdateUI());
        });
    }

    private void BarInit() {
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
        _tapToStartBacground.DOFade(0f, _timeToHide).onComplete += () => {
            _tapToStartBacground.gameObject.SetActive(false);
        };

        _tapToStartButton.DOMoveY(-1000F, _timeToHide);
    }

    private void WinUI() {
        _winUI.SetActive(true);
        
        Color alfaBackground = new Color(_winBackground.color.r, _winBackground.color.g, _winBackground.color.b, 0f);
        _winBackground.color = alfaBackground;
        _winBackground.DOFade(.8f, _winUISpeed).SetLink(_winBackground.gameObject);

        _winLabelTF.localScale = Vector3.zero;
        _winLabelTF.DOScale(Vector3.one, _loseUISpeed).SetLink(_winLabelTF.gameObject);

        _winButtonTF.DOLocalMove(_ButtonTargetTF.localPosition, _loseUISpeed).SetLink(_winButtonTF.gameObject);

        _winAreaTF.DOLocalRotate(new Vector3(0, 0, 360), 10f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetLink(_winAreaTF.gameObject);
        _winUIAnimator.SetTrigger(IS_WIN);
    }

    private void LoseUI() {
        _loseUI.SetActive(true);

        Color alfaBackground = new Color(_loseBackground.color.r, _loseBackground.color.g, _loseBackground.color.b, 0f);
        _loseBackground.color = alfaBackground;
        _loseBackground.DOFade(.8f, _loseUISpeed).SetLink(_loseBackground.gameObject);

        _loseLabelTF.DOShakeScale(_loseUISpeed, 0.5f).SetLink(_loseLabelTF.gameObject);

        _loseButtonTF.DOLocalMove(_ButtonTargetTF.localPosition, _loseUISpeed).SetLink(_loseLabelTF.gameObject);
    }

    public void ShowSettings() {
        _settingsPanel.SetActive(true);
        UpdateSoundVisual();
        UpdateMusicVisual();
    }

    public void HideSettings() {
        _settingsPanel.SetActive(false);
    }

    public void ToggleSound() {
        gameManager.soundManager.ToggleSound();
        UpdateSoundVisual();
    }

    public void ToggleMusic() {
        gameManager.soundManager.ToggleMusic();
        UpdateMusicVisual();
    }

    private void UpdateMusicVisual() {
        if (gameManager.soundManager.isMusic)
        {
            _musicLine.SetActive(false);
            _musicText.text = "Music: on";
        }
        else
        {
            _musicLine.SetActive(true);
            _musicText.text = "Music: off";
        }
    }

    private void UpdateSoundVisual() {
        if (gameManager.soundManager.isSound)
        {
            _soundLine.SetActive(false);
            _soundText.text = "Sound: on";
        }
        else
        {
            _soundLine.SetActive(true);
            _soundText.text = "Sound: off";
        }
    }
}
