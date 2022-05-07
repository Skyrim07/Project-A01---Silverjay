using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SKCell;
public sealed class ConversationManager : MonoSingleton<ConversationManager>
{
    const string ANIM_APPEAR_BOOL = "Appear";
    const string ANIM_POP_TRIGGER = "Pop";
    const int MAX_OPTION_COUNT = 6;
    const float TEXT_ROLL_TIME = 0.05f;
    const float TEXT_FAST_FORWARD_RATE = 0.5f;

    public Conversation conv; //Current conversation
    public Sentence sentence; //Current sentence
    public Vector3 panelPosition;
    public float panelScale;
    public ConvStarter convStarter;

    private int sentenceID; //Current sentence id
    private bool isTextRolling;
    private float textRollTime;
    private Coroutine textRollCoroutine;
    private Vector3 oPanelScale; //Original scale of in scene panel

    //References 
    [SerializeField] Transform panel_InScene;
    [SerializeField] Animator anim_InScene, anim_InUI;
    [SerializeField] TMP_Text titleText_InScene, titleText_InUI;
    [SerializeField] TMP_Text contentText_InScene, contentText_InUI;
    [SerializeField] Image avatarImage_InScene, avatarImage_InUI;
    [SerializeField] ConvOption[] convOptions_InScene, convOptions_InUI;

    #region Lifecycle
    private void Start()
    {
        oPanelScale = panel_InScene.localScale;

        RegisterAudio();
    }

    private void Update()
    {
        panel_InScene.position = panelPosition;
        if (conv != null)
        {
            if (Input.GetKeyDown(GlobalLibrary.INPUT_INTERACT_KEYCODE)|| Input.GetKeyDown(GlobalLibrary.INPUT_PROCEED_KEYCODE))
            {
                NextSentence();
            }
        }
    }
    #endregion
    private static void RegisterAudio()
    {
        EventDispatcher.AddListener(EventDispatcher.UI, EventRef.UI_CONV_ON_NEXT_SENTENCE, new SJEvent(() =>
        {
            SKAudioManager.instance.PlaySound(GlobalLibrary.AUDIO_UI_CLICK, null, false, 1, UnityEngine.Random.Range(0.7f, 1.3f), 0);
        }));
        EventDispatcher.AddListener(EventDispatcher.UI, EventRef.UI_CONV_ON_SELECT_OPTION, new SJEvent(() =>
        {
            SKAudioManager.instance.PlaySound(GlobalLibrary.AUDIO_UI_CLICK, null, false, 1, UnityEngine.Random.Range(0.7f, 1.3f), 0);
        }));
    }
    public void StartConversation(int convID, int sentenceID = 0, ConvStarter convStarter = null)
    {
        StartConversation(GlobalLibrary.G_UI_CONVERSATIONS[convID], sentenceID, convStarter);
    }
    public void StartConversation(Conversation conv, int sentenceID = 0, ConvStarter convStarter = null)
    {
        this.conv = conv;
        this.sentenceID = sentenceID;
        this.sentence = conv.sentences[sentenceID];
        this.convStarter = convStarter;
        if (convStarter)
        {
            //contentText_InScene.alignment = convStarter.textAlignment;
            convStarter.active = false;
            panel_InScene.localScale = convStarter.panelScale * oPanelScale;
        }
        else
        {
            panel_InScene.localScale = oPanelScale;
        }

        textRollTime = TEXT_ROLL_TIME;

        if (conv.type == ConversationType.InScene)
        {
            //In scene
            SetState_ConvPanelInScene(true);
        }
        else if (conv.type == ConversationType.InUI) //TODO
        {
            //In UI
            SetState_ConvPanelInUI(true);
        }

        ProcessSentence(conv, sentenceID, false);
    }

    public void ProcessSentence(Conversation conv, int sentID, bool popAnim = true)
    {
        if (conv == null)
            return;
        
        if (textRollCoroutine != null)
            StopCoroutine(textRollCoroutine);

        EventDispatcher.Dispatch(EventDispatcher.UI, EventRef.UI_CONV_ON_NEXT_SENTENCE);

        if (sentenceID >= conv.sentences.Count)
        {
            EndConversation();
            return;
        }

        if(convStarter!=null)
        {
            if (convStarter.anim)
            {
                convStarter.anim.ResetTrigger("Pop");
                convStarter.anim.SetTrigger("Pop");
            }
            if(convStarter.voices.Length > 0)
            {
                SKAudioManager.instance.PlaySound(convStarter.voices[UnityEngine.Random.Range(0, convStarter.voices.Length)].name, null, false, 1, UnityEngine.Random.Range(0.8f,1.2f),0f);
            }
        }

        Sentence s = conv.sentences[(sentID)];
        sentence = s;
        sentenceID = sentID;
        if (conv.type == ConversationType.InScene)
            ProcessSentence_InScene(s, popAnim);
        else
            ProcessSentence_InUI(s);
    }
    public void ProcessSentence_InScene(Sentence sent, bool popAnim = true)
    {
        if (popAnim)
        {
            anim_InScene.ResetTrigger(ANIM_POP_TRIGGER);
            anim_InScene.SetTrigger(ANIM_POP_TRIGGER);
        }

        titleText_InScene.text = sent.speakerLocalID>0?GlobalManager.instance.GetLocalizationText(sent.speakerLocalID):"";
        textRollCoroutine = StartCoroutine(TextRollCR(contentText_InScene, sent.GetContent()));
        //TODO: Avatar

        for (int i = 0; i < MAX_OPTION_COUNT; i++)
        {
            if (convOptions_InScene.Length <= i)
                break;
            convOptions_InScene[i].SetState(false);
        }

        if (sent.options!=null && sent.options.Count > 0) //Have some options
        {
            for (int i = 0; i < sent.options.Count; i++)
            {
                if (sent.options[i].locked)
                    continue;

                convOptions_InScene[i].SetState(true);
                convOptions_InScene[i].Initialize(sent.options[i]);
                convOptions_InScene[i].UpdateVisual();
            }
        }
    }
    IEnumerator TextRollCR(TMP_Text textComponent, string targetText)
    {
        isTextRolling = true;
        string curText;
        for (int i = 1; i <= targetText.Length; i++)
        {
            curText = targetText.Substring(0, i);
            textComponent.text = curText;
            yield return new WaitForSeconds(textRollTime);
        }

        isTextRolling = false;
    }
    IEnumerator TextRollCR(Text textComponent, string targetText)
    {
        isTextRolling = true;
        string curText;
        for (int i = 1; i <= targetText.Length; i++)
        {
            curText = targetText.Substring(0, i);
            textComponent.text = curText;
            yield return new WaitForSeconds(textRollTime);
        }

        isTextRolling = false;
    }

