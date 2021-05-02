using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public GameObject oniBossPrefab;
    public Animator anim;
    [SerializeField] private bool completedLevel = false;
    private int yokaiCount;
    private int oniCount;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!completedLevel) {
            yokaiCount = YokaiController._instance.yokaiCount;
            oniCount = OniController._instance.oniCount;
            Debug.Log($"Yokai: {yokaiCount} | Oni: {oniCount}");

            if(yokaiCount <= 0 && oniCount <= 0) {
                completedLevel = true;
                anim.SetTrigger("ZoomIn");
                Instantiate(oniBossPrefab, OniController._instance.RandomDirection(), Quaternion.identity);
            }
        }

    }
}
