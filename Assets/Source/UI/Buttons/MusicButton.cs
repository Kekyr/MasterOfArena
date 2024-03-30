using System;

public class MusicButton : AudioButton
{
    private AudioSettingsSO _audioSettings;

    public void Init(AudioSettingsSO audioSettings)
    {
        if (audioSettings == null)
            throw new ArgumentNullException(nameof(audioSettings));

        _audioSettings = audioSettings;
        enabled = true;
    }

    protected override void Switch()
    {
        _audioSettings.SwitchMusic();
        base.Switch();
    }
}