using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class LeaderBoardMenuForRecordTimesControlScript : MonoBehaviour
{
    public GameObject leaderboardScrollviewContent;
    public GameObject leaderboardEntryPrefabRef;
    public GameObject recordTimeLeaderBoardRef;

    public TextMeshProUGUI leaderboardTitelRef;

    public string statisticsNameForLoadingLeaderboard = "";


    private void OnEnable()
    {
        DestroyAllPreviousLeaderboardEnetrisOrPrefab();
        LoadGlobalRankLeaderBoard();
    }

    //Loading LeaderBoards 
    public void LoadGlobalRankLeaderBoard()
    {
        var leaderboardFachRequest = new GetLeaderboardRequest
        {
            StatisticName = statisticsNameForLoadingLeaderboard,
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

                    var statValue = playerLeaderboardEntry.StatValue;
                    var minutesInStats = statValue / 60;
                    var secondInStats = statValue % 60;

                    //Processing the rank of the Player
                    var currentPlayerPosition = leaderboardRequestList.Count - playerLeaderboardEntry.Position;

                    spwanLeaderboardEntryScript.SetValueForLeaderBoardPrefab(

                        currentPlayerPosition.ToString(),
                        playerLeaderboardEntry.DisplayName,
                        $"{minutesInStats:00} : {secondInStats:00}"
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

    public void LoadPlayerRankLeaderboard()
    {
        var leaderboardFachRequest = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = statisticsNameForLoadingLeaderboard,
            MaxResultsCount = 5,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            leaderboardFachRequest,
            request =>
            {
                var leaderboardRequestList = request.Leaderboard;
                leaderboardRequestList.Reverse();

                foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderboardRequestList)
                {
                    var spwanLeaderboardEntry = Instantiate(leaderboardEntryPrefabRef, leaderboardScrollviewContent.transform);
                    LeaderBoardPrefabScrollviewControllScript spwanLeaderboardEntryScript = spwanLeaderboardEntry.GetComponent<LeaderBoardPrefabScrollviewControllScript>();

                    var statValue = playerLeaderboardEntry.StatValue;
                    var minutesInStats = statValue / 60;
                    var secondInStats = statValue % 60;

                    //Processing the rank of the Player
                    var currentPlayerPosition = leaderboardRequestList.Count - playerLeaderboardEntry.Position;

                    spwanLeaderboardEntryScript.SetValueForLeaderBoardPrefab(

                        currentPlayerPosition.ToString(),
                        playerLeaderboardEntry.DisplayName,
                        $"{minutesInStats:00} : {secondInStats:00}"
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
