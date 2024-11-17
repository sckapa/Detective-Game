using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void StartGame()
    {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        GameManager.level = 0;
        SceneManager.LoadScene("MainGame");
    }
}
