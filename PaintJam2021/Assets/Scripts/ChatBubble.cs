using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour {

    public GameObject dialogBox;
    private Animator animator; // initialzed in DisplayDialog();

    private void Start() {
        dialogBox.SetActive(false);
    }

    private void Update() {

    }

    public void DisplayDialog(string text) {
        dialogBox.SetActive(true);
        TextMeshProUGUI chatText = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        if(chatText != null) {
            chatText.SetText(text);
        }
        animator = gameObject.GetComponentInChildren<Animator>();
        animator.SetTrigger("DisplayChat");
    }

    public void HideDialog(string text) {
        animator.SetTrigger("HideChat");
        TextMeshProUGUI chatText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if(chatText != null) {
            chatText.SetText(text);
        }
        Invoke("SetActiveFalse", 1);
    }

    // Need this method in order to call Invoke() in HideDialog()
    public void SetActiveFalse() {
        dialogBox.SetActive(false);
    }
}
