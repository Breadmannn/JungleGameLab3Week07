using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour, IStatus
{
    static PlayerController _instance;
    public static PlayerController Instance => _instance;

    [Header("컴포넌트")]
    public PlayerSkill PlayerSkill => _playerSkill;
    [SerializeField] PlayerSkill _playerSkill;

    [Header("스테이터스")]
    public int Health => _hp;
    [SerializeField] int _hp = 100;
    [SerializeField] int _maxHp = 100;

    [Header("마법")]
    public Elemental PlayerElemental => playerElemental;
    public bool HasInputThisBeat { get { return _hasInputThisBeat; } set { _hasInputThisBeat = value; } } // 현재 비트에서 입력 여부
    public Elemental playerElemental;
    [SerializeField] GameObject _singleFieldEffect;
    bool _hasInputThisBeat = false;     // 현재 비트에서 입력 여부
    Friend _friend;                     // 친구

    [Header("비트 판정")]
    bool _isPerfect;

    [Header("모션 관련")]
    Animator _anim;       // Wriggle 애니메이터
    Animator _visualAnim; // 비주얼 애니메이터

    [Header("튜토리얼")]
    public bool tutorial;
    TutorialManager _tutorialManager; // 튜토리얼 매니저
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        _playerSkill = GetComponent<PlayerSkill>();
        _anim = GetComponent<Animator>();

        _visualAnim = transform.Find("Visual").GetComponent<Animator>();
        _tutorialManager = FindAnyObjectByType<TutorialManager>();               //튜토리얼 매니저 찾기
    }

    private void Start()
    {
        InputManager.Instance.selectElementalAction += SelectElemental; // 원소 선택 이벤트 등록
        _friend = FindAnyObjectByType<Friend>();
    }

    // 플레이어 마법 시전
    public void SelectElemental(Elemental elemental)
    {
        if (_hasInputThisBeat)
        {
            Manager.UI.showJudgeTextAction(false);
            Debug.Log("[즉시 Miss] 큰 박자 내 연속 입력");
            return;
        }

        playerElemental = elemental;
        _isPerfect = RhythmManager.Instance.IsJudging;
        Manager.UI.showJudgeTextAction(_isPerfect);
        if (_isPerfect)
        {
            Attack();
        }
        _hasInputThisBeat = true; // 큰 박자 내 첫 입력 처리 완료
    }

    // 마법 공격 (친구가 시전한 마법과 조합)
    public void Attack()
    {

        ElementalEffect interaction = _playerSkill.GetInteraction(playerElemental, _friend.RealElemental);
        _friend.Anim.SetTrigger("AttackTrigger");
        _visualAnim.SetTrigger("AttackTrigger");

        _playerSkill.ApplyInteraction(interaction);
        Manager.Sound.PlayEffect(Effect.BossDeath);
        if (!tutorial)
        {
            _friend.PrepareElemental(Elemental.None); // 친구 마법 예고 다시
        }
        else if(tutorial)
        {
            _friend.PrepareElemental(_tutorialManager.tutoElemental); // 튜토리얼 마법 예고
        }
    }

    // 플레이어 데미지 피해 
    public void TakeDamage(int amount, Elemental skillElement)
    {
        _hp = Mathf.Clamp(_hp - amount, 0, _maxHp);
        //Debug.Log($"플레이어 HP: {_hp}");
        if (_hp <= 0)
            Die();
    }

    public void Die()
    {
        Manager.Sound.PlayEffect(Effect.PlayerDeath);

        /* 
           사망 애니메이션 재생
        */

        /* 몇 초 뒤에 넘어가게 해야 함 */
        Manager.Scene.LoadScene(SceneType.GameOverScene);
        Debug.Log("플레이어 사망 이벤트 발생");
    }

    //꿈틀거리기
    public void Wriggle(float speed)
    {
        _anim.speed = speed;
        _anim.SetTrigger("WriggleTrigger");
    }

    public void SetSingleField(bool state)
    {
        _singleFieldEffect.SetActive(state);
    }
    public void TutorialElemental()//튜토리얼 전용 하드코딩 이벤트....
    {
        _friend.PrepareElemental(_tutorialManager.tutoElemental); // 튜토리얼 마법 예고

    }
}