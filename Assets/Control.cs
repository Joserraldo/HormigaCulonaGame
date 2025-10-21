using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Control : MonoBehaviour
{
    public GameObject personaje;
    public float velocidadRotacion = 60f;
    public float velocidadTraslacion = 0.2f;
    public float factorEscala = 0.1f;

    private bool rotando;

    void Start()
    {
      
        BindClick("HolaMundo", () => Debug.Log("¡Hola Mundo!"));
        BindClick("+", () => Escalar(factorEscala));
        BindClick("-", () => Escalar(-factorEscala));

     
        BindClick("X+", () => Mover(Vector3.right));
        BindClick("X-", () => Mover(Vector3.left));
        BindClick("Y+", () => Mover(Vector3.up));
        BindClick("Y-", () => Mover(Vector3.down));
        BindClick("Z+", () => Mover(Vector3.forward));
        BindClick("Z-", () => Mover(Vector3.back));

     
        BindHold("Rotar", () => rotando = true, () => rotando = false);
    }

    void Update()
    {
        if (rotando && personaje)
            personaje.transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.World);
    }

    
    void Mover(Vector3 dir)
    {
        if (personaje)
            personaje.transform.Translate(dir * velocidadTraslacion, Space.World);
    }

    void Escalar(float f)
    {
        if (!personaje) return;
        Vector3 nueva = personaje.transform.localScale + Vector3.one * f;
        nueva = Vector3.Max(nueva, Vector3.one * 0.1f);
        personaje.transform.localScale = nueva;
    }


    void BindClick(string nombre, Action accion)
    {
        var go = GameObject.Find(nombre);
        var btn = go ? go.GetComponent<Button>() : null;
        if (btn) btn.onClick.AddListener(() => accion());
    }

    void BindHold(string nombre, Action onDown, Action onUp)
    {
        var go = GameObject.Find(nombre);
        if (!go) return;

        var et = go.GetComponent<EventTrigger>() ?? go.AddComponent<EventTrigger>();
        et.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.PointerDown, callback = new EventTrigger.TriggerEvent() });
        et.triggers[^1].callback.AddListener((_) => onDown());

        et.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.PointerUp, callback = new EventTrigger.TriggerEvent() });
        et.triggers[^1].callback.AddListener((_) => onUp());
    }
}
