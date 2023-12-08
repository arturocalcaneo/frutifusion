using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] public Personaje personaje;
    [SerializeField] protected Text frase;
    [SerializeField] protected Text accionpeticion;
    [SerializeField] protected Text fruta;
    private string[] piezasFrase;
    private string[] frutaEnFrase;
    [SerializeField] public AudioController audioController;
    [SerializeField] public AudioClip buttonClickAudio;
    

    public void Button_ComenzarJuego(){
        audioController.PlaySfx(buttonClickAudio);
        SceneManager.LoadScene("SeleccionarPersonaje");
    }

    public void Button_ComoJugar(){
        audioController.PlaySfx(buttonClickAudio);
        print("Como Jugar?");
    }

    public void Button_Salir(){
        string sceneName = SceneManager.GetActiveScene().name;

        audioController.PlaySfx(buttonClickAudio);

        if(sceneName == "Nivel1" || sceneName == "Nivel2" || sceneName == "Nivel3" || sceneName == "Nivel4"){
            SceneManager.LoadScene("Inicio");
        }else{
            Application.Quit();

            // Si el juego está siendo ejecutado desde el editor..
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    [SerializeField] public InputField inputNombre;
    
    public void Button_Avanzar(){
        string sceneName = SceneManager.GetActiveScene().name;

        audioController.PlaySfx(buttonClickAudio); // Emitir sonido al dar clic

        if(sceneName != "Nivel1" && sceneName != "Nivel2" && sceneName != "Nivel3" && sceneName != "Nivel4"){
            // Si no estamos en algún nivel y damos en avanzar, significa que iremos al primer nivel.

            if(sceneName == "SeleccionarPersonaje" && inputNombre != null ){
                PlayerPrefs.SetString("NombreJugador", inputNombre.text);
            }

            SceneManager.LoadScene("Nivel1");
        }else{
            if(sceneName == "Nivel1"){
                SceneManager.LoadScene("Nivel2");
            }else if(sceneName == "Nivel2"){
                SceneManager.LoadScene("Nivel3");
            }else if(sceneName == "Nivel3"){
                SceneManager.LoadScene("Nivel4");
            }
        }
        
    }

    public void Button_VolverAIntentar(){        
        audioController.PlaySfx(buttonClickAudio); // Emitir sonido al dar clic

        //SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    public void VerEstadisticas(){
        SceneManager.LoadScene("Estadistica");
    }

    public void Inicio(){
        SceneManager.LoadScene("Inicio");
    }

    public Text Frase {
        get => frase; //int numeroAleatorio = random.Next(1, 4);
        set => frase = value;
    }

    public Text AccionPeticion {
        get => accionpeticion;
        set => accionpeticion = value;
    }

    public Text Fruta {
        get => fruta;
        set => fruta = value;
    }

    public string[] PiezasFrase {
        get => piezasFrase;
        set {
            piezasFrase = value;

            // Asignar sprite de acuerdo al pronombre y accion
        }
    }
}
