// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Sala 3 - Viaje temporal
let posicion = 0;
let direccion = 1;
let ronda = 1;
const totalRondas = 5;
let velocidad = 2;
let anchoZona = 10;

function moverIndicador() {
    const indicador = document.getElementById("indicador");

    if (!indicador) return;

    posicion += direccion * velocidad / 2;
    if (posicion >= 100) {
        posicion = 100;
        direccion = -1;
    }
    if (posicion <= 0) {
        posicion = 0;
        direccion = 1;
    }
    indicador.style.left = posicion + "%";
}

setInterval(moverIndicador, 20);

function activarViaje() {
    const mitadZona = anchoZona / 2;
    const limiteInferior = 50 - mitadZona;
    const limiteSuperior = 50 + mitadZona;
    const mensaje = document.getElementById("mensaje");

    if (posicion >= limiteInferior && posicion <= limiteSuperior) {
        if (ronda === totalRondas) {
            mensaje.innerText = "¡Viaje temporal estabilizado!";
            document.getElementById("botonViaje").style.display = "none";

            setTimeout(function () {
                document.getElementById("formViaje").submit();
            }, 1000);
            return;
        }

        ronda++;
        velocidad += 0.8;
        anchoZona -= 1;
        posicion = 0;
        direccion = 1;
        document.getElementById("travelZone").style.left = (50 - anchoZona / 2) + "%";
        document.getElementById("travelZone").style.width = anchoZona + "%";
        mensaje.innerText = `Ronda ${ronda} de ${totalRondas}. Ahora es más rápido.`;
    } else {
        mensaje.innerText = `El salto falló. Intentá nuevamente. Ronda ${ronda} de ${totalRondas}.`;
    }
}



// Sala 4 - Batalla Final
let tiempoRestante = 10;
let clicksCount = 0;
let juegoEnProgreso = false;

function iniciarTemporizador() {
    tiempoRestante = 10;
    clicksCount = 0;
    juegoEnProgreso = true;
    document.getElementById('btnIniciar').disabled = true;
    document.getElementById('tiempoDisplay').style.display = 'block';
    document.getElementById('thanosImg').style.display = 'block';
    document.getElementById('clicksDisplay').style.display = 'block';
    document.getElementById('mensajeFinal').style.display = 'none';
    document.getElementById('mensajeDerrota').style.display = 'none';
    document.getElementById('clicksCount').textContent = '0';

    const intervalo = setInterval(() => {
        tiempoRestante--;
        document.getElementById('tiempoDisplay').textContent = `00:${String(tiempoRestante).padStart(2, '0')}`;

        if (tiempoRestante <= 0) {
            clearInterval(intervalo);
            juegoEnProgreso = false;
            document.getElementById('thanosImg').style.display = 'none';
            document.getElementById('clicksDisplay').style.display = 'none';
            if (clicksCount >= 50){
                document.getElementById('mensajeDerrota').style.display = 'none';
            }
            else{
                document.getElementById('mensajeDerrota').style.display = 'block';
            }
            document.getElementById('btnIniciar').disabled = false;
        }
    }, 1000);
}

function clickEnThanos() {
    if (!juegoEnProgreso) return;

    clicksCount++;
    document.getElementById('clicksCount').textContent = clicksCount;

    if (clicksCount >= 50) {
        juegoEnProgreso = false;
        document.getElementById('tiempoDisplay').style.display = 'none';
        document.getElementById('thanosImg').style.display = 'none';
        document.getElementById('clicksDisplay').style.display = 'none';
        document.getElementById('mensajeFinal').style.display = 'block';
        document.getElementById('btnContinuar').style.display = 'block';
        document.getElementById('btnIniciar').disabled = false;
        document.getElementById('btnIniciar').style.display = 'none';

    }
}
