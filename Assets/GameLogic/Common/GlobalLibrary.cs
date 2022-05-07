using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores global static variables
/// </summary>
public static class GlobalLibrary
{
    #region Common

    public static readonly string G_PLAYER_TAG = "Player";
    public static readonly string G_GROUND_TAG = "Ground";
    public static readonly string G_CAMERA_CONFINER_TAG = "CamConfiner";
    public static readonly float G_CAMERA_DEADZONE_WIDTH = 0.2f;

    public static readonly Dictionary<int, Action<int, int>> G_CONV_OPTION_ACTIONS = new Dictionary<int, Action<int, int>>()
    {
        {0,(x,y)=>{ } },

        //JUMP TO SENTENCE
        {ConvActionRef.JUMP_TO_SENTENCE,
            (x,y) =>
            {
                ConversationManager.instance.Action_100_JumpToSentence(x,y);
            } 
        },
        //END CONVERSATION
        {ConvActionRef.END_CONVERSATION,
            (x,y) =>
            {
                ConversationManager.instance.Action_101_EndConversation(x,y);
            }
        },
         //UNLOCK OPTION
        {ConvActionRef.UNLOCK_OPTION,
            (x,y) =>
            {
                ConversationManager.instance.Action_102_UnlockOption(x,y);
            }
        },
            //LOCK OPTION
        {ConvActionRef.LOCK_OPTION,
            (x,y) =>
            {
                ConversationManager.instance.Action_103_LockOption(x,y);
            }
        },
    };
    #endregion

    #region Player
    public static readonly float G_PLAYER_DOWN_PLATFORM_EFFECTOR_DISABLE_TIME = 0.8f;
    public static readonly float G_PLAYER_FAST_RUN_SPD_BONUS = 1.5f;
    public static readonly float G_PLAYER_FAST_RUN_AUDIO_PITCH_BONUS = 1.3f;
    public static readonly float G_PLAYER_LOOK_DOWN_CAM_Y_DELTA = 0.25f;
    public static readonly float G_PLAYER_GRAVITY_SCALE = 1f;
    #endregion

    #region SceneControl
    public static readonly string G_SCENE_TAG_SPAWNPOINT = "SpawnPoint";
    public static readonly string G_SCENE_TAG_CHECKPOINT = "CheckPoint";

    public static readonly Dictionary<SceneTitle, SceneSpecifics> G_SCENE_SPECIFICS = new Dictionary<SceneTitle, SceneSpecifics>()
    {
        {SceneTitle.MainMenu, new SceneSpecifics()
        {
            sceneName = "Main Menu",
            cameraSize = 12f,
            postprocessingProfile = 2,
            disableUISceneLabel = true,
            disablePlayerGravity = false,
            disablePlayerDash=true,
            disableCameraDeadzone=true,
            enableSmoothOpening=true,
            weather = new SceneWeather()
            {
                type = WeatherType.Rain,
                probability = 0.5f,
                interval = 10f
            },
            bgms = new string[]
            {
                "loading",
            }
        }},
        {SceneTitle.FireflyFrontier, new SceneSpecifics()
        {
            sceneName = "Firefly Frontier",
            sceneNameLocalID = 1002,
            cameraSize = 17.5f,
            postprocessingProfile = 0,
            weather = new SceneWeather()
            {
                type = WeatherType.Rain,
                probability = 0.5f,
                interval = 15.0f
            },
            bgms = new string[]
            {
                "Firefly_Frontier_0",
                "Firefly_Frontier_1"
            }
        }},
        { SceneTitle.FireflyTavern, new SceneSpecifics()
        {
            sceneName = "Firefly Tavern",
            sceneNameLocalID = 1003,
            cameraSize = 10.0f,
            postprocessingProfile = 1,
            weather = new SceneWeather()
            {
                type = WeatherType.None,
                probability = 0.0f,
                interval = 0.0f
            },
            bgms = new string[]
            {
                "Tavern_0",
                "Tavern_1"
            }
        }},
         {SceneTitle.TestScene, new SceneSpecifics()
        {
            sceneName = "Test Scene",
            sceneNameLocalID = 1004,
            cameraSize = 17.5f,
            postprocessingProfile = 0,
            weather = new SceneWeather()
            {
                type = WeatherType.Rain,
                probability = 0.5f,
                interval = 15.0f
            },
            bgms = new string[]
            {
                "Firefly_Frontier_0",
                "Firefly_Frontier_1"
            }
        }},
    };

