using UnityEngine;
using static Define;
using System.Collections.Generic;

public class Water : Skill
{
    int _baseDamage = 1;
    int _strongDamage = 2;

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
            WideAttack(enemies, ElementalEffect.SuperWater, _strongDamage, false);
            playerSkill.DestroyGrease();
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
        }
        // 단일
        else if (playerSkill.IsSingleField)
        {
            enemies = GameManager.Instance.GetFrontEnemie();
            WideAttack(enemies, ElementalEffect.SingleWater, _strongDamage, false);
            playerSkill.DestroyGrease();
            Debug.Log($"1명 적에게 {_strongDamage} 데미지!");
        }
        // 범위
        else if (playerSkill.IsMultiField)
        {
            WideAttack(enemies, ElementalEffect.MultiWater, _baseDamage, false);
            playerSkill.DestroyGrease();
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
            
        }
        //기본
        else
        {
            enemies = GameManager.Instance.GetFrontEnemies();
            WideAttack(enemies, ElementalEffect.Water, _baseDamage);
        }

        
    }
}