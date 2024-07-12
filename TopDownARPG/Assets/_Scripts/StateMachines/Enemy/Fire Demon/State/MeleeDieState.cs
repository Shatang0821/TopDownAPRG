using UnityEngine;

public class MeleeDieState : EnemyBaseState
 {
     private bool _isDead;
     public MeleeDieState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
     {
     }
     
        public override void Enter()
        {
            base.Enter();
            _isDead = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_isDead)
            {
                Die();
            }
        }

        public override void AnimationEndCalled()
        {
            base.AnimationEndCalled();
            _isDead = true;
        }

        
        private void Die()
        {
            _isDead = false;
            enemy.gameObject.SetActive(false);
            enemy.RaiseOnDeathEvent();
        }
 }
