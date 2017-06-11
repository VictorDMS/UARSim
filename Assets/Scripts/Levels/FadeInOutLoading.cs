using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInOutLoading : MonoBehaviour{
    [SerializeField] private Text TitleText;
    private bool IsGrowing = true;
    public static int Timer = 0;
    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    private void Start(){
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        TitleText.fontSize = 100;
        IsGrowing = true;
    }

    private void FixedUpdate(){
        if (IsGrowing && TitleText.fontSize < 230){
            TitleText.fontSize += 5;
        }else if (IsGrowing && TitleText.fontSize >= 230){
            IsGrowing = false;
        }else if (!IsGrowing && TitleText.fontSize > 80){
            TitleText.fontSize -= 5;
        }else if (!IsGrowing && TitleText.fontSize <= 80){
            IsGrowing = true;
        }
    }
}
