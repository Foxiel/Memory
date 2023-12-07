using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    private Board _board;
    private Card[,] _state;
    private Card _card1;
    private Card _card2;
    private TMP_InputField _p1;
    private TMP_InputField _p2;
    private Player _player1;
    private Player _player2;
    private TextMeshProUGUI _error;

    private int _width = 4;
    private int _height = 4;
    private int _selected;
    private bool _turn;

    private void Awake()
    {
        try
        {
            _board = GetComponentInChildren<Board>(); // Zoekt naar een object met de component Board
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }

    private void NewGameClick()
    {
        try
        {
            // Zoekt naar de inputs van de players en maakt een nieuwe player aan
            _p1 = GameObject.Find("Player1").GetComponent<TMP_InputField>();
            _p2 = GameObject.Find("Player2").GetComponent<TMP_InputField>();
            _player1 = new Player(_p1.text, 0);
            _player2 = new Player(_p2.text, 0);

            NewGame();
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
    
    private void NewGame()
    {
        try
        {
            _state = new Card[_width, _height];
            GenerateCards();

            Camera.main.transform.position = new Vector3(_width / 2f, _height / 1.5f, -10f); // Zorgt ervoor dat het bord de juiste positie krijgt
            _board.Draw(_state); // Maakt het bord aan
            AssignTurn();
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }

    private void GenerateCards()
    {
        try
        {
            // Zorgt ervoor dat er 2 kaarten worden aangemaakt van elke soort op een willekeurige plek
            List<Card> cards = new List<Card>();
            for (int i = 0; i < _width * _height / 2; i++)
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
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++, index++)
                {
                    _state[x, y] = cards[index];
                    _state[x, y].position = new Vector3Int(x, y, 0);
                }
            }
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
    
    private void Update()
    {
        try
        {
            // Volgende 3 lijnen zoeken weer naar objecten met die naam en componenten
            TextMeshProUGUI turnText = GameObject.Find("Turn").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI score1Text = GameObject.Find("Score1").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI score2Text = GameObject.Find("Score2").GetComponent<TextMeshProUGUI>();

            if (_state != null && _selected < 2)
            {
                if (Input.GetMouseButtonDown(0)) // Als er op een kaart wordt geklikt wordt deze laten zien
                {
                    Reveal();
                }
            }
            else if (_selected == 2)
            {
                StartCoroutine(CheckMatch(1)); // Als er 2 kaarten zijn geselecteerd wordt er gekeken of ze matchen en wordt er een delay van 1 seconde ingezet
            }

            if (_player1 != null && _player2 != null) // Zet de juiste naam bij de beurt
            {
                if (_turn)
                {
                    turnText.text = $"Current turn: {_player1.name}";
                }
                else if (!_turn)
                {
                    turnText.text = $"Current turn: {_player2.name}";
                }

                string score1 = _player1.score.ToString();
                score1Text.text = score1; // Zet de juiste score bij de juiste speler

                string score2 = _player2.score.ToString();
                score2Text.text = score2;
            }
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
    
    private void Reveal()
    {
        try
        {
            // Volgende 2 lijnen pakken de positie van de muis en converten die naar een positie op het bord
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cardPosition = _board.tilemap.WorldToCell(worldPosition);

            Card card = GetCard(cardPosition.x, cardPosition.y);

            if (card.type == Card.Type.Invalid || card.revealed || card.matched)
            {
                return;
            }

            card.revealed = true;
            
            _selected++;
            if (_selected == 1)
            {
                _card1 = card;
            }
            else if (_selected == 2)
            {
                _card2 = card;
            }

            _board.Draw(_state);
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
    
    private Card GetCard(int x, int y)
    {
        try
        {
            return IsValid(x, y) ? _state[x, y] : new Card(); // Returned de kaart op de positie die is meegegeven
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
            return new Card();
        }
    }
    
    private bool IsValid(int x, int y)
    {
        try {
            return x >= 0 && x < _width && y >= 0 && y < _height; // Kijkt of de positie buiten het bord is
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
            return false;
        }
    }

    private IEnumerator CheckMatch(float delay)
    {
        yield return new WaitForSeconds(delay); // Zorgt ervoor dat er 1 seconde wordt gewacht bij het kijken of de 2 kaarten gelijk zijn
        try
        {
            // Controleert of de 2 kaarten gelijk zijn en voert daarna de juiste handelingen uit
            if (_card1 == null || _card2 == null)
            {
                yield break;
            }

            if (_card1.front == _card2.front)
            {
                _card1.matched = true;
                _card2.matched = true;
                if (_turn)
                {
                    _player1.score++;
                }
                else if (!_turn)
                {
                    _player2.score++;
                }
            }
            else
            {
                _card1.revealed = false;
                _card2.revealed = false;

                if (_turn)
                {
                    _turn = false;
                }
                else if (!_turn)
                {
                    _turn = true;
                }
            }

            _card1 = null;
            _card2 = null;
            _selected = 0;
            _board.Draw(_state);
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }

    private void AssignTurn()
    {
        try {
            // Geeft willekeurig de beurt aan een speler
            int randomTurn = Random.Range(1, 11);
            if (randomTurn <= 5)
            {
                _turn = true;
            }
            else
            {
                _turn = false;
            }
        }
        catch (Exception e)
        {
            _error = GameObject.Find("Error").GetComponent<TextMeshProUGUI>();
            _error.text = e.Message;
        }
    }
}