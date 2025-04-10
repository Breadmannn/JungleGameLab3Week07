using System.Collections;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public RhythmManager rhythmManager; // RhythmManager ����
    public SpriteRenderer floor;        // �ٴ� ������Ʈ�� SpriteRenderer
    public Color beatColor = Color.red; // ��Ʈ �߻� �� ����
    public Color baseColor = Color.white; // �⺻ ����
    public float fadeDuration = 0.5f;   // ���� ���̵� �ð� (��Ʈ ������ �Ϻ�)
    private float beatInterval;
    void Awake()
    {
        if (rhythmManager == null)
            rhythmManager = FindAnyObjectByType<RhythmManager>();
    }
    void Start()
    {
        rhythmManager.OnBeat += HandleBeat; // OnBeat �̺�Ʈ ����
        beatInterval = rhythmManager.GetBeatInterval();
        floor.color = baseColor; // �ʱ� ���� ����
    }
    void HandleBeat()
    {
        // ��Ʈ �߻� �� ���� ���� �� ���̵� ����
        StopAllCoroutines(); // ���� ���̵� �ߴ�
        StartCoroutine(FadeColor());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FadeColor()
    {
        floor.color = beatColor; // ��Ʈ ������ ���� ����
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            floor.color = Color.Lerp(beatColor, baseColor, elapsed / fadeDuration);
            yield return null;
        }

        floor.color = baseColor; // ������ �⺻ �������� ����
    }
}
