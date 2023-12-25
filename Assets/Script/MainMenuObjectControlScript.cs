using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuObjectControlScript : MonoBehaviour
{

    public GameObject mainMenuReference;
    public GameObject leaderBoard;
    public GameObject leaderboardSelectionRef;
    public GameObject winsLeaderboaderLevelSelectionRef;
    public GameObject recordTimeLeaderboardRef;

    public LeaderBoardMenuForRecordTimesControlScript leaderBoardMenuForRecordTimesControlScript;

    // Wins Record Leaderboard Selection function
    public void OnClickWinsLeaderboardSelectionMenuButton()
    {
        mainMenuReference.SetActive(false);
        leaderboardSelectionRef.SetActive(true);
    }

    public void OnLeaderboardBackButton()
    {
        leaderBoard.SetActive(false);
        leaderboardSelectionRef.SetActive(true);
    }

    public void OnLeaderBoardSelectionButton()
    {
        leaderboardSelectionRef.SetActive(false);
        leaderBoard.SetActive(true);
    }

    public void OnLeaderboardSelectionBackButton()
    {
        mainMenuReference.SetActive(true);
        leaderboardSelectionRef.SetActive(false);
    }

    //Record Time Leaderboard Level Selection Function
    public void OnLeaderboardSelectionRecordTimeButton()
    {
        winsLeaderboaderLevelSelectionRef.SetActive(true);
        leaderboardSelectionRef.SetActive(false);
    }

    public void OnLeaderboardSelectionRecordTimeBackButton()
    {
        winsLeaderboaderLevelSelectionRef.SetActive(false);
        leaderboardSelectionRef.SetActive(true);
    }

    public void OnRecordTimeLeaderboardLevelOneButtonClick()
    {
        leaderBoardMenuForRecordTimesControlScript.statisticsNameForLoadingLeaderboard = "Level 1 Record Times";
        winsLeaderboaderLevelSelectionRef.SetActive(false);
        recordTimeLeaderboardRef.SetActive(true);

    }

    public void OnRecordTimeLeaderboardLevelTwoButtonClick()
    {

        leaderBoardMenuForRecordTimesControlScript.statisticsNameForLoadingLeaderboard = "Level 2 Record Times";
        winsLeaderboaderLevelSelectionRef.SetActive(false);
        recordTimeLeaderboardRef.SetActive(true);
    }

    //Record Time Menu Function
    public void OnRecordTimeLeaderboardMenuBackButtonClick()
    {
        recordTimeLeaderboardRef.SetActive(false);
        winsLeaderboaderLevelSelectionRef.SetActive(true);
    }
}
