using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniBossController : MonoBehaviour
{
    public static OniBossController _instance;
    public float fireRate;
    private bool cantFire;
    private float coolDownTimer;
    private List<GameObject> activeBullets;
    void Awake() {
        _instance = this;
        fireRate = 4f;
        activeBullets = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!cantFire) {
            coolDownTimer -= Time.deltaTime;
            if(coolDownTimer < 0) {
                StartCoroutine(CleanupBullets());
                cantFire = true;
                Fire();
            }
        }
    }

    IEnumerator CleanupBullets() {
        for(int i = 0; i < activeBullets.Count; i++) {
            Bullets._instance.ReturnBulletToPool(activeBullets[i]);
            activeBullets.RemoveAt(i);
            yield return null;
        }
    }

    void Fire() {
        if(!cantFire) return;
        StartCoroutine(SpawnBullets());
        // for(int i = 0; i < 20; i++) {
        //     GameObject bullet = BossBullets._instance.GetPooledBullet();
        //     if(bullet != null) {
        //         bullet.SetActive(true);
        //         bullet.transform.position = transform.position;
        //         bullet.GetComponent<Rigidbody2D>().AddForce(RandomDirection() * 400);
        //         activeBullets.Add(bullet);
        //     }
        // }

        cantFire = false;
        coolDownTimer = fireRate;
    }

    IEnumerator SpawnBullets() {
        for(int i = 0; i < 20; i++) {
            GameObject bullet = BossBullets._instance.GetPooledBullet();
            if(bullet != null) {
                bullet.SetActive(true);
                bullet.transform.position = transform.position;
                float step = 1 * Time.deltaTime;
                Vector2 tracking = (-1 * Vector2.MoveTowards(transform.position, PlayerController._instance.playerPos, step));
                bullet.GetComponent<Rigidbody2D>().AddForce(PlayerController._instance.playerPos * 40);
                activeBullets.Add(bullet);
            }
            yield return new WaitForSeconds(.3f);
        }

        // while(true) {
        //     GameObject bullet = BossBullets._instance.GetPooledBullet();
        //     if(bullet != null) {
        //         bullet.SetActive(true);
        //         bullet.transform.position = transform.position;
        //         bullet.GetComponent<Rigidbody2D>().AddForce(RandomDirection() * 400);
        //         activeBullets.Add(bullet);
        //     }
        //     yield return new WaitForSeconds(.6f);
        // }
    }



    Vector2 RandomDirection() {
        return new Vector2(Random.Range(-1,2), Random.Range(-1,2));
    }
}


    // IEnumerator SpawnBullet() {
    //     while(true) {
    //         GameObject bullet = BossBullets._instance.GetPooledBullet();
    //         if(bullet != null) {
    //             bullet.SetActive(true);
    //             bullet.transform.position = transform.position;
    //             bullet.GetComponent<Rigidbody2D>().AddForce(RandomDirection() * 400);
    //             activeBullets.Add(bullet);
    //         }

    //         yield return new WaitForSeconds(.1f);
    //     }
    // }

    // void Update() {
    //     StartCoroutine(CleanupBullets());
    // }

    // IEnumerator CleanupBullets() {
    //     for(int i = 0; i < activeBullets.Count; i++) {
    //             Bullets._instance.ReturnBulletToPool(activeBullets[i]);
    //             activeBullets.RemoveAt(i);
    //             yield return new WaitForSeconds(5f);
    //     }
    // }