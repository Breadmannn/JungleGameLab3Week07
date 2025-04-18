using System.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class StageClearUIController : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeOutDuration = 0.5f;
    [SerializeField] float fadeInDuration = 0.5f;

    public void OnStageClear()
    {
        StartCoroutine(FadeOut());
    }

    public void OnStageStart()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        //fadePanel.canvas.enabled = true;

        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }

        ActivateStageClearCanvas();
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }

        // Fade In 완료 후 패널 비활성화
        //fadePanel.canvas.enabled = false;
    }

    private void ActivateStageClearCanvas()
    {

    }
}
