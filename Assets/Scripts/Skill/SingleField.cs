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
        Friend.Instance.SetSingleField(true);
        PlayerController.Instance.SetSingleField(true);
        playerSkill.IsSingleField = true;
    }
}