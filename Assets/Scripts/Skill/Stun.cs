

using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Stun : Skill
{
    void Start()
    {
        Init();
    }
    public override void Execute()
    {
        LogManager.Instance.LogBuff(2);
        if (GameManager.Instance.CurrentEnemyList == null || GameManager.Instance.CurrentEnemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        foreach (Enemy enemy in GameManager.Instance.CurrentEnemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.ApplyState(EnemyState.Stun);
                PlayerController.Instance.PlayerSkill.ExcuteEffect(ElementalEffect.Stun, enemy.transform.position);
            }
        }
        Debug.Log($"모든 적 스턴!");
    }
}