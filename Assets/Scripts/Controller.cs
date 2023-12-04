using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    private Board board;
    private Card[,] state;
    
    private int width = 4;
    private int height = 4;

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }
    
    private void Start()
    {
        NewGame();
        Camera.main.orthographicSize = 3f;
    }
    
    private void NewGame()
    {
        state = new Card[width, height];
        GenerateCards();
        
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        board.Draw(state);
    }

    private void GenerateCards()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y =0; y < height; y++)
            {
                for (int i = 0; i < 2; i++)
                {
                    Card card = new Card();
                    card.position = new Vector3Int(x, y, 0);
                    card.type = Card.Type.Front;
                    card.front = Random.Range(1, 9);
                    card.revealed = false;
                    card.matched = false;
                    
                    if (state[x, y] == null)
                    {
                        state[x, y] = card;
                    }
                    else
                    {
                        // if the card already exists, generate a new card
                        i--;
                    }
                }
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Reveal();
        }
    }
    
    private void Reveal()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cardPosition = board.tilemap.WorldToCell(worldPosition);

        Card card = GetCard(cardPosition.x, cardPosition.y);
        
        if (card.type == Card.Type.Invalid || card.revealed || card.matched)
        {
            return;
        }
        
        card.revealed = true;
        board.Draw(state);
    }
    
    private Card GetCard(int x, int y)
    {
        if (IsValid(x, y))
        {
            return state[x, y];
        }
        else
        {
            return new Card();
        }
    }
    
    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
