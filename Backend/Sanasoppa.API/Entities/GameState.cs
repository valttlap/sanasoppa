namespace Sanasoppa.API.Entities
{
    public enum GameState
    {
        NotStarted = 1,
        WaitingDasher = 2,
        GivingExplanations = 3,
        DasherValuingExplanations = 4,
        VotingExplanations = 5,
        CalculatingPoints = 6,
        GameEnded = 7
    }
}
