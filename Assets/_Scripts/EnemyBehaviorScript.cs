using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform _Player;

    [SerializeField] private float updateInterval = 0.2f;
    [SerializeField] private float updateTimer;
    public float stoppingDistance = 1;

    [Header("Separation Settings")]
    public float separationRadius = 2f;
    public float separationStrength = 3f;
    public LayerMask enemyLayer;

    [SerializeField] NavMeshAgent _agent;
    private static readonly Collider[] neighborBuffer = new Collider[16];

    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return;

        updateTimer -=Time.deltaTime;

        if(updateTimer <= 0)
        {
            updateTimer = updateInterval;
            Vector3 _separation = GetSeparationVector();
            Vector3 _destination = _Player.position + _separation;
            
            if(NavMesh.SamplePosition(_destination, out NavMeshHit hit, separationRadius +1, NavMesh.AllAreas))
                _agent.SetDestination(hit.position);
            else
                _agent.SetDestination(_Player.position);
        }        
    }

    private Vector3 GetSeparationVector()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, 
            separationRadius, neighborBuffer, enemyLayer);

        Vector3 push = Vector3.zero;
        int _neighbors = 0;
        for(int i = 0; i < count; i++)
        {
            Collider other = neighborBuffer[i];
            if (other.transform == transform)
                continue;

            Vector3 difference = transform.position - other.transform.position;
            float _dist = difference.magnitude;

            if(_dist > 0.001f && _dist < separationRadius)
            {
                push += difference.normalized * (separationRadius - _dist);
                _neighbors++;
            }
        }

        if(_neighbors > 0)
        {
            push /= _neighbors;
            push *= separationStrength;
        }
        return push;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }

}
