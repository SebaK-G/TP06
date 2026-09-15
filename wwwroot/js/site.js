// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Sala 3 - Viaje temporal
    let posicion = 0;
    let direccion = 2;

    function moverIndicador() {
        posicion += direccion;
        if (posicion >= 100) {
            direccion = -2;
        }
        if (posicion <= 0) {
            direccion = 2;
        }
        document.getElementById("indicador").style.left = posicion + "%";
    }

    setInterval(moverIndicador, 20);

    function activarViaje() {
        if (posicion >= 45 && posicion <= 55) {
            document.getElementById("mensaje").innerText = "¡Salto temporal estabilizado!";
            document.getElementById("botonViaje").style.display = "none";

            setTimeout(function () {
                document.getElementById("formViaje").submit();
            }, 1000);

        } else {
            document.getElementById("mensaje").innerText = "El salto falló. Intentá nuevamente.";
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
            document.getElementById('mensajeDerrota').style.display = 'block';
            document.getElementById('btnIniciar').disabled = false;
        }
    }, 1000);
}

function clickEnThanos() {
    if (!juegoEnProgreso) return;
    
    clicksCount++;
    document.getElementById('clicksCount').textContent = clicksCount;

    if (clicksCount >= 90) {
        juegoEnProgreso = false;
        document.getElementById('tiempoDisplay').style.display = 'none';
        document.getElementById('thanosImg').style.display = 'none';
        document.getElementById('clicksDisplay').style.display = 'none';
        document.getElementById('mensajeFinal').style.display = 'block';
        document.getElementById('btnContinuar').style.display = 'block';
        document.getElementById('btnIniciar').disabled = false;
    }
}
