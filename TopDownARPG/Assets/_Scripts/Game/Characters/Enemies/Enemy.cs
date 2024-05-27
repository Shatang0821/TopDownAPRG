using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject _enemy;

    private MoveComponent _moveComponent;

    [SerializeField]private NavMeshAgent _nav;

    public void Init(GameObject enemy)
    {
        this._enemy = enemy;
       // _moveComponent = new MoveComponent(,_enemy.transform);
    }
    
    public void AllowPlayer(GameObject play)
    {
        //var _vector3 = (play.transform.position - _enemy.transform.position).normalized;
        //_moveComponent.Move(_vector3);

        _nav.SetDestination(play.transform.position);
    }
}