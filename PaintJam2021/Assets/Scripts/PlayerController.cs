using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _instance;
    public float pickupDistance = 1.5f;

    // AudioSource for player interaction
    AudioSource audioSource;

    // Sound for shooting
    public AudioClip shootClip;
    public float volume = 1f;

    public float maxHealth = 10;

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

    public float movementSpeed {
        get {
            return currentSpeed;
        }
    }

    public float invulnTime {
        get {
            return currentInvulnTime;
        }
    }

    public Vector2 playerPos {
        get {
            return currentPos;
        }
    }
    private Rigidbody2D body;
    private Vector2 movement;
    private Vector2 lookDir = new Vector2(0, -1);
    private List<YokaiController> collectedYokai;
    private YokaiController activeYokai;
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentAttackPower;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentInvulnTime;
    private List<GameObject> activeBullets;
    private bool isInvulerable;
    private float invulnTimer;
    private Animator animator;
    private Vector2 currentPos;
    private bool isDead = false;

    void Awake() {
        _instance = this;
        body = GetComponent<Rigidbody2D>();
        collectedYokai = new List<YokaiController>();
        currentHealth = 10f;
        currentAttackPower = 1f;
        currentSpeed = 7f;
        currentInvulnTime = 1f;
        activeBullets = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead) {
            currentPos = body.position;
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if(!Mathf.Approximately(movement.x, 0f) || !Mathf.Approximately(movement.y, 0f)) {
                lookDir = movement;
                lookDir.Normalize();
            }
            else {

            }

            animator.SetFloat("MoveX", lookDir.x);
            animator.SetFloat("MoveY", lookDir.y);
            animator.SetFloat("Speed", movement.magnitude);

            if(isInvulerable) {
                invulnTimer -= Time.deltaTime;
                if(invulnTimer < 0) {
                    isInvulerable = false;
                }
            }

            if(Input.GetKeyDown(KeyCode.E)) {
                RaycastHit2D hit = Physics2D.Raycast(body.position + Vector2.up * .2f, lookDir, pickupDistance, LayerMask.GetMask("NPC"));
                if(hit.collider != null && hit.collider.CompareTag("Yokai")) {
                    activeYokai = hit.collider.gameObject.GetComponent<YokaiController>();
                    PickupYokai();
                }
            }

            // Mouse 1 click
            if(Input.GetMouseButtonDown(0)) {
                PlaySound(shootClip, volume);
                PopTheGlock();
            }

            for(int i = 0; i < activeBullets.Count; i++) {
                if(activeBullets[i].transform.position.x > 30 || activeBullets[i].transform.position.x < -25 || activeBullets[i].transform.position.y > 25 || activeBullets[i].transform.position.y < -25) {
                    Bullets._instance.ReturnBulletToPool(activeBullets[i]);
                    activeBullets.RemoveAt(i);
                }
            }
        }  
    }

    void FixedUpdate() {
        Vector2 pos = body.position;
        pos = Vector2.Lerp(pos, pos + movement, currentSpeed * Time.deltaTime);

        body.MovePosition(pos);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.CompareTag("BossBullet") || coll.gameObject.CompareTag("OniBoss") || coll.collider.CompareTag("Oni")) {
            ChangeHealth(-3);
            animator.SetTrigger("Hit");
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        if(coll.gameObject.CompareTag("BossBullet") || coll.gameObject.CompareTag("OniBoss") || coll.collider.CompareTag("Oni")) {
            ChangeHealth(-3);
            animator.SetTrigger("Hit");
        }
    }

    void OnCollisionExit2D(Collision2D coll) {
        
    }

    public void PlaySound(AudioClip clip, float volumeScale) {
        audioSource.PlayOneShot(clip, volumeScale);
    }

    public void ChangeHealth(float amount) {
        if(amount < 0) {
            if(isInvulerable) return;

            isInvulerable = true;
            invulnTimer = invulnTime;
        }

        currentHealth += amount;

        if(currentHealth > maxHealth) {
            maxHealth = ((currentHealth - maxHealth) + maxHealth);
        }
        
        UIHealthBar.instance.setValue(currentHealth / maxHealth);
        UIHealthBar.instance.setText(currentHealth);

        if(currentHealth <= 0) {
            if(amount != -100) {
                isDead = true;
                animator.SetBool("IsDead", true);
                movement = Vector2.zero;
            }
            SceneSwitcher s = new SceneSwitcher();
            StartCoroutine(s.EndGame(3f));
            //Destroy(gameObject);
        }
    }

    public void ChangeAttackPower(float amount) {
        currentAttackPower += amount;
    }

    public void ChangeMovementSpeed(float amount) {
        currentSpeed += amount;
    }

    void PickupYokai() {
        collectedYokai.Add(activeYokai);
        activeYokai.PickedUp();
    }

    void PopTheGlock() {
            GameObject bullet = Bullets._instance.GetPooledBullet();
            if(bullet != null) {
                bullet.SetActive(true);
                bullet.transform.position = body.position;

                Vector3 fireDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mouseDir = fireDir - gameObject.transform.position;
                mouseDir.z = 0f;
                mouseDir = mouseDir.normalized;
                bullet.GetComponent<Rigidbody2D>().AddForce(mouseDir * 600f);

                activeBullets.Add(bullet);
            }
    }

    public Vector3 GetFireRot(Vector2 dir) {
        Quaternion rot = Quaternion.identity;
        switch(dir) {
            case Vector2 v when v.Equals(Vector2.left):
                rot = Quaternion.Euler(0,0,90);
            break;

            case Vector2 v when v.Equals(Vector2.right):
                rot = Quaternion.Euler(0,0,-90);
            break;

            case Vector2 v when v.Equals(Vector2.up):
                rot = Quaternion.Euler(180,0,0);
            break;

            case Vector2 v when v.Equals(Vector2.down):
                rot = Quaternion.Euler(-180,0,0);
            break;
        }

        return rot.eulerAngles;
    }
}
