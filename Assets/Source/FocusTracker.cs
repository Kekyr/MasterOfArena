using System;
using Agava.WebUtility;
using UnityEngine;

public class FocusTracker : MonoBehaviour
{
    private readonly float PlayVolume = 0.1f;
    private readonly float PauseVolume = 0f;

    private readonly float PlayTimeScale = 1f;
    private readonly float PauseTimeScale = 0;

    private AudioSource _music;
    private TutorialHand _tutorialHand;

    private void OnEnable()
    {
        Application.focusChanged += OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
    }

    private void OnDisable()
    {
        Application.focusChanged -= OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
    }

    public void Init(Music music)
    {
        if (music == null)
        {
            throw new ArgumentNullException(nameof(music));
        }

        _music = music.GetComponent<AudioSource>();
        enabled = true;
    }

    public void Init(TutorialHand tutorialHand)
    {
        if (tutorialHand == null)
        {
            throw new ArgumentNullException(nameof(tutorialHand));
        }

        _tutorialHand = tutorialHand;
    }

    private void OnInBackgroundChangeApp(bool inApp)
    {
        MuteAudio(!inApp);
        PauseGame(!inApp);
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        MuteAudio(isBackground);
        PauseGame(isBackground);
    }

    private void MuteAudio(bool value)
    {
        _music.volume = value ? PauseVolume : PlayVolume;
    }

    private void PauseGame(bool value)
    {
        float playTimeScale = _tutorialHand.enabled ? _tutorialHand.TutorialTimeScale : PlayTimeScale;
        Time.timeScale = value ? PauseTimeScale : playTimeScale;
    }
}