    public static readonly string G_SCENE_LOADING_ASSET_NAME = "Loading";
    public static readonly string G_SCENE_PREPARE_ASSET_NAME = "Prepare";
    public static readonly Dictionary<SceneTitle, string> G_SCENE_ASSET_NAME = new Dictionary<SceneTitle, string>()
    {
        { SceneTitle.FireflyFrontier, "Firefly_Frontier_Main" },
        { SceneTitle.FireflyTavern, "Firefly_Tavern" },
        { SceneTitle.MainMenu, "Main_Menu" },
        { SceneTitle.TestScene, "Test_Scene" },
    };

    public static readonly Dictionary<int, SceneTitle> G_SCENE_INDEX = new Dictionary<int, SceneTitle>()
    {
        { 100, SceneTitle.MainMenu },
        { 102, SceneTitle.FireflyFrontier },
        { 103, SceneTitle.FireflyTavern },

        { 900, SceneTitle.TestScene },
    };
    #endregion

    #region UI
    public static readonly float G_UI_SCENE_LABEL_DURATION = 4.0f;

    //Inventory
    public static readonly int G_INVENTORY_COUNT_TEXT_LOCAL_ID = 9;

    //Conversation
    public static readonly Dictionary<int, Conversation> G_UI_CONVERSATIONS = new Dictionary<int, Conversation>()
    {
        {100, new Conversation()
        {
            id = 100,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 1,
                    content = "Conv 100 Sent 0 Content",
                },
                new Sentence()
                {
                    speakerLocalID = 2,
                    content = "Conv 100 Sent 1 Content",
                    options = new List<SentenceOption>()
                    {
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "Go Sent 0",
                            arg0 = 0
                        },
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "Go Sent 2",
                            arg0 = 2
                        }
                    },

                },
                new Sentence()
                {
                    speakerLocalID = 3,
                    content = "Conv 100 Sent 2 Content",
                },
            },
        } },

        {101, new Conversation()
        {
            id = 101,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3009,
                    localID = 7001,
                    content = "嗯？请留步",
                },
                new Sentence()
                {
                    speakerLocalID = 3009,
                    localID = 7002,
                    content = "虽然同为异客，但边陲中生存的大多数是流离失所并丧失信仰的生命，看起来你的归宿并非此处。",
                },
                new Sentence()
                {
                    speakerLocalID = 3009,
                    localID = 7003,
                    content = "看到我胸前的领带了吗？\r\n记住这个由蛇和羽翼联结的图案，这是禁忌的古神羽蛇的象征。",
                },
                new Sentence()
                {
                    speakerLocalID = 3009,
                    localID = 7004,
                    content = "如果遇到无法克服的困难的话，就向古神祈祷吧，信仰会抚平欲望和恶念，让内心变得强大而纯粹。",
                },
            },
        } },

        {102, new Conversation()
        {
            id = 102,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3010,
                    localID = 7005,
                    content = "呼姆，太美丽了......",
                },
                new Sentence()
                {
                    speakerLocalID = 3010,
                    localID = 7006,
                    content = "哦，初次见面，看来你也被这块巨石所吸引了啊，不瞒你说，我正在尝试解析巨石上的符文，但成效甚微。",
                },
                new Sentence()
                {
                    speakerLocalID = 3010,
                    localID = 7007,
                    content = "我听说，许多年前曾有一位德高望重的学者来到了此处，他在边陲建造了书库，使得这里一度成为百家争鸣的学术交流圣地。",
                },
                new Sentence()
                {
                    speakerLocalID = 3010,
                    localID = 7008,
                    content = "但是，书库和学者都被历史遗忘，只留下了这一块格格不入的巨石，那些先进的理论和学术都随之消逝，\r\n真令人惋惜，不是吗？",
                },
            },
        } },

        {103, new Conversation()
        {
            id = 103,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7009,
                    content = "这么小的家伙怎么也来酒馆厮混？\r\n唉，那群原住民心可真大......",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7010,
                    content = "给这家伙来一杯“气泡水”，我请客！",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7011,
                    content = "什么？你说你要去冒险？\r\n哈哈哈哈哈小屁孩真不知道天高地厚啊！",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7012,
                    content = "嗯？你想听听我的故事？",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7013,
                    content = "借着酒劲，我就偷偷告诉你，边陲的尽头有着一座小教堂，那是残存不多的王妃教的朝圣地，",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7014,
                    content = "“王妃的黄金羽毛会使人们涅槃重生、重获理智”，\r\n这是支撑着我在这个世界活下去的支柱。",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7015,
                    content = "如今，王妃教被视为异端，教堂也悉数被摄政王拆毁，边陲的“庇护教堂”可能是最后一座完好的了......",
                },
                 new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7016,
                    content = "但是，教堂的大门貌似被古老的符文封锁了，没办法进去可真是麻烦啊！\r\n难不成是出现了新的“庇护对象”？",
                },
                new Sentence()
                {
                    speakerLocalID = 3011,
                    localID = 7017,
                    content = "嘛！如果你有什么线索的话，跟我通通气，不然这一趟可真是白来了！",
                },
            },
        } },


        {104, new Conversation()
        {
            id = 104,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3012,
                    localID = 7018,
                    content = "吱吱，欢迎来到萤火边陲！去酒馆喝一杯吱！",
                },
                new Sentence()
                {
                    speakerLocalID = 3012,
                    localID = 7019,
                    content = "说起来，酒馆的“萤火鸡尾酒”可是老板秘方吱，就连村长都挡不住它的魅力吱！",
                },
            },
        } },

        {105, new Conversation()
        {
            id = 105,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3013,
                    localID = 7020,
                    content = "叽叽，安心吧，这里没有危险叽！巨石，在保护着我们叽！",
                },
                new Sentence()
                {
                    speakerLocalID = 3013,
                    localID = 7021,
                    content = "虽然不清楚石头上的符文叽，但总能感受到它散发着理智的能力叽！",
                },
            },
        } },

        {106, new Conversation()
        {
            id = 106,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3014,
                    localID = 7022,
                    content = "咕咕，想要冒险咕？去树屋找村长咕！村长会引导你的咕！",
                },
                new Sentence()
                {
                    speakerLocalID = 3014,
                    localID = 7023,
                    content = "就在最顶层的树屋咕，不过，最好带点礼物讨好讨好村长咕！",
                },
            },
        } },

        {107, new Conversation()
        {
            id = 107,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3015,
                    localID = 7024,
                    content = "库库，冒险者库，巨石上究竟是什么呢库？好想上去看看库！",
                },
                new Sentence()
                {
                    speakerLocalID = 3015,
                    localID = 7025,
                    content = "听说教堂中有登上巨石的秘法库，但现在进不去库！",
                },
            },
        } },

        {108, new Conversation()
        {
            id = 108,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3016,
                    localID = 7026,
                    content = "提提，教堂现在不能进入提，林中的少女需要被庇护提！",
                },
                new Sentence()
                {
                    speakerLocalID = 3016,
                    localID = 7027,
                    content = "除非村长批准，否则都不能进入提！",
                },
                new Sentence()
                {
                    speakerLocalID = 3016,
                    localID = 7028,
                    content = "提，村长很严格的提，不会被你的花言巧语蛊惑的提！",
                },
            },
        } },

         {110, new Conversation()
        {
            id = 110,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3012,
                    localID = 7029,
                    content = "吱吱，好想喝“拉米亚之暮”吱，但度数太高好容易醉吱！",
                },
            },
        } },

        {111, new Conversation()
        {
            id = 111,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3012,
                    localID = 7030,
                    content = "吱吱，有空可以采集材料吱！可以自己制作好玩的东西吱！",
                },
            },
        } },


        {112, new Conversation()
        {
            id = 112,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3013,
                    localID = 7031,
                    content = "叽叽，什么时候能再去“酒馆欢乐时光”活动叽，好期待大家聚在一起的感觉叽！",
                },
            },
        } },
        {113, new Conversation()
        {
            id = 113,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3013,
                    localID = 7032,
                    content = "叽叽，发现好危险的刺客叽，是来刺杀谁的吗？",
                },
            },
        } },

         {114, new Conversation()
        {
            id = 114,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3015,
                    localID = 7033,
                    content = "库库，林中的少女，真漂亮库，只可惜，在教堂里出不来了库！",
                },
            },
        } },

        {115, new Conversation()
        {
            id = 115,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3015,
                    localID = 7034,
                    content = "库库，听说村长家有仅剩的几本书籍了库，但是没人想去看呢库！",
                },
            },
        } },


        {116, new Conversation()
        {
            id = 116,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3016,
                    localID = 7035,
                    content = "提提，强烈推荐雪鸮盛宴提！老板娘的手艺，真的比得上雪鸮城的御用大厨提！",
                },
            },
        } },

        {117, new Conversation()
        {
            id = 117,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3016,
                    localID = 7036,
                    content = "提提，大家真的能一直这么愉快的活下去吗？\r\n萤火边陲，真的会一直这么温暖吗提......",
                },
            },
        } },

        {140, new Conversation()
        {
            id = 140,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3002,
                    localID = 7037,
                    content = "已被符文封锁",
                },
            },
        } },

        {141, new Conversation()
        {
            id = 141,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {

                //0
                new Sentence()
                {
                    speakerLocalID = 0,
                    localID = 7038,
                    requireResponse = true,
                    content = "小镇的公告板，贴着几张泛黄且被淋湿的告示。",
                    options = new List<SentenceOption>()
                    {
                        new SentenceOption()
                        {
                            content="村长留言",
                            localID = 7039,
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            arg0 = 1
                        },
                        new SentenceOption()
                        {
                            content="圣阿希雅医院告示",
                            localID = 3003,
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            arg0 = 7
                        },
                        new SentenceOption()
                        {
                            content="酒馆欢乐时光",
                            localID = 3004,
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            arg0 = 10
                        },
                        new SentenceOption()
                        {
                            content="求助：寻鸟启事",
                            localID = 3005,
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            arg0 = 12
                        },
                        new SentenceOption()
                        {
                            content="A级悬赏令",
                            localID = 3006,
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            arg0 = 13
                        }
                    }
                },
                //1
                new Sentence()
                {
                    speakerLocalID = 3007,
                    localID = 7040,
                    content = "【村长留言】希望诸位村民们以和为贵，不要再因为到底是那个王最好这种事情吵架。如果还有这种事情，一律按照寻衅滋事处理，在家里禁闭十四天。",
                },
                //2
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7041,
                    content = "【回复】 哈，我就想不到了，怎么会有人觉得雪鸮城的那个王好啊，鸟脑子被兽化没了？",
                },
                //3
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7042,
                    content = "【回复】 是个正常鸟就都知道真王才是傻子好吗。脑子没长眼睛也没有？看不到政府颁布的GDP数据是不是哦。",
                },
                //4
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7043,
                    content = "【回复】 有本事别叽叽歪歪，来线下掰头。",
                },
                //5
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7044,
                    content = "【回复】 谁怕谁啊羽毛还没换干净的小鸟崽子！",
                },
                //6
                new Sentence()
                {
                    speakerLocalID = 3007,
                    localID = 7045,
                    content = "【村长回复】 禁闭二十八天。",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
                //7
                new Sentence()
                {
                    speakerLocalID = 3003,
                    localID = 7046,
                    content = "请诸位父母多多关注自家小鸟，不要将任何可能会噎到小鸟嗓子里的东西，尤其是美味的橡果之类的，放在小鸟可以够到的地方。",
                },
                //8
                new Sentence()
                {
                    speakerLocalID = 3003,
                    localID = 7047,
                    content = "近月已经有数十起小鸟因为父母的一时疏忽被橡果噎到嗓子，送到医院的事件了。",
                },
                //9
                new Sentence()
                {
                    speakerLocalID = 3003,
                    localID = 7048,
                    content = "医院可以分出的人手有限，在兽化症肆虐之际，我们需要各位鸟儿们齐心协力，才能够共渡难关。",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
                //10
                new Sentence()
                {
                    speakerLocalID = 3004,
                    localID = 7049,
                    content = "尊敬的客人们注意：出于不可抗原因，酒馆的欢乐时光从原本的下午两点至五点改为下午的三点至六点。打折的菜品由原本的拉米亚之暮改为精酿啤酒和谷麦硬面包。希望客人们光顾。",
                },
                //11
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7050,
                    content = "【回复】好耶！这样我五点下班还能蹭上点打折的口粮。不错，谢了老板！",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
                //12
                new Sentence()
                {
                    speakerLocalID = 3005,
                    localID = 7051,
                    content = "（启事剩下的部分被撕下来了，可能丢失的鸟儿已经被找到了吧）",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
                 //13
                new Sentence()
                {
                    speakerLocalID = 3006,
                    localID = 7052,
                    content = "悬赏一只麻雀，雏鹰之森叛逃者，极度危险。如有目击请立即通报。",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
            },
        } },

        {142, new Conversation()
        {
            id = 142,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 0,
                    localID = 7053,
                    content = "（看上去历经风霜的，但仍旧屹立不倒的石头，上面用古文字雕刻着话语）",
                },
                new Sentence()
                {
                    speakerLocalID = 0,
                    localID = 7054,
                    content = "吾等的母神，有翼之蛇库库尔坎，乃是伟力慈爱的神祇。她的神力同她环绕世界的蛇神一样广大无边际。",
                },
                new Sentence()
                {
                    speakerLocalID = 0,
                    localID = 7055,
                    content = "调酒qi她育有三子，强大温柔的神之子嗣。长子名为#47&`ajfh，是她的利剑，为她斩却一切敌人。长女名为jf7%^&*@af，是她的神使，",
                },
                new Sentence()
                {
                    speakerLocalID = 0,
                    localID = 7056,
                    content = "让她的声音得以传遍大地。幼女名为阿希雅，是她的温柔与慈悲。",
                },
                new Sentence()
                {
                    speakerLocalID = 0,
                    localID = 7057,
                    content = "（写着长子和长女名字的地方被人划花了，每道划痕都又深又长，可见刻下这些划痕的人心中的愤怒）",
                },
            },
        } },
        {150, new Conversation()
        {
            id = 150,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                //0
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID = 7058,
                    content = "欢迎来到凤凰酒馆！请问需要来点什么吗？",
                    requireResponse = true,
                    options = new List<SentenceOption>()
                    {
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "菜单",
                            localID = 7059,
                            arg0 = 1
                        },
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "劝酒",
                            localID=7060,
                            arg0 = 2
                        },
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "关于凤凰酒馆",
                            localID=7061,
                            arg0 = 3
                        },
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "关于情报",
                            localID = 7062,
                            arg0 = 5
                        },
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "离开",
                            localID=7063,
                            arg0=7
                        },
                    },

                },

                //1
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID = 7064,
                    content = "菜单在这，有空的话欢迎去我儿子的杂货店参观哈，他的货总能给人带来惊喜。 ",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
                //2
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID = 7065,
                    content = "嗯？想和我比酒量？\r\n呵呵呵来吧！",
                    action_OnEnd = ConvActionRef.END_CONVERSATION
                },
                //3
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID = 7066,
                    content = "是的，这家酒馆的名字叫“凤凰酒馆”，因为我可是王妃殿下的同族呵呵呵！\r\n虽然没有王妃本人美丽的黄金羽毛，但可不要质疑我凤凰的身份哦。",
                    requireResponse = true,
                    arg0=0,
                    arg1=3,

                    options = new List<SentenceOption>()
                    {
                        new SentenceOption()
                        {
                            action_OnSelect = ConvActionRef.JUMP_TO_SENTENCE,
                            content = "关于王妃",
                            localID = 7067,
                            arg0 = 4
                        }
                    }
                    
                },
                //4
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID = 7068,
                    content = "王妃殿下是我们永远尊敬的存在。虽然如今她行踪不明，但在每个人心中，她仍代表着这个世界的希望。这也是凤凰酒馆能吸引这么多过客的原因吧。",
                    action_OnEnd = ConvActionRef.LOCK_OPTION,
                    action_OnEnd2 = ConvActionRef.END_CONVERSATION,
                    arg0 = 0,
                    arg1 = 2
                },
                 //5
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID=7069,
                    content = "嗯？想从我口中套情报？\r\n对不起，虽然客人们总会酒后吐真言，不过作为调酒师可不能泄露隐私呵呵呵。",
                },
                //6
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID=7070,
                    content = "不过，这里的顾客都是闯荡天南海北的冒险者，\r\n想要从他们口中套出情报，得靠你自己的努力呵呵呵。",
                    action_OnEnd = ConvActionRef.LOCK_OPTION,
                    action_OnEnd2 = ConvActionRef.END_CONVERSATION,
                    arg0 = 0,
                    arg1 = 3
                },
                //7
                new Sentence()
                {
                    speakerLocalID = 3017,
                    localID = 7071,
                    content = "这里随时欢迎你!",
                    action_OnEnd = ConvActionRef.END_CONVERSATION,
                },
            },
        } },

        //花岛旅客女
        {151, new Conversation()
        {
            id = 151,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3018,
                    localID = 7072,
                },
                new Sentence()
                {
                    speakerLocalID = 3018,
                    localID = 7073,
                },
                new Sentence()
                {
                    speakerLocalID = 3018,
                    localID = 7074,
                },
                new Sentence()
                {
                    speakerLocalID = 3018,
                    localID = 7075,
                },
                new Sentence()
                {
                    speakerLocalID = 3018,
                    localID = 7076,
                },
            },
        } },

        //花岛旅客男
        {152, new Conversation()
        {
            id = 152,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3019,
                    localID = 7077,
                },
                new Sentence()
                {
                    speakerLocalID = 3019,
                    localID = 7078,
                },
                new Sentence()
                {
                    speakerLocalID = 3019,
                    localID = 7079,
                },
            },
        } },
         {153, new Conversation()
        {
            id = 153,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3020,
                    localID = 7080,
                },
            },
        } },
        {154, new Conversation()
        {
            id = 154,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7081,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7082,
                },
            },
        } },
            {155, new Conversation()
        {
            id = 155,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7083,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7084,
                },
            },
        } },
        {156, new Conversation()
        {
            id = 156,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7085,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7086,
                },
            },
        } },
        {157, new Conversation()
        {
            id = 157,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7087,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7088,
                },
            },
        } },

        {158, new Conversation()
        {
            id = 158,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7089,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7090,
                },
            },
        } },
        {159, new Conversation()
        {
            id = 159,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7091,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7092,
                },
            },
        } },
        {160, new Conversation()
        {
            id = 160,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7093,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7094,
                },
            },
        } },
        {161, new Conversation()
        {
            id = 161,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7095,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7096,
                },
            },
        } },
         {162, new Conversation()
        {
            id = 162,
            type = ConversationType.InScene,
            sentences = new List<Sentence>()
            {
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7097,
                },
                new Sentence()
                {
                    speakerLocalID = 3008,
                    localID = 7098,
                },
            },
        } },
    };
    #endregion

    #region Audio
    public static readonly Dictionary<GroundAudioType, string> AUDIO_GROUND_FILE_NAME = new Dictionary<GroundAudioType, string>()
    {
        {GroundAudioType.None, "Grass_Ground" },
        {GroundAudioType.Normal, "Normal_Ground" },
        {GroundAudioType.Grass, "Grass_Ground_3" },
        {GroundAudioType.Wood, "Wood_Ground" },
    };
    public static readonly Dictionary<GroundAudioType, string> AUDIO_GROUND_DROP_FILE_NAME = new Dictionary<GroundAudioType, string>()
    {
        {GroundAudioType.None, "Grass_Ground" },
        {GroundAudioType.Normal, "Normal_Ground" },
        {GroundAudioType.Grass, "Grass_Drop" },
        {GroundAudioType.Wood, "Wood_Drop" },
    };

    public static readonly string AUDIO_PLAYER_JUMP = "Jump";
    public static readonly string AUDIO_PLAYER_DASH = "Dash";
    public static readonly string AUDIO_UI_CLICK = "ui_click";

    public static readonly string AUDIO_SCENE_TITLE = "ui_save";
    #endregion

    #region Console
    public static readonly KeyCode G_CONSOLE_ACTIVATE_KEY = KeyCode.LeftAlt;
    public static readonly KeyCode G_CONSOLE_INPUT_KEY = KeyCode.LeftControl;

    public static readonly string G_CONSOLE_ERROR_MESSAGE = "Command not valid!";

    public const string G_CONSOLE_TITLE_EXIT = "exit";
    public const string G_CONSOLE_TITLE_RESPAWN = "rsp";
    public const string G_CONSOLE_TITLE_CHECKPOINT = "ckp";

    public const string G_CONSOLE_TITLE_LOAD_SCENE = "loadscene";

    public const string G_CONSOLE_TITLE_LANGUAGE = "lang";
    public const string G_CONSOLE_TITLE_SET_VALUE = "set";
    public const string G_CONSOLE_TITLE_GIVE_ITEM = "give";


    public const string G_CONSOLE_CHR_PLAYER = "ply";

    public const string G_CONSOLE_ARG_PLAYER_SPEED = "spd";
    public const string G_CONSOLE_ARG_PLAYER_JUMPFORCE = "jpf";
    public const string G_CONSOLE_ARG_PLAYER_JUMPLIMIT = "jpl";
    public const string G_CONSOLE_ARG_PLAYER_GRAVITY = "grv";

    #endregion

    #region Input
    public static readonly KeyCode INPUT_EXIT_KEYCODE = KeyCode.Escape;
    public static readonly KeyCode INPUT_PROCEED_KEYCODE = KeyCode.Return;
    public static readonly KeyCode INPUT_ENTER_SCENE_KEYCODE = KeyCode.W;
    public static readonly KeyCode INPUT_JUMP_KEYCODE = KeyCode.Space;
    public static readonly KeyCode INPUT_UP_KEYCODE = KeyCode.W;
    public static readonly KeyCode INPUT_DOWN_KEYCODE = KeyCode.S;
    public static readonly KeyCode INPUT_LEFT_KEYCODE = KeyCode.A;
    public static readonly KeyCode INPUT_RIGHT_KEYCODE = KeyCode.D;
    public static readonly KeyCode INPUT_INTERACT_KEYCODE = KeyCode.E;
    public static readonly KeyCode INPUT_FAST_RUN_KEYCODE = KeyCode.LeftShift;
    public static readonly float INPUT_PLAYER_LOOK_DOWN_HOLD_TIME = 0.6f;

    public static readonly KeyCode INPUT_ATTACK_KEYCODE = KeyCode.Mouse0;

    public static readonly KeyCode INPUT_OPEN_INVENTORY_KEYCODE = KeyCode.Tab;
    #endregion
}
