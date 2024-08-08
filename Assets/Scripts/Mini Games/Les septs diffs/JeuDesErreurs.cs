using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JeuDesErreurs : Minigame
{
    [Header("Jeu des erreurs")]
    [SerializeField] CanvasGroup mainCanva;
    [SerializeField] Image image1;
    [SerializeField] Image image2;
    [SerializeField] RectTransform errorLayout;

    [Header("Difficulty")]
    [SerializeField] float differenceToleranceRadius = 1f;

    [Header("Graphique")]
    [SerializeField] Sprite diffSprite; // Displayed
    [SerializeField] Sprite graph1;
    [SerializeField] Sprite graph2;

    [Header("End Animation")]
    [SerializeField] CanvasGroup blocker;
    [SerializeField] Image gradientTop;
    [SerializeField] Image gradientBottom;
    [SerializeField] RectTransform ggText;
    [SerializeField] float textStartPosition;

    Dictionary<string, DifferencePair> differences;
    int amountOfErrorFound;

    private void Start()
    {
        ResetMinigame();
    }

    public override void StartMiniGame()
    {
        ResetMinigame();

        LeanTween.value(0f, 1f, 1f).setOnUpdate((float val) =>
        {
            mainCanva.alpha = val;
        });
        
        mainCanva.interactable = true;
        mainCanva.blocksRaycasts = true;

        image1.sprite = graph1;
        image2.sprite = graph2;

        //Setup events for differences
        InitDifferences();
        CreateMissingReferences();
        //SetDifficulty();
    }

    void ResetMinigame()
    {
        mainCanva.alpha = 0f;
        mainCanva.interactable = false;
        mainCanva.blocksRaycasts = false;

        amountOfErrorFound = 0;
        differences = new Dictionary<string, DifferencePair>();
        for (int i = 0; i < errorLayout.childCount; i++)
        {
            errorLayout.GetChild(i).gameObject.SetActive(false);
        }

        blocker.alpha = 0f;
        blocker.interactable = false;
        blocker.blocksRaycasts = false;
        ggText.localPosition = Vector3.right * textStartPosition;
    }

    void InitDifferences()
    {
        Difference diff;
        
        for (int i = 0; i < image1.rectTransform.childCount; i++)
        {
            diff = image1.rectTransform.GetChild(i).gameObject.AddComponent<Difference>();
            diff.image = diff.GetComponent<Image>();
            diff.onSelect.AddListener(Reveal);

            differences.Add(diff.name, new DifferencePair(diff, null));
        }

        for (int i = 0; i < image2.rectTransform.childCount; i++)
        {
            diff = image2.rectTransform.GetChild(i).gameObject.AddComponent<Difference>();
            diff.image = diff.GetComponent<Image>();
            diff.onSelect.AddListener(Reveal);

            if (differences.TryGetValue(diff.name, out DifferencePair pair))
            {

                differences[diff.name].impair = diff;
            }
            else
            {
                differences.Add(diff.name, new DifferencePair(null, diff));
            }
        }
    }

    void CreateMissingReferences()
    {
        foreach (KeyValuePair<string, DifferencePair> errors in differences)
        {
            if (errors.Value.pair == null && errors.Value.impair != null)
            {
                //Add item to image 1
                differences[errors.Key].pair=MirrorDifference(errors.Value.impair.gameObject,
                    image1.rectTransform);
            }
            else if (errors.Value.impair == null && errors.Value.pair != null)
            {
                //Add item to image 2
                differences[errors.Key].impair=MirrorDifference(errors.Value.pair.gameObject,
                    image2.rectTransform);
            }

            HideErrorGraphic(errors.Value);
            errors.Value.toggle = ShowIndicators();
        }
    }

    Difference MirrorDifference(GameObject objectToMirror, RectTransform parent)
    {
        Difference diff;
        diff = Instantiate(objectToMirror, parent).GetComponent<Difference>();
        diff.image = diff.GetComponent<Image>();
        diff.name = objectToMirror.name;
        diff.onSelect.AddListener(Reveal);
        return diff;
    }

    void Reveal(string diffName)
    {
        DifferencePair p = differences[diffName];
        if (p.revealed) return;
        LeanTween.value(0f, 1f, 0.5f).setOnUpdate((float val) =>
        {
            p.pair.image.fillAmount = val;
            p.impair.image.fillAmount = val;
        });
        LeanTween.imageColor(p.toggle.rectTransform, Color.green, 0.4f);
        p.revealed = true;
        amountOfErrorFound++;
        CheckVictory();
    }

    void CheckVictory()
    {
        if (amountOfErrorFound >= differences.Count)
        {
            Debug.Log("Triggerog anim");
            TriggerEndAnimation();
        }
    }

    //also end the mini game
    void TriggerEndAnimation()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.alphaCanvas(blocker, 1f, 1.5f).setOnComplete(() =>
        {
            LeanTween.moveLocal(ggText.gameObject, Vector3.zero, 0.5f);
        }));
        seq.append(0.3f);
        seq.append(LeanTween.scale(ggText, Vector3.one * 1.5f, 1f).setEaseOutExpo());
        seq.append(LeanTween.scale(ggText, Vector3.one, 0.3f).setEaseInBounce());
        seq.append(LeanTween.moveLocal(ggText.gameObject, Vector3.left * textStartPosition, 0.5f));
        seq.append(LeanTween.alphaCanvas(blocker, 0f, 0.7f));
        seq.append(LeanTween.alphaCanvas(mainCanva, 0f, 1f).setOnComplete(EndMiniGame));
    }

    

    Image ShowIndicators()
    {
        Image temp;
        temp = new GameObject("Indicator").AddComponent<Image>();
        temp.sprite = diffSprite;
        temp.preserveAspect = true;
        temp.rectTransform.SetParent(errorLayout,false);
        return temp;
    }

    void HideErrorGraphic(DifferencePair pair)
    {
        pair.pair.image.fillAmount = 0f;
        pair.impair.image.fillAmount = 0f;
    }

    void SetDifficulty()
    {
        foreach(KeyValuePair<string,DifferencePair> p in differences)
        {
            LeanTween.scale(p.Value.pair.gameObject, Vector3.one * differenceToleranceRadius, 0f);
            LeanTween.scale(p.Value.impair.gameObject, Vector3.one * differenceToleranceRadius, 0f);
        }
    }
}

class DifferencePair
{
    public Difference pair;
    public Difference impair;
    public Image toggle;
    public bool revealed = false;
    public DifferencePair (Difference pair, Difference impair)
    {
        this.pair = pair;
        this.impair = impair;
    }
}
