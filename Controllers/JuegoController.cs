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
    public IActionResult Iniciar(){
        return View();
    }
    public IActionResult Tutorial(){
        return View();
    }
    public IActionResult Integrantes(){
        return View();
    }




}
