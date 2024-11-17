using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class MonsterGenerator : MonoBehaviour
{
    [SerializeField]
    private TextAsset spritesheetXML;
    [SerializeField]
    private Sprite[] spritesheet;
    private Dictionary<string, Rect> parts;
    private Rigidbody2D monsterRigidbody;
    [SerializeField]
    private Transform[] spawnLocations;

    void Start()
    {
        LoadParts();

        int randomIndex = Random.Range(0, spawnLocations.Length);
        GenerateMonster("Monster", spawnLocations[randomIndex].position);

        int j = Random.Range(3, 5);
        for (int i = 0; i < j; i++)
        {
            if(i==randomIndex)
            {
                j++;
                continue;
            }
            GenerateMonster("Person", spawnLocations[i].position);
        }
    }

    void LoadParts()
    {
        parts = new Dictionary<string, Rect>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(spritesheetXML.text);
        XmlNodeList subTextures = xmlDoc.GetElementsByTagName("SubTexture");

        foreach (XmlNode subTexture in subTextures)
        {
            string name = subTexture.Attributes["name"].Value;
            float x = float.Parse(subTexture.Attributes["x"].Value);
            float y = float.Parse(subTexture.Attributes["y"].Value);
            float width = float.Parse(subTexture.Attributes["width"].Value);
            float height = float.Parse(subTexture.Attributes["height"].Value);
            parts[name] = new Rect(x, y, width, height);
        }
    }

    void GenerateMonster(string name, Vector3 spawnLocation)
    {
        string[] bodyParts = { "body" };
        string[] pairedParts = { "arm", "leg" };
        string[] facialParts = { "eye", "eyebrow", "mouth", "nose" };
        GameObject bodyGO = null;

        GameObject monsterGO = new GameObject(name);

        foreach (string part in bodyParts)
        {
            List<string> availableParts = new List<string>();
            foreach (string key in parts.Keys)
            {
                if (key.StartsWith(part))
                {
                    availableParts.Add(key);
                }
            }
            if (availableParts.Count == 0)
            {
                Debug.LogWarning("No available parts for: " + part);
                continue;
            }
            string selectedPart = availableParts[Random.Range(0, availableParts.Count)];
            bodyGO = new GameObject(selectedPart);
            SpriteRenderer renderer = bodyGO.AddComponent<SpriteRenderer>();
            renderer.sprite = GetSprite(selectedPart);
            bodyGO.transform.SetParent(monsterGO.transform);
        }

        if (bodyGO == null)
        {
            Debug.LogError("No body part found, cannot generate monster.");
            return;
        }

        foreach (string part in pairedParts)
        {
            List<string> availableParts = new List<string>();
            foreach (string key in parts.Keys)
            {
                if (key.StartsWith(part))
                {
                    availableParts.Add(key);
                }
            }
            if (availableParts.Count == 0)
            {
                Debug.LogWarning("No available parts for: " + part);
                continue;
            }
            string selectedPart = availableParts[Random.Range(0, availableParts.Count)];

            GameObject leftPartGO = new GameObject(selectedPart + "_left");
            SpriteRenderer leftRenderer = leftPartGO.AddComponent<SpriteRenderer>();
            leftRenderer.sprite = GetSprite(selectedPart);
            leftPartGO.transform.SetParent(monsterGO.transform);

            GameObject rightPartGO = new GameObject(selectedPart + "_right");
            SpriteRenderer rightRenderer = rightPartGO.AddComponent<SpriteRenderer>();
            rightRenderer.sprite = GetSprite(selectedPart);
            rightRenderer.flipX = true;
            rightPartGO.transform.SetParent(monsterGO.transform);

            if (part == "leg")
            {
                float legSpacing = 0.5f;
                rightPartGO.transform.position = bodyGO.transform.position + new Vector3(-legSpacing, -bodyGO.GetComponent<SpriteRenderer>().bounds.size.y / 2 - rightRenderer.bounds.size.y / 2, 0);
                leftPartGO.transform.position = bodyGO.transform.position + new Vector3(legSpacing, -bodyGO.GetComponent<SpriteRenderer>().bounds.size.y / 2 - leftRenderer.bounds.size.y / 2, 0);
            }
            else if (part == "arm")
            {
                float armHeight = bodyGO.GetComponent<SpriteRenderer>().bounds.size.y / 4;
                rightPartGO.transform.position = bodyGO.transform.position + new Vector3(-bodyGO.GetComponent<SpriteRenderer>().bounds.size.x / 2 - rightRenderer.bounds.size.x / 2, armHeight, 0);
                leftPartGO.transform.position = bodyGO.transform.position + new Vector3(bodyGO.GetComponent<SpriteRenderer>().bounds.size.x / 2 + leftRenderer.bounds.size.x / 2, armHeight, 0);
            }
        }

        GameObject noseGO = null;
        GameObject leftEyeGO = null;
        GameObject rightEyeGO = null;
        GameObject leftEyebrowGO = null;
        GameObject rightEyebrowGO = null;

        foreach (string part in facialParts)
        {
            List<string> availableParts = new List<string>();
            foreach (string key in parts.Keys)
            {
                if (key.StartsWith(part))
                {
                    availableParts.Add(key);
                }
            }
            if (availableParts.Count == 0)
            {
                Debug.LogWarning("No available parts for: " + part);
                continue;
            }
            string selectedPart = availableParts[Random.Range(0, availableParts.Count)];

            if (part == "eye")
            {
                leftEyeGO = new GameObject(selectedPart + "_left");
                SpriteRenderer leftRenderer = leftEyeGO.AddComponent<SpriteRenderer>();
                leftRenderer.sprite = GetSprite(selectedPart);
                leftEyeGO.transform.SetParent(monsterGO.transform);

                rightEyeGO = new GameObject(selectedPart + "_right");
                SpriteRenderer rightRenderer = rightEyeGO.AddComponent<SpriteRenderer>();
                rightRenderer.sprite = GetSprite(selectedPart);
                rightRenderer.flipX = true; 
                rightEyeGO.transform.SetParent(monsterGO.transform);
            }
            else if (part == "eyebrow")
            {
                leftEyebrowGO = new GameObject(selectedPart + "_left");
                SpriteRenderer leftRenderer = leftEyebrowGO.AddComponent<SpriteRenderer>();
                leftRenderer.sprite = GetSprite(selectedPart);
                leftEyebrowGO.transform.SetParent(monsterGO.transform);

                rightEyebrowGO = new GameObject(selectedPart + "_right");
                SpriteRenderer rightRenderer = rightEyebrowGO.AddComponent<SpriteRenderer>();
                rightRenderer.sprite = GetSprite(selectedPart);
                rightRenderer.flipX = true;
                rightEyebrowGO.transform.SetParent(monsterGO.transform);
            }
            else
            {
                GameObject facialPartGO = new GameObject(selectedPart);
                SpriteRenderer renderer = facialPartGO.AddComponent<SpriteRenderer>();
                renderer.sprite = GetSprite(selectedPart);
                facialPartGO.transform.SetParent(monsterGO.transform);

                float bodyHeight = bodyGO.GetComponent<SpriteRenderer>().bounds.size.y;
                if (part == "mouth")
                {
                    facialPartGO.transform.position = bodyGO.transform.position + new Vector3(0, bodyHeight / 4, 0);
                }
                else if (part == "nose")
                {
                    noseGO = facialPartGO;
                    facialPartGO.transform.position = bodyGO.transform.position + new Vector3(0, bodyHeight / 2, 0);
                }
            }
        }

        if (noseGO != null)
        {
            float eyeSpacing = noseGO.GetComponent<SpriteRenderer>().bounds.size.x;
            float eyeHeight = noseGO.GetComponent<SpriteRenderer>().bounds.size.y;

            if (leftEyeGO != null)
            {
                leftEyeGO.transform.position = noseGO.transform.position + new Vector3(-eyeSpacing, eyeHeight, 0);
            }
            if (rightEyeGO != null)
            {
                rightEyeGO.transform.position = noseGO.transform.position + new Vector3(eyeSpacing, eyeHeight, 0);
            }
            if (leftEyebrowGO != null)
            {
                leftEyebrowGO.transform.position = noseGO.transform.position + new Vector3(-eyeSpacing, eyeHeight + leftEyebrowGO.GetComponent<SpriteRenderer>().bounds.size.y, 0);
            }
            if (rightEyebrowGO != null)
            {
                rightEyebrowGO.transform.position = noseGO.transform.position + new Vector3(eyeSpacing, eyeHeight + rightEyebrowGO.GetComponent<SpriteRenderer>().bounds.size.y, 0);
            }
        }

        monsterGO.transform.position = spawnLocation;

        BoxCollider2D collider = monsterGO.AddComponent<BoxCollider2D>();
        Bounds bounds = new Bounds(monsterGO.transform.position, Vector3.zero);
        foreach (SpriteRenderer renderer in monsterGO.GetComponentsInChildren<SpriteRenderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        collider.size = bounds.size;
        collider.offset = bounds.center - monsterGO.transform.position;

        monsterGO.transform.localScale = new Vector3(1 / 3f, 1 / 3f, 1 / 3f);
        monsterRigidbody = monsterGO.AddComponent<Rigidbody2D>();
        monsterRigidbody.gravityScale = 0;
        monsterRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(ApplyRandomForces(monsterRigidbody));
    }

    IEnumerator ApplyRandomForces(Rigidbody2D monsterRigidbody)
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Vector2 randomForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(1f, 3f);
            monsterRigidbody.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }

    Sprite GetSprite(string name)
    {
        if (!parts.ContainsKey(name))
        {
            Debug.LogError("Part not found: " + name);
            return null;
        }

        Rect rect = parts[name];
        rect.y = spritesheet[0].texture.height - rect.y - rect.height;
        return Sprite.Create(spritesheet[0].texture, rect, new Vector2(0.5f, 0.5f));
    }
}