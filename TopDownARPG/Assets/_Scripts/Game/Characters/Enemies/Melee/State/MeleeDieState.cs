using UnityEngine;

public class MeleeDieState : EnemyBaseState
 {
     public MeleeDieState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
     {
     }
     
        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void AnimationEndCalled()
        {
            base.AnimationEndCalled();
            Die();
        }

        private void Die()
        {
            enemy.RaiseOnDeathEvent();
            enemy.gameObject.SetActive(false);
        }
        
        
 }
