using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class JuegoController : Controller
{
    private BD bd = new BD();

    public IActionResult Index(){
        return View();
    }
    public IActionResult Tutorial(){
        return View();
    }
    public IActionResult Integrantes(){
        return View();
    }

    public IActionResult Iniciar(){
        return View();
    }
    [HttpPost]
    public IActionResult Iniciar(string nombre){
        if (nombre == null){
            nombre = "";
        }
        bool nombreValido = nombre.Length >= 2 && nombre.Length <= 20;
        bool tieneLetras = false;

        foreach (char letra in nombre){
            if (char.IsLetter(letra)){
                tieneLetras = true;
            }
            else if (letra != ' '){
                nombreValido = false;
            }
        }
        if (!tieneLetras){
            nombreValido = false;
        }
        if (!nombreValido){
            ViewBag.Error = "Ingresá un nombre válido usando solo letras.";
            ViewBag.NombreIngresado = nombre;
            return View();
        }

        bd.CrearPartida(new Partidas { NombreParticipante = nombre });
        HttpContext.Session.SetString("NombreJugador", nombre);
        return RedirectToAction("Sala1");
    }

    public IActionResult Sala1(){
        return View();
    }

    [HttpPost]
    public IActionResult SubmitSala1(string respuesta){ /*Revisar*/
        if (respuesta == "1234"){
            return RedirectToAction("Sala2");
        }
        TempData["Error"] = "El código no es correcto. Intentá nuevamente.";
        return RedirectToAction("Sala1");
    }

    public IActionResult Sala2(){
        return View();
    }
    public IActionResult Sala3(){
        return View();
    }
    public IActionResult Sala4(){
        return View();
    }
    public IActionResult Final(){
        ViewBag.Nombre = HttpContext.Session.GetString("NombreJugador");
        return View();
    }


    [HttpPost]
    public IActionResult SubmitSala(int salaId, string respuesta, string extra) /*Revisar*/
    {
        object partidaIdObjeto = HttpContext.Session.GetInt32(PartidaIdKey);
        if (partidaIdObjeto == null)
        {
            return RedirectToAction("Identify");
        }

        int partidaId = Convert.ToInt32(partidaIdObjeto);
        string valorGuardado = string.Empty;
        bool resuelta = false;

        if (salaId == 1)
        {
            string valor = respuesta;
            if (valor == null)
            {
                valor = string.Empty;
            }
            valorGuardado = valor;
            resuelta = valor == "4652";
        }
        else if (salaId == 2)
        {
            string palabraSecreta = HttpContext.Session.GetString(Sala2PalabraClave);
            if (palabraSecreta == null || palabraSecreta.Length == 0)
            {
                palabraSecreta = new PalabrasAhorcado().ObtenerPalabra();
                HttpContext.Session.SetString(Sala2PalabraClave, palabraSecreta);
                HttpContext.Session.SetString(Sala2OcultaClave, new string('_', palabraSecreta.Length));
                HttpContext.Session.SetInt32(Sala2IntentosClave, 10);
                HttpContext.Session.SetString(Sala2LetrasUsadasClave, string.Empty);
            }

            string palabraOculta = HttpContext.Session.GetString(Sala2OcultaClave);
            if (palabraOculta == null)
            {
                palabraOculta = string.Empty;
            }

            string letrasGuardadas = HttpContext.Session.GetString(Sala2LetrasUsadasClave);
            if (letrasGuardadas == null)
            {
                letrasGuardadas = string.Empty;
            }

            List<string> letrasUsadas = new List<string>();
            if (letrasGuardadas.Length > 0)
            {
                letrasUsadas = letrasGuardadas.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            int intentos = ObtenerEnteroDeSesion(Sala2IntentosClave, 10);

            if (intentos <= 0)
            {
                TempData["Sala2Mensaje"] = "Perdiste. La palabra era: " + palabraSecreta;
                valorGuardado = palabraSecreta;
            }
            else
            {
                string letraIngresada = respuesta;
                if (letraIngresada == null)
                {
                    letraIngresada = extra;
                }
                if (letraIngresada == null)
                {
                    letraIngresada = string.Empty;
                }

                letraIngresada = letraIngresada.Trim();
                if (letraIngresada.Length == 0)
                {
                    TempData["Error"] = "Ingresá una letra para jugar.";
                    valorGuardado = palabraOculta;
                }
                else
                {
                    string letra = char.ToUpperInvariant(letraIngresada[0]).ToString();

                    if (letrasUsadas.Contains(letra, StringComparer.OrdinalIgnoreCase))
                    {
                        TempData["Error"] = "La letra '" + letra + "' ya fue usada.";
                        valorGuardado = palabraOculta;
                    }
                    else
                    {
                        char[] letras = palabraOculta.ToCharArray();
                        bool acerto = false;

                        for (int i = 0; i < palabraSecreta.Length; i++)
                        {
                            if (char.ToUpperInvariant(palabraSecreta[i]) == char.ToUpperInvariant(letra[0]))
                            {
                                letras[i] = palabraSecreta[i];
                                acerto = true;
                            }
                        }

                        letrasUsadas.Add(letra);
                        HttpContext.Session.SetString(Sala2LetrasUsadasClave, string.Join(" ", letrasUsadas));

                        if (acerto)
                        {
                            TempData["Sala2Mensaje"] = "¡Bien! La letra '" + letra + "' está en la palabra.";
                        }
                        else
                        {
                            int nuevosIntentos = intentos - 1;
                            HttpContext.Session.SetInt32(Sala2IntentosClave, nuevosIntentos);
                            TempData["Sala2Mensaje"] = "La letra '" + letra + "' no está en la palabra.";

                            if (nuevosIntentos <= 0)
                            {
                                TempData["Sala2Mensaje"] = "Perdiste. La palabra era: " + palabraSecreta;
                                HttpContext.Session.SetString(Sala2OcultaClave, palabraSecreta);
                            }
                        }

                        int intentosActuales = ObtenerEnteroDeSesion(Sala2IntentosClave, 10);
                        if (intentosActuales > 0)
                        {
                            string nuevaPalabra = new string(letras);
                            HttpContext.Session.SetString(Sala2OcultaClave, nuevaPalabra);
                            valorGuardado = nuevaPalabra;

                            bool palabraCompletada = string.Equals(nuevaPalabra, palabraSecreta, StringComparison.OrdinalIgnoreCase);
                            if (palabraCompletada)
                            {
                                TempData["Sala2Mensaje"] = "¡Ganaste! La palabra era: " + palabraSecreta;
                                resuelta = true;
                            }
                        }
                        else
                        {
                            valorGuardado = palabraSecreta;
                        }
                    }
                }
            }
        }
        else if (salaId == 3)
        {
            string valor = extra;
            if (valor == null)
            {
                valor = string.Empty;
            }
            valorGuardado = valor;
            resuelta = valor == "4729";
        }
        else if (salaId == 4)
        {
            string valor = respuesta;
            if (valor == null)
            {
                valor = string.Empty;
            }
            valorGuardado = valor;
            resuelta = string.Equals(valor.Trim(), "Tiempo,Espacio,Realidad,Poder,Mente,Alma", StringComparison.OrdinalIgnoreCase);
        }

        _repo.SaveRespuesta(partidaId, salaId, valorGuardado, resuelta);

        if (!resuelta)
        {
            int intentosSala2 = ObtenerEnteroDeSesion(Sala2IntentosClave, 10);
            if (salaId == 2 && intentosSala2 <= 0)
            {
                return RedirectToAction("Sala", new { id = salaId });
            }

            TempData["Error"] = "Clave incorrecta, intentá de nuevo.";
            return RedirectToAction("Sala", new { id = salaId });
        }

        if (salaId >= 4)
        {
            return RedirectToAction("Victory");
        }

        return RedirectToAction("Sala", new { id = salaId + 1 });
    }


}
