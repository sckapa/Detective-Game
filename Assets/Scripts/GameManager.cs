using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Image description1;
    private Image description2;
    private Image description3;
    private TextMeshProUGUI descriptionText1;
    private TextMeshProUGUI descriptionText2;
    private TextMeshProUGUI descriptionText3;
    [SerializeField]
    private AudioClip backgroundMusic;
    public static AudioSource audioSource;
    public static int level;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public IEnumerator WaitForMonster()
    {
        GameObject monster = null;
        while (monster == null)
        {
            monster = GameObject.Find("Monster");
            yield return null; 
        }

        PlaceMonsterParts(monster);
    }

    void PlaceMonsterParts(GameObject monster)
    {
        List<Transform> monsterParts = new List<Transform>();
        foreach (Transform child in monster.transform)
        {
            monsterParts.Add(child);
        }

        if (monsterParts.Count >= 3)
        {
            List<Transform> selectedParts = new List<Transform>();
            HashSet<string> selectedPartNames = new HashSet<string>();

            while (selectedParts.Count < 3)
            {
                Transform part = monsterParts[Random.Range(0, monsterParts.Count)];
                string partName = ExtractPartName(part.name);
                if (!selectedPartNames.Contains(partName))
                {
                    selectedParts.Add(part);
                    selectedPartNames.Add(partName);
                }
            }

            LoadUI();

            PlacePartAboveDescription(selectedParts[0], description1, descriptionText1);
            PlacePartAboveDescription(selectedParts[1], description2, descriptionText2);
            PlacePartAboveDescription(selectedParts[2], description3, descriptionText3);
        }
    }

    void PlacePartAboveDescription(Transform part, Image description, TextMeshProUGUI descriptionText)
    {
        GameObject imageGO = new GameObject(part.name + "_Image");
        imageGO.transform.SetParent(description.transform, false);

        Image image = imageGO.AddComponent<Image>();
        SpriteRenderer renderer = part.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            image.sprite = renderer.sprite;

            RectTransform imageRect = image.GetComponent<RectTransform>();
            if (imageRect != null)
            {
                float imageHeight = description.rectTransform.rect.height;
                float spriteHeight = renderer.sprite.bounds.size.y;
                float scaleFactor = imageHeight / spriteHeight;

                float spriteWidth = renderer.sprite.bounds.size.x * scaleFactor;
                imageRect.sizeDelta = new Vector2(spriteWidth, imageHeight);
                imageRect.anchorMin = new Vector2(0.5f, 0.5f);
                imageRect.anchorMax = new Vector2(0.5f, 0.5f);
                imageRect.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        string partName = ExtractPartName(part.name);
        descriptionText.text = partName;

        if (part.name.Contains("_left") || part.name.Contains("_right"))
        {
            string mirrorName = part.name.Contains("_left") ? part.name.Replace("_left", "_right") : part.name.Replace("_right", "_left");
            Transform mirrorPart = part.parent.Find(mirrorName);
            if (mirrorPart != null)
            {
                GameObject mirrorImageGO = new GameObject(mirrorPart.name + "_Image");
                mirrorImageGO.transform.SetParent(description.transform, false);

                Image mirrorImage = mirrorImageGO.AddComponent<Image>();
                SpriteRenderer mirrorRenderer = mirrorPart.GetComponent<SpriteRenderer>();
                if (mirrorRenderer != null)
                {
                    mirrorImage.sprite = mirrorRenderer.sprite;

                    RectTransform mirrorImageRect = mirrorImage.GetComponent<RectTransform>();
                    if (mirrorImageRect != null)
                    {
                        float imageHeight = description.rectTransform.rect.height;
                        float spriteHeight = mirrorRenderer.sprite.bounds.size.y;
                        float scaleFactor = imageHeight / spriteHeight;

                        float spriteWidth = mirrorRenderer.sprite.bounds.size.x * scaleFactor;
                        mirrorImageRect.sizeDelta = new Vector2(spriteWidth, imageHeight);
                        mirrorImageRect.anchorMin = new Vector2(0.5f, 0.5f);
                        mirrorImageRect.anchorMax = new Vector2(0.5f, 0.5f);
                        mirrorImageRect.pivot = new Vector2(0.5f, 0.5f);
                    }
                }
            }
        }
    }

    string ExtractPartName(string fullName)
    {
        string[] parts = fullName.Split('_');
        if (parts.Length > 1)
        {
            return parts[0];
        }
        return fullName;
    }

    void LoadUI()
    {
        Transform transform = GameObject.Find("Canvas").transform;
        description1 = FindInChildren(transform, "Description1").GetComponent<Image>();
        description2 = FindInChildren(transform, "Description2").GetComponent<Image>();
        description3 = FindInChildren(transform, "Description3").GetComponent<Image>();
        descriptionText1 = FindInChildren(transform, "DescriptionText1").GetComponent<TextMeshProUGUI>();
        descriptionText2 = FindInChildren(transform, "DescriptionText2").GetComponent<TextMeshProUGUI>();
        descriptionText3 = FindInChildren(transform, "DescriptionText3").GetComponent<TextMeshProUGUI>();
    }

    GameObject FindInChildren(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
            GameObject result = FindInChildren(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public void LevelWon()
    {
        level++;
        SceneManager.LoadScene("MainGame");
    }

    public void GameOver()
    {

        SceneManager.LoadScene("GameOver");
    }
}