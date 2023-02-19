using UnityEngine;

/// <summary>
/// singleton class���� �ʱ�ȭ �������� ���س��� �����ϴ� gamemanager class
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
