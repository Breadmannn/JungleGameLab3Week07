using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    #region Variables

    [Header("Status")]
    [SerializeField] BeatNotifier beatNotifier;
    Coroutine beatCoroutine;

    [Header("Title")]
    public Action toggleHelpPanelCanvasAction;

    [Header("InGame")]
    public Action<bool> showJudgeTextAction;
    public Action<string, string> activateSkillTextAction;
    public Action<int> updateWaveAction;


    // Fade in, out을 위한 스크립트
    [Header("Stage Clear")]
    StageClearUIController _stageClearUIController;
    GameObject _fadePanel;
    GameObject _clearUI;
    TextMeshProUGUI _clearText;
    TextMeshProUGUI _clearTextShadow;
    
    

    #endregion


    public void SetStageClearCanvas(GameObject stageClearUIController)
    {
        _stageClearUIController = stageClearUIController.GetComponent<StageClearUIController>();
        
        foreach (Transform child in _stageClearUIController.transform)
        {
            if (child.name == "FadePanel")
            {
                _fadePanel = child.gameObject;
            }
            else if (child.name == "ClearUI")
            {
                _clearUI = child.gameObject;
                _clearText = child.Find("ClearText").gameObject.GetComponent<TextMeshProUGUI>();
                _clearTextShadow = child.Find("ClearTextShadow").gameObject.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public void FadeOut()
    {
        _stageClearUIController.OnStageClear();
    }

    // 250417 추가: 스테이지 클리어 관련
    public void ShowStageResult(int stage)
    {
        _clearText.text = $"Stage {stage + 1} Cleared!";
        _clearTextShadow.text = $"Stage {stage + 1} Cleared!";
        //_clearUI.GetComponent<Canvas>().enabled = true;
        _clearUI.SetActive(true);
    }

    public void HideResult()
    {
        //_stageClearCanvas.GetComponent<Canvas>().enabled = false;
        _stageClearUIController.OnStageStart();
        _clearUI.SetActive(false);
    }

    public void Clear()
    {
        toggleHelpPanelCanvasAction = null;

        showJudgeTextAction = null;
        activateSkillTextAction = null;
        updateWaveAction = null;
    }
}