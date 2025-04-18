using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 동료 마법 예고 UI
public class UI_FriendCastVisualCanvas : MonoBehaviour
{
    Canvas _friendCastVisualCanvas;

    [SerializeField] Image _backgroundImage;              // 말풍선
    [SerializeField] Image _elementalImage;               // 원소 이미지
    [SerializeField] Sprite[] _elementalSprites;          // 원소 Sprite

    [Header("Color")]
    Color _colorCertain;
    Color _colorRed;
    Color _colorGreen;
    Color _colorBlue;
    

    [Header("Animation")]
    Animator uiAnim;
    private Vector3 posOriginal;
    private float shakeMagnitude = 0.2f;
    private float timeOfAnimation = 0.5f;

    void Awake()
    {
        _friendCastVisualCanvas = GetComponent<Canvas>();

        posOriginal = transform.localPosition;

        Image[] images = GetComponentsInChildren<Image>();
        _backgroundImage = images[0]; // 말풍선
        _elementalImage = images[1];  // 원소 이미지

        ColorUtility.TryParseHtmlString("#FFFFFF", out _colorCertain);
        ColorUtility.TryParseHtmlString("#FFC2C2", out _colorRed);
        ColorUtility.TryParseHtmlString("#C2FFC2", out _colorGreen);
        ColorUtility.TryParseHtmlString("#C2C2FF", out _colorBlue);
        

        uiAnim = GetComponent<Animator>();
    }

    // 원소 이미지 및 말풍선 색 설정
    public void SetElementalImage(Define.Elemental elemental)
    {
        _friendCastVisualCanvas.enabled = true;
        int idx = (int)elemental;
        _elementalImage.sprite = _elementalSprites[idx];
        if (elemental == Define.Elemental.Fire)
            _backgroundImage.color = _colorRed;
        else if (elemental == Define.Elemental.Water)
            _backgroundImage.color = _colorBlue;
        else if (elemental == Define.Elemental.Grass)
            _backgroundImage.color = _colorGreen;
        else
            _backgroundImage.color = _colorCertain;
        uiAnim.SetTrigger("Show");

        //if (isLying)
        //{
        //    uiAnim.SetTrigger("Show");
        //    StartCoroutine(ShakeWhenLying());
        //}
        //else
        //{
        //    uiAnim.SetTrigger("Show");
        //    //StartCoroutine(ShakeWhenLying());
        //}
        //ShowLying
    }

    public IEnumerator ShakeWhenLying()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeOfAnimation)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = new(posOriginal.x + Random.Range(-shakeMagnitude, shakeMagnitude),
            transform.localPosition.y, transform.localPosition.y);
            yield return null;
        }

        transform.localPosition = posOriginal;
    }

    public void SetAnim()
    {
        uiAnim.SetTrigger("Beat");
    }
}