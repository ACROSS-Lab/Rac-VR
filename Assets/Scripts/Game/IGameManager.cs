public interface IGameManager
{
    void AddScore(WasteType type, int points);
    void MinusScore(WasteType type, int points);
    void IncrementCharactersTalkedTo();
}