using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colision_ValidarFruta : MonoBehaviour
{
    [SerializeField] GameObject pantallaGanaste;
    [SerializeField] GameObject pantallaPerdiste;
    [SerializeField] Personaje personaje;
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO verificar funcioanlidad e implementar código adicional si se requiere.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(gameObject.tag == other.gameObject.tag){
            print("¡Ganaste!");

            
            pantallaGanaste.SetActive(true);
        }else{
            print("Volver a Jugar.");

            pantallaPerdiste.SetActive(true);
        }
    }
}
