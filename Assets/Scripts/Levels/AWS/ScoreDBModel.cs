//CharacterCreator.cs
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ScoreDBModel
{
    //TODO - For compiling  
    public Text resultText;


    public string cognitoIdentityPoolString;

    private CognitoAWSCredentials credentials;
    private IAmazonDynamoDB _client;
    private DynamoDBContext _context;

    private List<ScoreDBEntity> scoreDBEntities = new List<ScoreDBEntity>();
    private int currentScoreIndex;

    //Listeners
    //createOperation.onClick.AddListener(CreateScoreInTable);
    //refreshOperation.onClick.AddListener(FetchAllScoreFromAWS);

    //NextCharacterButton.onClick.AddListener(CycleNextCharacter);
    //PrevCharacterButton.onClick.AddListener(CyclePrevCharacter);

    private DynamoDBContext Context{
        get{
            if (_context == null)
                _context = new DynamoDBContext(_client);

            return _context;
        }
    }

    private void Start(){
        credentials = new CognitoAWSCredentials(cognitoIdentityPoolString, RegionEndpoint.EUCentral1);
        credentials.GetIdentityIdAsync(delegate (AmazonCognitoIdentityResult<string> result)
        {
            if (result.Exception != null){
                Debug.LogError("exception hit: " + result.Exception.Message);
            }

            // Create a DynamoDB client, passing in our credentials from Cognito.
            var ddbClient = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUCentral1);

            resultText.text += ("\n*** Retrieving table information ***\n");

            // Create a DescribeTableRequest to get information about our table, and ensure we can access it.
            var request = new DescribeTableRequest{
                TableName = @"Score"
            };

            ddbClient.DescribeTableAsync(request, (ddbresult) =>
            {
                if (result.Exception != null){
                    resultText.text += result.Exception.Message;
                    Debug.Log(result.Exception);
                    return;
                }

                var response = ddbresult.Response;

                // Debug information
                TableDescription description = response.Table;
                resultText.text += ("Name: " + description.TableName + "\n");
                resultText.text += ("# of items: " + description.ItemCount + "\n");
                resultText.text += ("Provision Throughput (reads/sec): " + description.ProvisionedThroughput.ReadCapacityUnits + "\n");
                resultText.text += ("Provision Throughput (reads/sec): " + description.ProvisionedThroughput.WriteCapacityUnits + "\n");

            }, null);

            // Set our _client field to the dynamoDB client.
            _client = ddbClient;
            FetchAllScoreFromAWS();
        });
    }

    private void LoadScore(ScoreDBEntity scoreDbEntity)
    {
        //selectedBody = scoreDbEntity.DeviceID;
        //selectedFace = scoreDbEntity.DisplayName;
        //selectedHair = scoreDbEntity.Level;
        //selectedShirt = scoreDbEntity.RobotConfiguration;
        //selectedPants = scoreDbEntity.TimeElapsed;

        //// Update the character component images with those of the loaded character entity.
        //selectedHairImage.overrideSprite = allSprites.First(sprite => sprite.name == characterEntity.HairSpriteName);
        //selectedFaceImage.overrideSprite = allSprites.First(sprite => sprite.name == characterEntity.FaceSpriteName);
        //selectedBodyImage.overrideSprite = allSprites.First(sprite => sprite.name == characterEntity.BodySpriteName);
        //selectedShirtImage.overrideSprite = allSprites.First(sprite => sprite.name == characterEntity.ShirtSpriteName);
        //selectedPantsImage.overrideSprite = allSprites.First(sprite => sprite.name == characterEntity.PantsSpriteName);
        //selectedShoesImage.overrideSprite = allSprites.First(sprite => sprite.name == characterEntity.ShoesSpriteName);

        //// Update character stats with those from the loaded entity.
        //characterNameText.text = scoreDbEntity.DeviceID;
        //nameInput.text = scoreDbEntity.DisplayName;
        //ageInput.text = scoreDbEntity.Level.ToString();
        //strengthInput.text = scoreDbEntity.RobotConfiguration;
        //dexterityInput.text = scoreDbEntity.TimeElapsed.ToString();
    }

    private void CycleNextScore()
    {
        if (scoreDBEntities.Count <= 0) return;

        if (currentScoreIndex < scoreDBEntities.Count - 1)
        {
            currentScoreIndex++;
            LoadScore(scoreDBEntities[currentScoreIndex]);
        }
        else
        {
            LoadScore(scoreDBEntities.First());
            currentScoreIndex = 0;
        }
    }

    private void CyclePrevScore()
    {
        if (scoreDBEntities.Count <= 0) return;

        if (currentScoreIndex > 0)
        {
            currentScoreIndex--;
            LoadScore(scoreDBEntities[currentScoreIndex]);
        }
        else
        {
            LoadScore(scoreDBEntities.Last());
            currentScoreIndex = scoreDBEntities.Count - 1;
        }
    }

    private void FetchAllScoreFromAWS()
    {
        resultText.text = "\n***LoadTable***";
        Table.LoadTableAsync(_client, "CharacterCreator", (loadTableResult) =>
        {
            if (loadTableResult.Exception != null)
            {
                resultText.text += "\n failed to load characters table";
            }
            else
            {
                try
                {
                    var context = Context;

                    // Note scan is pretty slow for large datasets compared to a query, as we are not searching on the index.
                    var search = context.ScanAsync<ScoreDBEntity>(new ScanCondition("Age", ScanOperator.GreaterThan, 0));
                    search.GetRemainingAsync(result =>
                    {
                        if (result.Exception == null)
                        {
                            scoreDBEntities = result.Result;

                            // Load the first character into the character display
                            if (scoreDBEntities.Count > 0) LoadScore(scoreDBEntities.First());
                        }
                        else
                        {
                            Debug.LogError("Failed to get async table scan results: " + result.Exception.Message);
                        }
                    }, null);
                }
                catch (AmazonDynamoDBException exception)
                {
                    Debug.Log(string.Concat("Exception fetching characters from table: {0}", exception.Message));
                    Debug.Log(string.Concat("Error code: {0}, error type: {1}", exception.ErrorCode, exception.ErrorType));
                }

            }
        });
    }

    private void CreateScoreInTable(){
        var newScore = new ScoreDBEntity {
            UserID = System.Guid.NewGuid().ToString(),
            DeviceID = "",
            Level = 1,
            TimeElapsed = 0.0f,
            Timestamp= "",
            RobotConfiguration = "",
            DisplayName = ""
        };

        // Save the character asynchronously to the table.
        Context.SaveAsync(newScore, (result) =>
        {
            if (result.Exception == null)
                resultText.text += @"character saved";
        });
    }


    public static void getScoreFirst(ref Text ScoreLevelP1, ref Text PlayerLevelP1){

    }
    public static void getScoreSecond(ref Text ScoreLevelP2, ref Text PlayerLevelP2){

    }
    public static void getScoreThird(ref Text ScoreLevelP3, ref Text PlayerLevelP3){

    }
}