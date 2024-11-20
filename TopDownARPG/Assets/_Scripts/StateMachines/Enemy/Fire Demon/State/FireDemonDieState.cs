using UnityEngine;

public class FireDemonDieState : FireDemonBaseState
 {
     public FireDemonDieState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
     {
     }
     

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (stateTimer >= 2.0f)
            {
                Die();
            }
            
        }
        
        private void Die()
        {
            enemy.gameObject.SetActive(false);
            enemy.RaiseOnDeathEvent();
        }
 }
