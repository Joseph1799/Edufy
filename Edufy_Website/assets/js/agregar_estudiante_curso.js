const urlParams = new URLSearchParams(window.location.search);
const idCurso = urlParams.get("id");
var tarjetaEstudiantes = document.getElementById("card-estudiantes");
var mostrandoEstudiantes = false;
var boton2 = document.getElementById("agregar-estudiante");

function handleOcultarEstudiantes() {
  if (!mostrandoEstudiantes) {
    tarjetaEstudiantes.style.display = "block";
    mostrandoEstudiantes = true;
    boton2.innerText = "Cancelar";
    boton2.classList.remove("btn-primary");
    boton2.classList.remove("btn-sm");
    boton2.classList.add("btn-danger");
    boton2.classList.add("btn-sm");
  } else {
    tarjetaEstudiantes.style.display = "none";
    mostrandoEstudiantes = false;
    boton2.innerText = "Agregar Estudiante";
    boton2.classList.remove("btn-danger");
    boton2.classList.add("btn-primary");
    boton2.classList.add("btn-sm");
  }
}

document
  .getElementById("agregar-estudiante")
  .addEventListener("click", function () {
    handleOcultarEstudiantes();
  });
