using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

    float tic;

	// Use this for initialization
	void Start () {
        tic = 0;
	}
	
	// Update is called once per frame
	void Update () {
        tic += Time.deltaTime;
        if (tic > 5f)
        {
            if (Input.GetMouseButton(0))
            {
                Application.LoadLevel(0);
                //FIXME 나중에 메인으로 두기
            }
        }
	}
}
