using UnityEngine;
using static Define;

public class SingleField : Skill
{
    void Start()
    {
        Init();
       
    }

    public override void Execute()
    {
        LogManager.Instance.LogBuff(1);
        FindAnyObjectByType<UI_ElementIcon>().SetBackColorSingle();
        PlayerSkill playerSkill = PlayerController.Instance.PlayerSkill;
        Friend.Instance.SetSingleField(true);
        PlayerController.Instance.SetSingleField(true);
        playerSkill.IsSingleField = true;
    }
}