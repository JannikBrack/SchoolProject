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

    float nextAttack = 0f;
    float nextAttackStep = 0f;
    [SerializeField] float cooldownTime = 5f;
    [SerializeField] float cooldownAttackTime = 2f;
    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        agent.speed = ChaseSpeed;
        nextAttackStep = Time.time + cooldownTime;
        nextAttack = Time.time + cooldownAttackTime;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.speed = ChaseSpeed;
            agent.SetDestination(target.position);

            if (distance <= 5f)
            {
                agent.SetDestination(transform.position);
                if (Time.time - nextAttackStep < cooldownTime)
                {
                    Debug.Log(1);
                    agent.speed = 50f;
                    agent.SetDestination(target.position);
                    if (distance <= agent.stoppingDistance && Time.time - nextAttack < cooldownAttackTime)
                    {
                        Debug.Log(2);
                        target.GetComponent<PlayerHealth>().GetDamage(damage);
                        FaceTarget();
                        agent.SetDestination(transform.position);
                        nextAttack = Time.time + cooldownAttackTime;
                    }
                    nextAttackStep = Time.time;
                }
                
            }
        }
        else
        {
            agent.speed = IdleSpeed;
            Vector3 patrolPosition = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            if (!(agent.stoppingDistance <= Vector3.Distance(patrolPosition, transform.position))) agent.SetDestination(new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50)));
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 5f);
    }


}
