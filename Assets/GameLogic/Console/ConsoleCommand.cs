using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ConsoleCommand
{
    public string title;
    public string[] args;
    public void Execute()
    {
        switch (title)
        {
            case GlobalLibrary.G_CONSOLE_TITLE_EXIT:
                InGameConsole.instance.SetConsoleActive(false);
                break;
            case GlobalLibrary.G_CONSOLE_TITLE_RESPAWN:
                if(args!=null && args.Length == 1)
                {
                        int spawnPoint = int.Parse(args[0]);
                        PlayerModel.Instance.PlayerMain.TeleportToSpawnPoint(spawnPoint);
                }
                else
                {
                    InGameConsole.instance.OnCommandInvalid();
                }
                break;
            case GlobalLibrary.G_CONSOLE_TITLE_LANGUAGE:
                if (args != null && args.Length == 1)
                {
                    int lang = int.Parse(args[0]);
                    GlobalManager.instance.SetLanguage((SKCell.LanguageSupport)lang);
                }
                else
                {
                    InGameConsole.instance.OnCommandInvalid();
                }
                break;
            case GlobalLibrary.G_CONSOLE_TITLE_CHECKPOINT:
                if (args != null && args.Length == 1)
                {
                        int checkPoint = int.Parse(args[0]);
                        PlayerModel.Instance.PlayerMain.TeleportToCheckPoint(checkPoint);
                }
                else
                {
                    InGameConsole.instance.OnCommandInvalid();
                }
                break;
            case GlobalLibrary.G_CONSOLE_TITLE_LOAD_SCENE:
                if (args != null && args.Length == 1)
                {
                    int sceneIndex = int.Parse(args[0]);
                    SceneInfo info = new SceneInfo();
                    info.index = GlobalLibrary.G_SCENE_INDEX[sceneIndex];
                    SceneController.instance.LoadSceneAsset(info);
                }
                else
                {
                    InGameConsole.instance.OnCommandInvalid();
                }
                break;
            case GlobalLibrary.G_CONSOLE_TITLE_SET_VALUE:
                if (args != null && args.Length == 3)
                {
                    if (args[0] == GlobalLibrary.G_CONSOLE_CHR_PLAYER)
                    {
                        float value = float.Parse(args[2]);
                        switch (args[1])
                        {
                            case GlobalLibrary.G_CONSOLE_ARG_PLAYER_SPEED:
                                PlayerModel.Instance.PlayerMovement.moveSpeed = value;
                                break;
                            case GlobalLibrary.G_CONSOLE_ARG_PLAYER_JUMPLIMIT:
                                PlayerModel.Instance.PlayerMovement.jumpLimit = (int)value;
                                break;
                            case GlobalLibrary.G_CONSOLE_ARG_PLAYER_JUMPFORCE:
                                PlayerModel.Instance.PlayerMovement.jumpForce = value;
                                break;
                            case GlobalLibrary.G_CONSOLE_ARG_PLAYER_GRAVITY:
                                PlayerModel.Instance.PlayerMovement.SetGravity(value);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    InGameConsole.instance.OnCommandInvalid();
                }
                break;
            default:
                InGameConsole.instance.OnCommandInvalid();
                break;
        }
    }
}

