
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] float lookRadius;
    [SerializeField] Animator animator;

    public float ChaseSpeed;
    public float AttackSpeed;
    public float IdleSpeed;
    public float damage;

    float ChaseTime;
    [SerializeField] float cooldownTime;
    private float attackCooldown;
    Transform target;
    NavMeshAgent agent;
    float distance;

    //sets iportant variables
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        agent.speed = ChaseSpeed;
        ResetCooldown();
        attackCooldown = 0.18f;
    }

    // tells the ai where to go depending on the distance between the player and the zombie
    void FixedUpdate()
    {
        damage = EnemyManager.instance.zombie_Damage;

        if (PlayerManager.instance.deadPlayer || PlayerManager.instance.gamePaused) agent.isStopped = true;
        distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius && !PlayerManager.instance.deadPlayer)
        {
            
            if (distance <= 10f)
            {
                Charge();
            }
            else
            {
                Chase();
            }
        }
        else
        {
            ResetCooldown();
            agent.speed = IdleSpeed;
            agent.SetDestination(target.position);
            //animator.SetTrigger("Idle");
            Patrol();
        }
    }

    //charging for attack depenting on a cooldown
    private void Charge()
    {
        //animator.SetTrigger("Idle");
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

    // attacks the player
    private void Attack()
    {
        if (distance <= agent.stoppingDistance)
        {
            //animator.SetTrigger("Kick");
            agent.SetDestination(transform.position);
            if (attackCooldown > 0)
                attackCooldown -= Time.deltaTime;
            else
            {
                target.GetComponent<PlayerHealth>().GetDamage(damage);
                FaceTarget();
                ResetCooldown();
                attackCooldown = 0.18f;
            }
        }
        else
        {
            //animator.SetTrigger("Dash");

            agent.SetDestination(target.position);
            agent.speed = AttackSpeed;
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
        FaceTarget();
        //animator.SetTrigger("Run");
        agent.speed = ChaseSpeed;
        agent.SetDestination(target.position);
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 5f);
    }
    private void ResetCooldown()
    {
        ChaseTime = cooldownTime;
    }
    }
