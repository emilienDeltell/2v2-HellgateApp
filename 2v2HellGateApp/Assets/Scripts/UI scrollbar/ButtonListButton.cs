using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonListButton : MonoBehaviour
{
    private string myTextString;
    [SerializeField]
    private TextMeshProUGUI myText;
    [SerializeField]
    private ButtonListControl buttonControl;
    [Header("Fight Object Only")]
    [SerializeField] private Animator fightObj_anim;
    [SerializeField] private TextMeshProUGUI tmp_result;
    [SerializeField] private TextMeshProUGUI tmp_enemyNames;
    [SerializeField] private TextMeshProUGUI tmp_enemyBuilds;
    [SerializeField] private TextMeshProUGUI tmp_report;
    [SerializeField] private TextMeshProUGUI tmp_hellgateNumber;
    // not the button control, but the place where I store all the TextFields 


    public void SetText(string textString) {
        myTextString = textString;
        myText.text = textString;
    }

    public void OnClick() {
        buttonControl.ButtonClicked (myTextString);
        //disable the list object and change the TextField of the appropriate 

    }

    public void OnFightClicked() {
        fightObj_anim.SetTrigger ("click");
    }

    public void SetFightListTextFields(string result, string enemyNames, string enemyBuilds, string report, int hellgateNumber) {
        tmp_result.text = result;
        tmp_enemyNames.text = enemyNames;
        tmp_enemyBuilds.text = enemyBuilds;
        tmp_report.text = report;
        tmp_hellgateNumber.text = hellgateNumber.ToString();
    }
}
