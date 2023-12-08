using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Personaje personaje;
    private string sceneName;
    [SerializeField] public Frase frase;
    [SerializeField] public UIController uiController;
    [SerializeField] public GameObject pictoPronombrePersonal;
    [SerializeField] public GameObject picto_AccionOPeticion;
    [SerializeField] public GameObject[] pictogramasFruta;
    
    private Sprite pictogramaPronombre;
    private Sprite pictogramaAccionOPeticion;

    private System.Random random = new System.Random();
    private List<string> listaFrutas;
    private int currentIndex;
    private int numPictogramasXEscena = 2;

    [SerializeField] public AudioController audioController;
    private string[] fraseGenerada;

    [SerializeField] public Text cronometro;
    private float tiempoPasado = 0f;
    private bool cronometroCorriendo = false;
    
    [SerializeField] public Button altavoz;
    private bool escuchando = false;

    void Start(){
        
        SceneName = SceneManager.GetActiveScene().name;

        if(SceneName == "Nivel1" || SceneName == "Nivel2" || SceneName == "Nivel3" || SceneName == "Nivel4"){
            if (SceneName == "Nivel2"){
                numPictogramasXEscena= 3;
            }else if(SceneName == "Nivel3" || SceneName == "Nivel4"){
                numPictogramasXEscena= 4;

                if(SceneName == "Nivel4"){
                    // Iniciar Cronómetro
                    cronometroCorriendo= true;
                }
            }else{
                numPictogramasXEscena= 2;
            }

            // Si el jugador dió al botón volver a intentar
            if( PlayerPrefs.GetString("volvioAIntentar") == "true" && PlayerPrefs.GetString("perdio") == "true" ){
                fraseGenerada= frase.FraseAlmacenada;
            }else{
                fraseGenerada= frase.GenerarFraseAleatoria();
            }

            
            string pronombre = fraseGenerada[0];
            string accion = fraseGenerada[1];
            string articulo = fraseGenerada[2];
            string fruta = fraseGenerada[3];

            uiController.Frase.text = $"{pronombre}";
            uiController.AccionPeticion.text = $"{accion} {articulo}";
            uiController.Fruta.text = fruta;

            // Colocar Sprites en el Juego...
            // Pictograma para Pronombre Personal
            if(pronombre == "Nosotros"){
                AsignarSprite(pictoPronombrePersonal, "2D/Pronombres/Nosotros"); // Definir el pictograma en el GameObject de Pronombre
            }else if(pronombre == "Vosotros"){
                AsignarSprite(pictoPronombrePersonal, "2D/Pronombres/Ustedes"); // Definir el pictograma en el GameObject de Pronombre
            }else if(pronombre == "Ellos" || pronombre == "Ellas"){
                AsignarSprite(pictoPronombrePersonal, "2D/Pronombres/Ellos"); // Definir el pictograma en el GameObject de Pronombre
            }else{
                AsignarSprite(pictoPronombrePersonal, "2D/Pronombres/" + pronombre); // Definir el pictograma en el GameObject de Pronombre
            }

            // Pictograma para Acciones o Peticiones
            if(accion == "doy" || accion == "das" || accion == "da" || accion == "damos" || accion == "dais" || accion == "dan")
            {
                AsignarSprite(picto_AccionOPeticion, "2D/Acciones_O_Peticiones/Dar");
            }
            else if(accion == "quiero" || accion == "quieres" || accion == "quiere" || accion == "queremos" || accion == "queréis" || accion == "quieren")
            {
                AsignarSprite(picto_AccionOPeticion, "2D/Acciones_O_Peticiones/Quiero");
            }
            else if (accion == "necesito" || accion == "necesitas" || accion == "necesita" || accion == "necesitamos" || accion == "necesitáis" || accion == "necesitan")
            {
                AsignarSprite(picto_AccionOPeticion, "2D/Acciones_O_Peticiones/Quiero");
            }
            else if(accion == "voy a comprar" || accion == "vas a comprar" || accion == "va a comprar" || accion == "vamos a comprar" || accion == "vais a comprar" || accion == "van a comprar")
            {
                AsignarSprite(picto_AccionOPeticion, "2D/Acciones_O_Peticiones/Voy a comprar");
            }
            else if (accion == "estoy comiendo" || accion == "estás comiendo" || accion == "está comiendo" || accion == "estamos comiendo" || accion == "estáis comiendo" || accion == "están comiendo")
            {
                AsignarSprite(picto_AccionOPeticion, "2D/Acciones_O_Peticiones/Comiendo");
            }
            else{
                AsignarSprite(picto_AccionOPeticion, null);
            }

            // Imprimir Nombre del Jugador
            Debug.Log( PlayerPrefs.GetString("NombreJugador") );

            // TODO Cambiar personaje según el que se haya seleccionado.

            // Guardar la frase generada por si el jugador quiere volver a intentar la partida.
            frase.FraseAlmacenada = fraseGenerada;
            
            // Generar las frutas
            GameObject pictogramasPadre = GameObject.Find("Pictogramas");
            GenerarFrutasPictograma( pictogramasPadre, fruta ); // Generar los pictogramas de frutas según el nivel.

            // Escuchar frase en el juego.
            StartCoroutine( ReproducirFraseAudio(fraseGenerada, 3f) );
        }
    }

    public void Button_EscucharFrase(){
        // Volver a escuchar frase en el juego.
        if( escuchando == false ){
            StartCoroutine( ReproducirFraseAudio(fraseGenerada, 0.5f) );
        }
    }

    public void Button_Avanzar(){
        PlayerPrefs.DeleteKey("volvioAIntentar");
        PlayerPrefs.DeleteKey("perdio");
    }

    public void Button_VolverAIntentar(GameObject obj){
        PlayerPrefs.SetString("volvioAIntentar", "true");

        if(obj.tag == "btn_perdiste_reintentar"){
            PlayerPrefs.SetString("perdio", "true");
        }else{
            PlayerPrefs.SetString("perdio", "false");
        }

        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    private void AsignarSprite(GameObject objeto, string rutaSprite)
    {
        Sprite sprite = Resources.Load<Sprite>(rutaSprite);
        if (sprite != null)
        {
            objeto.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            Debug.LogError("Sprite no encontrado: " + rutaSprite);
        }
    }

    public void Button_PersonajeSeleccionado(GameObject button){
        personaje.Nombre = button.tag;
    }

    public string SceneName {
        get => sceneName;
        set {
            sceneName = value;
        }
    }

    private void GenerarFrutasPictograma(GameObject PictogramasPadre, string frutaContemplada){
        Transform parent = PictogramasPadre.transform;

        // Recorrer los GameObject (children) y hacer algo con cada uno de ellos..

        // Generar un número aleatorio entre los 3 números
        List<int> indices = new List<int>();
        listaFrutas = frase.ObtenerListaDeFrutas();

        // Crear lista de índices..
        for (int i = 0; i < parent.childCount; i++)
            indices.Add(i);

        // Barajar la lista..
        for (int i = 0; i < indices.Count; i++)
        {
            int temp = indices[i];
            int randomIndex = random.Next(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        GameObject child;

        // Asignar fruta correcta al GameObject con la ultima posición barajada
        child = parent.GetChild(indices[indices.Count - 1]).gameObject; //Obtener la posición del Pictograma (El GameObject padre), la posición fué barajeada.

        child.transform.GetChild(0).gameObject.tag= frutaContemplada; // Asignar etiqueta al GameObject de la fruta que será la correcta.

        GameObject repositorio= GameObject.Find("FondoRepositorio");
        Transform parent2 = repositorio.transform;
        
        parent2.GetChild(0).gameObject.tag = frutaContemplada;
        //repositorio.transform.tag= fruta;

        AsignarSprite(child.transform.GetChild(0).gameObject, frase.SpriteFruta(frutaContemplada)); // Asignar su sprite según sea la fruta.

        // Lista para frutas excluidas que ya forman parte del juego.
        List<string> valoresExcluidos = new List<string> { frutaContemplada };

        // Seleccionar y utilizar los primeros 'n' índices
        for (int i = 0; i < numPictogramasXEscena; i++) // 'n' es el número de hijos que quieres seleccionar
        {
            child = parent.GetChild(indices[i]).gameObject; //Obtener la posición del Pictograma (El GameObject padre), la posición fué barajeada.

            // SpriteFruta() devuelve un arreglo de dos posiciones
            string[] frutaAleatoria = frase.SpriteFruta(valoresExcluidos);
            valoresExcluidos.Add(frutaAleatoria[0]);
            

            child.transform.GetChild(0).gameObject.tag= frutaAleatoria[0]; // Asignar etiqueta al GameObject de la fruta que será la correcta.
            AsignarSprite(child.transform.GetChild(0).gameObject, frutaAleatoria[1]); // Asignar un sprite aleatorio según las frutas que se dispongan.
        }
    }

    IEnumerator ReproducirFraseAudio(string[] frase, float waitSeconds){
        // Informar al GameManager que la frase está siendo escuchada
        escuchando = true;
        yield return new WaitForSeconds( waitSeconds );

        /**
         *  El nombre de los audios debe coincidir exactamente con la cadena de texto.
        */
        string pronombres = "Audio/pronombres/";
        string accionespeticiones = "Audio/accion_peticion/";
        string articulos = "Audio/";
        string frutas = "Audio/frutas/";

        // Audio según el pronombre:
        pronombres += frase[0];

        // Audio según la acción o petición en la frase:
        accionespeticiones += frase[1];

        // Audio según el artículo:
        articulos += frase[2];

        // Audio según la fruta
        frutas += frase[3];

        // Mostrar Sprite de Altavoz en modo escucha
        Sprite nuevaImagen = Resources.Load<Sprite>("2D/altavoz");
        Image imagenBoton= altavoz.GetComponent<Image>();
                imagenBoton.sprite = nuevaImagen;

        // AUDIO DEL PRONOMBRE
        AudioClip clipPronombre = Resources.Load<AudioClip>(pronombres);
        audioController.PlaySfx(clipPronombre);

        yield return new WaitForSeconds(audioController.AudioSource.clip.length);
        // AUDIO DE LA ACCIÓN Y/O PETICIÓN
        AudioClip clipAP = Resources.Load<AudioClip>(accionespeticiones);
        audioController.PlaySfx(clipAP);

        yield return new WaitForSeconds(audioController.AudioSource.clip.length);
        // AUDIO DEL ARTÍCULO
        AudioClip clipArticulo = Resources.Load<AudioClip>(articulos);
        audioController.PlaySfx(clipArticulo);

        yield return new WaitForSeconds(audioController.AudioSource.clip.length);
        // AUDIO DE LA FRUTA
        AudioClip clipFruta = Resources.Load<AudioClip>(frutas);
        audioController.PlaySfx(clipFruta);

        yield return new WaitForSeconds(audioController.AudioSource.clip.length);
        // Mostrar Sprite de Altavoz en modo escucha
        nuevaImagen = Resources.Load<Sprite>("2D/altavoz_sinescuchar");
        imagenBoton.sprite = nuevaImagen;

        // Informar al GameManager que ya no se está escuchando la frase
        escuchando = false;
    }


    void Update(){
        if(SceneName == "Nivel4"){
            // Actualizar el cronómetro si está en marcha
            if (cronometroCorriendo)
            {
                tiempoPasado += Time.deltaTime;
                ActualizarTextoCronometro();
            }
        }
    }

    void ActualizarTextoCronometro()
    {
        // Formatear el tiempo en minutos:segundos:centésimas
        int minutos = Mathf.FloorToInt(tiempoPasado / 60);
        int segundos = Mathf.FloorToInt(tiempoPasado % 60);

        // Actualizar el texto del cronómetro
        cronometro.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    public void IniciarCronometro()
    {
        // Iniciar el cronómetro
        cronometroCorriendo = true;
    }
}
