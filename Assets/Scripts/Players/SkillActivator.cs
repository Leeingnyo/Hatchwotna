using UnityEngine;
using System.Collections;

public class SkillActivator : MonoBehaviour {

    public KeyCode skill01;
    public KeyCode skill02;
    public KeyCode skill03;
    public KeyCode skill04;

    public float cooldownTime = 0.1f;
    float tic = 0;
    bool isSkillActivated = false;

    Player player;

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
        if (true && !isSkillActivated)
            // FIXME true는 해치웠나 페이즈인지 확인하기
        {
            if (Input.GetKey(skill01))
            {
                BaseAttack();
                Debug.Log((player.transform.position.x < 0 ? "플레이어1" : "플레이어2") + " 스킬 1 발동");
                FindObjectOfType<MainLogic>().DamageHp(2, player.playerNumber);
                FindObjectOfType<EffectManager>().EffectGen(1);
            }
            else if (Input.GetKey(skill02))
            {
                BaseAttack();
                Debug.Log((player.transform.position.x < 0 ? "플레이어1" : "플레이어2") + " 스킬 2 발동");
                FindObjectOfType<MainLogic>().DamageHp(3, player.playerNumber);
                FindObjectOfType<EffectManager>().EffectGen(2);
            }
            else if (Input.GetKey(skill03))
            {
                BaseAttack();
                Debug.Log((player.transform.position.x < 0 ? "플레이어1" : "플레이어2") + " 스킬 3 발동");
                FindObjectOfType<MainLogic>().DamageHp(6, player.playerNumber);
                FindObjectOfType<EffectManager>().EffectGen(3);
            }
            else if (Input.GetKey(skill04))
            {
                BaseAttack();
                Debug.Log((player.transform.position.x < 0 ? "플레이어1" : "플레이어2") + " 스킬 4 발동");
                FindObjectOfType<MainLogic>().DamageHp(10, player.playerNumber);
                FindObjectOfType<EffectManager>().EffectGen(4);
            }
        }
        else if (isSkillActivated)
        {
            tic += Time.deltaTime;
            if (tic > cooldownTime) {
                tic = 0;
                isSkillActivated = false;
                player.SetState((int)PlayerState.Normal);
                player.animation.SetInteger("State", (int)PlayerState.Normal);
            }
        }
    }

    void BaseAttack()
    {
        isSkillActivated = true;
        player.SetState((int)PlayerState.Attack);
        player.animation.SetInteger("State", (int)PlayerState.Attack);
    }
}
