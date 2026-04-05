using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region Variables

    //Components
    [Header("Components")]
    [SerializeField] CharacterController _CC;
    //Inputs
    [Header("Inputs")]
    InputAction _moveA;
    Vector2 _moveInput;
    Vector3 currentVelocity;
    Vector3 dashDirection;
    InputAction _changeA;
    InputAction _dashA;
    InputAction _AttackA;
    //Movement
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float acceleration = 50f;
    [SerializeField] float deceleration = 40f;
    [SerializeField] float rotationSpeed = 15f;
    //Dash
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 0.5f;
    float dashTimer;
    float cooldownTimer;
    bool isDashing;
    //Otras
    [Header("Other Settings")]
    [SerializeField] float _TurnVelocity = 0.2f;
    [SerializeField] float _maxHealth = 100;
    [SerializeField] float _Health = 100;
    [SerializeField] float AttackDamage = 5;
    //Cedric
    [Header("Cedric Settings")]
    [SerializeField] GameObject _C;
    [SerializeField] bool _CAct;
    //Thalya
    [Header("Thalya Settings")]
    [SerializeField] GameObject _T;
    [SerializeField] bool _TAct;
    //MeleeAttack
    [Header("Combat Settings")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float swordRange = 1.5f;
    [SerializeField] LayerMask enemyLayers;
    [Header("Combo Settings")]
    [SerializeField] int comboStep = 0;
    [SerializeField] float comboResetTime = 0.8f;
    [SerializeField] float lastClickedTime;
    [Header("Ranged Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [Header("Attack Cooldowns")]
    [SerializeField] float swordCooldown = 0.4f;
    [SerializeField] float gunCooldown = 0.2f;
    [SerializeField] float nextSwordTime = 0f;
    [SerializeField] float nextGunTime = 0f;

    #endregion
    #region Awake

    void Awake()
    {
        _CC = GetComponent<CharacterController>();
        _moveA = InputSystem.actions["Move"];
        _changeA = InputSystem.actions["Change"];
        _dashA = InputSystem.actions["Dash"];
        _AttackA = InputSystem.actions["Attack"];
    }

    #endregion
    #region Detectores

    public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();

    public void OnDash(InputValue value)
    {
        if (value.isPressed && cooldownTimer <= 0) StartDash();
    }

    public void OnAttack(InputValue value)
    {
        if(_CAct == true)
        {
            if (value.isPressed)
            {
                if (Time.time - lastClickedTime > comboResetTime)
                {
                    comboStep = 0;
                }

                if (Time.time >= nextSwordTime)
                {
                    ExecuteComboStep();
                }
            }
        }
        
        if(_TAct == true)
        {
            if (value.isPressed && Time.time >= nextGunTime)
            {
                RangedAttack();
                nextGunTime = Time.time + gunCooldown;
            }
        }
        
    }

    #endregion
    #region Update

    void Update()
    {
        HandleTimers();
        
        if (isDashing)
        {
            ApplyDash();
        }
        else
        {
            Movement();
        }
    }

    #endregion
    #region FixedUpdate (Physics)







    #endregion
    #region IsGrounded







    #endregion
    #region Gravity









    #endregion
    #region Movement

    void Movement()
    {
        Vector3 targetDirection = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
        float targetSpeed = targetDirection.magnitude * moveSpeed;
        float currentSpeed = new Vector3(currentVelocity.x, 0, currentVelocity.z).magnitude;
        float accelRate = (targetSpeed > 0.01f) ? acceleration : deceleration;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetDirection * moveSpeed, accelRate * Time.deltaTime);
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        _CC.Move(currentVelocity * Time.deltaTime);
    }

    #endregion
    #region Dash

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
        Vector3 inputDir = new Vector3(_moveInput.x, 0, _moveInput.y);
        dashDirection = inputDir.magnitude > 0 ? inputDir.normalized : transform.forward;
    }

    void ApplyDash()
    {
        _CC.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0) isDashing = false;
    }

    #endregion
    #region Change







    #endregion
    #region BasicAttack
    
    // CEDRIC ATTACKS
    void ExecuteComboStep()
    {
        lastClickedTime = Time.time;
        
        if (comboStep == 0)
        {
            Debug.Log("--- ATAQUE 1: Tajo rápido ---");
            Attack(10, 1.5f);
            nextSwordTime = Time.time + 0.25f;
        }
        else if (comboStep == 1)
        {
            Debug.Log("--- ATAQUE 2: Tajo inverso ---");
            Attack(10, 1.5f);
            nextSwordTime = Time.time + 0.25f;
        }
        else if (comboStep == 2)
        {
            Debug.Log("--- ATAQUE 3: ESTOCADA FINAL ---");
            Attack(25, 2.5f);
            nextSwordTime = Time.time + 0.6f;
        }

        // Avanzar al siguiente paso o volver al inicio
        comboStep++;
        if (comboStep > 2) comboStep = 0;
    }
    
    void Attack(float damage, float range)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, range, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Golpeaste a " + enemy.name + " con " + damage + " de daño.");
            //  Take Damage (damage)
        }
    }

    // THALYA ATTACKS
    void RangedAttack()
    {
        //Animacion
        Debug.Log("Disparo");
        // Pool de la bala
    }
    
    #endregion
    #region SpecialAttack
    







    #endregion
    #region Ulti
    








    #endregion
    #region IDanageable
    
    public void TakeDamage(float damage)
    {

    }

    #endregion
    #region Timers

    void HandleTimers()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    #endregion
    #region Gizmos

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        float visualRange = (comboStep == 2) ? 2.5f : 1.5f; 
        Gizmos.DrawWireSphere(attackPoint.position, visualRange);
    }
    
    #endregion
}
