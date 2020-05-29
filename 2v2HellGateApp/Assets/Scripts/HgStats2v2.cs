using System.Collections.Generic;
[System.Serializable]
public class HgStats2v2 
{
    public bool solo;
    public string player1;
    public string player1CurrentBuild;
    public string player2;
    public string player2CurrentBuild;
    public int hellgates;
    public int wins;
    public int losses;
    public int empty;
    public int draw;
    public int currentWinStreak;
    public int longestWinStreak;
    public float winrate;

    public List<Fight> Fights = new List<Fight> ();
    public List<PlayerBuild> PlayerBuilds = new List<PlayerBuild> ();
    public List<EnemyBuild> EnemyBuilds = new List<EnemyBuild> ();
    public List<EnemyName> EnemyNames = new List<EnemyName> ();

}
