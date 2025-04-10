using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public GameManager gameManager; // �ν����Ϳ��� ����


   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Ű���� �Է� ����
        if (Input.GetKeyDown(KeyCode.UpArrow)) // Flame (ȭ��)
        {
            gameManager.SelectTag("ȭ��");
            Debug.Log("Flame Ű �Է�: ȭ��");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) // Lightning (����)
        {
            gameManager.SelectTag("����");
            Debug.Log("Lightning Ű �Է�: ����");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) // Oil (�⸧)
        {
            gameManager.SelectTag("�⸧");
            Debug.Log("Oil Ű �Է�: �⸧");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // Frost (�ñ�)
        {
            gameManager.SelectTag("�ñ�");
            Debug.Log("Frost Ű �Է�: �ñ�");
        }
    }
}
