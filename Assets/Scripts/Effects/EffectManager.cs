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
    // public float smokeAlphaMultiplier;

    public int smokeRegenNum;

    bool isSmokeFading = false;


	// Use this for initialization - Temporary
	void Start () {

        /*
        bossX = 0.0f;
        bossY = 1.2f;
        radiusX = 1.0f;
		radiusY = 1.2f;
        durationMultiplier = 0.1f;
        */

		// elapsedTime = 6.0f; // Temp

		// StartCoroutine ("EffectRotation");
	}
	
	// Update is called once per frame
	void Update () {
        /*
        float alphaAdditional = mainLogic.timerSum * smokeAlphaMultiplier;
        foreach(GameObject smoke in smokeEffect)
        {
            Color origColor = smoke.GetComponent<Renderer>().material.color;

            smoke.GetComponent<Renderer>().material.color = 
                new Color(origColor.r, origColor.g, origColor.b, Mathf.Clamp(origColor.a + alphaAdditional, 0f, 1f));
        }
         */
	
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

        // float alphaAdditional = mainLogic.timerSum * smokeAlphaMultiplier;
 
        SetRandPosition(out posX, out posY);
        
        GameObject smoke = (GameObject) Instantiate(smokeEffect[Random.Range(0, 3)], new Vector3(posX, posY, 0), Quaternion.identity);
        
        /*
        // Smoke alpha (transparency) increse
        Color origColor = smoke.GetComponent<Renderer>().material.color;
        Debug.Log("smoke alpha : " + ((Color32)origColor).a);
        smoke.GetComponent<Renderer>().material.color = 
            new Color(origColor.r, origColor.g, origColor.b, Mathf.Clamp(0.2f + alphaAdditional, 0f, 1f));
        */

        // Smoke initial alpha (transparency) setting
        Color origColor = smoke.GetComponent<Renderer>().material.color;
        smoke.GetComponent<Renderer>().material.color = new Color(origColor.r, origColor.g, origColor.b, 0.2f);

        smoke.transform.SetParent(smokeParent.transform);
        StartCoroutine(SmokeThicker(smoke, 8.0f));
    }

    void SetRandPosition(out float posX, out float posY)
    {
        posX = Random.Range(bossX - radiusX, bossX + radiusX + 1);
        posY = Random.Range(bossY - radiusY, bossY + radiusY + 1);
    }

    public void HideSmokes()
    {
        isSmokeFading = true;
        // smokeParent.SetActive(false);
        foreach (Transform smokeTf in smokeParent.transform)
        {
            StartCoroutine(FadeEffect(smokeTf.gameObject, 2.5f, true));
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

        if (effect.name.Contains("smoke")) isSmokeFading = false;

        if(destroyOnEnd) Destroy(effect);
    }

    IEnumerator SmokeThicker(GameObject smoke, float duration)
    {
        Color colorStart = smoke.GetComponent<Renderer>().material.color;
        Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 1.0f);

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            if (isSmokeFading) yield break;
            smoke.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, t / duration);
            yield return null;
        }
        smoke.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, 1);
        yield return null;
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
