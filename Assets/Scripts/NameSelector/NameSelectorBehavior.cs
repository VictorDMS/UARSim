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
        GameManager.PlayerName = PlayerNameInputField.text;
        GameManager.EmailPlayer = EmailInputField.text;
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.StartSim;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
    }
}
