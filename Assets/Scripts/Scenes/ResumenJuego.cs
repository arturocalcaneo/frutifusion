using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DatosJugador
{
    public string nombre;
    public string personaje;
    public byte nivel;
    public int tiempoUltimoNivel;
}

[CreateAssetMenu(fileName = "NuevoRegistroJugador", menuName = "RegistroJugador")]
public class RegistrosJugadores : ScriptableObject
{
    public List<DatosJugador> jugadores = new List<DatosJugador>();
}

public class ResumenJuego : MonoBehaviour
{
    void Start(){
        string sceneName = SceneManager.GetActiveScene().name;

        if( sceneName == "Estadistica" ){
            // TODO Imprimir resumen del Juego.
        }
    }
}
