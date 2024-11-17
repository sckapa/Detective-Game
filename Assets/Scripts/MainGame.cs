using TMPro;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score;

    void Start()
    {
        if (!GameManager.audioSource.isPlaying)
        {
            GameManager.audioSource.Play();
            GameManager.audioSource.volume = 0.5f;
        }

        score.text = GameManager.level.ToString();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(gameManager.WaitForMonster());
    }
}
