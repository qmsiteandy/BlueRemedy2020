using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour {

    public float smoothTime = 0.2f;
    public float smoothDelay = 0.35f;
    public Slider redSlider;
    public Slider smoothSlider;
    public Text healthPersent;

    private int healthMax = 0;
    private int healthNow;
    private float elapsed = 0f;
    private bool canSmooth = true;
    

	// Use this for initialization
	void Start () {
        healthMax = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().health;

        redSlider.value = redSlider.maxValue = healthMax;
        smoothSlider.value = smoothSlider.maxValue = healthMax;
        healthPersent.text = "100%";
    }
	
    public void SetHealthUI(int health)
    {
        healthNow = health;

        redSlider.value = healthNow;

        elapsed = 0f;
        if (canSmooth) StartCoroutine(SmoothSlider());
        canSmooth = false;

        int persent = (int)((float)healthNow / healthMax * 100);
        healthPersent.text = "" + persent + "%";
    }

    IEnumerator SmoothSlider()
    {
        while (elapsed < smoothDelay)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }
        canSmooth = true;

        while (smoothSlider.value > healthNow && canSmooth)
        {
            int num = (int)Mathf.Lerp(smoothSlider.value, healthNow, smoothTime);

            smoothSlider.value = num;

            yield return null;
        }
    }
}
