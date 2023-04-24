using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private Material material;
    private Color startColor;
    public Color endColor1 = new Color(0.5f, 0.1f, 0.9f);
    public Color endColor2 = new Color(0.5f, 0.3f, 0.1f);
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponentInChildren<Renderer>().material;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        startColor = gameManager.GetCurrentPlayer().Color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateColor(int player){
       if(player == 1){
        material.color = endColor1;
       }
       if(player == 2){
        material.color = endColor2;
       }
        gameObject.GetComponent<Hero>().isChanged = true;
        Debug.Log("changed Color " + material.color);
    }
}
