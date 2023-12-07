using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tile tileBack;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;
    private TextMeshProUGUI _error;

    private void Awake()
    {
        try
        {
            tilemap = GetComponent<Tilemap>(); // Zoekt naar een object met de component Tilemap
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
    
    public void Draw(Card[,] state) // Tekent het bord
    {
        try
        {
            int width = state.GetLength(0);
            int height = state.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Card card = state[x, y];
                    tilemap.SetTile(card.position, getTile(card));
                }
            }
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
    
    private Tile getTile(Card card) // Pakt de juiste kaart
    {
        try
        {
            if (card.revealed)
            {
                return getRevealed(card);
            }
            else if (card.matched)
            {
                return null;
            }
            else
            {
                return tileBack;
            }
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
            return null;
        }
    }
    
    private Tile getRevealed(Card card) // Als de kaart is omgedraaid pakt hij de juiste kaart
    {
        try
        {
            switch (card.front)
            {
                case 1: return tileNum1;
                case 2: return tileNum2;
                case 3: return tileNum3; 
                case 4: return tileNum4;
                case 5: return tileNum5;
                case 6: return tileNum6;
                case 7: return tileNum7;
                case 8: return tileNum8;
                default: return null;
            }
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
            return null;
        }
    }
}
