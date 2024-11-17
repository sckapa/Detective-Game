using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private Animator animator;


    void Start()
    {
        if (!GameManager.audioSource.isPlaying)
        {
            GameManager.audioSource.Play();
            GameManager.audioSource.volume = 0.5f;
        }
        
        score.text = GameManager.level.ToString();
    }

    public void RestartGame()
    {
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        GameManager.level = 0;
        SceneManager.LoadScene("MainGame");
    }
}
