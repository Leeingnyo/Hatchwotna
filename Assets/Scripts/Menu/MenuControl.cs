using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuControl : MonoBehaviour {

    public Animator mainMenuAnim;

    public GameObject buttonSet;
    public GameObject helpMessage;

    public Text[] helpMsgs;
    public Button helpNext;
    public Button helpPrev;
    public Text pageNotation;
    public int helpPage = 0;

    public int helpPageMax = 4;

    /*
    public void MainMenuLoaded()
    {
        mainMenuAnim.SetTrigger("MainMenuLoaded");
    }
    */

    public void StartGame()
    {
        Application.LoadLevel("ingame");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void HelpMessage()
    {
        // TODO 애니메이션 Fade Out으로 구현?
        buttonSet.SetActive(false);

        helpMessage.SetActive(true);
    }

    public void HelpMsgNext()
    {
        if (helpPage < helpPageMax)
        {
            helpMsgs[helpPage++].gameObject.SetActive(false);
            helpMsgs[helpPage].gameObject.SetActive(true);
            if (helpPage == helpPageMax) helpNext.gameObject.SetActive(false);
            else if (helpPage == 1) helpPrev.gameObject.SetActive(true);
            PageNotationUpdate();
        }
    }
    public void HelpMsgPrev()
    {
        if(helpPage > 0)
        {
            helpMsgs[helpPage--].gameObject.SetActive(false);
            helpMsgs[helpPage].gameObject.SetActive(true);
            if (helpPage == 0) helpPrev.gameObject.SetActive(false);
            else if (helpPage == helpPageMax - 1) helpNext.gameObject.SetActive(true);
            PageNotationUpdate();
        }
    }

    public void HelpMsgExit()
    {
        buttonSet.SetActive(true);
        helpMessage.SetActive(false);
        helpPage = 0;
    }

    void PageNotationUpdate()
    {
        pageNotation.text = (helpPage + 1) + " / " + (helpPageMax + 1);
    }
}
