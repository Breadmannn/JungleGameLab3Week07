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
        PlayerController.Instance.PlayerSkill.ExcuteEffect(ElementalEffect.MultiField, Vector3.down * 5);
        PlayerController.Instance.PlayerSkill.IsMultiField = true;
    }

    void OnDestroy()
    {
        PlayerController.Instance.PlayerSkill.IsMultiField = false;
    }
}