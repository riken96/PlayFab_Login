using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class LeaderBoardsMenuControllesScript : MonoBehaviour
{

    public GameObject leaderboardScrollviewContent;
    public GameObject leaderboardEntryPrefabRef;
    public GameObject leaderboardSelctionMenuRef;
    public GameObject leaderboardRef;

    public TextMeshProUGUI leaderboardTitelRef;

    private void OnEnable()
    {
        DestroyAllPreviousLeaderboardEnetrisOrPrefab();
        LoadGlobalRankLeaderBoard();
    }

    public void OnClickGlobleRankButton()
    {
        DestroyAllPreviousLeaderboardEnetrisOrPrefab();
        LoadGlobalRankLeaderBoard();
    }

    //Loading LeaderBoards 
    public void LoadGlobalRankLeaderBoard()
    {
        var leaderboardFachRequest = new GetLeaderboardRequest
        {
            StatisticName = "Wins Record",
            MaxResultsCount = 10,
        };

        PlayFabClientAPI.GetLeaderboard(
            leaderboardFachRequest,
            request =>
            {
                var leaderboardRequestList = request.Leaderboard;

                foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderboardRequestList)
                {
                    var spwanLeaderboardEntry = Instantiate(leaderboardEntryPrefabRef, leaderboardScrollviewContent.transform);
                    LeaderBoardPrefabScrollviewControllScript spwanLeaderboardEntryScript = spwanLeaderboardEntry.GetComponent<LeaderBoardPrefabScrollviewControllScript>();

                    //Processing the rank of the Player
                    var currentPlayerPosition = leaderboardRequestList.Count - playerLeaderboardEntry.Position;

                    spwanLeaderboardEntryScript.SetValueForLeaderBoardPrefab(

                        currentPlayerPosition.ToString(),
                        playerLeaderboardEntry.DisplayName,
                        playerLeaderboardEntry.StatValue.ToString()

                   );
                }

                leaderboardTitelRef.text = "Displaying Global Rank";
            },
            errorCallback =>
            {
                leaderboardTitelRef.text = "Global Leaderboard faild to load";
                Debug.Log(errorCallback.ErrorMessage);
            }
        );
    }

    public void DestroyAllPreviousLeaderboardEnetrisOrPrefab()
    {
        if (leaderboardScrollviewContent.transform.childCount == 0) return;

        for (int i = 0; i < leaderboardScrollviewContent.transform.childCount; i++)
        {
            Destroy(leaderboardScrollviewContent.transform.GetChild(i).gameObject);
        }
    }


    public void OnLeaderBoardSelectionBackButton()
    {
        leaderboardSelctionMenuRef.SetActive(true);
        leaderboardRef.SetActive(false);
    }

    public void OnClickPlayerRankButton()
    {
        DestroyAllPreviousLeaderboardEnetrisOrPrefab();
        LoadPlayerRankLeaderboard();
    }

    public void LoadPlayerRankLeaderboard()
    {
        var leaderboardFachRequest = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = "Wins Record",
            MaxResultsCount = 5,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            leaderboardFachRequest,
            request =>
            {
                var leaderboardRequestList = request.Leaderboard;

                foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderboardRequestList)
                {
                    var spwanLeaderboardEntry = Instantiate(leaderboardEntryPrefabRef, leaderboardScrollviewContent.transform);
                    LeaderBoardPrefabScrollviewControllScript spwanLeaderboardEntryScript = spwanLeaderboardEntry.GetComponent<LeaderBoardPrefabScrollviewControllScript>();

                    //Processing the rank of the Player
                    var currentPlayerPosition = leaderboardRequestList.Count - playerLeaderboardEntry.Position;

                    spwanLeaderboardEntryScript.SetValueForLeaderBoardPrefab(

                        currentPlayerPosition.ToString(),
                        playerLeaderboardEntry.DisplayName,
                        playerLeaderboardEntry.StatValue.ToString()
                   );
                }

                leaderboardTitelRef.text = "Displaying Global Rank";
            },
            errorCallback =>
            {
                leaderboardTitelRef.text = "Global Leaderboard faild to load";
                Debug.Log(errorCallback.ErrorMessage);
            }
        );
    }
}
