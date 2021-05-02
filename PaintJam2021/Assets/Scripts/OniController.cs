using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniController : MonoBehaviour
{    
    public static OniController _instance;
    public float moveSpeed = 1f;
    public float maxHealth = 10f;

    // Sound for death of Oni
    public AudioClip soundClip;
    public float volume = 1f;
    public bool isBoss = false;


    public int oniCount {
        get {
            return currentOniCount;
        }
        set {
            currentOniCount = value;
        }
    }
    public float health {
        get {
            return currentHealth;
        }
    }

    public float attackPower {
        get {
            return currentAttackPower;
        }
    }

    private static int currentOniCount;
    [SerializeField] private Vector2 targetPos;
    private Vector2 moveDir = new Vector2(0,0);
    [SerializeField] private bool movedToTarget = true;
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentAttackPower;
    private Animator animator;
    
    void Awake() {
        _instance = this;
        if(!isBoss) {
            currentOniCount++;
        }
        targetPos = Vector2.zero;
        currentHealth = maxHealth;
        currentAttackPower = 2;
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(movedToTarget) {
            targetPos = RandomDirection();
        }

        if(!Mathf.Approximately(Vector3.Distance(transform.position, targetPos), 0)) {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, step / 3);
            movedToTarget = false;
        }
        else {
            movedToTarget = true;
        }

        animator.SetFloat("MoveX", targetPos.x);
    }

    void FixedUpdate() {
  
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.collider.CompareTag("Bullet") && !isBoss) {
            Bullets._instance.ReturnBulletToPool(coll.gameObject);
            HitByBullet(PlayerController._instance.attackPower);
        }
        if(coll.collider.CompareTag("Bullet") && isBoss) {
            Bullets._instance.ReturnBulletToPool(coll.gameObject);
            HitByBullet(PlayerController._instance.attackPower);
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {

    }

    void HitByBullet(float amount) {
        currentHealth -= amount;

        if(currentHealth <= 0) {
            // Trigger death animation sound
            PlayerController._instance.PlaySound(soundClip, volume);

            if(isBoss) {
                // SceneSwitcher endScene = new SceneSwitcher();
                // StartCoroutine(endScene.EndGame(2f));
                PlayerController._instance.ChangeHealth(-100);
            }
            else {
                --currentOniCount;
            }
            Destroy(gameObject);
        }
    }

    public Vector2 RandomDirection() {
        return new Vector2(Random.Range(-20,20), Random.Range(-20,20));
    }
}
