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
        bool nombreValido = nombre.Length >= 1 && nombre.Length <= 20;
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
        else if (bd.ExisteNombre(nombre) != null){
            ViewBag.Error = "El nombre ya está en uso. Ingresá otro.";
            ViewBag.NombreIngresado = nombre;
            return View();
        }
        else{
            bd.CrearPartida(new Partidas { NombreParticipante = nombre });
            HttpContext.Session.SetString("NombreJugador", nombre);
            return RedirectToAction("Sala1");
        }
    }
    public IActionResult Final(){
        ViewBag.Nombre = HttpContext.Session.GetString("NombreJugador");
        return View();
    }




    public IActionResult Sala1(){
        return View();
    }
    [HttpPost]
    public IActionResult ResponderSala1(string respuesta){ 
        if (respuesta == "7092"){
            return RedirectToAction("Sala2");
        }
        else{
            ViewBag.Error = "El código no es correcto. Intentá nuevamente.";
            return View("Sala1");
        }
    }
    public IActionResult MensajesSala1(string personaje){
        ViewBag.Personaje = personaje;
        return View("Sala1");
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







}
