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
