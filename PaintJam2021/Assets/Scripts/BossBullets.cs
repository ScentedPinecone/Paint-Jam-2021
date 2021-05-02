using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullets : MonoBehaviour
{
    public static BossBullets _instance;
    public GameObject bulletPrefab;
    public List<GameObject> pooledBullets;
    public int bulletCount;

    void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pooledBullets = new List<GameObject>();
        GameObject t;
        for(int i = 0; i < bulletCount; i++) {
            t = Instantiate(bulletPrefab);
            t.SetActive(false);
            pooledBullets.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject GetPooledBullet() {
        for(int i = 0; i < bulletCount; i++) {
            if(!pooledBullets[i].activeInHierarchy) {
                return pooledBullets[i];
            }
        }

        return null;
    }

    public void ReturnBulletToPool(GameObject bullet) {
        bullet.SetActive(false);
    }  
}
