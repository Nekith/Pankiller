using UnityEngine;
using System.Collections;

public class BlinkingSpotlight : MonoBehaviour
{
	private Light bulb;
	private float timer;
	private float duration;
	private int flicks;

	void Start()
	{
        bulb = GetComponent<Light>();
		duration = Random.Range(2.5f, 5.0f);
		flicks = 3;
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= duration) {
			if (flicks > 0) {
				duration = 0.1f;
				timer = 0f;
				if (flicks % 2 == 1) {
                    bulb.intensity = 0f;
				} else {
                    bulb.intensity = 8f;
				}
				flicks -= 1;
			} else {
				duration = Random.Range(3.0f, 7.0f);
				flicks = 3;
				timer = 0f;
                bulb.intensity = 8f;
			}
		}
	}
}
