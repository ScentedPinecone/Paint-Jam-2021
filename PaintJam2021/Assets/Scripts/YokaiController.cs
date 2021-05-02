using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiController : MonoBehaviour
{
    public static YokaiController _instance;
    public float attackBonus = 0f;
    public float healthBonus = 0f;
    public float speedBonus = 0f;

    // Sound for picking up Oni
    public AudioClip soundClip;
    public float volume = 1f;
    public int yokaiCount {
        get {
            return currentYokaiCount;
        }
        set {
            currentYokaiCount = value;
        }
    }
    private static int currentYokaiCount;
    private PlayerController player;

    void Awake() {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentYokaiCount++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnTriggerEnter2D(Collider2D coll) {
    //    if(coll.CompareTag("Player")) {
    //        ChatBubble chatBbl = gameObject.GetComponent<ChatBubble>();
    //        if(chatBbl != null) {
    //            chatBbl.DisplayDialog("Yoooo (kai)!");
    //        }
    //        player = coll.gameObject.GetComponent<PlayerController>();
    //    }
    //}

    public string randDialog() {

        // An array of dialogues
        string[] dialog = {
            "Tummy rumbling...", 
            "This is just souper.", 
            "Souperman has come to save us!", 
            "40 cans down",
            "I'm gonna pea soup...",
            "I am become borscht,",
            "the slurper of soups..",
            "Pbbbbttt",
            "Ohhh...",
            "Please...",
            "Help me..."
        };

        // Create a Random number
        System.Random rand = new System.Random();

        // Generate a random index less than the size of the array
        int index = rand.Next(dialog.Length);

        // Return the result
        string pickDialog = dialog[index];
        return pickDialog;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.collider.CompareTag("Player")) {
            // collides with player
        }
        else if(coll.collider.name.Contains("Bullet")) {
            Bullets._instance.ReturnBulletToPool(coll.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Player")) {
            ChatBubble chatBbl = gameObject.GetComponent<ChatBubble>();
            if (chatBbl != null) {
                string rand = randDialog();
                chatBbl.DisplayDialog(rand);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if(coll.CompareTag("Player")) {
            ChatBubble chatBbl = gameObject.GetComponent<ChatBubble>();
            if(chatBbl != null) {
                chatBbl.HideDialog("Oh, okay");
            }
        }
    }

    public void PickedUp() {
        PlayerController._instance.ChangeAttackPower(attackBonus);
        PlayerController._instance.ChangeHealth(healthBonus);
        PlayerController._instance.ChangeMovementSpeed(speedBonus);
        --currentYokaiCount;
        PlayerController._instance.PlaySound(soundClip, volume);
        gameObject.SetActive(false);
    }
}
