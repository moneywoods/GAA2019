using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializerBehavior : MonoBehaviour
{
    public GameObject m_StarMakerPrefab = null;
    public GameObject m_Canvas;
    public GameObject m_PlayerCommand;

    private void Awake()
    {
        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        starMaker.GetComponent<StarMaker>().MakeWorld();


        // UIオブジェクトを生成.
        GameObject menu = Instantiate(m_Canvas);
        menu.SetActive(false);

        // プレイヤーコントローラを生成し,にプレイヤーキャラクターをセット.
        GameObject playerController = Instantiate(m_PlayerCommand);
        var scriptPlayerController = playerController.GetComponent<PlayerCommandBehavior>();
        scriptPlayerController.SetPlayerCharacter(GameObject.FindGameObjectWithTag("PlayerCharacter"));
        scriptPlayerController.SetCurrentSceneMenu(menu);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
