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
    [SerializeField] float moveSpeed = 8;
    [SerializeField] float acceleration = 50;
    [SerializeField] float deceleration = 40;
    [SerializeField] float rotationSpeed = 15;
    //Dash
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 20;
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
    [SerializeField] Transform firePoint;
    [Header("Attack Cooldowns")]
    [SerializeField] float swordCooldown = 0.4f;
    [SerializeField] float gunCooldown = 0.2f;
    [SerializeField] float nextSwordTime = 0;
    [SerializeField] float nextGunTime = 0;
    [Header("Change Settings")]
    [SerializeField] bool _isChanging;
    [SerializeField] int changeCooldown = 1;
    [Header("Charge Attack Settings")]
    [SerializeField] float minChargeTime = 1.0f;
    [SerializeField] float maxChargeTime = 2.0f;
    [SerializeField] float chargedDamage = 50;
    [SerializeField] float chargedWidth = 2;
    [SerializeField] float chargedRange = 15;
    [SerializeField] float chargeTimer = 0;
    [SerializeField] bool isCharging = false;
    [Header("Spirit Attack Settings")]
    [SerializeField] float nextSpiritTime = 0;
    [SerializeField] float spiritCooldown = 20;
    [SerializeField] Transform spawner;
    [Header("Gravity Settings")]
    [SerializeField] private Transform _groundSensor;
    [SerializeField] private float _groundSensorRadius;
    [SerializeField] private LayerMask _groundLayer;
    private float _gravity = -9.81f;
    Vector3 _playerGravity;


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

    public void OnChange(InputValue value)
    {
        if (value.isPressed && !_isChanging)
        {
            _isChanging = true;
            StartCoroutine(Change());
        }
    }

    public void OnSpecialAttack(InputValue value)
    {
        if(_CAct)
        {
            if (value.isPressed && Time.time >= nextSpiritTime)
            {
                SpawnSpirit();
                nextSpiritTime = Time.time + spiritCooldown;
            }
        }

        if(_TAct == true)
        {
            if (value.isPressed)
            {
                isCharging = true;
                chargeTimer = 0f;
                Debug.Log("Cargando disparo...");
            }
            else
            {
                isCharging = false;
                
                if (chargeTimer >= minChargeTime)
                {
                    ExecuteChargedShoot();
                }
                else
                {
                    Debug.Log("Carga insuficiente, disparo cancelado o disparo normal.");
                }
                chargeTimer = 0f;
            }
        }
        
    }

    #endregion
    #region Update

    void Update()
    {
        HandleTimers();
        Gravity();
        
        if (isDashing)
        {
            ApplyDash();
        }
        else
        {
            Movement();
        }

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            // VFX? Charging
        }
    }

    #endregion
    #region FixedUpdate (Physics)







    #endregion
    #region IsGrounded

    bool IsGrounded()
    {
        return Physics.CheckSphere(_groundSensor.position, _groundSensorRadius, _groundLayer);
    }

    #endregion
    #region Gravity

    void Gravity()
    {
        if(!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }
        _CC.Move(_playerGravity * Time.deltaTime);
    }

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

    IEnumerator Change()
    {
        if(_TAct)
        {
            _TAct = false;
            _CAct = true;
            _T.SetActive(false);
            _C.SetActive(true);
            yield return new WaitForSeconds(1);
            _isChanging = false;
        }
        else if(_CAct)
        {
            _CAct = false;
            _TAct = true;
            _C.SetActive(false);
            _T.SetActive(true);
            yield return new WaitForSeconds(1);
            _isChanging = false;
        }
    }

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
            IDamageable damageable = enemy.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            IKnockbackable knockbackable = enemy.GetComponent<IKnockbackable>();
            if (knockbackable != null)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                float force = 10;
                float duration = 0.2f;
                knockbackable.ApplyKnockback(direction * force, duration);
            }
        }
    }

    // THALYA ATTACKS
    void RangedAttack()
    {
        //Animacion
        GameObject bullet = PoolManager.Instance.GetPooledObject("ElectricBullets", firePoint.position, firePoint.rotation);
        bullet.SetActive(true);
    }
    
    #endregion
    #region SpecialAttack

    //CEDRIC SPECIAL ATTACK
    void SpawnSpirit()
    {
        GameObject Spirit = PoolManager.Instance.GetPooledObject("Spirits", spawner.position, spawner.rotation);
        Spirit.SetActive(true);
    }

    //THALYA SPECIAL ATTACK
    void ExecuteChargedShoot()
    {
        Debug.Log("¡DISPARO CARGADO!");
        GameObject Misile = PoolManager.Instance.GetPooledObject("Misiles", firePoint.position, firePoint.rotation);
        Misile.SetActive(true);
    }

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
