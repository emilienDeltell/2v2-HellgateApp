using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetIdenfitier : MonoBehaviour
{
    GameManager gm;
    [SerializeField]
    private TextMeshProUGUI myText;

    private void Start() {
        gm = FindObjectOfType<GameManager> ();
    }

    public void SetTargetedTextFieldToPlayerBuild() {
        gm.TargetedText = myText;
        gm.PlayerBuildScrollList ();            
    }

    public void SetTargetedTextFieldToEnemyBuild() {
        gm.TargetedText = myText;
        gm.EnemyBuildScrollList ();
    }

    public void SetTargetedTextFieldToEnemyName() {
        gm.TargetedText = myText;
        gm.EnemyNameScrollList ();
    }


}
