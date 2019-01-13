using System;
using System.Collections;
using UnityEngine;

public enum ControllerType
{
    None,
    PlayStation,
    XBox
}

public class GamepadDetect : MonoBehaviour
{
    private const int PlayStationControllerNameLength = 19;
    private const int XBoxOneControllerNameLength = 33;
    private Array mkeyCodeArray;
    public ControllerType CurrentControllerType { get; set; }

    private void Awake()
    {
        mkeyCodeArray = Enum.GetValues(typeof(KeyCode));
        StartCoroutine(CheckControllerType());
    }

    private void Update()
    {
        if (CurrentControllerType == ControllerType.None)
        {
            Debug.Log("没有检测到游戏手柄");
            return;
        }

        CheckKeyCodeInput();
        CheckAxisInput();
    }

    private void CheckKeyCodeInput()
    {
        foreach (var keyCode in mkeyCodeArray)
        {
            if (Input.GetKey((KeyCode) keyCode))
            {
                Debug.Log($"按下了{keyCode}");
            }
        }
    }

    private void CheckAxisInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) > 0f || Mathf.Abs(vertical) > 0f)
        {
            print($"Horizontal: {horizontal} Vertical: {vertical}");
        }
        
        float rightJoystickX = Input.GetAxis("RightJoystickX");
        float rightJoystickY = Input.GetAxis("RightJoystickY");
        if (Mathf.Abs(rightJoystickX) > 0f || Mathf.Abs(rightJoystickY) > 0f)
        {
            print($"RightJoystickX: {rightJoystickX} RightJoystickY: {rightJoystickY}");
        }
    }

    private IEnumerator CheckControllerType()
    {
        while (true)
        {
            CurrentControllerType = ControllerType.None;
            var names = Input.GetJoystickNames();
            if (names.Length > 1)
            {
                print($"手柄数量: {names.Length} 暂不支持多个手柄");
            }

            foreach (var joystickName in names)
            {
                print($"手柄名字: {joystickName}");
                if (joystickName.Length == PlayStationControllerNameLength)
                {
                    CurrentControllerType = ControllerType.PlayStation;
                }
                else if (joystickName.Length == XBoxOneControllerNameLength)
                {
                    CurrentControllerType = ControllerType.XBox;
                }
            }

            if (CurrentControllerType == ControllerType.PlayStation)
            {
                Debug.Log("检测到PlayStation手柄");
            }
            else if (CurrentControllerType == ControllerType.XBox)
            {
                Debug.Log("检测到XBox手柄");
            }

            yield return new WaitForSeconds(1f);
        }
    }
}