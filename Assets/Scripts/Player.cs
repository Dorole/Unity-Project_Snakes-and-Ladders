[System.Serializable]
public class Player
{
    public string playerName;
    public Piece piece;

    public enum PlayerType
    {
        CPU,
        Human,
    }

    public PlayerType playerType;

}
