using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//eu-central-1:82914392-8310-4b1e-b102-97613bc74f7d
public class ScoreDBModel : MonoBehaviour{

    static private List<ScoreDBEntity> scoreDBEntities = new List<ScoreDBEntity>();
    static public bool ScoreRetrieved = false;

    public static void GetScoreTable(ScoreDBEntity NewScore)
    {
        Table.LoadTableAsync(DBModel._client, "Score", loadTableResult => {
            if (loadTableResult.Exception != null){
                Debug.Log("\n failed to load score from AWS");
            }
            else{
                try{
                    DynamoDBContext context = DBModel.Context;
                    AsyncSearch<ScoreDBEntity> search = context.ScanAsync<ScoreDBEntity>(
                                                                new ScanCondition("Level", ScanOperator.Equal, (int)LevelsManager.getCurrentLevel()));
                    
                    search.GetRemainingAsync(result => {
                        if (result.Exception == null){
                            result.Result.Add(NewScore);
                            scoreDBEntities = result.Result.OrderBy( o => o.TimeElapsed).ToList();
                            ScoreRetrieved = true;
                        }
                        else{
                            Debug.LogError("Failed to get async table scan results: " + result.Exception.Message);
                        }
                    }, null);
                }
                catch (AmazonDynamoDBException exception){
                    Debug.Log(string.Concat("Exception fetching characters from table: {0}", exception.Message));
                    Debug.Log(string.Concat("Error code: {0}, error type: {1}", exception.ErrorCode, exception.ErrorType));
                }
            }
        });
    }

    public static void CreateScoreInTable(ScoreDBEntity NewScore)
    {
        DBModel.Context.SaveAsync(NewScore, (result) => {
            if (result.Exception == null){
                Debug.Log("character saved");
            }
        });
    }
    
    public static void getScoreFirst(ref Text ScoreLevelP1, ref Text PlayerLevelP1){
        if(scoreDBEntities.Count > 0){
            ScoreLevelP1.text = scoreDBEntities.ElementAt(0).TimeElapsed.ToString() + " s";
            PlayerLevelP1.text = "1. " + scoreDBEntities.ElementAt(0).DisplayName.ToString();
        }
    }
    public static void getScoreSecond(ref Text ScoreLevelP2, ref Text PlayerLevelP2){
        if (scoreDBEntities.Count > 1){
            ScoreLevelP2.text = scoreDBEntities.ElementAt(1).TimeElapsed.ToString() + " s";
            PlayerLevelP2.text = "2. " + scoreDBEntities.ElementAt(1).DisplayName.ToString();
        }
    }
    public static void getScoreThird(ref Text ScoreLevelP3, ref Text PlayerLevelP3){
        if (scoreDBEntities.Count > 2){
            ScoreLevelP3.text = scoreDBEntities.ElementAt(2).TimeElapsed.ToString() + " s";
            PlayerLevelP3.text = "3. " + scoreDBEntities.ElementAt(2).DisplayName.ToString();
        }
    }
    public static void loadCurrentScorePosition(ref Text PlayerLevelActual, ScoreDBEntity CurrentScore){
        for (int i = 0; i < scoreDBEntities.Count; i++){
            if(scoreDBEntities.ElementAt(i) == CurrentScore) {
                PlayerLevelActual.text = (i+1).ToString() + ". " + PlayerLevelActual.text;
                break;
            }
        }
    }
}