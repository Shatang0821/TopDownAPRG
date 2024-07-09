
    public class MeleeMovementState : EnemyBaseState
    {
        public MeleeMovementState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (enemy.IsTakenDamaged)
            {
                enemy.TakenDamageState();
                return;
            }
            if (enemy.InAttackRange)
            {
                //攻撃
                enemyStateMachine.ChangeState(MeleeStateEnum.Attack);
                return;
            }
        }
    }
