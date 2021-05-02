using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    public Image mask;
    public GameObject healthText;
    float orignalSize;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        orignalSize = mask.rectTransform.rect.width;
        TextMeshProUGUI healthValue = healthText.GetComponent<TextMeshProUGUI>();
        if (healthValue != null) {
            healthValue.SetText("10");
        }
    }

    // Update is called once per frame
    public void setValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, orignalSize * value);
    }

    public void setText(float value) {
        TextMeshProUGUI healthValue = healthText.GetComponent<TextMeshProUGUI>();
        if (healthValue != null) {
            string maxHealth = value.ToString();
            healthValue.SetText(maxHealth);
        }
    }
}
