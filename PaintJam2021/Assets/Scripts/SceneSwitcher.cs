using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public GameObject howToPlayPanel;
    public GameObject closeButton;

    void Awake() {
        CloseHowToPlay();
    }
    public void SceneSwitch() {
        SceneManager.LoadScene(1);
    }

    public void PlayAgain() {
        SceneManager.LoadScene(0);
    }

    public IEnumerator EndGame(float wait) {
        Debug.Log("SHoudl Reset");
        yield return new WaitForSecondsRealtime(wait);
        YokaiController._instance.yokaiCount = 0;
        OniController._instance.oniCount = 0;
        SceneManager.LoadScene(2);
    }

    public void ShowHowToPlay() {
        howToPlayPanel.SetActive(true);
        closeButton.SetActive(true);
    }

    public void CloseHowToPlay() {
        if(howToPlayPanel != null) {
            howToPlayPanel.SetActive(false);
            closeButton.SetActive(false);
        }
    }
}
