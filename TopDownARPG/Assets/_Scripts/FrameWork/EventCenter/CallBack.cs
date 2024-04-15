namespace FrameWork.EventCenter
{
    public delegate void CallBack();
    public delegate void CallBack<in T>(T arg);
    public delegate void CallBack<in T1, in T2>(T1 arg1,T2 arg2);
}
