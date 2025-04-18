using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class InputManager : MonoBehaviour
{
    [Header("싱글톤")]
    static InputManager _instance;
    public static InputManager Instance => _instance;

    [Header("Input System")]
    InputSystemActions _inputSystemActions;

    [Header("액션")]
    public Action<Elemental> selectElementalAction;
    public Action<int> makePressBtnFX;


    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    public void Init()
    {
        _inputSystemActions = new InputSystemActions();
        _inputSystemActions.Enable();                        // InputSystemActions 활성화
        _inputSystemActions.Player.Enable();                 // Player Action Map 활성화

        _inputSystemActions.Player.Cast.performed += OnCast;
        _inputSystemActions.Player.Cast.canceled += OnCast;
    }

    void OnCast(InputAction.CallbackContext context)
    {
        if (!RhythmManager.Instance.IsJudging)
        {
            Debug.Log("[입력 무시] 현재 큰 박자 아님");
            return;
        }

        if (context.performed)
        {
            Vector2 castValue = context.ReadValue<Vector2>();

            // 키보드 입력 감지
            if (castValue.y == 1) // W : 불
            {

                selectElementalAction.Invoke(Elemental.Fire);
                makePressBtnFX?.Invoke(0);
                Debug.Log("w 입력");
            }
            else if (castValue.x == 1) // E : 물
            {
                selectElementalAction.Invoke(Elemental.Water);
                makePressBtnFX?.Invoke(1);
                Debug.Log("e 입력");
            }
            else if (castValue.x == -1) // Q : 풀
            {
                selectElementalAction.Invoke(Elemental.Grass);
                makePressBtnFX?.Invoke(3);
                Debug.Log("q 입력");
            }
        }
        //else if (context.canceled)
        //{
        //    // Cast 액션이 취소되었을 때의 처리
        //    Debug.Log("Cast 액션 취소됨");
        //}
    }

    public void Clear()
    {
        selectElementalAction = null;

        _inputSystemActions.Player.Disable();
        _inputSystemActions.UI.Disable();

        _inputSystemActions.Player.Cast.performed -= OnCast;
        _inputSystemActions.Player.Cast.canceled -= OnCast;

        _inputSystemActions.Disable();
        _inputSystemActions = null;
    }

    void OnDestroy()
    {
        Clear();
    }
}