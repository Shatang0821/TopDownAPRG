using UnityEngine;

public class FireDemonDieState : EnemyBaseState
 {
     private bool _isDead;
     public FireDemonDieState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
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
            if (stateTimer >= 3.0f)
            {
                Die();
            }
            if (_isDead)
            {
                Die();
            }
            
        }

        
        private void Die()
        {
            _isDead = false;
            enemy.gameObject.SetActive(false);
            enemy.RaiseOnDeathEvent();
        }
 }
