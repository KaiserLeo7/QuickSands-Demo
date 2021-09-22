using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//COMMENTED BY FARAMARZ HOSSEINI


//has all the scene transitions
public class SceneChange : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader; 

    public void NewGame() {
        File.Delete(Application.persistentDataPath + "/player.savefile");
        Sands.SaveSystem.Pdata = new Sands.PlayerData();
        Sands.SaveSystem.LoadAll();
        Sands.SaveSystem.SaveAll();
        StartCoroutine(LoadLevel("CharSelect"));
    }

    //called with new game but if there is a save file doesn't do anything
    public void NewGame2()
    {
      if(!File.Exists(Application.persistentDataPath + "/player.savefile"))
            StartCoroutine(LoadLevel("CharSelect"));
    }

  public void warning()
    {
        StartCoroutine(LoadLevel("SaveWarning"));
    }

    public void QuitGame() {
        Sands.SaveSystem.SaveAll();
        Application.Quit();
    }

    //if the player quit during a travel loads back to travel
    public void characterSelected() {
        Sands.BattleSaver.LoadBattle();
        if(Sands.BattleSaver.IsInTravel)
            StartCoroutine(LoadLevel("Travel"));
        else
            StartCoroutine(LoadLevel("Town"));
    }

    public void BlackSmithSelected()
    {

        StartCoroutine(LoadLevel("BlackSmith"));
    }

    public void TradeGoodsSelected()
    {

        StartCoroutine(LoadLevel("TradeGoods"));
    }

    public void InnSelected()
    {

        StartCoroutine(LoadLevel("Inn"));
    }

    public void QuestBoardSelected()
    {

        StartCoroutine(LoadLevel("QuestBoard"));
    }

    public void ShipShopSelected()
    {

        StartCoroutine(LoadLevel("ShipShop"));
    }

    public void InventorySelected()
    {

        StartCoroutine(LoadLevel("InvManager"));
    }

    public void MapSelected()
    {
        StartCoroutine(LoadLevel("Map"));
    }

    public void MapCloseSelected()
    {
        StartCoroutine(LoadLevel("Town"));
    }

    public void ReturnToTownSelected()
    {
        StartCoroutine(LoadLevel("Town"));
    }

    public void MainMenuSelected()
    {
        StartCoroutine(LoadLevel("Main Menu"));
    }

    //scene fading transition
    IEnumerator LoadLevel(string sceneName)
    {
        levelLoader.StartTransition();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
