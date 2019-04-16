using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QWEASDZXCController : MonoBehaviour
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

    public GameObject QWEASDZXC;
    // Start is called before the first frame update
    void Awake()
    {
        if( spriteRenderer == null )
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if( m_W == null )
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;

        transform.localScale = new Vector3(
            localScale.x / lossScale.x * 1.0f,
            localScale.y / lossScale.y * 1.0f,
            localScale.z / lossScale.z * 1.0f
            );
    }

    public void SetQWEASDZXC(Direction direction )
    {
        if ( direction == Direction.Top)
        {
            spriteRenderer.sprite = m_W;
        }
        else if (direction == Direction.RightTop)
        {
            spriteRenderer.sprite = m_E;
        }
        else if (direction == Direction.Right)
        {
            spriteRenderer.sprite = m_D;
        }
        else if (direction == Direction.RightBottom)
        {
            spriteRenderer.sprite = m_C;
        }
        else if (direction == Direction.Bottom)
        {
            spriteRenderer.sprite = m_X;
        }
        else if( direction == Direction.LeftBottom)
        {
            spriteRenderer.sprite = m_Z;
        }
        else if (direction == Direction.Left)
        {
            spriteRenderer.sprite = m_A;
        }
        else if (direction == Direction.LeftTop)
        {
            spriteRenderer.sprite = m_Q;
        }
    }
}
