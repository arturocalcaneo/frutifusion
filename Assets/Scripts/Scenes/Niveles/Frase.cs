using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Frase : MonoBehaviour
{
    private string pronombre;
    private string accion_peticion;
    private string fruta;
    private System.Random rand= new System.Random();

    private readonly string[] pronombresSingular = { "Él", "Ella", "Usted", "Yo", "Tú" };
    private readonly string[] pronombresPlural = { "Ellos", "Ellas", "Ustedes", "Nosotros"/*, "Vosotros"*/ };


    private readonly Dictionary<string, string[]> acciones = new Dictionary<string, string[]>
    {
        { "yo", new string[] { "doy", "quiero", "necesito", "voy a comprar", "estoy comiendo" } },
        { "tú", new string[] { "das", "quieres", "necesitas", "vas a comprar", "estás comiendo" } },
        { "él/ella/usted", new string[] { "da", "quiere", "necesita", "va a comprar", "está comiendo" } },
        { "nosotros", new string[] { "damos", "queremos", "necesitamos", "vamos a comprar", "estamos comiendo" } },
        /* { "vosotros", new string[] { "das", "queréis", "necesitáis", "vais a comprar", "estáis comiendo" } }, */
        { "ellos/ellas/ustedes", new string[] { "dan", "quieren", "necesitan", "van a comprar", "están comiendo" } }
    };


    private readonly string[] frutas = {"coco","limón","mango","manzana","melón","pera","piña","plátano","uva"};

    private readonly Dictionary<string, string> spritesFrutas = new Dictionary<string, string>{
        {"coco", "coco"},
        {"limón", "limón"},
        {"durazno", "durazno"},
        {"manzana", "manzana"},
        {"melón", "melón"},
        {"pera", "pera"},
        {"piña", "piña"},
        {"plátano", "plátano"},
        {"uva", "uva"},
        {"mango", "mango"}
    };

    public string[] GenerarFraseAleatoria()
    {
        // Determinar al azar si el pronombre será en singular o plural
        bool esPlural = rand.Next(2) == 0; // 50% de probabilidad para plural

        string pronombre = esPlural ? pronombresPlural[rand.Next(pronombresPlural.Length)] 
                                    : pronombresSingular[rand.Next(pronombresSingular.Length)];

        // Elegir una acción acorde al tipo de pronombre
        string grupoPronombre = DeterminarGrupoPronombre(pronombre);
        string accion = acciones[grupoPronombre][rand.Next(acciones[grupoPronombre].Length)];

        // Elegir una fruta al azar
        string fruta = frutas[rand.Next(frutas.Length)];

        // Determinar el artículo correcto para la fruta
        string articulo = DeterminarArticulo(fruta);

        string[] piezas = {pronombre, accion, articulo, fruta};
        
        return piezas;
    }

    // Función para determinar el artículo basado en la última letra de la fruta
    private string DeterminarArticulo(string fruta)
    {
        return fruta.EndsWith("o") || fruta.EndsWith("ón") || fruta.EndsWith("on") ? "un" : "una";
    }

    // Función para determinar el grupo de pronombre basado en el pronombre seleccionado
    private string DeterminarGrupoPronombre(string pronombre)
    {
        switch (pronombre)
        {
            case "Yo":
                return "yo";
            case "Tú":
                return "tú";
            case "Nosotros":
            case "Nosotras":
                return "nosotros";
            /*case "Vosotros":
            case "Vosotras":
                return "vosotros";*/
            case "Ellos":
            case "Ellas":
            case "Ustedes":
                return "ellos/ellas/ustedes";
            default: // Para "Él", "Ella", "Usted"
                return "él/ella/usted";
        }
    }

    public List<string> ObtenerListaDeFrutas(){
        List<string> listaFrutas = new List<string>();

        for(int i=0; i < frutas.Length; i++){
            listaFrutas.Add(frutas[i]);
        }

        return listaFrutas;
    }

    public string[] SpriteFruta(List<string> valoresExcluidos){
        string[] array= new string[2];
        
        array[0]= ObtenerValorAleatorioExcluyendo(spritesFrutas, valoresExcluidos);
        array[1]= "2D/PictogramasFrutas/" + array[0];

        return array;
    }

    public string SpriteFruta(string fruta){
        return "2D/PictogramasFrutas/" + spritesFrutas[fruta];
    }

    private string ObtenerValorAleatorioExcluyendo(Dictionary<string, string> dict, List<string> valoresExcluidos)
    {
        // Filtra los valores que no estén en la lista de valores excluidos
        var valoresDisponibles = dict.Values.Where(valor => !valoresExcluidos.Contains(valor)).ToList();

        // Verifica si hay valores disponibles después de la exclusión
        if (valoresDisponibles.Count > 0)
        {
            // Genera un índice aleatorio para seleccionar un valor
            int indiceAleatorio = UnityEngine.Random.Range(0, valoresDisponibles.Count);
            
            // Retorna el valor aleatorio seleccionado
            return valoresDisponibles[indiceAleatorio];
        }
        else
        {
            // Retorna un valor por defecto si no hay valores disponibles
            return null;
        }
    }

    public void RemoverSpriteFruta(string fruta){
        spritesFrutas.Remove(fruta);
    }

    public string[] FraseAlmacenada {
        get{
            string[] temp = {
                PlayerPrefs.GetString("F_Pronombre"),
                PlayerPrefs.GetString("F_AP"), // Acción y/o Petición
                PlayerPrefs.GetString("F_Articulo"),
                PlayerPrefs.GetString("F_Fruta"),
            };
            return temp;
        }

        set{
            PlayerPrefs.SetString("F_Pronombre", value[0]);
            PlayerPrefs.SetString("F_AP", value[1]); // Acción y/o Petición
            PlayerPrefs.SetString("F_Articulo", value[2]);
            PlayerPrefs.SetString("F_Fruta", value[3]);
        }
    }
}