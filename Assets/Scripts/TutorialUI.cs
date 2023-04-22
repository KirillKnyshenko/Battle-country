using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
public class TutorialUI : MonoBehaviour
{
    private UIManager _UIManager;
    private Vector3 _fromPos;
    private Vector3 _aimPos;
    [SerializeField] private float _speed;
    public void Init(UIManager UIManager) {
        _UIManager = UIManager;

        gameObject.SetActive(false);

        _UIManager.gameManager.OnTutorialToStart.AddListener(StartTutorial);
        _UIManager.gameManager.OnGameStarted.AddListener(StopTutorial);
    }

    private void StartTutorial() {
        gameObject.SetActive(true);
        LevelManager levelManager = _UIManager.gameManager.levelManager;

        PlayerCore playerCore = levelManager.players.Where(x => x.GetComponent<Player>() != null).First<PlayerCore>();

        _fromPos = playerCore.bases[0].transform.position + Vector3.back;

        Base baseAim = levelManager.bases.Where(x => x.playerCore == null).First<Base>();
        _aimPos = baseAim.transform.position + Vector3.back;

        LoopTutorial();
    }

    private void LoopTutorial() {
        if (_UIManager.gameManager.IsTutorialToStart())
        {
            transform.position = _fromPos;
            transform.DOMove(_aimPos, _speed).onComplete += LoopTutorial;
        }
    }

    private void StopTutorial() {
        gameObject.SetActive(false);
        
    }
}
