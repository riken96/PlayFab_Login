using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardPrefabScrollviewControllScript : MonoBehaviour
{
    public TextMeshProUGUI rankTxt;
    public TextMeshProUGUI userNameTxt;
    public TextMeshProUGUI scoreTxt;

    //SetValue Rank, UserName and Score on Leader Board
    public void SetValueForLeaderBoardPrefab(string rank, string name, string score)
    {
        rankTxt.text = rank;
        userNameTxt.text = name;
        scoreTxt.text = score;
    }
}
