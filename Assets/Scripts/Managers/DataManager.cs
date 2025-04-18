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
    Dictionary<int, (AudioClip music, float bpm, GameObject background)> _stageDataDict = new Dictionary<int, (AudioClip, float, GameObject)>();

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
        //{
        //    0, new float[][]
        //    {
        //        new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        //        new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        //        new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }
        //    }
        //},

        // 스테이지 1: 쉬움 -> 기존노래
        {
            0, new float[][]
            {
                new float[]{0.9f, 0.1f, 0.0f, 0.0f, 0.0f, 0.0f },
                new float[]{0.8f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f },
                new float[]{0.7f, 0.3f, 0.0f, 0.0f, 0.0f, 0.0f }
            }
        },

        // 스테이지 2: 노멀 -> 기존노래
        {
            1, new float[][]
            {
                new float[]{0.5f, 0.3f, 0.2f, 0.0f, 0.0f, 0.0f },
                new float[]{0.5f, 0.3f, 0.0f, 0.2f, 0.0f, 0.0f },
                new float[]{0.5f, 0.3f, 0.0f, 0.0f, 0.2f, 0.0f }
            }
        },

        // 스테이지 3: 어려움 -> 보스노래
        {
            2, new float[][]
            {
                new float[]{0.4f, 0.3f, 0.15f, 0.0f, 0.15f, 0.0f },
                new float[]{0.3f, 0.3f, 0.0f, 0.15f, 0.15f, 0.1f },
                new float[]{0.1f, 0.3f, 0.1f, 0.1f, 0.1f, 0.3f }
            }
        },

        // 스테이지 4: 짱어려움 -> 보스노래
        {
            3, new float[][]
            {
                new float[]{0.1f, 0.4f, 0.1f, 0.1f, 0.1f, 0.2f },
                new float[]{0.2f, 0.2f, 0.1f, 0.1f, 0.1f, 0.3f },
                new float[]{0.1f, 0.2f, 0.1f, 0.1f, 0.1f, 0.4f }
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
        //_enemyPrefabDict.Add(EnemyType., Resources.LoadAll<Enemy>("Prefabs/Enemies/Normal").ToList());
        _enemyPrefabDict.Add(EnemyType.Normal, Resources.LoadAll<Enemy>("Prefabs/Enemies/Normal").ToList());
        _enemyPrefabDict.Add(EnemyType.Strong, Resources.LoadAll<Enemy>("Prefabs/Enemies/Strong").ToList());
        _enemyPrefabDict.Add(EnemyType.FireRes, Resources.LoadAll<Enemy>("Prefabs/Enemies/FireRes").ToList());
        _enemyPrefabDict.Add(EnemyType.WaterRes, Resources.LoadAll<Enemy>("Prefabs/Enemies/WaterRes").ToList());
        _enemyPrefabDict.Add(EnemyType.GrassRes, Resources.LoadAll<Enemy>("Prefabs/Enemies/GrassRes").ToList());
        _enemyPrefabDict.Add(EnemyType.Special, Resources.LoadAll<Enemy>("Prefabs/Enemies/Special").ToList());

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

        // BGM, BPM, backgorund Image 로드
        LoadStageData();
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
                float NormalWeight = _enemyPrefabDict[EnemyType.Normal].Count > 0 ? weights[i][0] / _enemyPrefabDict[EnemyType.Normal].Count : 0f;
                float StrongWeight = _enemyPrefabDict[EnemyType.Strong].Count > 0 ? weights[i][1] / _enemyPrefabDict[EnemyType.Strong].Count : 0f;
                float FireResWeight = _enemyPrefabDict[EnemyType.FireRes].Count > 0 ? weights[i][2] / _enemyPrefabDict[EnemyType.FireRes].Count : 0f;
                float WaterResWeight = _enemyPrefabDict[EnemyType.WaterRes].Count > 0 ? weights[i][3] / _enemyPrefabDict[EnemyType.WaterRes].Count : 0f;
                float GrassResWeight = _enemyPrefabDict[EnemyType.GrassRes].Count > 0 ? weights[i][4] / _enemyPrefabDict[EnemyType.GrassRes].Count : 0f;
                float SpecialWeight = _enemyPrefabDict[EnemyType.Special].Count > 0 ? weights[i][5] / _enemyPrefabDict[EnemyType.Special].Count : 0f;


                _enemyPrefabDict[EnemyType.Normal].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, NormalWeight)));
                _enemyPrefabDict[EnemyType.Strong].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, StrongWeight)));
                _enemyPrefabDict[EnemyType.FireRes].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, FireResWeight)));
                _enemyPrefabDict[EnemyType.WaterRes].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, WaterResWeight)));
                _enemyPrefabDict[EnemyType.GrassRes].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, GrassResWeight)));
                _enemyPrefabDict[EnemyType.Special].ForEach(enemy => _stageWaveInfoDict[stage][i].Add((enemy, SpecialWeight)));
            }
        }
    }

    public Dictionary<int, List<(Enemy enemy, float weight)>> GetStageWaveInfo(int stage)
    {
        return _stageWaveInfoDict.ContainsKey(stage) ? _stageWaveInfoDict[stage] : new Dictionary<int, List<(Enemy, float)>>();
    }

    public (AudioClip music, float bpm, GameObject background) GetStageData(int stage)
    {
        return _stageDataDict.ContainsKey(stage) ? _stageDataDict[stage] : _stageDataDict[0];

        //return (null, 60f, null);
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

    private void LoadStageData()
    {
        //_stageDataDict[0] = (
        //    music: Resources.Load<AudioClip>("Sounds/BGM/Title"),
        //    bpm: 70f,
        //    background: Resources.Load<GameObject>("Background/Boss")
        //);
        _stageDataDict[0] = (
            music: Resources.Load<AudioClip>("Sounds/BGM/Main"),
            bpm: 70f,
            background: Resources.Load<GameObject>("Background/Boss")
        );
        _stageDataDict[1] = (
            music: Resources.Load<AudioClip>("Sounds/BGM/Main"),
            bpm: 74f,
            background: Resources.Load<GameObject>("Background/Boss")
        );
        _stageDataDict[2] = (
            music: Resources.Load<AudioClip>("Sounds/BGM/Boss"),
            bpm: 78f,
            background: Resources.Load<GameObject>("Background/Boss")
        );
        _stageDataDict[3] = (
            music: Resources.Load<AudioClip>("Sounds/BGM/Boss"),
            bpm: 82f,
            background: Resources.Load<GameObject>("Background/Boss")
        );
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