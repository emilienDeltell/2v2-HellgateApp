using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonListControl : MonoBehaviour
{
    private GameManager gm;
    [SerializeField]
    private GameObject buttonTemplate;

    
    

    //trocar o intArray pela lista de builds

    DatabaseHandler databaseHandler;

    private List<GameObject> buttons = new List<GameObject> ();
   

    void Start() {
        gm = FindObjectOfType<GameManager> ();

        //GenerateList ();
        
        /*
        for (int i = 1; i <= 20; i++) {
            GameObject button = Instantiate (buttonTemplate) as GameObject;
            button.SetActive (true);

            button.GetComponent<ButtonListButton> ().SetText ("Button #" + i);
            button.transform.SetParent (buttonTemplate.transform.parent, false);
        }
        */
    }



    public void GeneratePlayerBuildList() {
        if (buttons.Count > 0) {
            foreach (GameObject button in buttons) {
                Destroy (button.gameObject);
            }
            buttons.Clear ();
        }
        databaseHandler = FindObjectOfType<DatabaseHandler> ();
        //Foreach player build, spawn a button and set its name
        foreach (PlayerBuild i in databaseHandler.HGStats2v2[0].PlayerBuilds) {
            GameObject button = Instantiate (buttonTemplate) as GameObject;        
            button.SetActive (true);
            button.GetComponent<ButtonListButton> ().SetText (i.playerBuild);
            button.transform.SetParent (buttonTemplate.transform.parent, false);
            buttons.Add (button);
        }
    }

    public void GenerateEnemyBuildList() {
        if (buttons.Count > 0) {
            foreach (GameObject button in buttons) {
                Destroy (button.gameObject);
            }
            buttons.Clear ();
        }
        databaseHandler = FindObjectOfType<DatabaseHandler> ();
        if (databaseHandler.HGStats2v2[0].EnemyNames.Count > 0) {
            foreach (EnemyBuild i in databaseHandler.HGStats2v2[0].EnemyBuilds) {
                GameObject button = Instantiate (buttonTemplate) as GameObject;
                button.SetActive (true);
                button.GetComponent<ButtonListButton> ().SetText (i.enemyBuild);
                button.transform.SetParent (buttonTemplate.transform.parent, false);
                buttons.Add (button);
            }
        }
    }

    public void GenerateEnemyNameList() {
        if (buttons.Count > 0) {
            foreach (GameObject button in buttons) {
                Destroy (button.gameObject);
            }
            buttons.Clear ();
        }
        databaseHandler = FindObjectOfType<DatabaseHandler> ();
        if (databaseHandler.HGStats2v2[0].EnemyNames.Count > 0) { 
            foreach (EnemyName i in databaseHandler.HGStats2v2[0].EnemyNames) {
                GameObject button = Instantiate (buttonTemplate) as GameObject;
                button.SetActive (true);
                button.GetComponent<ButtonListButton> ().SetText (i.enemyName);
                button.transform.SetParent (buttonTemplate.transform.parent, false);
                buttons.Add (button);
            }
        }
    }

    public void ButtonClicked(string myTextString) {
        //with the string from the button now grabbed, all I have to do it set the targeted Textfield with these values
        gm.TargetedText.text = myTextString;

        //maybye delete the buttons from the screen before doing anything else
        gm.ScrollList.SetActive (false);
    }



    //Fight Object
    public void GenerateFightList() {
        if (buttons.Count > 0) {
            foreach (GameObject button in buttons) {
                Destroy (button.gameObject);
            }
            buttons.Clear ();
        }

        databaseHandler = FindObjectOfType<DatabaseHandler> ();
        var reversedFightList = databaseHandler.HGStats2v2[0].Fights;
        reversedFightList.Reverse ();


        if (databaseHandler.HGStats2v2[0].Fights.Count > 0) {           
            foreach (Fight i in reversedFightList) {          
                GameObject button = Instantiate (buttonTemplate) as GameObject;
                button.SetActive (true);
                //SetText (i.enemyName);
                button.transform.SetParent (buttonTemplate.transform.parent, false);
                button.transform.Find("FightButton").GetComponent<ButtonListButton> ().SetFightListTextFields (i.result, i.firstEnemyName + ", " + i.secondEnemyName, i.firstEnemyBuild + ", " + i.secondEnemyBuild, i.report, i.fightNumber);
                buttons.Add (button);
                
            }
        }







    }



}
