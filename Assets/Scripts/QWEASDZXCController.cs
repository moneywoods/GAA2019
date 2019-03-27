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
            Debug.Log("W not found.");
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

    public void SetQWEASDZXC(PlayerCommandBehavior.Direction direction )
    {
        if ( direction == PlayerCommandBehavior.Direction.W)
        {
            spriteRenderer.sprite = m_W;
        }
        else if (direction == PlayerCommandBehavior.Direction.E)
        {
            spriteRenderer.sprite = m_E;
        }
        else if (direction == PlayerCommandBehavior.Direction.D)
        {
            spriteRenderer.sprite = m_D;
        }
        else if (direction == PlayerCommandBehavior.Direction.C)
        {
            spriteRenderer.sprite = m_C;
        }
        else if (direction == PlayerCommandBehavior.Direction.X)
        {
            spriteRenderer.sprite = m_X;
        }
        else if( direction == PlayerCommandBehavior.Direction.Z)
        {
            spriteRenderer.sprite = m_Z;
        }
        else if (direction == PlayerCommandBehavior.Direction.A)
        {
            spriteRenderer.sprite = m_A;
        }
        else if (direction == PlayerCommandBehavior.Direction.Q)
        {
            spriteRenderer.sprite = m_Q;
        }
    }
}
