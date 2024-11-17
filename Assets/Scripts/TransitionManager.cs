using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject missed;
    [SerializeField]
    private GameObject won;
    [SerializeField]
    private GameObject lost;
    [SerializeField]
    private GameObject time;

    public void Won()
    {
        StartCoroutine(LevelWon());
    }

    IEnumerator LevelWon()
    {
        won.SetActive(true);

        yield return new WaitForSeconds(2f);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        FindAnyObjectByType<GameManager>().LevelWon();
    }

    public void Missed()
    {
        StartCoroutine(MissedShot());
    }

    IEnumerator MissedShot()
    {
        missed.SetActive(true);

        yield return new WaitForSeconds(5f);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        FindAnyObjectByType<GameManager>().GameOver();
    }

    public void Lost()
    {
        StartCoroutine(LevelLost());
    }

    IEnumerator LevelLost()
    {
        lost.SetActive(true);

        yield return new WaitForSeconds(5f);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        FindAnyObjectByType<GameManager>().GameOver();
    }

    public void TimeUp()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        time.SetActive(true);

        yield return new WaitForSeconds(5f);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        FindAnyObjectByType<GameManager>().GameOver();
    }
}
