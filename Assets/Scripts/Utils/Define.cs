using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        TitleScene,
        InGameScene,
        GameOverScene,
        GameClearScene,
        TutorialScene
    }

    // 적 상태
    public enum EnemyState
    {
        None,      // 없음
        Stun,     // 감전, 기절
    }

    // 원소 효과
    public enum ElementalEffect
    {

        Fire = (1 << Elemental.Fire) | (1 << Elemental.Fire),          // 불 = 불 + 불
        Water = (1 << Elemental.Water) | (1 << Elemental.Water),          // 물 = 물 + 물
        Grass = (1 << Elemental.Grass) | (1 << Elemental.Grass),        // 풀 = 풀 + 풀
        //------------------------------------------------------------------------------------
        MultiField = (1 << Elemental.Fire) | (1 << Elemental.Water),          //멀티  = 불 + 물
        SingleField = (1 << Elemental.Fire) | (1 << Elemental.Grass),         // 단일 = 불 + 풀
        Stun = (1 << Elemental.Water) | (1 << Elemental.Grass),     // 속박 = 물 + 풀
        MultiFire,
        MultiGrass,
        MultiWater,
        SingleFire,
        SingleGrass,
        SingleWater,
        None
    }

    // 플레이어 마법 속성
    public enum Elemental
    {
        Fire = 0,      // 화염
        Water = 1,      // 물
        Grass = 2,     // 풀
        None
    }

    // 적 타입
    public enum EnemyType
    {
        Normal,    // 기본타입
        Strong,    // 강한 타입
        FireRes,    // 불저항
        WaterRes,   // 물저항
        GrassRes,   // 풀저항
        Special,   // 탱크
        Tutorial
    }
    public enum Translation
    {
        불 = (1 << Elemental.Fire) | (1 << Elemental.Fire),          // 불 = 불 + 불
        물 = (1 << Elemental.Water) | (1 << Elemental.Water),          // 물 = 물 + 물
        풀 = (1 << Elemental.Grass) | (1 << Elemental.Grass),        // 풀 = 풀 + 풀
        //------------------------------------------------------------------------------------
        범위 = (1 << Elemental.Fire) | (1 << Elemental.Water),          //멀티  = 불 + 물
        단일 = (1 << Elemental.Fire) | (1 << Elemental.Grass),         // 단일 = 불 + 풀
        속박 = (1 << Elemental.Water) | (1 << Elemental.Grass),     // 속박 = 물 + 풀

    }

    #region Sound
    public enum BGM
    {
        None,
        Title,              // TitleScene
        Main,               // InGameScene
        Boss,               // BossScene
        GameOver            // GameOverScene
    }
    public enum Effect
    {
        BestElemental,      // 최고 공격
        BossDeath,          // 보스 사망
        BtnClick,           // 버튼 클릭
        EnemyDeath,         // 일반 적 사망
        GameStart,          // 게임 시작
        NormalElemental,    // 일반 공격
        PlayerCast,         // 플레이어 마법 시전
        PlayerDeath,        // 플레이어 사망
    }

    #endregion
}