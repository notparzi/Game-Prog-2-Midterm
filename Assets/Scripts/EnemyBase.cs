using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected float moveSpeed = 1.5f;
    [SerializeField] protected GameColor enemyColor;

    protected Transform player;

    protected virtual void Start()
    {
        var p = FindFirstObjectByType<Player>();
        if (p != null) player = p.transform;
        GetComponent<Renderer>().material.color = GameColorUtil.ToColor(enemyColor);
    }

    protected virtual void Update()
    {
        if (player == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        Move();
    }

    protected virtual void Move()
    {
        Vector3 target = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    public bool TakeHit(GameColor bulletColor)
    {
        if (bulletColor == enemyColor)
        {
            Die();
            return true;
        }
        return false;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            GameManager.Instance.GameOver();
    }
}