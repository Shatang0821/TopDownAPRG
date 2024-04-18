using FrameWork.FSM;

namespace SK
{
    public enum PlayerStateEnum
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }
    public class PlayerStateMachine : StateMachine
    {
        private PlayerIdleState _playerIdleState;
        PlayerStateMachine()
        {
            InstantiateState();
            InitializeState(_playerIdleState);
        }

        /// <summary>
        /// 状態クラスのインスタンスを作成する       
        /// </summary>
        private void InstantiateState()
        {
            _playerIdleState = new PlayerIdleState(PlayerStateEnum.Idle.ToString(), this);
        }
    }
}