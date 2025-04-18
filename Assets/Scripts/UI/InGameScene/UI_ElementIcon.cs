using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_ElementIcon : MonoBehaviour
{
    private Image _backgroundImage; 
    
    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
    }

    private void Start()
    {
        
    }


    public void SetColorBack(int n)
    {
        if (PlayerController.Instance.PlayerSkill.IsMultiField)
        {
            SetBackColorMulti();
        }
        else if (PlayerController.Instance.PlayerSkill.IsSingleField)
        {
            SetBackColorSingle();
        }
        else
        {
            SetBackColorNone();
        }
    }
    
    //singleField, multiField 활성화를 보여주는 ui
    public void SetBackColorSingle()
    {
        _backgroundImage.color = new Color(1, 1, 0, 1f);   
    }
    public void SetBackColorMulti()
    {
        _backgroundImage.color = new Color(1, 0, 1, 1f);
    }
    
    public void SetBackColorNone()
    {
        _backgroundImage.color = new Color(1, 1, 1, 0.5f);
    }
}
