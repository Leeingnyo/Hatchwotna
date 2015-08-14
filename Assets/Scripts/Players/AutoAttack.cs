using UnityEngine;
using System.Collections;

public class AutoAttack : MonoBehaviour {

    Player player;
    float autoAttackCooltime = 5f;
    float restTic;
    public int damage = 10;
    int count = 0;

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
    }
	
    // Update is called once per frame
    void Update () {
        if (player.IsRest())
        {
            restTic += Time.deltaTime;
            if (restTic > autoAttackCooltime)
            {
                FindObjectOfType<MainLogic>().DamageHp(damage, player.playerNumber);
                //FIXME 데미지 계산
                //TODO Effect is required
                Debug.Log((player.transform.position.x < 0 ? "플레이어1" : "플레이어2") + " 자동 공격");
                count++;
                restTic = 0;
            }
        }
        else
        {
            restTic = 0;
            count = 0;
        }
    }
}
