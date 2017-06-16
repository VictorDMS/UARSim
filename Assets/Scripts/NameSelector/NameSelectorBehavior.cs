using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NameSelectorBehavior : MonoBehaviour {

    [SerializeField]private Button StartGameButton;
    [SerializeField]private Text PlayerNameInputField;
    [SerializeField]private Text EmailInputField;
    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;
    
    public void Start(){
        LayerFadeInOut.SetActive(false);
        StartGameButton.interactable = false;
    }

    public void Update(){
        if ((PlayerNameInputField.text.Length > 3 && PlayerNameInputField.text.Length < 9)){
            StartGameButton.interactable = true;
        }else{
            StartGameButton.interactable = false;
        }
    }

    public void onClickStartGameButton(){
        string Description = "From NameSelector To StartGame: Email - ";
        GameManager.GameID = System.DateTime.UtcNow.ToBinary().ToString() + SystemInfo.deviceUniqueIdentifier;
        GameManager.PlayerName = PlayerNameInputField.text;
        if (VerifyEmailAddress(EmailInputField.text)){
            GameManager.EmailPlayer = EmailInputField.text;
            Description += "Yes";
        }else{
            GameManager.EmailPlayer = "";
            Description += "No";
        }
        
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.StartSim;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
        EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.MenuGame, Description);
    }
    static private bool VerifyEmailAddress(string address)
    {
        string[] atCharacter;
        string[] dotCharacter;
        atCharacter = address.Split('@');
        if (atCharacter.Length == 2){
            dotCharacter = atCharacter[1].Split('.');
            if (dotCharacter.Length >= 2){
                if (dotCharacter[dotCharacter.Length - 1].Length == 0){
                    return false;
                }else{
                    return true;
                }
            }else{
                return false;
            }
        }
        else{
            return false;
        }
    }
}
