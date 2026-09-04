// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let formulario = document.getElementById("formularioNombre");

if (formulario != null) {
	formulario.addEventListener("submit", function (event) {
		let nombre = document.getElementById("nombre").value;
		let error = document.getElementById("errorNombre");
		let nombreValido = nombre.length >= 2 && nombre.length <= 40;
		let tieneLetras = false;

		for (let letra of nombre) {
			if (/[a-zA-ZáéíóúÁÉÍÓÚñÑ]/.test(letra)) {
				tieneLetras = true;
			}
			else if (letra !== " ") {
				nombreValido = false;
			}
		}

		if (!tieneLetras) {
			nombreValido = false;
		}

		error.textContent = "";

		if (!nombreValido) {
			event.preventDefault();
			error.textContent = "Ingresá un nombre válido usando solo letras.";
		}
	});
}
