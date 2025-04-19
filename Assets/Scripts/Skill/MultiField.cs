using UnityEngine;
using static Define;

public class MultiField : Skill
{
    
    void Start()
    {
        Init();
        
        
    }

    public override void Execute()
    {
        LogManager.Instance.LogBuff();
        FindAnyObjectByType<UI_ElementIcon>().SetBackColorMulti();
        
        PlayerController.Instance.PlayerSkill.ExcuteEffect(ElementalEffect.MultiField, Vector3.down * 5);
        PlayerController.Instance.PlayerSkill.IsMultiField = true;
    }

    void OnDestroy()
    {
        FindAnyObjectByType<UI_ElementIcon>().SetBackColorNone();
        PlayerController.Instance.PlayerSkill.IsMultiField = false;
    }
}