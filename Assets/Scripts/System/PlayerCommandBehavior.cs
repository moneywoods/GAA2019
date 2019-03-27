using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandBehavior : MonoBehaviour
{
    public enum Direction
    {
        // Q, W, E,
        // A,    D,
        // Z, X, C
        // この順番になっているのはfor文で回したときにDirection型n * 45度で角度が出せるからです. 
        D, // 右
        E, // 右上
        W, // 上
        Q, // 左上
        A, // 左
        Z, // 左下
        X, // 下
        C, // 右下
        //W, // 上
        //E, // 右上
        //D, // 右
        //C, // 右下
        //X, // 下
        //Z, // 左下
        //A, // 左
        //Q,  // 左上
        ENUM_MAX,
        NONE
    }

    private GameObject m_PlayerCharacter;
    private TakoController m_PlayerScript;
    // public GameObject LandStarType;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーキャラクター系
        if( m_PlayerCharacter == null )
        {
            FindPlayerCharacter();
        }

        if( m_PlayerScript != null )
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.W);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.E);

            }
            else if (Input.GetKey(KeyCode.D))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.D);

            }
            else if (Input.GetKey(KeyCode.C))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.C);

            }
            else if (Input.GetKey(KeyCode.X))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.X);

            }
            else if (Input.GetKey(KeyCode.Z))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.Z);

            }
            else if (Input.GetKey(KeyCode.A))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.A);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.Q);
            }
        }

        // リセットボタン
        if( Input.GetKey( KeyCode.L ) )
        {

            GameObject starMaker = GameObject.FindWithTag("StarMaker");
            starMaker.GetComponent<StarMaker>().DestroyWorld();
            starMaker.GetComponent<StarMaker>().MakeWorld();
        }
    }

    public void SetPlayerCharacter( GameObject _PlayerCharacter )
    {
        m_PlayerCharacter = _PlayerCharacter;
        m_PlayerScript = m_PlayerCharacter.GetComponent<TakoController>();
    }

    public void DiscardPlayerCharacterControll()
    {
        if( m_PlayerCharacter != null)
        {
            m_PlayerCharacter = null;
        }
    }

    private void FindPlayerCharacter() // シーン中の"Player"タグのついたオブジェクトを検索し,自身にセットする(現状"Player"オブジェクトはシーン中1つとしてます).
    {
        if( m_PlayerCharacter == null )
        {
            SetPlayerCharacter( GameObject.FindGameObjectWithTag("PlayerCharacter") );
        }
    }
}
