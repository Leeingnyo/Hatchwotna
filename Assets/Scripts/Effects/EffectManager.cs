using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {

    public GameObject[] explosionEffect;
    // public GameObject smokeEffect;

    public float elapsedTime;
    // public GameMaster gm;
    public MainLogic mainLogic;
    
    public float bossX, bossY;
    public float radiusX, radiusY;
    public float durationMultiplier;


	// Use this for initialization - Temporary
	void Start () {

        bossX = 0.0f;
        bossY = 1.2f;
        radiusX = 1.0f;
		radiusY = 1.2f;
        durationMultiplier = 0.1f;

		elapsedTime = 6.0f; // Temp

		// StartCoroutine ("EffectRotation");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void EffectGen(int skillType)
    {
        
        float posX = Random.Range(bossX - radiusX, bossX + radiusX);
        float posY = Random.Range(bossY - radiusY, bossY + radiusY);
        GameObject explosion;
        float duration = elapsedTime * durationMultiplier;

		Debug.Log ("EffectGen entered!");

        switch(skillType)
        {
            case 1:
                explosion = (GameObject)Instantiate(explosionEffect[0], new Vector3(posX, posY, 0), Quaternion.identity);
                break;
            case 2:
				explosion = (GameObject)Instantiate(explosionEffect[1], new Vector3(posX, posY, 0), Quaternion.identity);
                break;
            case 3:
				explosion = (GameObject)Instantiate(explosionEffect[2], new Vector3(posX, posY, 0), Quaternion.identity);
                break;
            case 4:
				explosion = (GameObject)Instantiate(explosionEffect[3], new Vector3(posX, posY, 0), Quaternion.identity);
                break;
            default:
                Debug.Log("Wrong SkillType number in EffectGen()");
				explosion = null;
                break;
        }

		Debug.Log ("Explosion instantiate finished!");

        // FadeOut coroutine start
        StartCoroutine(FadeEffect(explosion, duration));

        posX = Random.Range(bossX - radiusX, bossX + radiusX + 1);
        posY = Random.Range(bossY - radiusY, bossY + radiusY + 1);

        // Instantiate(smokeEffect, new Vector3(posX, posY, 0), Quaternion.identity);
    }

    IEnumerator FadeEffect(GameObject effect, float duration)
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
        Destroy(effect);
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
