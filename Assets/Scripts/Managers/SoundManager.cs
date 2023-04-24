using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private GameManager _gameManager;
    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;
    [SerializeField] private AudioSource _audioSource;
    private bool _isSound = true;
    public bool isSound => _isSound;
    private float _soundVolume = .5f;
    private bool _isMusic = true;
    public bool isMusic => _isMusic;

    public void Init(GameManager gameManager) {
        _gameManager = gameManager;

        _gameManager.OnWin.AddListener(WinSound);
        _gameManager.OnLose.AddListener(LoseSound);

        _isSound = _gameManager.saveData.soundVolume;
        _soundVolume = _isSound ? .5f : 0f;

        _isMusic = _gameManager.saveData.musicVolume;
        if (!_isMusic)
        {
            _audioSource.Stop();
        }

        foreach (var myBase in _gameManager.levelManager.bases)
        {
            myBase.OnUnitTaken.AddListener(PopupSound);
        }
    }

    private void WinSound() {
        AudioPlay(_audioClipRefsSO.win, Camera.main.transform.position, _soundVolume);
    }

    private void LoseSound() {
        AudioPlay(_audioClipRefsSO.lose, Camera.main.transform.position, _soundVolume);
    }
    private void PopupSound() {
        AudioPlay(_audioClipRefsSO.unitTaken, Camera.main.transform.position, _soundVolume);
    }


    private void AudioPlay(AudioClip[] audioClip, Vector3 pos, float volume = .5f) {
        AudioSource.PlayClipAtPoint(audioClip[Random.Range(0, audioClip.Length)], pos, volume);
    }

    private void AudioPlay(AudioClip audioClip, Vector3 pos, float volume = .5f) {
        AudioSource.PlayClipAtPoint(audioClip, pos, volume);
    }

    public bool ToggleSound() {
        if (_isSound)
        {
            _isSound = false;

            _soundVolume = 0f;

            _gameManager.saveData.SetAudioData(_isMusic, _isSound);

            return true;
        }
        else
        {
            _isSound = true;
            
            _soundVolume = .5f;

            _gameManager.saveData.SetAudioData(_isMusic, _isSound);

            return false;
        }
    }

    public bool ToggleMusic() {
        if (_isMusic)
        {
            _isMusic = false;
            _audioSource.Stop();

            _gameManager.saveData.SetAudioData(_isMusic, _isSound);

            return true;
        }
        else
        {
            _isMusic = true;
            _audioSource.Play();

            _gameManager.saveData.SetAudioData(_isMusic, _isSound);

            return false;
        }
    }
}
