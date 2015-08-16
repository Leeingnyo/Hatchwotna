using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int state;
    public int playerNumber = 1;
    public Animator animation;
    public MainLogic mainlogic;

    // Use this for initialization
    void Start () {
        animation = GetComponent<Animator>();
        mainlogic = FindObjectOfType<MainLogic>();
    }

    // Update is called once per frame
    void Update () {
        animation.SetBool("IsHatchwotna", mainlogic.hatchWotnaState && mainlogic.targetPlayerNumber == playerNumber);
    }

    public void SetState(int state)
    {
        this.state = state;
    }

    public bool IsRest()
    {
        return this.state == (int)PlayerState.Normal;
    }
}

enum PlayerState {
    Normal = 0,
    Attack,
}