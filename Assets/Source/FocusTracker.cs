using UnityEngine;
using YG;

public class FocusTracker : MonoBehaviour
{
    private readonly float PauseVolume = 0f;
    private readonly float PauseTimeScale = 0;

    private AudioSource _music;
    private TutorialHand _tutorialHand;

    private float _currentVolume;
    private float _currentTimeScale;


    private void OnEnable()
    {
        YandexGame.onShowWindowGame += OnShowWindow;
        YandexGame.onHideWindowGame += OnHideWindow;
    }

    private void OnDisable()
    {
        YandexGame.onShowWindowGame -= OnShowWindow;
        YandexGame.onHideWindowGame -= OnHideWindow;
    }

    public void Init(TutorialHand tutorialHand)
    {
        _tutorialHand = tutorialHand;
    }

    public void Init(Music music)
    {
        _music = music.GetComponent<AudioSource>();
        enabled = true;
    }

    private void OnShowWindow()
    {
        _music.volume = _currentVolume;
        Time.timeScale = _currentTimeScale;
    }

    private void OnHideWindow()
    {
        _currentVolume = _music.volume;
        _currentTimeScale = Time.timeScale;
        _music.volume = PauseVolume;
        Time.timeScale = PauseTimeScale;
    }
}