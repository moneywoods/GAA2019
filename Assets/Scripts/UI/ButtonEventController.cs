using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ButtonEventController : MonoBehaviour
{
    private Text text;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.red;
        color.pressedColor = Color.blue;

        component.colors = color;
        text = transform.root.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ----------------------------------------------------------------------------------
    //
    // Event Trigger
    //
    // ----------------------------------------------------------------------------------
    public void OnSelected()
    {

    }

    public void OnDeseleccted()
    {

    }
    public void OnClick()
    {
        Debug.Log("Button click!" + gameObject.name );
    }
    public void OnSubmit()
    {
        Debug.Log("Submit!");
    }
    // ----------------------------------------------------------------------------------
    //
    // Accessor
    //
    // ----------------------------------------------------------------------------------
    public void SetText( string newText )
    {
        text.text = newText;
    }
}
