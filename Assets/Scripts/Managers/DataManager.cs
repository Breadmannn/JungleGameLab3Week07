using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
using static DataManager;


// 데이터 관리
// 스킬, 적, 웨이브 등 정보
public class DataManager
{
    // 적 Prefab 딕셔너리
    Dictionary<EnemyType, List<Enemy>> _enemyPrefabDict = new Dictionary<EnemyType, List<Enemy>>();

    [Header("Wave")]
    //int _maxWave = 9;
    //public Dictionary<int, List<(Enemy enemy, float weight)>> WaveInfoDict => _waveInfoDict;
    //Dictionary<int, List<(Enemy enemy, float weight)>> _waveInfoDict = new Dictionary<int, List<(Enemy enemy, float weight)>>(); // key: 웨이브 번호, value: List<(적 Prefab, 가중치)>

    // 스테이지별 웨이브 정보
    Dictionary<int, Dictionary<int, List<(Enemy enemy, float weight)>>> _stageWaveInfoDict = new Dictionary<int, Dictionary<int, List<(Enemy enemy, float weight)>>>();

    // 스테이지별 메타데이터
    Dictionary<int, (AudioClip music, float bpm, Sprite background)> _stageDataDict = new Dictionary<int, (AudioClip, float, Sprite)>();

    //float[][] _waveWeights =
    //{
    //    // [0]: Normal, [1]: Special, [2]: Confuse
    //    new float[] { 1f, 0f, 0f },         // 웨이브 1: Normal 100%, Special 0%, Confuse 0%
    //    new float[] { 0.8f, 0f, 0.2f },          // 웨이브 1: Normal 100%, Special 0%, Confuse 0%
    //    new float[] { 0.9f, 0.1f, 0f },         // 웨이브 1: Normal 100%, Special 0%, Confuse 0%
    //    new float[] { 0.8f, 0.1f, 0.1f },     // 웨이브 2: Normal 70%, Special 0%, Confuse 30%
    //    new float[] { 0.7f, 0.1f, 0.3f },     // 웨이브 2: Normal 70%, Special 20%, Confuse 10%
    //    new float[] { 0.6f, 0.2f, 0.2f },     // 웨이브 2: Normal 60%, Special 20%, Confuse 20%
    //    new float[] { 0.5f, 0.3f, 0.2f },     // 웨이브 2: Normal 50%, Special 30%, Confuse 20%
    //    new float[] { 0.4f, 0.3f, 0.3f },     // 웨이브 2: Normal 40%, Special 30%, Confuse 30
    //    new float[] { 0.3f, 0.4f, 0.3f }    // 웨이브 3: Normal 30%, Special 40%, Confuse 30%
    //};

    Dictionary<int, float[][]> _stageWaveWeights = new Dictionary<int, float[][]>
    {
        // [0]: 허수아비 [1]: 약한적 [2]: 강한적 [3]: 불저항 [4]: 물저항 [5]: 풀저항 [6]: 탱커

        // 스테이지 0: 튜토리얼 -> 로비노래
        {
            0, new float[][]
            {
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }
            }
        },

        // 스테이지 1: 쉬움 -> 기존노래
        {
            1, new float[][]
            {
                new float[]{0.0f, 0.9f, 0.1f, 0.0f, 0.0f, 0.0f, 0.0f },
                new float[]{0.0f, 0.8f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f },
                new float[]{0.0f, 0.7f, 0.3f, 0.0f, 0.0f, 0.0f, 0.0f }
            }
        },

        // 스테이지 2: 노멀 -> 기존노래
        {
            2, new float[][]
            {
                new float[]{0.0f, 0.5f, 0.3f, 0.2f, 0.0f, 0.0f, 0.0f },
                new float[]{0.0f, 0.5f, 0.3f, 0.0f, 0.2f, 0.0f, 0.0f },
                new float[]{0.0f, 0.5f, 0.3f, 0.0f, 0.0f, 0.2f, 0.0f }
            }
        },

        // 스테이지 3: 어려움 -> 보스노래
        {
            3, new float[][]
            {
                new float[]{0.0f, 0.4f, 0.3f, 0.15f, 0.0f, 0.15f, 0.0f },
                new float[]{0.0f, 0.3f, 0.3f, 0.0f, 0.15f, 0.15f, 0.1f },
                new float[]{0.0f, 0.1f, 0.3f, 0.1f, 0.1f, 0.1f, 0.3f }
            }
        },

