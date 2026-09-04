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
    public IActionResult Sala1(){
        return View();
    }

    [HttpPost]
    public IActionResult SubmitSala(string respuesta){ /*Revisar*/
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

    [HttpPost]
    public IActionResult Iniciar(string nombre){
        if (nombre == null){
            nombre = "";
        }
        bool nombreValido = nombre.Length >= 2 && nombre.Length <= 40;
        bool tieneLetras = false;

        foreach (char letra in nombre){
            if (char.IsLetter(letra))
            {
                tieneLetras = true;
            }
            else if (letra != ' ')
            {
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


}
