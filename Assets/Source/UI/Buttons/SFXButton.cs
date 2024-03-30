using System;

public class SFXButton : AudioButton
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
        _audioSettings.SwitchSFX();
        base.Switch();
    }
}