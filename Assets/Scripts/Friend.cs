using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Define;

public class Friend : MonoBehaviour
{
    static Friend _instance;
    public static Friend Instance => _instance;

    //public Elemental VisualElemental => _visualElemental;
    public Elemental RealElemental => _realElemental;
    [SerializeField] Elemental _realElemental = Elemental.None;     // 실제 속성
    //UI_FriendCastVisualCanvas _friendCastVisualCanvas;              // 동료 마법 예고 UI
    [SerializeField] UI_FriendCastVisualCanvas _friendCastVisualCanvas;              // 동료 마법 예고 UI
    Animator _visualAnim;                                               // 애니메이터
    public Animator Anim => _visualAnim; // 애니메이터
    Animator _anim;                                         // 비주얼 애니메이터
    [SerializeField] GameObject _singleFieldEffect;
    
    // 튜토리얼 관련 변수
    public bool isTutorial = false;                          //튜토리얼인지 체크
    public bool isFreePractice = false;
    TutorialManager tutorialManager;           // 튜토리얼 매니저
    void Awake()
    {
        if (_instance == null)
            _instance = this;

        _anim = GetComponent<Animator>();
        //_friendCastVisualCanvas = GetComponentInChildren<UI_FriendCastVisualCanvas>();
        _visualAnim = transform.Find("Visual").GetComponent<Animator>();
    }

    // 마법 준비
    public void PrepareElemental(Elemental elemental)
    {
        if (isTutorial)
        {
            if (isFreePractice)
            {
                _realElemental = GetRandomElemental();
            }
            else
            {
                _realElemental = elemental;
                Debug.LogError("튜토리얼진행중");
            }
        }
        else
        {
            // 무작위 속성 선택해서 예고
            //_visualElemental = GetRandomElemental();
            _realElemental = GetRandomElemental();
            //TryMistake();

            // 동료 마법 예고 UI에 원소 표시
        }
        _friendCastVisualCanvas.SetElementalImage(_realElemental);
        
        //Debug.Log($"친구 예고: 실제({_realElemental})");
    }

    public Elemental GetRandomElemental()
    {
        Elemental randomElemental = (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1);
        return randomElemental;
    }
    public void Wriggle()
    {
        _anim.SetTrigger("WriggleTrigger");
    }

    public void SetSingleField(bool state)
    {
        _singleFieldEffect.SetActive(state);
    }
    public void TutorialElemental(Elemental elemental)
    {
        
        _realElemental = elemental;
        _friendCastVisualCanvas.SetElementalImage(_realElemental);

    }
}