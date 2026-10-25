using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 4f;

    public GameColor Color {get; private set;}

    public void Init(GameColor color)
    {
        Color = color;
        GetComponent<Renderer>().material.color = GameColorUtil.ToColor(color);
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg == null) return;

        dmg.TakeHit(Color);
        Destroy(gameObject);
    }
}