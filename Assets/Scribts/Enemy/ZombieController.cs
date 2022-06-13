using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] float lookRadius = 10f;

    public float ChaseSpeed = 5f;
    public float IdleSpeed = 2f;
    [SerializeField] float damage;

    float ChaseTime = 0f;
    float attackTime = 0f;
    [SerializeField] float cooldownTime;
    [SerializeField] float cooldownAttackTime;
    Transform target;
    NavMeshAgent agent;
    float distance;
    bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        agent.speed = ChaseSpeed;
        StartCooldown();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius && !PlayerManager.instance.deadPlayer)
        {
            Chase();
            if (distance <= 7f)
            {
                Charge();
            }
            else
            {
                StartCooldown();
                agent.SetDestination(target.position);
            }

        }
        else
        {
            Patrol();
        }
    }
    private void Charge()
    {
        agent.SetDestination(transform.position);
        if (ChaseTime > 0)
        {
            ChaseTime -= Time.deltaTime;
        }
        else
        {
            Attack();
        }
        
    }
    private void Attack()
    {
        agent.SetDestination(target.position);
        agent.speed = 1000f;
        if (distance <= agent.stoppingDistance)
        {

            agent.SetDestination(transform.position);
            target.GetComponent<PlayerHealth>().GetDamage(damage);
            FaceTarget();
            StartCooldown();
            
        }
    }
    private void Patrol()
    {
        agent.speed = IdleSpeed;
        Vector3 patrolPosition = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        if (!(agent.stoppingDistance <= Vector3.Distance(patrolPosition, transform.position))) agent.SetDestination(new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50)));
    }
    private void Chase()
    {
        agent.speed = ChaseSpeed;
        agent.SetDestination(target.position);
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 5f);
    }
    private void StartCooldown()
    {
        ChaseTime = cooldownTime;
    }
    }
