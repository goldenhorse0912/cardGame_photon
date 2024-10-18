using UnityEngine;

public static class PegsScoreManager
{
    // Static fields to store Red and Blue Pegs scores
    private static int redPegsScore;
    private static int bluePegsScore;
    private static int redPegsPastScore;
    private static int bluePegsPastScore;
    public static int maxScore = 121;
    public static bool isNewGameStarted = true;
    public static bool isPlayerADealer = true;
    // Property to get/set Red Pegs score
    public static int RedPegsScore
    {
        get { return redPegsScore; }
        set { redPegsScore = value; }
    }

    // Property to get/set Blue Pegs score
    public static int BluePegsScore
    {
        get { return bluePegsScore; }
        set { bluePegsScore = value; }
    }

    public static int RedPegsPastScore
    {
        get { return redPegsPastScore; }
        set { redPegsPastScore = value; }
    }

    // Property to get/set Blue Pegs score
    public static int BluePegsPastScore
    {
        get { return bluePegsPastScore; }
        set { bluePegsPastScore = value; }
    }

    // Method to reset scores to default values
    public static void ResetScores()
    {
        PegsScoreManager.isNewGameStarted = true;
        Debug.Log("Reset Score");
        redPegsScore = bluePegsScore = redPegsPastScore = bluePegsPastScore = 0;

    }

    // Method to print current scores (for debugging purposes)
    public static void PrintScores()
    {
        Debug.Log($"Red Pegs Score: {redPegsScore}, Blue Pegs Score: {bluePegsScore}");
    }
}