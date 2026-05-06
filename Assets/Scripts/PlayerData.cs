using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    //vida
    public float _maxHealth = 100;
    public float _Health = 100;

    //Ataque
    public float AttackDamage = 10;
    public float BulletDamage = 10;

    //dash
    public float dashSpeed = 20;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    //velocidad
    public float moveSpeed = 8;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
