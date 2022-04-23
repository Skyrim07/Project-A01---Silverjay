using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SKCell;

public sealed class InGameConsole : MonoSingleton<InGameConsole>
{
    public bool isActive = false;

    //References
    [SerializeField] private InputField input;
    [SerializeField] GameObject consoleGO;

    private void Start()
    {
        consoleGO.SetActive(isActive);

        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CM_ON_SCENE_EXIT, new SJEvent(() =>
        {
            SetConsoleActive(false);
        }));
    }
    private void Update()
    {
        if (Input.GetKeyDown(GlobalLibrary.G_CONSOLE_ACTIVATE_KEY))
        {
            SetConsoleActive(!isActive);
        }
        if (isActive)
        {
            if (Input.GetKeyDown(GlobalLibrary.G_CONSOLE_INPUT_KEY))
            {
                OnPressInputKey();
            }
        }
    }

    public void OnEndEditConsole(string text)
    {
        string[] split = text.Split(' ');
        if (split.Length == 0)
            return;

        ConsoleCommand command = new ConsoleCommand
        {
            title = split[0]
        };

        if (split.Length > 1)
        {
            command.args = new string[split.Length - 1];
            for (int i = 1; i < split.Length; i++)
            {
                command.args[i - 1] = split[i];
            }
        }

        command.Execute();
    }

    public void OnCommandInvalid()
    {
        input.text = GlobalLibrary.G_CONSOLE_ERROR_MESSAGE;
        input.ActivateInputField();
    }

    public void OnPressInputKey()
    {
        input.ActivateInputField();
    }
    public void SetConsoleActive(bool isActive)
    {
        if (isActive && !this.isActive)
            EventDispatcher.Dispatch(EventDispatcher.Common, EventRef.CONSOLE_ON_OPEN);
        else if (!isActive && this.isActive)
            EventDispatcher.Dispatch(EventDispatcher.Common, EventRef.CONSOLE_ON_CLOSE);

        this.isActive = isActive;
        consoleGO.SetActive(isActive);  
        if(isActive)
            input.ActivateInputField();
    }
}
