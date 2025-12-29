public interface IGameManager
{
    void AddScore(int questID);
    void MinusScore(int questID);
    void AddQuest(Quest quest);
    void CompleteQuest(Quest quest);
}