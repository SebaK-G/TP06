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

// Sala 2 - Laberinto
const MAZE_DATA = [
    [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
    [1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1],
    [1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1],
    [1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1],
    [1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1],
    [1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1],
    [1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1],
    [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1],
    [1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1],
    [1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1],
    [1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1],
    [1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1],
    [1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
    [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
    [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]
];

const TILE_SIZE = 40;
const PLAYER_RADIUS = 6;
const PLAYER_SPEED = 1.5;

let mazeCanvas = null;
let mazeCtx = null;
let playerX = 20;
let playerY = 20;
const exitX = 13.5;
const exitY = 13;
let keys = {};
let mazecompleted = false;

// Event listeners de teclado
document.addEventListener('keydown', (e) => {
    if (['w', 'a', 's', 'd'].includes(e.key.toLowerCase())) {
        keys[e.key.toLowerCase()] = true;
        e.preventDefault();
    }
}, false);

document.addEventListener('keyup', (e) => {
    if (['w', 'a', 's', 'd'].includes(e.key.toLowerCase())) {
        keys[e.key.toLowerCase()] = false;
    }
}, false);

function isWall(x, y) {
    const gridX = Math.floor(x / TILE_SIZE);
    const gridY = Math.floor(y / TILE_SIZE);

    if (gridY < 0 || gridY >= MAZE_DATA.length || gridX < 0 || gridX >= MAZE_DATA[0].length) {
        return true;
    }
    return MAZE_DATA[gridY][gridX] === 1;
}

function canMoveTo(x, y) {
    const offsets = [[0, 0], [PLAYER_RADIUS, 0], [-PLAYER_RADIUS, 0], [0, PLAYER_RADIUS], [0, -PLAYER_RADIUS]];
    return offsets.every(offset => !isWall(x + offset[0], y + offset[1]));
}

function updateMaze() {
    if (mazecompleted) return;

    let newX = playerX;
    let newY = playerY;

    if (keys['w']) newY -= PLAYER_SPEED;
    if (keys['s']) newY += PLAYER_SPEED;
    if (keys['a']) newX -= PLAYER_SPEED;
    if (keys['d']) newX += PLAYER_SPEED;

    if (canMoveTo(newX, playerY)) playerX = newX;
    if (canMoveTo(playerX, newY)) playerY = newY;

    const distToExit = Math.sqrt(
        Math.pow(playerX - (exitX * TILE_SIZE + TILE_SIZE / 2), 2) +
        Math.pow(playerY - (exitY * TILE_SIZE + TILE_SIZE / 2), 2)
    );

    if (distToExit < 15) completeMaze();
}

function drawMaze() {
    mazeCtx.fillStyle = '#0f1419';
    mazeCtx.fillRect(0, 0, mazeCanvas.width, mazeCanvas.height);

    for (let row = 0; row < MAZE_DATA.length; row++) {
        for (let col = 0; col < MAZE_DATA[row].length; col++) {
            if (MAZE_DATA[row][col] === 1) {
                mazeCtx.fillStyle = '#1a3a52';
                mazeCtx.fillRect(col * TILE_SIZE, row * TILE_SIZE, TILE_SIZE, TILE_SIZE);
                mazeCtx.strokeStyle = '#0d1f2d';
                mazeCtx.lineWidth = 1;
                mazeCtx.strokeRect(col * TILE_SIZE, row * TILE_SIZE, TILE_SIZE, TILE_SIZE);
            }
        }
    }

    mazeCtx.fillStyle = '#00ff00';
    mazeCtx.beginPath();
    mazeCtx.arc(exitX * TILE_SIZE + TILE_SIZE / 2, exitY * TILE_SIZE + TILE_SIZE / 2, 10, 0, Math.PI * 2);
    mazeCtx.fill();

    mazeCtx.strokeStyle = '#00aa00';
    mazeCtx.lineWidth = 2;
    mazeCtx.beginPath();
    mazeCtx.arc(exitX * TILE_SIZE + TILE_SIZE / 2, exitY * TILE_SIZE + TILE_SIZE / 2, 12, 0, Math.PI * 2);
    mazeCtx.stroke();

    mazeCtx.fillStyle = '#00d4ff';
    mazeCtx.beginPath();
    mazeCtx.arc(playerX, playerY, PLAYER_RADIUS, 0, Math.PI * 2);
    mazeCtx.fill();

    mazeCtx.strokeStyle = '#00ffff';
    mazeCtx.lineWidth = 2;
    mazeCtx.beginPath();
    mazeCtx.arc(playerX, playerY, PLAYER_RADIUS, 0, Math.PI * 2);
    mazeCtx.stroke();
}

function completeMaze() {
    mazecompleted = true;
    const statusEl = document.getElementById('mazeStatus');
    if (statusEl) statusEl.style.display = 'block';
    setTimeout(() => {
        const form = document.getElementById('laberintoForm');
        if (form) form.submit();
    }, 1500);
}

function mazeGameLoop() {
    if (mazeCtx) {
        updateMaze();
        drawMaze();
    }
    requestAnimationFrame(mazeGameLoop);
}

// Iniciar cuando el DOM esté listo
setTimeout(() => {
    mazeCanvas = document.getElementById('mazeCanvas');
    if (mazeCanvas) {
        mazeCtx = mazeCanvas.getContext('2d');
        mazeGameLoop();
    }
}, 100);
