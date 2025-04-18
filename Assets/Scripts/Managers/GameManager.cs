using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System.Collections;

// 게임 전반적인 관리 담당
// 적 소환, 플레이어 및 동료 관리, 웨이브 관리 등
public class GameManager : MonoBehaviour
{
    [Header("싱글톤")]
    static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("적 및 웨이브")]
    [SerializeField] int _currentStage = 0;                                               // 현재 스테이지
    int _currentWave = 0;                                                                 // 현재 웨이브
    int _waveMonsterCount = 0;                                                            // 웨이브 몬스터 수
    [SerializeField] List<(Enemy enemyPrefab, float weight)> _currentWaveSpawnInfoList;   // 현재 웨이브의 적 정보 리스트
    [SerializeField] Transform _enemySpawnPoint;                                          // 적 소환 지점
    int _noneMonsterCount = 0;                                                             // 공백 몬스터 수

    [Header("플레이어 및 동료")]
    public PlayerController PlayerController => _playerController;
    public Friend Friend => _friend;
    [SerializeField] PlayerController _playerController;
    [SerializeField] Friend _friend;

    Enemy _currentEnemy; // 추후에 리스트로 바꿔서 관리하기
    public Enemy CurrentEnemy => _currentEnemy;

    public List<Enemy> CurrentEnemyList => _currentEnemyList;
    [SerializeField] List<Enemy> _currentEnemyList = new List<Enemy>();      // 현재 적 리스트 

    Coroutine NextStageCoroutine; // 스테이지 클리어 패널이 두 번 나오지 않도록 코루틴 레퍼런스 저장

    //[Header("스테이지 메타데이터")]
    //[SerializeField] SpriteRenderer _background; // 배경 생기면 넣어야 할 가능성. 당장엔 안씀.

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _playerController = FindAnyObjectByType<PlayerController>();
        _friend = FindAnyObjectByType<Friend>();
        _enemySpawnPoint = FindAnyObjectByType<EnemySpawnPoint>().transform;
    }

    void Start()
    {
        string sceneName = Manager.Scene.GetActiveScene().name;
        _friend.PrepareElemental(Elemental.None);
        //if (sceneName == "InGameScene")
        //{
        //    LoadStage(_currentStage);
        //}
        if (sceneName == "TutorialScene")
        {
            InitializeTutorial();
        }
        
        _friend.PrepareElemental(Elemental.Fire);
        if (sceneName == "InGameScene")
        {
            LoadStage(_currentStage);
        }

        //Debug.Log($"웨이브 {_currentWave} 시작! 가중치: Normal={_weightedEnemies.First(e => _normalEnemyPrefabList.Contains(e.prefab)).weight}, Special={_weightedEnemies.First(e => _specialEnemyPrefabList.Contains(e.prefab)).weight}, Confuse={_weightedEnemies.First(e => _confuseEnemyPrefabList.Contains(e.prefab)).weight}");
    }


    // 240417 추가함수
    // 스테이지 인덱스로 정보를 로드
    public void LoadStage(int stageIndex)
    {
        Debug.LogWarning("LoadStage 호출");

        _currentStage = stageIndex;
        _currentWave = 0;
        _currentEnemyList.Clear();

        var stageData = Manager.Data.GetStageData(stageIndex);
        // background 변경하는 부분. 임시 생략
        BGM bgm = stageIndex switch
        {
            0 => BGM.Main,
            1 => BGM.Main,
            2 => BGM.Boss,
            3 => BGM.Boss,
            _ => BGM.Title
        };
        Manager.Sound.PlayBGM(bgm);
        RhythmManager.Instance.SetBpm(stageIndex switch
        {
            //0 => 66,
            0 => 66,
            1 => 70,
            2 => 74,
            3 => 78,
            _ => 66
        }); // 240417 인덱스 있는 함수도 설정해야함

        //Debug.Log($"Current BPM: {RhythmManager.Instance.BPM}");

        StartWave();
    }

    public void StartWave()
    {
        //if (Manager.Data.WaveInfoDict.TryGetValue(_currentWave, out List<(Enemy enemy, float weight)> currentWaveInfoList))
        if (Manager.Data.GetStageWaveInfo(_currentStage).TryGetValue(_currentWave, out var currentWaveInfoList))
        {
            _currentWaveSpawnInfoList = currentWaveInfoList;
            // 250417: 몬스터 개체수 지정하는 부분
            //_waveMonsterCount = _currentStage == 0 ? 1 : 20;
            _waveMonsterCount = 30;
            _currentWave++;
            Manager.UI.updateWaveAction?.Invoke(_currentWave); // UI 업데이트

            RhythmManager.Instance.IncreaseBPM();
            if (_currentWave < 6)
            {
                if (_currentWave <= 3)
                {
                    _noneMonsterCount = 10;
                }
                else
                {
                    _noneMonsterCount = 5;
                }
            }
            else
            {
                _noneMonsterCount = 0;
            }
            //Debug.Log($"웨이브 {_currentWave} 시작! 가중치: Normal={currentWaveInfoList.First(e => _normalEnemyPrefabList.Contains(e.prefab)).weight}, Special={waveConfig.First(e => _specialEnemyPrefabList.Contains(e.prefab)).weight}, Confuse={waveConfig.First(e => _confuseEnemyPrefabList.Contains(e.prefab)).weight}");
        }
        else
        {
            // 250417: UI에 결과 canvas 만들어야함
            //Manager.Scene.LoadScene(SceneType.GameClearScene);
            //Debug.Log($"웨이브 {_currentWave} 설정이 없습니다! Clear Scene으로 이동합니다.");
            //return;
            if (NextStageCoroutine == null)
            {
                NextStageCoroutine = StartCoroutine(ShowClearCanvasAndProceed());
            }
        }
    }

    private void InitializeTutorial()
    {
        // 상태 초기화
        _currentStage = 0;
        _currentWave = 0;
        _currentWaveSpawnInfoList = null;
        _currentEnemyList.Clear();
        _waveMonsterCount = 0;
        _noneMonsterCount = 0;

        // BGM, BPM 설정
        Manager.Sound.PlayBGM(BGM.Main);
        RhythmManager.Instance.SetBpm(70);

        // 필드에서 EnemyType.Tutorial 몬스터 찾기
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();


        _currentEnemy = allEnemies[0];
        _currentEnemyList.Add(_currentEnemy);
    }

    // 적 소환
    public void SpawnEnemy(Vector3 spawnPoint)
    {
        if (!CheckSpawnCondition())
            return;
        if (_currentWave < 6)
        {
            if (_noneMonsterCount > 0)
            {
                if (Random.value > 0.3f)
                {
                    Enemy enemy = GetRandomEnemy();
                    _currentEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
                    _currentEnemyList.Add(_currentEnemy);
                    _waveMonsterCount--;
                    //Debug.Log($"웨이브 {_currentWave} 몬스터 타입{enemy}");
                }
                else { _waveMonsterCount--; _noneMonsterCount--; }
            }
            else
            {
                Enemy enemy = GetRandomEnemy();
                _currentEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
                _currentEnemyList.Add(_currentEnemy);
            }
        }
        else
        {
            Enemy enemy = GetRandomEnemy();
            _currentEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
            _currentEnemyList.Add(_currentEnemy);
        }
        _waveMonsterCount--;
    }

    Enemy GetRandomEnemy()
    {
        float totalWeight = _currentWaveSpawnInfoList.Sum(enemySpawnInfo => enemySpawnInfo.weight);
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var enemySpawnInfo in _currentWaveSpawnInfoList)
        {
            currentWeight += enemySpawnInfo.weight;
            if (randomValue <= currentWeight)
                return enemySpawnInfo.enemyPrefab;
        }
        return _currentWaveSpawnInfoList.Last().enemyPrefab;
    }

    bool CheckSpawnCondition()
    {
        if (Manager.Scene.GetActiveScene().name == "InGameScene")
        {
            if (_waveMonsterCount <= 0)
            {
                if (_currentEnemyList.Count == 0)
                {
                    StartWave(); // 다음 웨이브 시작
                    Debug.Log($"웨이브 {_currentWave} 시작!");
                }
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // 적 중 가장 앞에 있는 적 반환 (여러 개면 전부 반환)
    public List<Enemy> GetFrontEnemies()
    {
        float minX = float.MaxValue;
        for(int i=0; i<_currentEnemyList.Count; i++)
        {
            if (_currentEnemyList[i] == null)
                continue;
            if (_currentEnemyList[i].transform.position.x < minX)
                minX = _currentEnemyList[i].transform.position.x;
        }

        return _currentEnemyList.Where(enemy => Mathf.Approximately(enemy.transform.position.x, minX)).ToList();
    }
    
    // 적 중 가장 앞에 있는 적 반환 (1명만)
    public List<Enemy> GetFrontEnemy()
    {
        float minX = float.MaxValue;
        int minIndex = -1;
        for(int i=0; i<_currentEnemyList.Count; i++)
        {
            if (_currentEnemyList[i] == null)
                continue;
            if (_currentEnemyList[i].transform.position.x < minX)
            {
                minX = _currentEnemyList[i].transform.position.x;
                minIndex = i;
            }
        }

        return new List<Enemy> { _currentEnemyList[minIndex] };
    }

    // 소환 지점 검사
    public bool CheckSpawnPoint(out Vector3 spawnPoint)
    {
        spawnPoint = _enemySpawnPoint.position;
        if (_enemySpawnPoint == null || _currentEnemyList.Count == 0)
            return true;

        float spawnX = _enemySpawnPoint.position.x;
        List<Enemy> spawnPointEnemies = new List<Enemy>();
        foreach (Enemy enemy in _currentEnemyList)
        {
            if (Mathf.Approximately(enemy.transform.position.x, spawnX))
                spawnPointEnemies.Add(enemy);
        }
        if(spawnPointEnemies.Count == 0)
            return true;
        else if (spawnPointEnemies.Count == 1)
        {
            if(spawnPointEnemies[0].transform.position == _enemySpawnPoint.position)
                spawnPoint = _enemySpawnPoint.position + new Vector3(0, -4f, 0);
            return true;
        }
        return false;
    }

    public void NextStage()
    {
        Debug.LogWarning($"NextStage:{_currentStage}");
        LoadStage(++_currentStage);
    }

    IEnumerator ShowClearCanvasAndProceed()
    {
        // 스테이지 정지
        Manager.UI.FadeOut();
        yield return new WaitForSeconds(1.2f);
        Manager.UI.ShowStageResult(_currentStage);
        Time.timeScale = 0f;


        // 키 입력 전까지 스테이지 시작 대기
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        // 스테이지 재시작
        Time.timeScale = 1f;
        Manager.UI.HideResult();
        if (_currentStage < 3)
        {
            NextStage();
        }
        else
        {
            Manager.Scene.LoadScene(SceneType.GameClearScene);
        }

        NextStageCoroutine = null;
    }
}
