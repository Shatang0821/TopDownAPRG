namespace FrameWork.Pool
{
    public interface IPoolable
    {
        /// <summary>
        /// インスタンスが使用可能を示す
        /// </summary>
        bool IsActive { get; set; }
        /// <summary>
        /// リセット関数
        /// </summary>
        void Reset();
    }
}
