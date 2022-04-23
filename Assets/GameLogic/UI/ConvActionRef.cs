public sealed class ConvActionRef
{
    /// <summary>
    /// arg0: sentence index
    /// </summary>
    public static readonly int JUMP_TO_SENTENCE = 100;
    public static readonly int END_CONVERSATION = 101;

    /// <summary>
    /// arg0: sentence index, arg1: option index
    /// </summary>
    public static readonly int UNLOCK_OPTION = 102;
    /// <summary>
    /// arg0: sentence index, arg1: option index
    /// </summary>
    public static readonly int LOCK_OPTION = 103;
}
