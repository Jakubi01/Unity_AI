using System.Collections.Generic;
using UnityEngine;

/*
 * FSM (Finite State Machine, 유한 상태 기계)
 *      - 한 번에 하나의 상태만 가지며, 특정 조건에 따라 다른 상태로 전환되는 구조
 *          * State : 현재 상태 (Idle, Attack, Patrol, Chase, Die)
 *
 *          * Transition : 상태 변경 조건
 *          * Entry / Exit : 상태 진입 시 동작 / 상태 탈출 시 동작
 *          * Update : 상태 지속 중 반복 처리 (예, 추격 중 이동)
 */

// TODO : Implement patrol function

public class Enemy_FSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Die
    }
    
    [SerializeField]
    private List<State> states = new();

    public State state = State.Idle;
    private Animator _animator;
    public Transform targetTransform;
    public const float ChaseRange = 20f;
    private const float AttackRange = 1.5f;
    private const float Speed = 2f;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        UpdateTransition();
    }

    private void UpdateTransition()
    {
        switch (state)
        {
            case State.Idle:
                MonsterIdleState();
                break;
            
            case State.Patrol:
                MonsterPatrolState();
                break;
            
            case State.Chase:
                MonsterChaseState();
                break;
            
            case State.Attack:
                MonsterAttackState();
                break;
            
            case State.Die:
                MonsterDieState();
                break;
        }

        CheckTransition();
    }

    private void MonsterIdleState()
    {
        ChangeAnimationState(nameof(State.Idle));
    }

    private void MonsterPatrolState()
    {
        ChangeAnimationState(nameof(State.Patrol));
    }

    private void MonsterChaseState()
    {
        ChangeAnimationState(nameof(State.Chase));

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetTransform.position,
            Time.deltaTime * Speed
            );

        Vector3 targetDir = (targetTransform.position - transform.position).normalized;
        targetDir.y = 0;
        
        transform.forward = targetDir;
    }

    private void MonsterAttackState()
    {
        ChangeAnimationState(nameof(State.Attack));
    }

    private void MonsterDieState()
    {
        ChangeAnimationState(nameof(State.Die));
    }

    private void ChangeAnimationState(string strState)
    {
        // foreach(State s in states)
        // {
        //     _animator.SetBool(nameof(s), false);
        // }
        
        _animator.SetBool(nameof(State.Idle), false);
        _animator.SetBool(nameof(State.Chase), false);
        _animator.SetBool(nameof(State.Attack), false);
        
        _animator.SetBool(strState, true);
    }

    private void CheckTransition()
    {
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < AttackRange)
        {
            state = State.Attack;
        }
        else if (distance > AttackRange)
        {
            state = State.Chase;
        }
        else if(distance > ChaseRange)
        {
            state = State.Idle;
        }
        // add patrol state condition
    }

    private void DoPatrol()
    {
        
    }
}