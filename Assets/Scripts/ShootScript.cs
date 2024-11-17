using System.Collections;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip winMusic;
    [SerializeField]
    private AudioClip loseMusic;
    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private TimerBar timerBar;
    private AudioSource audioSource; 

    private RectTransform crosshair;
    public int bullets = 1;

    void Awake()
    {
        if (crosshair == null)
        {
            crosshair = GameObject.Find("Crosshair").GetComponent<RectTransform>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (crosshair != null)
        {
            crosshair.position = mousePosition;
        }
        else
        {
            Debug.Log("Crosshair not found");
        }

        if (bullets > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                timerBar.PauseTimer();

                GameManager.audioSource.Pause();

                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null && hit.collider.gameObject.name == "Monster")
                {
                    StartCoroutine(WaitBeforeCallingGameManager(1));
                }
                else if(hit.collider != null && hit.collider.gameObject.name == "Person")
                {
                    StartCoroutine(WaitBeforeCallingGameManager(2));
                }
                else
                {
                    StartCoroutine(WaitBeforeCallingGameManager(3));
                }
                bullets--;
            }
        }
    }

    IEnumerator WaitBeforeCallingGameManager(int outcome)
    {
        // 1 = monster
        // 2 = person
        // 3 = missed

        audioSource.clip = shootSound;
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        if (outcome==1)
        {
            audioSource.clip = winMusic;
            audioSource.Play();

            FindAnyObjectByType<TransitionManager>().Won();
        }
        else if(outcome==2)
        {
            audioSource.clip = loseMusic;
            audioSource.Play();

            FindAnyObjectByType<TransitionManager>().Lost();
        }
        else
        {
            audioSource.clip = loseMusic;
            audioSource.Play();

            FindAnyObjectByType<TransitionManager>().Missed();
        }
    }
}