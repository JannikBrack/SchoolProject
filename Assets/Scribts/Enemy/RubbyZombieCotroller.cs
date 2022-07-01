
using UnityEngine;
using UnityEngine.AI;

public class RubbyZombieCotroller : MonoBehaviour
{
    [SerializeField] float lookRadius;
    [SerializeField] Animator animator;


    public float ChaseSpeed;
    public float IdleSpeed;
    [SerializeField] float damage;

    float ChaseTime;
    [SerializeField] float cooldownTime;
    Transform target;
    NavMeshAgent agent;
    float distance;

    //sets iportant variabless
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        agent.speed = ChaseSpeed;
        StartCooldown();
    }

    // tells the ai where to go depending on the distance between the player and the zombie
    void FixedUpdate()
    {
        damage = EnemyManager.instance.zombie_Damage * 2;

        distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius && !PlayerManager.instance.deadPlayer || PlayerManager.instance.gamePaused)
        {
            Chase();
            if (distance <= 30f)
            {
                Charge();
            }
            else
            {
                animator.SetTrigger("Chase");
                StartCooldown();
                agent.speed = IdleSpeed;
                agent.SetDestination(target.position);
            }

        }
        else
        {
            Patrol();
        }
    }
    //charging for attack depenting on a cooldown
    private void Charge()
    {
        FaceTarget();
        agent.SetDestination(transform.position);
        if (ChaseTime > 0)
        {
            if(!(distance < 10f))
            animator.SetTrigger("Charge");
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
        agent.speed = ChaseSpeed;
        if (distance <= agent.stoppingDistance)
        {
            if (distance <= 4f)
            animator.SetTrigger("Idle");
            agent.SetDestination(transform.position);
            target.GetComponent<PlayerHealth>().GetDamage(damage);
            FaceTarget();
            StartCooldown();

        }
        else animator.SetTrigger("Attack");
    }
    private void Patrol()
    {
        animator.SetTrigger("Idle");
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
