using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private SaveDataSO _saveDataSO;
    [SerializeField] private LevelListSO _levelListSO;
    [SerializeField] private RectTransform _bar;
    private void Start()
    {
        _bar.transform.localScale = new Vector3(0f, 1f, 1f);
        _bar.DOScale(Vector3.one, 2f).onComplete += delegate {
            _saveDataSO.Load();
            SceneManager.LoadScene(_levelListSO.GetCurrentLevelName());
        };
    }
}
