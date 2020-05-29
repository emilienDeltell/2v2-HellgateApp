using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Main Game")]
    [SerializeField] private List<Toggle> CurrentToggles = new List<Toggle> ();
    [SerializeField] private List<TextMeshProUGUI> CurrentTexts = new List<TextMeshProUGUI> ();
    [SerializeField] private List<TextMeshProUGUI> totalNums = new List<TextMeshProUGUI> ();
    [SerializeField] private TextMeshProUGUI currentWinStreak;
    [SerializeField] private TextMeshProUGUI longestWinStreak;
    [SerializeField] private TextMeshProUGUI winrate;
    [SerializeField] private Button battleReportButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Color textColorChange;
    private Color originalTextColor;
    [SerializeField] Color enabledTextColor;
    DatabaseHandler databaseHandler;
    bool won;
    bool lost;
    bool empty;
    bool draw;
    [Header ("Profile Creation")]
    [SerializeField] public GameObject popUpObject;
    [SerializeField] public GameObject warningMessage;
    [SerializeField] private string creationString;
    [SerializeField] private string profileInputString;
    [SerializeField] private TextMeshProUGUI profileMessageText;
    [SerializeField] private Button soloButton;
    [SerializeField] private Button duoButton;
    [SerializeField] private TMP_InputField player1NameInputField;
    [SerializeField] private TMP_InputField player2NameInputField;
    [SerializeField] private TMP_InputField player1BuildInputField;
    [SerializeField] private TMP_InputField player2BuildInputField;
    [SerializeField] private Button createProfileButton;

    [Header ("Battle Report")]
    [SerializeField] private TextMeshProUGUI battleReportTitle;
    [SerializeField] private TextMeshProUGUI targetedText;
    [SerializeField] private GameObject reportObject;
    [SerializeField] private TextMeshProUGUI player1NameText;
    [SerializeField] private TextMeshProUGUI player2NameText;    
    [SerializeField] private TextMeshProUGUI battleRepP1ButtonBuildText;
    [SerializeField] private TextMeshProUGUI battleRepP2ButtonBuildText;
    [SerializeField] private Button player2BuildBtn;
    bool reportFilled;

    [SerializeField] private TextMeshProUGUI enemy1NameText;
    [SerializeField] private TextMeshProUGUI enemy2NameText;
    [SerializeField] private TextMeshProUGUI enemy1BuildText;
    [SerializeField] private TextMeshProUGUI enemy2BuildText;
    [SerializeField] private TMP_InputField reportInputField;

    [Header ("Scroll List")]
    [SerializeField] private GameObject scrollList;
    [SerializeField] private ButtonListControl btnListCntrl;
    [SerializeField] private GameObject createNewPopup;
    [SerializeField] private TMP_InputField createNewInputfield;
    [SerializeField] private GameObject cn_warningMessage;

    [Header ("Scroll Fight List")]
    [SerializeField] private ButtonListControl fightObjListControl;
    [SerializeField] private GameObject scrollFightList;
    // at all the win/ lose / draw / empty functions I have to change the text of the Battle Report
    
    private lists myList;
    enum lists
    {
        PLAYERBUILD,
        ENEMYBUILD,
        ENEMYNAME
    }

    public TextMeshProUGUI ProfileMessageText { get => profileMessageText; set => profileMessageText = value; }
    public string CreationString { get => creationString; set => creationString = value; }
    public string ProfileInputString { get => profileInputString; set => profileInputString = value; }
    public TextMeshProUGUI TargetedText { get => targetedText; set => targetedText = value; }
    public GameObject ScrollList { get => scrollList; set => scrollList = value; }


    // Start is called before the first frame update
    void Start() {
        databaseHandler = FindObjectOfType<DatabaseHandler> ();
        originalTextColor = CurrentTexts[0].color;
        //Disable what is not supposed to be enabled
        for (int i = 1; i < 5; i++) {
            CurrentToggles[i].gameObject.SetActive (false);
            CurrentTexts[i].gameObject.SetActive (false);
        }
        battleReportButton.gameObject.SetActive (false);
        cancelButton.gameObject.SetActive (false);
        saveButton.gameObject.SetActive (false);
        //Grab the database


    }

    public void FightScrollList() {
        scrollFightList.SetActive (true);
        fightObjListControl.GenerateFightList ();
    }

    public void ReportCompleted() {
        reportFilled = true;
        reportObject.SetActive (false);
        saveButton.gameObject.SetActive (true);
    }

    public void ReportBack() {
        reportObject.SetActive (false);
    }

    public void ScrollListBack() {
        ScrollList.SetActive (false);
    }

    public void PlayerBuildScrollList() { //passar aqui um enum para
        myList = lists.PLAYERBUILD;
        ScrollList.SetActive (true);
        btnListCntrl.GeneratePlayerBuildList ();

    }

    public void EnemyBuildScrollList() {
        myList = lists.ENEMYBUILD;
        ScrollList.SetActive (true);
        btnListCntrl.GenerateEnemyBuildList ();
    }

    public void EnemyNameScrollList() {
        myList = lists.ENEMYNAME;
        ScrollList.SetActive (true);
        btnListCntrl.GenerateEnemyNameList ();
    }


    public void DesactivateScrollList() {
        //Maybye delete the buttons on the list before desactivating the object
        ScrollList.SetActive (false);
    }

    public void CreateNewPopup() {
        createNewPopup.SetActive (true);
    }

    public void BackCreateNew() {
        createNewPopup.SetActive (false);
        createNewInputfield.text = "";
    }

    public void CreateNewConfirmButton() {
        if (createNewInputfield.text != string.Empty) { 
            switch (myList) {
                case lists.PLAYERBUILD:
                    // create a temporary variable called playerBuild and
                    PlayerBuild playerBuild = new PlayerBuild ();
                    //Add the content 
                    playerBuild.playerBuild = createNewInputfield.text.ToString ();
                    createNewInputfield.text = "";
                    //Acess the database and add a new Build
                    databaseHandler.HGStats2v2[0].PlayerBuilds.Add (playerBuild);
                    btnListCntrl.GeneratePlayerBuildList ();
                    break;
                case lists.ENEMYNAME:
                    EnemyName enemyName = new EnemyName ();
                    enemyName.enemyName = createNewInputfield.text.ToString ();
                    createNewInputfield.text = "";
                    databaseHandler.HGStats2v2[0].EnemyNames.Add (enemyName);
                    btnListCntrl.GenerateEnemyNameList ();
                    break;
                case lists.ENEMYBUILD:
                    EnemyBuild enemyBuild = new EnemyBuild ();
                    enemyBuild.enemyBuild = createNewInputfield.text.ToString ();
                    createNewInputfield.text = "";
                    databaseHandler.HGStats2v2[0].EnemyBuilds.Add (enemyBuild);
                    btnListCntrl.GenerateEnemyBuildList ();
                    break;
            }
            cn_warningMessage.SetActive (false);
            createNewPopup.SetActive (false);
        } else {
            //display a message saying field is empty
            cn_warningMessage.SetActive (true);
        }
    }

   

    public void CloseFightList() {
        scrollFightList.SetActive (false);
    }


    public void BattleReportButton() {
        UpdateBattleReport ();
        reportObject.SetActive (true);
    }

    public void WipeScore() {
        //Wipe Out all the progress
        foreach (var x in totalNums) {
            x.text = "0";
        }
        currentWinStreak.text = "0";
        longestWinStreak.text = "0";
        winrate.text = "0%";
        databaseHandler.HGStats2v2[0].hellgates = 0;
        databaseHandler.HGStats2v2[0].wins = 0;
        databaseHandler.HGStats2v2[0].losses = 0;
        databaseHandler.HGStats2v2[0].empty = 0;
        databaseHandler.HGStats2v2[0].draw = 0;
        databaseHandler.HGStats2v2[0].currentWinStreak = 0;
        databaseHandler.HGStats2v2[0].longestWinStreak = 0;
        databaseHandler.HGStats2v2[0].winrate = 0;
        databaseHandler.HGStats2v2[0].player1 = "";
        databaseHandler.HGStats2v2[0].player1CurrentBuild = "";
        databaseHandler.HGStats2v2[0].player2 = "";
        databaseHandler.HGStats2v2[0].player2CurrentBuild = "";
        if (databaseHandler.HGStats2v2[0].Fights.Count > 0) {
            databaseHandler.HGStats2v2[0].Fights.Clear ();
        }
        if (databaseHandler.HGStats2v2[0].PlayerBuilds.Count > 0) {
            databaseHandler.HGStats2v2[0].PlayerBuilds.Clear ();
        }
        if (databaseHandler.HGStats2v2[0].EnemyBuilds.Count > 0) {
            databaseHandler.HGStats2v2[0].EnemyBuilds.Clear ();
        }
        if (databaseHandler.HGStats2v2[0].EnemyNames.Count > 0) {
            databaseHandler.HGStats2v2[0].EnemyNames.Clear ();
        }

        databaseHandler.OnClick_SaveDatabase ();

        //display the gameObject

        popUpObject.SetActive (true);
        TextChange (ProfileMessageText, CreationString);
        soloButton.gameObject.SetActive (true);
        duoButton.gameObject.SetActive (true);
        player1NameInputField.gameObject.SetActive (false);
        player1BuildInputField.gameObject.SetActive (false);
        player2NameInputField.gameObject.SetActive (false);
        player2BuildInputField.gameObject.SetActive (false);
        createProfileButton.gameObject.SetActive (false);
    }

    public void Joined() {
        LoadFromJsonDatabase ();
        // enable the other 4 buttons and the other 4 toggles and remove interactability of itselft
        CurrentToggles[0].interactable = false;
        CurrentTexts[0].fontStyle = FontStyles.Strikethrough;
        CurrentTexts[0].color = enabledTextColor;
        //Enable what is suposd to be enabled
        for (int i = 1; i < 5; i++) {
            CurrentToggles[i].interactable = true;
            CurrentToggles[i].gameObject.SetActive (true);

            CurrentTexts[i].gameObject.SetActive (true);
        }
        //Display the save and cancel buttons but only enable the cancel;
        battleReportButton.gameObject.SetActive (true);
        cancelButton.gameObject.SetActive (true);
        battleReportButton.interactable = false;
        battleReportButton.gameObject.SetActive (true);
        //saveButton.interactable = false;
        //saveButton.gameObject.SetActive (true);


    }

    public void Won() {
        //change color and interactability of all the toggles
        battleReportTitle.text = "Victory";
        won = true;
        saveButton.interactable = true;
        battleReportButton.interactable = true;
        foreach (var x in CurrentToggles) {
            x.interactable = false;
        }
        
        CurrentTexts[1].fontStyle = FontStyles.Strikethrough;
        CurrentTexts[1].color = enabledTextColor;
        CurrentTexts[2].color = enabledTextColor;
        CurrentTexts[3].color = enabledTextColor;
        CurrentTexts[4].color = enabledTextColor;

        

    }
    public void Lost() {
        battleReportTitle.text = "Defeat";
        lost = true;
        saveButton.interactable = true;
        battleReportButton.interactable = true;
        foreach (var x in CurrentToggles) {
            x.interactable = false;
        }
        CurrentTexts[2].fontStyle = FontStyles.Strikethrough;
        CurrentTexts[1].color = enabledTextColor;
        CurrentTexts[2].color = enabledTextColor;
        CurrentTexts[3].color = enabledTextColor;
        CurrentTexts[4].color = enabledTextColor;
    }
    public void Empty() {
        battleReportTitle.text = "No Contest";
        empty = true;
        saveButton.interactable = true;
        battleReportButton.interactable = true;
        foreach (var x in CurrentToggles) {
            x.interactable = false;
        }
        CurrentTexts[3].fontStyle = FontStyles.Strikethrough;
        CurrentTexts[1].color = enabledTextColor;
        CurrentTexts[2].color = enabledTextColor;
        CurrentTexts[3].color = enabledTextColor;
        CurrentTexts[4].color = enabledTextColor;
    }
    public void Draw() {
        battleReportTitle.text = "Draw";
        draw = true;
        saveButton.interactable = true;
        battleReportButton.interactable = true;

        foreach (var x in CurrentToggles) {
            x.interactable = false;
        }
        CurrentTexts[4].fontStyle = FontStyles.Strikethrough;
        CurrentTexts[1].color = enabledTextColor;
        CurrentTexts[2].color = enabledTextColor;
        CurrentTexts[3].color = enabledTextColor;
        CurrentTexts[4].color = enabledTextColor;
    }

    public void Save() {
        //Add +1 to the hellgates database
        databaseHandler.HGStats2v2[0].hellgates += 1;
        totalNums[0].text = databaseHandler.HGStats2v2[0].hellgates.ToString ();

        //Check if the current win streak is higher then the longest win streak
        if (won) {
            Debug.Log ("it0s a win");

            databaseHandler.HGStats2v2[0].wins += 1;
            databaseHandler.HGStats2v2[0].currentWinStreak += 1;
            currentWinStreak.text = databaseHandler.HGStats2v2[0].currentWinStreak.ToString ();
            totalNums[1].text = databaseHandler.HGStats2v2[0].wins.ToString ();
            if (databaseHandler.HGStats2v2[0].currentWinStreak > databaseHandler.HGStats2v2[0].longestWinStreak) {

                databaseHandler.HGStats2v2[0].longestWinStreak += 1;
                longestWinStreak.text = databaseHandler.HGStats2v2[0].longestWinStreak.ToString ();
            }
        }

        if (lost) {
            databaseHandler.HGStats2v2[0].currentWinStreak = 0;
            currentWinStreak.text = databaseHandler.HGStats2v2[0].currentWinStreak.ToString ();

            databaseHandler.HGStats2v2[0].losses += 1;
            totalNums[2].text = databaseHandler.HGStats2v2[0].losses.ToString ();
        }

        if (empty) {
            databaseHandler.HGStats2v2[0].empty += 1;
            totalNums[3].text = databaseHandler.HGStats2v2[0].empty.ToString ();
        }

        if (draw) {
            databaseHandler.HGStats2v2[0].currentWinStreak = 0;
            currentWinStreak.text = databaseHandler.HGStats2v2[0].currentWinStreak.ToString ();
            databaseHandler.HGStats2v2[0].draw += 1;
            totalNums[4].text = databaseHandler.HGStats2v2[0].draw.ToString ();
        }

        //Calculate the winrate
        float contentGates = databaseHandler.HGStats2v2[0].wins + databaseHandler.HGStats2v2[0].losses;
        databaseHandler.HGStats2v2[0].winrate = (databaseHandler.HGStats2v2[0].wins * 100) / contentGates;

        winrate.text = databaseHandler.HGStats2v2[0].winrate.ToString ("F0") + "%";

        
        Fight fight = new Fight ();
        if (enemy1NameText.text != "Add Name") {
            fight.firstEnemyName = enemy1NameText.text.ToString ();
        } else {
            fight.firstEnemyName = "";
        }

        if (enemy2NameText.text != "Add Name") {
            fight.secondEnemyName = enemy2NameText.text.ToString ();
        } else {
            fight.secondEnemyName = "";
        }

        if (enemy1BuildText.text != "Add Build") {
            fight.firstEnemyBuild = enemy1BuildText.text.ToString ();
        } else {
            fight.firstEnemyBuild = "";
        }

        if (enemy2BuildText.text != "Add Build") {
            fight.secondEnemyBuild = enemy2BuildText.text.ToString ();
        } else {
            fight.secondEnemyBuild = "";
        }

        if (reportInputField.text != string.Empty) {
            fight.report = reportInputField.text.ToString ();
        } else {
            fight.report = "";
        }

        fight.firstPlayerName = player1NameText.text.ToString ();
        fight.firstPlayerBuild = battleRepP1ButtonBuildText.text.ToString ();        
        fight.secondPlayerName = player2NameText.text.ToString ();
        fight.secondPlayerBuild = battleRepP2ButtonBuildText.text.ToString ();       
        fight.fightNumber = databaseHandler.HGStats2v2[0].hellgates; // the +1 its because its not saved yet
        fight.result = battleReportTitle.text.ToString ();
        databaseHandler.HGStats2v2[0].Fights.Add (fight);
        databaseHandler.OnClick_SaveDatabase ();
        Cancel ();
        
    }

    public void Cancel() {
        enemy1NameText.text = "Add Name";
        enemy2NameText.text = "Add Name";
        enemy1BuildText.text = "Add Build";
        enemy2BuildText.text = "Add Build";
        reportInputField.text = "";
        reportFilled = false;


        foreach (var x in CurrentToggles) {
            x.isOn = false;
        }

        foreach (var x in CurrentTexts) {
            x.fontStyle = FontStyles.Normal;
            x.color = originalTextColor;
        }


        CurrentToggles[0].interactable = true;
        //CurrentTexts[0].fontStyle = FontStyles.Normal;
        //CurrentTexts[0].color = originalTextColor;

        won = false;
        lost = false;
        empty = false;
        draw = false;

        for (int i = 1; i < 5; i++) {
            CurrentToggles[i].gameObject.SetActive (false);
            CurrentTexts[i].color = Color.white;
            CurrentTexts[i].gameObject.SetActive (false);
        }
        //Display the save and cancel buttons but only enable the cancel;
        cancelButton.gameObject.SetActive (false);
        saveButton.interactable = false;
        saveButton.gameObject.SetActive (false);
        reportObject.SetActive (false);
        battleReportButton.interactable = false;
        battleReportButton.gameObject.SetActive (false);

    }

    public void LoadFromJsonDatabase() {
        totalNums[0].text = databaseHandler.HGStats2v2[0].hellgates.ToString ();
        totalNums[1].text = databaseHandler.HGStats2v2[0].wins.ToString ();
        totalNums[2].text = databaseHandler.HGStats2v2[0].losses.ToString ();
        totalNums[3].text = databaseHandler.HGStats2v2[0].empty.ToString ();
        totalNums[4].text = databaseHandler.HGStats2v2[0].draw.ToString ();
        currentWinStreak.text = databaseHandler.HGStats2v2[0].currentWinStreak.ToString ();
        longestWinStreak.text = databaseHandler.HGStats2v2[0].longestWinStreak.ToString ();


            winrate.text = databaseHandler.HGStats2v2[0].winrate.ToString ("F0") + "%";





    }

    #region Profile Creation
    public void CreateProfile() {
        if (databaseHandler.HGStats2v2[0].solo == true) {
            // if true then only check if the player 1 inputs have been filled
            if (player1NameInputField.text != string.Empty && player1BuildInputField.text != string.Empty) {
                databaseHandler.HGStats2v2[0].player1 = player1NameInputField.text.ToString ();
                databaseHandler.HGStats2v2[0].player1CurrentBuild = player1BuildInputField.text.ToString ();
                AddPlayerBuild (player1BuildInputField.text.ToString ());
                //Acess the player 1 text and player 2 text of battle report
                databaseHandler.OnClick_SaveDatabase ();
                warningMessage.SetActive (false);
                popUpObject.SetActive (false);
                LoadFromJsonDatabase ();
                player1NameInputField.text = "";
                player1BuildInputField.text = "";
            } else { // else I have to display a message warning the player to enter all fields
                warningMessage.SetActive (true);
            }

        } else { // else means that its a duo so I have to check all strings
            if(player1NameInputField.text != string.Empty && player1BuildInputField.text != string.Empty && player2NameInputField.text != string.Empty && player2BuildInputField.text != string.Empty) {
                databaseHandler.HGStats2v2[0].player1 = player1NameInputField.text.ToString ();
                databaseHandler.HGStats2v2[0].player1CurrentBuild = player1BuildInputField.text.ToString ();
                databaseHandler.HGStats2v2[0].player2 = player2NameInputField.text.ToString ();
                databaseHandler.HGStats2v2[0].player2CurrentBuild = player2BuildInputField.text.ToString ();
                AddPlayerBuild (player1BuildInputField.text.ToString ());
                AddPlayerBuild (player2BuildInputField.text.ToString ());
                //Acess the player 1 text and player 2 text of battle report
                databaseHandler.OnClick_SaveDatabase ();
                warningMessage.SetActive (false);
                popUpObject.SetActive (false);
                LoadFromJsonDatabase ();
                player1NameInputField.text = "";
                player1BuildInputField.text = "";
                player2NameInputField.text = "";
                player2BuildInputField.text = "";
                
            } else { // else display the warning messaga
                warningMessage.SetActive (true);
            }

        }
   
       
   
}

    public void TextChange(TextMeshProUGUI text, string message) {
        text.text = message;
    }

    public void Solo() {
        databaseHandler.HGStats2v2[0].solo = true;
        TextChange (ProfileMessageText, ProfileInputString);
        soloButton.gameObject.SetActive (false);
        duoButton.gameObject.SetActive (false);
        player1NameInputField.gameObject.SetActive (true);
        player1BuildInputField.gameObject.SetActive (true);
        createProfileButton.gameObject.SetActive (true);

    }

    public void Duo() {
        databaseHandler.HGStats2v2[0].solo = false;
        TextChange (ProfileMessageText, ProfileInputString);
        soloButton.gameObject.SetActive (false);
        duoButton.gameObject.SetActive (false);
        player1NameInputField.gameObject.SetActive (true);
        player1BuildInputField.gameObject.SetActive (true);
        player2NameInputField.gameObject.SetActive (true);
        player2BuildInputField.gameObject.SetActive (true);
        createProfileButton.gameObject.SetActive (true);
    }

    #endregion

    public void AddFight() {
        Fight fight = new Fight ();
        fight.result = "Loss";
        databaseHandler.HGStats2v2[0].Fights.Add (fight);
    }

    public void AddPlayerBuild(string build) {
        PlayerBuild pb = new PlayerBuild ();
        pb.playerBuild = build;
        databaseHandler.HGStats2v2[0].PlayerBuilds.Add (pb);
    }

    public void AddEnemyBuild(string build) {
        EnemyBuild eb = new EnemyBuild ();
        eb.enemyBuild = build;
        databaseHandler.HGStats2v2[0].EnemyBuilds.Add (eb);
    }

    public void TextColorChange (TextMeshProUGUI text, Color color){
        text.color = color;
    }

    public void UpdateBattleReport() {
        //check if its a solo or duo HG
        if (databaseHandler.HGStats2v2[0].solo != true) { 
            player1NameText.text = databaseHandler.HGStats2v2[0].player1;
            player2NameText.text = databaseHandler.HGStats2v2[0].player2;
            //enable the Button Component for the player 2 Build
            player2BuildBtn.enabled = true;
            if (reportFilled != true) { 
                battleRepP1ButtonBuildText.text = databaseHandler.HGStats2v2[0].player1CurrentBuild;
                battleRepP2ButtonBuildText.text = databaseHandler.HGStats2v2[0].player2CurrentBuild;
            }
        } else {
            player1NameText.text = databaseHandler.HGStats2v2[0].player1;
            player2NameText.text = "";
            //disable the Button Component for the player 2 build
            player2BuildBtn.enabled = false;
            if (reportFilled != true) {
                battleRepP1ButtonBuildText.text = databaseHandler.HGStats2v2[0].player1CurrentBuild;
                battleRepP2ButtonBuildText.text = "";
            }

        }
    }

}
