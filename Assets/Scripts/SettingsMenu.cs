using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _displayModeDropdown;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private List<Resolution> _filteredResolutions;
    private FullScreenMode _selectedDisplayMode;
    private int _selectedResolutionIndex;

    public void SetDisplayMode(int displayModeIndex)
    {
        _selectedDisplayMode = OptionIndexToDisplayMode(displayModeIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        _selectedResolutionIndex = resolutionIndex;
    }

    public void Apply()
    {
        Screen.SetResolution(_filteredResolutions[_selectedResolutionIndex].width, _filteredResolutions[_selectedResolutionIndex].height, _selectedDisplayMode);
    }

    private void Awake()
    {
        FillDisplayModeOptions();
        FillResolutionOptions();
        Apply();
    }

    private void FillDisplayModeOptions()
    {
        List<string> displayModeOptions = new List<string>
        {
            "Full Screen Window",
            "Windowed",
            "Exclusive Full Screen"
        };

        _selectedDisplayMode = Screen.fullScreenMode;
        int currentDisplayModeIndex = DisplayModeToOptionIndex(_selectedDisplayMode);
        
        _displayModeDropdown.ClearOptions();

        _displayModeDropdown.AddOptions(displayModeOptions);
        _displayModeDropdown.value = currentDisplayModeIndex;
        _displayModeDropdown.RefreshShownValue();
    }

    private void FillResolutionOptions()
    {
        Resolution[] availableResolutions = Screen.resolutions;
        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        _filteredResolutions = new List<Resolution>();
        _resolutionDropdown.ClearOptions();

        for (int i = availableResolutions.Length - 1; i >= 0; i--)
        {
            if (availableResolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                _filteredResolutions.Add(availableResolutions[i]);
            }
        }

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + (int)_filteredResolutions[i].refreshRateRatio.value + "Hz";
            resolutionOptions.Add(resolutionOption);
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _selectedResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(resolutionOptions);
        _resolutionDropdown.value = _selectedResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    private int DisplayModeToOptionIndex(FullScreenMode fullScreenMode)
    {
        switch (fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                return 0;
            case FullScreenMode.Windowed:
                return 1;
            case FullScreenMode.ExclusiveFullScreen:
                return 2;
            default:
                return 0;
        }
    }

    private FullScreenMode OptionIndexToDisplayMode(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                return FullScreenMode.FullScreenWindow;
            case 1:
                return FullScreenMode.Windowed;
            case 2:
                return FullScreenMode.ExclusiveFullScreen;
            default:
                return FullScreenMode.FullScreenWindow;
        }
    }
}
