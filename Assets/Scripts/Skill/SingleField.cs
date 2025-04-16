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
        PlayerSkill playerSkill = PlayerController.Instance.PlayerSkill;
        playerSkill.ExcuteEffect(ElementalEffect.SingleField, Vector3.down * 5);
        playerSkill.IsSingleField = true;
    }

    void OnDestroy()
    {
        playerSkill.IsSingleField = false;
    }
}