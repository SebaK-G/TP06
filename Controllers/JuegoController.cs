using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class JuegoController : Controller
{
    private BD bd = new BD();

    private void GuardarEstadoSala(int numeroSala, string respuesta, string pista)
    {
        string nombreJugador = HttpContext.Session.GetString("NombreJugador");
        if (string.IsNullOrWhiteSpace(nombreJugador))
        {
            return;
        }

        Partidas partida = bd.ObtenerPartidaPorNombre(nombreJugador);
        if (partida == null)
        {
            return;
        }

        HttpContext.Session.SetString($"EstadoSala{numeroSala}", true.ToString());
        bd.GuardarEstadoSala(partida.Id, numeroSala, respuesta, pista);
    }

    private void CargarEstadoDesdeBD()
    {
        string nombreJugador = HttpContext.Session.GetString("NombreJugador");
        if (string.IsNullOrWhiteSpace(nombreJugador))
        {
            return;
        }

        Partidas partida = bd.ObtenerPartidaPorNombre(nombreJugador);
        if (partida == null)
        {
            return;
        }

        HttpContext.Session.SetString("IdPartida", partida.Id.ToString());

        foreach (int numeroSala in new[] { 1, 2, 3, 4 })
        {
            bool completada = bd.TieneSalaCompletada(partida.Id, numeroSala);
            HttpContext.Session.SetString($"EstadoSala{numeroSala}", completada.ToString());
        }
    }

    private IActionResult RedirigirUltimaSalaGuardada()
    {
        string nombreJugador = HttpContext.Session.GetString("NombreJugador");
        if (string.IsNullOrWhiteSpace(nombreJugador))
        {
            return RedirectToAction("Iniciar");
        }

        Partidas partida = bd.ObtenerPartidaPorNombre(nombreJugador);
        if (partida == null)
        {
            return RedirectToAction("Iniciar");
        }

        int? ultimaSala = bd.ObtenerUltimaSalaCompletada(partida.Id);
        if (ultimaSala == null)
        {
            return RedirectToAction("Sala1");
        }

        return ultimaSala switch
        {
            1 => RedirectToAction("Sala2"),
            2 => RedirectToAction("Sala3"),
            3 => RedirectToAction("Sala4"),
            _ => RedirectToAction("Final")
        };
    }

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
        string ultimoNombre = HttpContext.Session.GetString("UltimoNombreJugador");
        if (!string.IsNullOrWhiteSpace(ultimoNombre))
        {
            ViewBag.NombreIngresado = ultimoNombre;
        }
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

        HttpContext.Session.SetString("UltimoNombreJugador", nombre);

        Partidas partidaExistente = bd.ObtenerPartidaPorNombre(nombre);
        if (partidaExistente != null){
            HttpContext.Session.SetString("NombreJugador", nombre);
            CargarEstadoDesdeBD();
            return RedirigirUltimaSalaGuardada();
        }

        bd.CrearPartida(new Partidas { NombreParticipante = nombre });
        HttpContext.Session.SetString("NombreJugador", nombre);
        CargarEstadoDesdeBD();
        return RedirectToAction("Sala1");
    }

    public IActionResult Comenzar(){
        string ultimoNombre = HttpContext.Session.GetString("UltimoNombreJugador");
        if (!string.IsNullOrWhiteSpace(ultimoNombre))
        {
            return RedirectToAction("NuevaPartida");
        }

        return RedirectToAction("Iniciar");
    }

    public IActionResult NuevaPartida(){
        string nombre = HttpContext.Session.GetString("UltimoNombreJugador") ?? HttpContext.Session.GetString("NombreJugador");

        if (string.IsNullOrWhiteSpace(nombre)){
            return RedirectToAction("Iniciar");
        }

        Partidas partida = bd.ObtenerPartidaPorNombre(nombre);
        if (partida != null){
            bd.ReiniciarPartida(partida.Id);
        }

        HttpContext.Session.Clear();
        HttpContext.Session.SetString("NombreJugador", nombre);
        HttpContext.Session.SetString("UltimoNombreJugador", nombre);
        return RedirectToAction("Sala1");
    }

    public IActionResult Sala1(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        CargarEstadoDesdeBD();
        if (HttpContext.Session.GetString("EstadoSala1") == true.ToString()){
            return RedirectToAction("Sala2");
        }
        else{
            return View();
        }
    }
    [HttpPost]
    public IActionResult ResponderSala1(string respuesta){ 
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("NombreJugador"))){
            return RedirectToAction("Iniciar");
        }

        if (respuesta == "5675"){
            GuardarEstadoSala(1, respuesta, "Código principal");
            HttpContext.Session.SetString("EstadoSala1", true.ToString());
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
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        CargarEstadoDesdeBD();
        if (HttpContext.Session.GetString("EstadoSala1") != true.ToString()){
            return RedirectToAction("Sala1");
        }
        return View();
    }
    [HttpPost]
    public IActionResult CompletarSala2(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        if (HttpContext.Session.GetString("EstadoSala1") != true.ToString()){
            return RedirectToAction("Sala1");
        }
        GuardarEstadoSala(2, "Laberinto completado", "Salida verde");
        return RedirectToAction("Sala3");
    }
    public IActionResult Sala3(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        CargarEstadoDesdeBD();
        if (HttpContext.Session.GetString("EstadoSala2") != true.ToString()){
            return RedirectToAction("Sala2");
        }
        HttpContext.Session.SetString("EstadoSala3EnCurso", true.ToString());
        return View();
    }
    [HttpPost]
    public IActionResult CompletarSala3(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        if (HttpContext.Session.GetString("EstadoSala2") != true.ToString()){
            return RedirectToAction("Sala2");
        }
        if (HttpContext.Session.GetString("EstadoSala3EnCurso") != true.ToString()){
            return RedirectToAction("Sala3");
        }
        if (HttpContext.Session.GetString("EstadoSala3") == true.ToString()){
            return RedirectToAction("Sala4");
        }

        GuardarEstadoSala(3, "Viaje temporal estabilizado", "Momento correcto");
        HttpContext.Session.SetString("EstadoSala3", true.ToString());
        HttpContext.Session.SetString("EstadoSala3EnCurso", false.ToString());
        return RedirectToAction("Sala4");
    }  
    public IActionResult Sala4(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        CargarEstadoDesdeBD();
        if (HttpContext.Session.GetString("EstadoSala3") != true.ToString()){
            return RedirectToAction("Sala3");
        }
        HttpContext.Session.SetString("EstadoSala4EnCurso", true.ToString());
        return View();
    }
    [HttpPost]
    public IActionResult CompletarSala4(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }
        if (HttpContext.Session.GetString("EstadoSala3") != true.ToString()){
            return RedirectToAction("Sala3");
        }
        if (HttpContext.Session.GetString("EstadoSala4EnCurso") != true.ToString()){
            return RedirectToAction("Sala4");
        }
        if (HttpContext.Session.GetString("EstadoSala4") == true.ToString()){
            return RedirectToAction("Final");
        }

        GuardarEstadoSala(4, "Thanos vencido", "Última batalla");
        HttpContext.Session.SetString("EstadoSala4", true.ToString());
        HttpContext.Session.SetString("EstadoSala4EnCurso", false.ToString());
        return RedirectToAction("Final");
    }  
    public IActionResult Final(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Sala4");
        }
        else{
            string nombre = HttpContext.Session.GetString("NombreJugador");
            ViewBag.Nombre = nombre;
            ViewBag.TiempoPartida = bd.CalcularTiempoPartida(nombre);
            HttpContext.Session.SetString("UltimoNombreJugador", nombre);
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("UltimoNombreJugador", nombre);
            return View();
        }
    }

    public IActionResult Lobby(){
        if (HttpContext.Session.GetString("NombreJugador") == null){
            return RedirectToAction("Iniciar");
        }

        string salaRetorno = HttpContext.Session.GetString("SalaRetorno") ?? "Sala1";
        ViewBag.SalaRetorno = salaRetorno;
        ViewBag.NombreJugador = HttpContext.Session.GetString("NombreJugador");
        return View();
    }

    [HttpPost]
    public IActionResult SalirSala(string sala){
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("NombreJugador"))){
            return RedirectToAction("Iniciar");
        }

        if (!string.IsNullOrWhiteSpace(sala)){
            HttpContext.Session.SetString("SalaRetorno", sala);
        }

        return RedirectToAction("Lobby");
    }




}
