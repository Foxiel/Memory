using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    private Board board;
    private Card[,] state;
    private Card card1;
    private Card card2;
    
    
    private int width = 4;
    private int height = 4;
    private int selected;

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
        List<Card> cards = new List<Card>();
        for (int i = 0; i < width * height / 2; i++)
        {
            cards.Add(new Card { type = Card.Type.Front, front = i + 1 });
            cards.Add(new Card { type = Card.Type.Front, front = i + 1 });
        }
        
        for (int i = 0; i < cards.Count; i++)
        {
            int j = Random.Range(i, cards.Count);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
        
        int index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++, index++)
            {
                state[x, y] = cards[index];
                state[x, y].position = new Vector3Int(x, y, 0);
            }
        }
    }
    
    private void Update()
    {
        if (selected < 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Reveal();
            }
        }
        else if (selected == 2)
        {
            StartCoroutine(CheckMatch(1));
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
        selected++;
        if (selected == 1)
        {
            card1 = card;
        }
        else if (selected == 2)
        {
            card2 = card;
        }
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

    private IEnumerator CheckMatch(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (card1 == null || card2 == null)
        {
            yield break;
        }
        
        if (card1.front == card2.front)
        {
            card1.matched = true;
            card2.matched = true;
        }
        else
        {
            card1.revealed = false;
            card2.revealed = false;
        }
        
        card1 = null;
        card2 = null;
        selected = 0;
        board.Draw(state);
    }

    
}
