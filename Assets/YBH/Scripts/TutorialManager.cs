using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static Define;
public class TutorialManager : MonoBehaviour
{
    enum TutorialStep // 튜토리얼 단계
    {
        None,
        Intro,
        Grass,
        Fire,
        Water,
        Combine_FireWater,
        ConbineCast,
        Combine_FireGrass,
        SingleCast,
        Combine_WaterGrass,
        FreePractice,
        End
    }
    
    TutorialStep _currentStep = TutorialStep.Intro; // 현재 튜토리얼 단계
    public Elemental tutoElemental = Elemental.None;       // 튜토리얼 속성
    [Header("튜토리얼 UI")]
    Canvas _tutorialCanvas;                         // 튜토리얼 캔버스
    TextMeshProUGUI _tutorialText;                  //튜토리얼 텍스트
    [Header("튜토리얼 변수들")]
    Friend _friend;                                 //친구
    PlayerController _playerController;             //플레이어
    InputManager _inputManager;                     // InputManager
    [Header("플레이어 입력속성")]
    bool fire;
    bool water;
    bool grass;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _tutorialCanvas = transform.Find("TutorialCanvas").GetComponent<Canvas>();//튜토리얼 캔버스
        _tutorialText = _tutorialCanvas.GetComponentInChildren<TextMeshProUGUI>();//튜토리얼 텍스트
        _friend = FindAnyObjectByType<Friend>();                                  //친구찾기
        _playerController = FindAnyObjectByType<PlayerController>();              //플레이어 찾기
        
    }
    void Start()
    {
        _inputManager = FindAnyObjectByType<InputManager>(); // InputManager
        _inputManager.selectElementalAction += HandleTutorialInput;
        CurrentTutorialEvent(_currentStep);
        //친구가 특정마법만 준비하는 스크립트 제작해야함
        //불 물 풀
        //복합마법 불 물
        //불 물 복합의 추가사용
        //복합마법 불 풀
        //불 풀 복합의 추가사용
        //속박마법 물 풀
        //자유 연습 구간
        //튜토리얼 언제든 종료단계
    }

    // Update is called once per frame
    void Update()
    {
    }
    void CurrentTutorialEvent(TutorialStep tutostep)
    {
        _currentStep = tutostep; // 현재 튜토리얼 단계 설정
        switch (_currentStep)
        {
            case TutorialStep.Intro: IntroTutorial(); break;
            case TutorialStep.Grass: GrassTutorial(); break;
            case TutorialStep.Fire: FireTutorial(); break;
            case TutorialStep.Water:WaterTutorial(); break;
            case TutorialStep.Combine_FireWater: CombineFireWaterTutorial(); break;     //범위시너지
            case TutorialStep.ConbineCast: CombineCast(); break;                        //멀티필드
            case TutorialStep.Combine_FireGrass: CombineFireGrassTutorial(); break;     //단일시너지
            case TutorialStep.SingleCast: SingleCast(); break;                          //단일필드
            case TutorialStep.Combine_WaterGrass: CombineWaterGrassTutorial(); break;   //속박
            case TutorialStep.FreePractice: FreePracticeTutorial(); break;              //자유 연습
            case TutorialStep.End: TutorialEnd(); break;                                //튜토리얼 종료
        }
    }
    void HandleTutorialInput(Elemental input)
    {
        switch (input)
        {
            case Elemental.Fire:
                fire = true; water = false; grass = false;
                break;
            case Elemental.Water:
                water = true; fire = false; grass = false;
                break;
            case Elemental.Grass:
                grass = true; fire = false; water = false;
                break;
        }
        CurrentTutorialEvent(_currentStep); // 현재 튜토리얼 불러오기
        Debug.LogError(_currentStep);
    }
    #region 튜토리얼 이벤트들(하드코딩)
    void IntroTutorial()
    {
        _tutorialText.text = "일어났구나!";
        _friend.Anim.SetTrigger("HitTrigger"); // 친구 애니메이션 시작
        //튜토리얼 시작
        //튜토리얼 시작 버튼 클릭시
        //튜토리얼 시작
        _friend.PrepareElemental(Elemental.None);
        //대충 텍스트 다 나오면
        //        _tutorialText.text = "일어났구나!" +"그럼 슬슬 마법연습 시작해볼까?"
        StartCoroutine(NextStepAfterDelay(3f)); // 3초 후 다음 단계로 넘어감
    }
    void GrassTutorial()
    {
        Tutorial("리듬에 맞춰서 풀속성 마법을 사용해봐!!\n" + "풀 속성은 Q로 사용할 수 있어!!!\n", Elemental.Grass);
        if (grass)
        {
            Proceed("잘했어! 풀 마법 성공이야!\n"+"적한테 피해를 줬어!");
        }

    }
    void FireTutorial()
    {
        Tutorial("마찬가지로 불로도 피해를 입혀보자!!\n" +
            "불 속성은 W로 사용할 수 있어!!!\n", Elemental.Fire);
        if (fire)
        {
            Proceed("좋아! 불 마법도 성공!");
        }
        // _playerController


    }
    void WaterTutorial()
    {
        Tutorial("물!!!!!!!!!!!!\n" + "물은 E로 사용해!!!!!!!!!!!!", Elemental.Water);
        if (water)
        {
            Proceed("해냈다! 속성 마법들은 전부 성공이야!");
        }

    }
    void CombineFireWaterTutorial() // 멀티필드 튜토리얼
    {
        Tutorial("불물불물불물!@!@!@!@.\n", Elemental.Water);
        if (fire)
        {
            Proceed("안개다!\n"+"이건 범위마법을 쓰기 위한 준비야!");
        }
    }
    void CombineCast()
    {
        Tutorial("안개가 깔렸으니 같은 속성을 사용해보자!", Elemental.Water);
        if (water)
        {
            Proceed("좋았어! 범위공격 성공!\n" +"안개에 속성마법을 사용하면 범위공격이야.");
        }
    }
    void CombineFireGrassTutorial() //단일필드 튜토리얼
    {
        Tutorial("이번에는 풀과 불 마법을 더해보자!", Elemental.Grass);
        if (fire)
        {
            Proceed("마력이 흐르는게 느껴져?\n" + "이상태로 속성마법을 써보자!");
        }

    }
    void SingleCast()
    {
        Tutorial("여기서 속성마법을 사용하면..", Elemental.Grass);
        if (grass)
        {
            Proceed("속성 마법이 강화되서 더 큰 피해를 줘!");
        }
    }
    void CombineWaterGrassTutorial() //속박 튜토리얼
    {
        Tutorial("그리고 물과 풀 마법!", Elemental.Grass);
        if (water)
        {
            Proceed("덩굴이 감싸고… 움직임이 멈췄어! 적들은 몇 박자 동안 못 움직여!");
        }
    }
    void FreePracticeTutorial() // 자유 연습 튜토리얼
    {
        _playerController.tutorial = false;
        _tutorialText.text = "원하는만큼 자유롭게 연습하고 돌아가자!\n" + "돌아가고 싶어지면 Space를 눌러!";
    }
    void TutorialEnd()
    {

    }
    #endregion 튜토리얼이벤트들(하드코딩)
    void Tutorial(string text, Elemental elemental)
    {
        _tutorialText.text = text;
        tutoElemental = elemental; // 튜토리얼 속성 설정
        _friend.TutorialElemental(tutoElemental);
    }
    void Proceed(string successText)
    {
        _tutorialText.text = successText;
        Debug.LogError($"튜토리얼 성공: {_currentStep}");
        StartCoroutine(NextStepAfterDelay(3f));
    }
    IEnumerator NextStepAfterDelay(float delaySeconds) // 튜토리얼 다음단계로 넘어가기
    {
        yield return new WaitForSeconds(delaySeconds);
        Debug.LogError("튜토리얼 다음 단계로 넘어감");
        // 현재 단계 + 1 → 다음 단계로 전환
        _currentStep = (TutorialStep)((int)_currentStep + 1);
        CurrentTutorialEvent(_currentStep);
        Boolreset();

    }
    void Boolreset()
    {
        fire = false;
        water = false;
        grass = false;
    }
}