    public void EndTextRolling()
    {
        if(textRollCoroutine != null)
            StopCoroutine(textRollCoroutine);

        if (conv == null)
            return;

        TMP_Text text = conv.type == ConversationType.InScene ? contentText_InScene : contentText_InUI;
        text.text = sentence.GetContent();
        isTextRolling = false;
    }
    public void ProcessSentence_InUI(Sentence sent)
    {
        
    }

    public void SetState_ConvPanelInScene(bool isOn)
    {
        if(anim_InScene)
        anim_InScene.SetBool(ANIM_APPEAR_BOOL, isOn);
    }
    public void SetState_ConvPanelInUI(bool isOn)
    {
        if(anim_InUI)
        anim_InUI.SetBool(ANIM_APPEAR_BOOL, isOn);
    }
    public void EndConversation()
    {
        conv = null;
        sentence = null;

        SetState_ConvPanelInScene(false);
        SetState_ConvPanelInUI(false);
        if (textRollCoroutine != null)
            StopCoroutine(textRollCoroutine);
        if (convStarter != null && convStarter.isPlayerIn)
        {
            convStarter.OnConversationEnd();
            CommonUtils.InvokeAction(0.2f, () =>
            {
                convStarter.Activate();
            });
        }
    }

    public void NextSentence()
    {
        if (!isTextRolling)
        {
            if (sentence!=null && sentence.requireResponse)
                return;

            if (sentence != null && sentence.action_OnEnd != 0)
                GlobalLibrary.G_CONV_OPTION_ACTIONS[sentence.action_OnEnd].Invoke(sentence.arg0, sentence.arg1);
            if (sentence != null && sentence.action_OnEnd2 != 0)
                GlobalLibrary.G_CONV_OPTION_ACTIONS[sentence.action_OnEnd2].Invoke(sentence.arg2, sentence.arg3);
            ProcessSentence(conv, ++sentenceID);
        }
        else
        {
            EndTextRolling();
        }
    } 

    public void OnSelectResponse(int id)
    {
        if (sentence == null)
            return;

        SentenceOption option = sentence.options[id];
        Action<int, int> action = GlobalLibrary.G_CONV_OPTION_ACTIONS[option.action_OnSelect];
        action.Invoke(option.arg0, option.arg1);

        EventDispatcher.Dispatch(EventDispatcher.UI, EventRef.UI_CONV_ON_SELECT_OPTION);
    }

    #region Actions
    /// <summary>
    /// Jump to sentence [arg0]
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    public void Action_100_JumpToSentence(int arg0, int arg1)
    {
        ProcessSentence(conv, arg0);
    }
    public void Action_101_EndConversation(int arg0, int arg1)
    {
        EndConversation();
    }
    public void Action_102_UnlockOption(int arg0, int arg1)
    {
        conv.sentences[arg0].options[arg1].locked = false;
    }
    public void Action_103_LockOption(int arg0, int arg1)
    {
        conv.sentences[arg0].options[arg1].locked = true;
    }
    #endregion

}

public class Conversation
{
    public int id;
    public ConversationType type;
    public List<Sentence> sentences;
}

public class Sentence
{
    public int speakerLocalID;
    public string content; // normal text, if localID<0, apply
    public int localID = -1; //localization text, if >= 0, apply
    public bool requireResponse;
    public int action_OnEnd, action_OnEnd2;
    public int arg0, arg1; //args for action on end 1
    public int arg2, arg3; //args for action on end 2

    public List<SentenceOption> options;

    public string GetContent()
    {
        if (localID < 0)
            return content;
        else
        {
           return GlobalManager.instance.GetLocalizationText(localID);
        }
    }
}

public class SentenceOption
{
    public string content;
    public int localID = -1; //localization
    public bool locked; //will not be visible if locked
    public int action_OnSelect; //>0 -> have some actions
    public int arg0, arg1;
}

public enum ConversationType
{
    InScene,
    InUI
}
