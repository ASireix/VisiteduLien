using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : Minigame
{

    [Header("Positionning")]
    [SerializeField] float spacingX;
    [SerializeField] float spacingY;
    [SerializeField] int column;
    [SerializeField] Vector3 selectedPositionCardFirst;
    [SerializeField] Vector3 selectedPositionCardSecond;

    [Header("Parameters")]
    [SerializeField] bool launchOnStart;
    [SerializeField] float revealLength;
    [SerializeField] List<MemoryCardParameter> memoryCards;
    [SerializeField] Texture2D backface;
    int[] _idsTableRepartitionTable;
    Dictionary<MemoryCard, int> _memoryCardDico = new Dictionary<MemoryCard, int>();
    [SerializeField] MemoryCard cardPrefab;
    List<MemoryCard> _selectedCards = new List<MemoryCard>();

    int maxPoint;
    int _currentPoint;
    int _displayedCard;
    bool _waitingForCall;

    void Start()
    {
        if (launchOnStart) StartMiniGame();
    }

    public override void StartMiniGame()
    {
        Debug.Log("Calling mji game");
        _currentPoint = 0;
        maxPoint = memoryCards.Count;
        foreach (var item in _memoryCardDico)
        {
            Destroy(item.Key);
        }

        _memoryCardDico.Clear();
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        _idsTableRepartitionTable = new int[memoryCards.Count * 2];
        int index = 0;

        for (int i = 0; i < memoryCards.Count; i++)
        {
            _idsTableRepartitionTable[index] = i;
            _idsTableRepartitionTable[index + 1] = i;

            index += 2;
        }
        System.Random rng = new System.Random();
        rng.Shuffle(_idsTableRepartitionTable);

        for (int i = 0; i < _idsTableRepartitionTable.Length; i++)
        {
            int j = _idsTableRepartitionTable[i];
            MemoryCard card = Instantiate(cardPrefab, transform, false);

            card.transform.localPosition = new Vector3(spacingX * (i % column),
                                                        -spacingY * (i / column),
                                                         0f);

            card.Init(memoryCards[j].face, backface, j, memoryCards[j].tiling, memoryCards[j].offset);
            card.onDeselect.AddListener(RemoveCards);
            card.onSelect.AddListener(SelectCard);
            card.onRequestSelect.AddListener(GraphicalSelectCard);
            _memoryCardDico.TryAdd(card, j);
        }

        DisplayAllCards();
    }

    void DisplayAllCards()
    {
        foreach (KeyValuePair<MemoryCard, int> valuePair in _memoryCardDico)
        {
            valuePair.Key.gameObject.SetActive(true);
            valuePair.Key.RevealCard(revealLength);
        }
    }

    void GraphicalSelectCard(MemoryCard card)
    {
        if (_selectedCards.Count == 0)
        {
            _selectedCards.Add(card);
            card.SelectCard(selectedPositionCardFirst);
        }else if (_selectedCards.Count == 1)
        {
            _selectedCards.Add(card);
            card.SelectCard(selectedPositionCardSecond);
        }
    }

    void CheckCards((MemoryCard, MemoryCard) selection)
    {
        if (selection.Item1._id == selection.Item2._id)
        {
            RemoveCards(selection.Item1);
            RemoveCards(selection.Item2);
            selection.Item1.DestroyCard();
            selection.Item2.DestroyCard();
            _currentPoint++;
            if (_currentPoint >= maxPoint)
            {
                EndMiniGame();
            }

        }
        else
        {
            foreach (var item in _selectedCards)
            {
                item.DeselectCard();
            }
            _selectedCards.Clear();
        }
    }

    void SelectCard(MemoryCard card)
    {
        //_selectedCards.Add(card);
        _displayedCard++;
        if (_displayedCard >= 2 && !_waitingForCall)
        {
            _waitingForCall = true;
            Invoke("CallCheckCardFromInvoke", 0.5f);
        }
    }

    void CallCheckCardFromInvoke()
    {
        _waitingForCall = false;
        CheckCards((_selectedCards[0], _selectedCards[1]));
        _displayedCard = 0;
    }

    void RemoveCards(MemoryCard[] cardsToRemove)
    {
    }

    void RemoveCards(MemoryCard card)
    {
        _selectedCards.Remove(card);
    }

    public void EditorDisplay()
    {
        _idsTableRepartitionTable = new int[memoryCards.Count * 2];
        int index = 0;

        for (int i = 0; i < memoryCards.Count; i++)
        {
            _idsTableRepartitionTable[index] = i;
            _idsTableRepartitionTable[index + 1] = i;


            index += 2;
        }

        for (int i = 0; i < _idsTableRepartitionTable.Length; i++)
        {
            int j = _idsTableRepartitionTable[i];
            MemoryCard card = Instantiate(cardPrefab, transform, false);

            card.transform.localPosition = new Vector3(spacingX * (i % column),
                                                        -spacingY * (i / column),
                                                         0f);
        }
    }

    public void EditorRemove()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
