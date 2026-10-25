using UnityEngine;

public class Player : MonoBehaviour, ISwitchable
{
    [Header("Shooting")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f;

    [Header("Targeting")]
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float rotateSpeed = 360f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visuals")]
    [SerializeField] private Renderer barrelRenderer;

    private GameColor currentColor = GameColor.Red;
    private Renderer rend;
    private Transform target;
    private float timer;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        ApplyColor();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        FindNearestEnemy();
        RotateToTarget();

        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            Shoot();
        }
    }

    void OnMouseDown()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        ISwitchable switchable = GetComponent<ISwitchable>();
        if (switchable != null) switchable.SwitchState();
    }

    public void SwitchState()
    {
        currentColor = (GameColor)(((int)currentColor + 1) % 3);
        ApplyColor();
    }

    void ApplyColor()
    {
        Color c = GameColorUtil.ToColor(currentColor);
        rend.material.color = c;
        if (barrelRenderer != null)
            barrelRenderer.material.color = c;
    }

    void FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange, enemyLayer);
        float best = float.MaxValue;
        target = null;

        foreach (var h in hits)
        {
            float d = (h.transform.position - transform.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                target = h.transform;
            }
        }
    }

    void RotateToTarget()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion goal = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, goal, rotateSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        Bullet b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.Init(currentColor);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}