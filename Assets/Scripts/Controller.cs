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
                Card card = new Card();
                card.position = new Vector3Int(x, y, 0);
                card.type = Card.Type.Front;
                card.front = Random.Range(1, 9);
                card.revealed = false;
                card.matched = false;
                state[x, y] = card;
            }
        }
    }
}