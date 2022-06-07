using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float lookRadius = 10f;
    public float speed = 5f;
    [SerializeField] float damage;

    float nextAttack = 0f;
    [SerializeField] float coldownTime = 2f;
    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float distace = Vector3.Distance(target.position, transform.position);
        if (distace <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distace <= agent.stoppingDistance)
            {
                if (Time.time > nextAttack)
                {
                    //Attackanimation
                    target.GetComponent<PlayerHealth>().GetDamage(damage);
                    FaceTarget();
                    nextAttack = Time.time + coldownTime;
                }
                
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 5f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
