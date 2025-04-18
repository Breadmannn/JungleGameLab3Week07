using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_ElementIcon : MonoBehaviour
{
    private Image _backgroundImage;
    public Sprite[] elementalImages;
    public Sprite[] multiFieldImages;
    public Sprite[] singleFieldImages;
    public Image[] elementalIcons;
    public Image[] elementalBackIcons;
    [Header("Color")]
    Color _colorCertain;
    Color _colorRed;
    Color _colorGreen;
    Color _colorBlue;
    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#353535", out _colorCertain);
        ColorUtility.TryParseHtmlString("#FFC2C2", out _colorRed);
        ColorUtility.TryParseHtmlString("#C2FFC2", out _colorGreen);
        ColorUtility.TryParseHtmlString("#C2C2FF", out _colorBlue);
    }

    private void Start()
    {
        SetBackColorNone();
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
        elementalIcons[0].sprite = elementalImages[0];
        elementalBackIcons[0].color = _colorCertain;
        elementalIcons[1].sprite = elementalImages[1];
        elementalBackIcons[1].color = _colorCertain;
        elementalIcons[2].sprite = elementalImages[2];
        elementalBackIcons[2].color = _colorCertain;
    }

    public void SetUpgradeIcon()
    {
        if (PlayerController.Instance.PlayerSkill.IsMultiField)
        {
            if (Friend.Instance.RealElemental == Define.Elemental.Water)
            {
                elementalIcons[0].sprite = multiFieldImages[0];
                elementalBackIcons[0].color = _colorBlue;
                elementalIcons[1].sprite = elementalImages[1];
                elementalBackIcons[1].color = _colorCertain;
                elementalIcons[2].sprite = elementalImages[2];
                elementalBackIcons[2].color = _colorCertain;
            }
            else if (Friend.Instance.RealElemental == Define.Elemental.Fire)
            {
                elementalIcons[1].sprite = multiFieldImages[1];
                elementalBackIcons[1].color = _colorRed;
                elementalIcons[0].sprite = elementalImages[0];
                elementalBackIcons[0].color = _colorCertain;
                elementalIcons[2].sprite = elementalImages[2];
                elementalBackIcons[2].color = _colorCertain;
            }
            else if (Friend.Instance.RealElemental == Define.Elemental.Grass)
            {
                elementalIcons[2].sprite = multiFieldImages[2];
                elementalBackIcons[2].color = _colorGreen;
                elementalIcons[0].sprite = elementalImages[0];
                elementalBackIcons[0].color = _colorCertain;
                elementalIcons[1].sprite = elementalImages[1];
                elementalBackIcons[1].color = _colorCertain;
            }
        }
        else if (PlayerController.Instance.PlayerSkill.IsSingleField)
        {
            if (Friend.Instance.RealElemental == Define.Elemental.Water)
            {
                elementalIcons[0].sprite = singleFieldImages[0];
                elementalBackIcons[0].color = _colorBlue;
                elementalIcons[1].sprite = elementalImages[1];
                elementalBackIcons[1].color = _colorCertain;
                elementalIcons[2].sprite = elementalImages[2];
                elementalBackIcons[2].color = _colorCertain;
            }
            else if (Friend.Instance.RealElemental == Define.Elemental.Fire)
            {
                elementalIcons[1].sprite = singleFieldImages[1];
                elementalBackIcons[1].color = _colorRed;
                elementalIcons[0].sprite = elementalImages[0];
                elementalBackIcons[0].color = _colorCertain;
                elementalIcons[2].sprite = elementalImages[2];
                elementalBackIcons[2].color = _colorCertain;
            }
            else if (Friend.Instance.RealElemental == Define.Elemental.Grass)
            {
                elementalIcons[2].sprite = singleFieldImages[2];
                elementalBackIcons[2].color = _colorGreen;
                elementalIcons[0].sprite = elementalImages[0];
                elementalBackIcons[0].color = _colorCertain;
                elementalIcons[1].sprite = elementalImages[1];
                elementalBackIcons[1].color = _colorCertain;
            }
        }
    }
}
