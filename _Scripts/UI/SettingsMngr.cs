using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingsMngr : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _qualitySettingsDropdown;

    public void SetQualityLevelDropdown(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
    }

    public void SetMasterVolume()
    {

    }
}