        // 스테이지 4: 짱어려움 -> 보스노래
        {
            4, new float[][]
            {
                new float[]{0.0f, 0.1f, 0.4f, 0.1f, 0.1f, 0.1f, 0.2f },
                new float[]{0.0f, 0.2f, 0.2f, 0.1f, 0.1f, 0.1f, 0.3f },
                new float[]{0.0f, 0.1f, 0.2f, 0.1f, 0.1f, 0.1f, 0.4f }
            }
        }
    };

    [Header("스킬")]
    Dictionary<ElementalEffect, Skill> _skillDict = new Dictionary<ElementalEffect, Skill>();   // 스킬 딕셔너리
    public Dictionary<ElementalEffect, Skill> SkillDict => _skillDict;
    

    Dictionary<string, SkillInfo> _skillInfoDict = new Dictionary<string, SkillInfo>();
    public Dictionary<string, SkillInfo> SkillInfoDict => _skillInfoDict;
    


    public void Init()
    {
        // 적 Prefab 로드 및 웨이브 정보 설정
        //_enemyPrefabDict.Add(EnemyType.Normal, Resources.LoadAll<Enemy>("Prefabs/Enemies/Normal").ToList());
        //_enemyPrefabDict.Add(EnemyType.Special, Resources.LoadAll<Enemy>("Prefabs/Enemies/Special").ToList());
        //_enemyPrefabDict.Add(EnemyType.Confuse, Resources.LoadAll<Enemy>("Prefabs/Enemies/Confuse").ToList());

        // 250417 KWS: 테스트 몬스터 로드. 추후 프리펩 이름 변경요망
        _enemyPrefabDict.Add(EnemyType.TestType0, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage0").ToList());
        _enemyPrefabDict.Add(EnemyType.TestType1, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage1").ToList());
        _enemyPrefabDict.Add(EnemyType.TestType2, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage2").ToList());
        _enemyPrefabDict.Add(EnemyType.TestType3, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage3").ToList());
        _enemyPrefabDict.Add(EnemyType.TestType4, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage4").ToList());
        _enemyPrefabDict.Add(EnemyType.TestType5, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage5").ToList());
        _enemyPrefabDict.Add(EnemyType.TestType6, Resources.LoadAll<Enemy>("Prefabs/Enemies/TestStage6").ToList());

        //SetWaveInfo();
        SetStageWaveInfo();

        //Debug.Log($"몬스터 프리팹 로드 완료 {_enemyPrefabDict[EnemyType.Normal].Count} {_enemyPrefabDict[EnemyType.Special].Count} {_enemyPrefabDict[EnemyType.Confuse].Count}");

        // 스킬 Prefab 로드
        GameObject[] skillEffectPrefabs = Manager.Resource.LoadAll<GameObject>($"Prefabs/Skills");
        foreach (GameObject skillEffectPrefab in skillEffectPrefabs)
        {
            string skillEffectPrefabName = skillEffectPrefab.name;
            if (Enum.TryParse(skillEffectPrefabName, out ElementalEffect elementalEffectType))
            {
                _skillDict.Add(elementalEffectType, skillEffectPrefab.GetComponent<Skill>());
            }
        }

        // 스킬 정보 로드
        LoadSkillInfo();
    }

    // 웨이브 정보 설정
    //public void SetWaveInfo()
    //{
    //    for (int i = 0; i < _maxWave; i++)
    //    {
    //        _waveInfoDict[i] = new List<(Enemy, float)>();
    //        float normalWeight = _enemyPrefabDict[EnemyType.Normal].Count > 0 ? _waveWeights[i][0] / _enemyPrefabDict[EnemyType.Normal].Count : 0f;
    //        float specialWeight = _enemyPrefabDict[EnemyType.Special].Count > 0 ? _waveWeights[i][1] / _enemyPrefabDict[EnemyType.Special].Count : 0f;
    //        float confuseWeight = _enemyPrefabDict[EnemyType.Confuse].Count > 0 ? _waveWeights[i][2] / _enemyPrefabDict[EnemyType.Confuse].Count : 0f;
    //        _enemyPrefabDict[EnemyType.Normal].ForEach(enemyPrefab => _waveInfoDict[i].Add((enemyPrefab, normalWeight)));
    //        _enemyPrefabDict[EnemyType.Special].ForEach(enemyPrefab => _waveInfoDict[i].Add((enemyPrefab, specialWeight)));
    //        _enemyPrefabDict[EnemyType.Confuse].ForEach(enemyPrefab => _waveInfoDict[i].Add((enemyPrefab, confuseWeight)));
    //        Debug.Log($"웨이브 {i + 1} 설정: {_waveInfoDict[i].Count} 항목");
    //    }
    //}

    void SetStageWaveInfo()
    {
        foreach (var stage in _stageWaveWeights.Keys)
        {
            _stageWaveInfoDict[stage] = new Dictionary<int, List<(Enemy, float)>>();
            var weights = _stageWaveWeights[stage];

            for (int i = 0; i < weights.Length; i++)
            {
                _stageWaveInfoDict[stage][i] = new List<(Enemy, float)>();
                float TestType0Weight = _enemyPrefabDict[EnemyType.TestType0].Count > 0 ? weights[i][0] / _enemyPrefabDict[EnemyType.TestType0].Count : 0f;
                float TestType1Weight = _enemyPrefabDict[EnemyType.TestType1].Count > 0 ? weights[i][1] / _enemyPrefabDict[EnemyType.TestType1].Count : 0f;
                float TestType2Weight = _enemyPrefabDict[EnemyType.TestType2].Count > 0 ? weights[i][2] / _enemyPrefabDict[EnemyType.TestType2].Count : 0f;
                float TestType3Weight = _enemyPrefabDict[EnemyType.TestType3].Count > 0 ? weights[i][3] / _enemyPrefabDict[EnemyType.TestType3].Count : 0f;
                float TestType4Weight = _enemyPrefabDict[EnemyType.TestType4].Count > 0 ? weights[i][4] / _enemyPrefabDict[EnemyType.TestType4].Count : 0f;
                float TestType5Weight = _enemyPrefabDict[EnemyType.TestType5].Count > 0 ? weights[i][5] / _enemyPrefabDict[EnemyType.TestType5].Count : 0f;
                float TestType6Weight = _enemyPrefabDict[EnemyType.TestType6].Count > 0 ? weights[i][6] / _enemyPrefabDict[EnemyType.TestType6].Count : 0f;

                _enemyPrefabDict[EnemyType.TestType0].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType0Weight)));
                _enemyPrefabDict[EnemyType.TestType1].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType1Weight)));
                _enemyPrefabDict[EnemyType.TestType2].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType2Weight)));
                _enemyPrefabDict[EnemyType.TestType3].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType3Weight)));
                _enemyPrefabDict[EnemyType.TestType4].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType4Weight)));
                _enemyPrefabDict[EnemyType.TestType5].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType5Weight)));
                _enemyPrefabDict[EnemyType.TestType6].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, TestType6Weight)));
            }
        }
    }

    public Dictionary<int, List<(Enemy enemy, float weight)>> GetStageWaveInfo(int stage)
    {
        return _stageWaveInfoDict.ContainsKey(stage) ? _stageWaveInfoDict[stage] : new Dictionary<int, List<(Enemy, float)>>();
    }

    public (AudioClip music, float bpm, Sprite background) GetStageData(int stage)
    {
        //return _stageDataDict.ContainsKey(stage) ? _stageDataDict[stage] : (null, 120f, null);

        return (null, 60f, null);
    }

    public void LoadSkillInfo()
    {
        TextAsset json = Manager.Resource.Load<TextAsset>("JSON/SkillInfo");
        var test = JsonUtility.FromJson<SkillInfoList>(json.text);
        for (int i = 0; i < test.skillInfoList.Count; i++)
        {
            var info = test.skillInfoList[i];
            _skillInfoDict[info.SkillName] = info;
        }
    }
    [Serializable]
    public class SkillInfo
    {
        public string SkillName;
        public string SkillDescription;
    }

    [Serializable]
    public class SkillInfoList
    {
        public List<SkillInfo> skillInfoList = new List<SkillInfo>();
    }

}