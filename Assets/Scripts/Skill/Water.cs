using UnityEngine;
using static Define;
using System.Collections.Generic;

public class Water : Skill
{
    int _fieldDamage = 1;
    int _baseDamage = 2;
    int _strongDamage = 3;

    void Start()
    {
        Init();
    }

    public override void Execute()
    {
        if (GameManager.Instance.CurrentEnemyList == null || GameManager.Instance.CurrentEnemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        PlayerSkill playerSkill = PlayerController.Instance.PlayerSkill;
        List<Enemy> enemies = GameManager.Instance.CurrentEnemyList;
        
        // 단일, 범위 둘 다
        if (playerSkill.IsSingleField && playerSkill.IsMultiField) 
        {
            WideAttack(enemies, ElementalEffect.MultiWater, _fieldDamage, false);
            enemies = GameManager.Instance.GetFrontEnemy();
            WideAttack(enemies, ElementalEffect.SingleWater, _strongDamage, false);
            playerSkill.DestroyMultiField();
            playerSkill.DestroySingleField();
            Debug.Log($"모든 적에게 {_fieldDamage} 데미지!");
            Debug.Log($"1명 적에게 {_strongDamage} 데미지!");
        }
        // 단일
        else if (playerSkill.IsSingleField)
        {
            enemies = GameManager.Instance.GetFrontEnemy();
            WideAttack(enemies, ElementalEffect.SingleWater, _strongDamage, false);
            playerSkill.DestroySingleField();
            Debug.Log($"1명 적에게 {_strongDamage} 데미지!");
        }
        // 범위
        else if (playerSkill.IsMultiField)
        {
            WideAttack(enemies, ElementalEffect.MultiWater, _fieldDamage, false);
            playerSkill.DestroyMultiField();
            Debug.Log($"모든 적에게 {_fieldDamage} 데미지!");
            
        }
        //기본
        else
        {
            enemies = GameManager.Instance.GetFrontEnemies();
            WideAttack(enemies, ElementalEffect.Water, _baseDamage);
        }

        
    }
}