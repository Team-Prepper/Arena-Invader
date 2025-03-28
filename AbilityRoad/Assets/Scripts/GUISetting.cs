using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GUISetting : GUIPopUp {

    [System.Serializable]
    struct Option {
        public string key;
        public string value;
    }

    [SerializeField] private Option[] _langOpt;
    [SerializeField] private EHDropdownWrapper _langDropdown;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicMasterSlider;

    public override void SetOff()
    {
        Close();
    }

    public override void Open()
    {
        base.Open();
        
        int idx = 0;
        string[] options = new string[_langOpt.Length];

        for (int i = 0; i < _langOpt.Length; i++)
        {
            if (LangManager.Instance.NowLang.CompareTo(_langOpt[i].value) == 0)
            {
                idx = i;
            }
            options[i] = _langOpt[i].key;
        }

        _langDropdown.SetDropdownOption(options);

        _langDropdown.value = idx;
        _langDropdown.onValueChanged.AddListener(LangSet);

        _musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);

        _audioMixer.GetFloat("Master", out float volume);
        _musicMasterSlider.value = Mathf.Pow(10, volume / 20);
    }

    public void LangSet(int idx)
    {
        LangManager.Instance.ChangeLang(_langOpt[_langDropdown.value].value);

    }

    public void SetMasterVolume(float volume)
    {
        if (volume <= _musicMasterSlider.minValue)
        {
            _audioMixer.SetFloat("Master", -80);
            return;

        }

        _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

}
