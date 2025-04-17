using System;
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

    [Header("Stage Clear")]
    [SerializeField] GameObject _stageClearCanvas;
    [SerializeField] TextMeshProUGUI _resultText;

    #endregion


    public void SetStageClearCanvas(GameObject stageClearCanvas)
    {
        _stageClearCanvas = stageClearCanvas;
        _resultText = stageClearCanvas.transform.GetComponentInChildren<TextMeshProUGUI>(); 
    }

    // 250417 추가: 스테이지 클리어 관련
    public void ShowStageResult(int stage)
    {
        _stageClearCanvas.GetComponent<Canvas>().enabled = true;
        _resultText.text = $"Stage {stage + 1} Cleared!";
    }

    public void HideResult()
    {
        _stageClearCanvas.GetComponent<Canvas>().enabled = false;
    }


    public void Clear()
    {
        toggleHelpPanelCanvasAction = null;

        showJudgeTextAction = null;
        activateSkillTextAction = null;
        updateWaveAction = null;
    }
}