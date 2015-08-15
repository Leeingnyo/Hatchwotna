using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour {

    public GameObject[] explosionEffectP1;
    public GameObject[] explosionEffectP2;
    public GameObject[] smokeEffect;

    public GameObject smokeParent;

    // public float elapsedTime;
    public MainLogic mainLogic;
    
    public float bossX, bossY;
    public float radiusX, radiusY;
    public float durationMultiplier;

    public int smokeRegenNum;


	// Use this for initialization - Temporary
	void Start () {

        bossX = 0.0f;
        bossY = 1.2f;
        radiusX = 1.0f;
		radiusY = 1.2f;
        durationMultiplier = 0.1f;

		// elapsedTime = 6.0f; // Temp

		// StartCoroutine ("EffectRotation");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void EffectGen(int skillType)
    {
        float posX, posY;
        List<GameObject> explosions = new List<GameObject>();
        float duration = 0.5f /* mainLogic.timerSum * durationMultiplier */;

		Debug.Log ("EffectGen entered!");

        switch(skillType)
        {
            case 11:
            case 12:
            case 13:
            case 14:
                for (int i = 0; i < 3; i++)
                {
                    SetRandPosition(out posX, out posY);
                    explosions.Add((GameObject)Instantiate(explosionEffectP1[i], new Vector3(posX, posY, 0), Quaternion.identity));
                }
                break;
            case 21:
            case 22:
            case 23:
            case 24:
                SetRandPosition(out posX, out posY);
                explosions.Add((GameObject)Instantiate(explosionEffectP2[Random.Range(0, 3)], new Vector3(posX, posY, 0), Quaternion.identity));
                break;
            default:
                Debug.Log("Wrong SkillType number in EffectGen()");
				explosions = null;
                break;
        }

		Debug.Log ("Explosion instantiate finished!");

        // FadeOut coroutine start
        foreach (GameObject explosion in explosions)
        {
            StartCoroutine(FadeEffect(explosion, duration, true));
        }

        SetRandPosition(out posX, out posY);

        GameObject smoke = (GameObject) Instantiate(smokeEffect[Random.Range(0, 3)], new Vector3(posX, posY, 0), Quaternion.identity);
        smoke.transform.SetParent(smokeParent.transform);
    }

    void SetRandPosition(out float posX, out float posY)
    {
        posX = Random.Range(bossX - radiusX, bossX + radiusX + 1);
        posY = Random.Range(bossY - radiusY, bossY + radiusY + 1);
    }

    public void HideSmokes()
    {
        // smokeParent.SetActive(false);
        foreach (Transform smokeTf in smokeParent.transform)
        {
            StartCoroutine(FadeEffect(smokeTf.gameObject, 1.0f, true));
        }
    }

    public void ShowSmokes()
    {
        for(int i = 0; i < smokeRegenNum; i++)
        {
            float posX = Random.Range(bossX - radiusX, bossX + radiusX + 1);
            float posY = Random.Range(bossY - radiusY, bossY + radiusY + 1);

            GameObject smoke = (GameObject)Instantiate(smokeEffect[Random.Range(0, 3)], new Vector3(posX, posY, 0), Quaternion.identity);
            smoke.transform.SetParent(smokeParent.transform);
        }
    }

    IEnumerator FadeEffect(GameObject effect, float duration, bool destroyOnEnd)
    {
        Color colorStart = effect.GetComponent<Renderer>().material.color;
        Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0.0f);

        for(float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            effect.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, t / duration);
            yield return null;
        }
        effect.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, 1);
        yield return null;

        if(destroyOnEnd) Destroy(effect);
    }

	// For Test
	IEnumerator EffectRotation()
	{
		int index = 0;
		while (true) 
		{
			EffectGen(index % 4 + 1);
			index += 1;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
