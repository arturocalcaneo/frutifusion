using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Personaje : MonoBehaviour
{
    private string nombre;

    public string Nombre {
        get => nombre;
        set {
            nombre = value;
            
            // Almacenar el personaje seleccionado
            PlayerPrefs.SetString("Personaje", nombre);

            print("Se ha elegido el personaje: " + Nombre);
        }
    }
}