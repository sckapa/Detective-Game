using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    private Slider slider;
    [HideInInspector]
    public float duration = 10f;
    private float elapsedTime = 0f;
    private bool isPaused = false;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        slider.maxValue = duration;
        slider.value = duration;
    }

    void Update()
    {
        if (!isPaused && elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(duration, 0, elapsedTime / duration);
        }

        if (elapsedTime >= duration)
        {
            FindAnyObjectByType<ShootScript>().bullets = 0;
            FindAnyObjectByType<TransitionManager>().TimeUp();
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        slider.value = duration;
        isPaused = false;
    }

    public void PauseTimer()
    {
        isPaused = true;
    }
}