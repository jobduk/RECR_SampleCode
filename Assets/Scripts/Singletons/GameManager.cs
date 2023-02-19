using UnityEngine;

/// <summary>
/// singleton class들의 초기화 순서들을 정해놓고 실행하는 gamemanager class
/// </summary>
public class GameManager : MySingleton<GameManager>
{
    public DataDictionary dataDictionary;
    public BuildManager buildManager;
    public CharacterMain characterMain;
    public TitleController titleController;

    new private void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        dataDictionary.Init();
        characterMain.Init();
        titleController.Init();
        buildManager.Init();
    }
}
