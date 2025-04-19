using System.IO;
using System.Text;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    static LogManager _instance;
    public static LogManager Instance => _instance;

    [System.Serializable]
    public struct LogData
    {
        public bool Clear;              // 클리어 여부
        public float TotalPlayTime;     // 총 플레이 시간
        public float MinRestTime;       // 최소 휴식 시간 (스테이지 클리어 상태)
        public float MaxRestTime;       // 최대 휴식 시간 (스테이지 클리어 상태)
        public float AvgRestTime;       // 평균 휴식 시간 (스테이지 클리어 상태)
        public int MaxStage;            // 최고 도달 스테이지
        public int AttackSkill;          // 공격 스킬 사용 횟수
        public int buffSkill;           // 버프 스킬 사용 횟수
    }

    float totalTime = 0;
    float totalRestTime = 0;
    float restTime = 0;
    int numRest = 0;
    bool isCheckingRestTime = false;
    LogData logData;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        logData = new LogData();
        logData.Clear = false;
        logData.TotalPlayTime = 0;
        logData.MinRestTime = 0;
        logData.MaxRestTime = 0;
        logData.AvgRestTime = 0;
        logData.MaxStage = 0;
        logData.AttackSkill = 0;
        logData.buffSkill = 0;


    }

    private void Update()
    {
        totalTime += Time.deltaTime;
        if (isCheckingRestTime)
        {
            restTime += Time.deltaTime;
        }
    }

    public void LogClear(bool state)
    {
        logData.Clear = state;
    }
    public void LogTotalTime()
    {
        logData.TotalPlayTime = totalTime;
    }

    public void LogRest(bool start)
    {
        if (start)
        {
            restTime = 0;
            isCheckingRestTime = true;
        }
        else
        {
            numRest++;
            isCheckingRestTime = false;
            totalRestTime += restTime;
            if (logData.MinRestTime > restTime)
            {
                logData.MinRestTime = restTime;
            }
            if (logData.MaxRestTime < restTime)
            {
                logData.MaxRestTime = restTime;
            }
        }
    }

    public void LogMax(int stageNum)
    {
        if (stageNum > logData.MaxStage)
        {
            logData.MaxStage = stageNum;
        }
    }

    public void LogAttack()
    {
        logData.AttackSkill += 1;
    }

    public void LogBuff()
    {
        logData.buffSkill += 1;
    }

    public void ExportToCSV(string fileName = "log.csv")
    {
        LogTotalTime();

        if (numRest > 0)
        {
            logData.AvgRestTime = totalRestTime / numRest;
        }

        StringBuilder csv = new StringBuilder();

        // 헤더
        csv.AppendLine("Clear,TotalPlayTime,MinRestTime,MaxRestTime,AvgRestTime,MaxStage,AttackSkill,BuffSkill");

        // 데이터 저장
        csv.AppendLine($"{logData.Clear}," +
                   $"{logData.TotalPlayTime}," +
                   $"{logData.MinRestTime}," +
                   $"{logData.MaxRestTime}," +
                   $"{logData.AvgRestTime}," +
                   $"{logData.MaxStage}," +
                   $"{logData.AttackSkill}," +
                   $"{logData.buffSkill}");

        string buildFolderPath = GetBuildFolderPath();

        string filePath = Path.Combine(buildFolderPath, fileName);

        try
        {
            File.WriteAllText(filePath, csv.ToString());
            Debug.Log($"Log exported to build folder: {filePath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to write CSV file: {e.Message}");
        }
    }

    private string GetBuildFolderPath()
    {
#if UNITY_EDITOR
        return Directory.GetParent(Application.dataPath).FullName;
#else
    return Application.dataPath; // This points to the build folder in standalone builds
#endif
    }

    private void OnApplicationQuit()
    {
        ExportToCSV();
    }
}