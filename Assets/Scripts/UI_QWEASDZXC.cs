using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QWEASDZXC : MonoBehaviour
{
    public Sprite m_W;
    public Sprite m_E;
    public Sprite m_D;
    public Sprite m_C;
    public Sprite m_X;
    public Sprite m_Z;
    public Sprite m_A;
    public Sprite m_Q;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQWEASDZXC( char direction )
    { 
        if (direction == 'W' || direction == 'w')
        {
            spriteRenderer.sprite = m_W;
        }
        else if (direction == 'E' || direction == 'e')
        {
            spriteRenderer.sprite = m_E;
        }
        else if (direction == 'D' || direction == 'd')
        {
            spriteRenderer.sprite = m_D;
        }
        else if (direction == 'C' || direction == 'c')
        {
            spriteRenderer.sprite = m_C;
        }
        else if (direction == 'X' || direction == 'x')
        {
            spriteRenderer.sprite = m_X;
        }
        else if (direction == 'Z' || direction == 'z')
        {
            spriteRenderer.sprite = m_Z;
        }
        else if (direction == 'A' || direction == 'a')
        {
            spriteRenderer.sprite = m_A;
        }
        else if (direction == 'Q' || direction == 'q')
        {
            spriteRenderer.sprite = m_Q;
        }
    }
}
