using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MemoryCard : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public Memory memory;
    public int _id { get; private set; }
    bool _inTransition;
    bool _selected;
    [HideInInspector] public bool canSelect;

    [System.NonSerialized] public UnityEvent<MemoryCard> onRequestSelect = new UnityEvent<MemoryCard>();
    [System.NonSerialized] public UnityEvent<MemoryCard> onSelect = new UnityEvent<MemoryCard>();
    [System.NonSerialized] public UnityEvent<MemoryCard> onDeselect = new UnityEvent<MemoryCard>();

    Quaternion startRot;
    Vector3 startPos;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_inTransition) return;
        if (!_selected){
            //SelectCard();
            onRequestSelect?.Invoke(this);
        }
    }

    public void RevealCard(float duration)
    {
        if (_inTransition) return;

        _inTransition = true;

        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 180f, 1f));
        seq.append(duration);
        seq.append(LeanTween.rotateAroundLocal(gameObject, Vector3.back, 180f, 1f).
        setOnComplete(() => { _inTransition = false; }));
    }

    public void SelectCard(Vector3 position)
    {
        _selected = true;
        _inTransition = true;
        LeanTween.moveLocal(gameObject, position, 1f);
        LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 180f, 1f).setOnComplete(() =>
        {
            _inTransition = false;
            onSelect?.Invoke(this);
        });
    }

    public void DeselectCard()
    {
        _selected = false;
        _inTransition = true;
        LeanTween.moveLocal(gameObject, startPos, 1f);
        LeanTween.rotateAroundLocal(gameObject, Vector3.back, 180f, 1f).setOnComplete(() =>
        {
            _inTransition = false;
            onDeselect?.Invoke(this);
        });
    }

    public void DestroyCard()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1f).
        setEaseInBounce().
        setOnComplete(() =>
        {
            transform.localRotation = startRot;
            gameObject.SetActive(false);
        });
    }

    public void AssignImage(Texture2D texture)
    {
        Material mat = GetComponent<Renderer>().materials[0];
        mat.SetTexture("_MainTex", texture);
    }

    public void AssignBackface(Texture2D texture)
    {
        Material mat = GetComponent<Renderer>().materials[1];
        mat.SetTexture("_MainTex", texture);
    }

    public void AssignID(int id)
    {
        _id = id;
    }

    public void Init(Texture2D front, Texture2D back, int id, Vector2 tiling, Vector2 offset)
    {
        startRot = transform.localRotation;
        startPos = transform.localPosition;

        Renderer renderer = GetComponent<Renderer>();
        renderer.materials[0].SetTexture("_MainTex", front);
        renderer.materials[0].SetVector("_TextureTiling", tiling);
        renderer.materials[0].SetVector("_TextureOffset", offset);
        renderer.materials[1].SetTexture("_MainTex", back);

        AssignImage(front);
        AssignBackface(back);
        AssignID(id);
    }
}
