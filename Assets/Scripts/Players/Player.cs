using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int state;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        
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