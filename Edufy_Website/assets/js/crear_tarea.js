var boton = document.getElementById("mostrarFormulario");
var formulario = document.getElementById("formulario");
var mostrandoFormulario = false;

function handleMostrarFormulario() {
  if (!mostrandoFormulario) {
    boton.innerText = "Cancelar";
    boton.classList.remove("btn-outline-primary");
    boton.classList.add("btn-outline-danger");
    formulario.style.display = "block";
    mostrandoFormulario = true;
  } else {
    boton.innerText = "Crear Nueva Tarea";
    boton.classList.remove("btn-outline-danger");
    boton.classList.add("btn-outline-primary");
    formulario.style.display = "none";
    mostrandoFormulario = false;
    // Borrar los campos del formulario
    var campos = formulario.querySelectorAll("input, textarea, select");
    campos.forEach(function (campo) {
      campo.value = "";
    });
  }
}
boton.addEventListener("click", function () {
  handleMostrarFormulario();
});

var form = document.querySelector("form");
var botonEnviar = document.getElementById("enviarTareaBtn");
let tareaTitulo = document.getElementById("tareaTitulo");
let tareaFecha = document.getElementById("tareaFecha");
let tareaDesc = document.getElementById("tareaDesc");
let errorMessage = document.getElementById("error-message-titulo");
let errorMessage2 = document.getElementById("error-message-fecha");
let errorMessage3 = document.getElementById("error-message-desc");

botonEnviar.addEventListener("click", function () {
  var formIsValid = true;

  // Verifica si algún campo está vacío
  form.querySelectorAll("input, textarea, select").forEach(function (input) {
    if (!tareaTitulo.value.trim()) {
      errorMessage.style.display = "block";
      formIsValid = false;
    } else {
      errorMessage.style.display = "none";
    }
    if (!tareaFecha.value.trim()) {
      errorMessage2.style.display = "block";
      formIsValid = false;
    } else {
      errorMessage2.style.display = "none";
    }
    if (!tareaDesc.value.trim()) {
      errorMessage3.style.display = "block";
      formIsValid = false;
    } else {
      errorMessage3.style.display = "none";
    }

    if (formIsValid) {
      // Obtener los valores del formulario
      const tareaTitulo = document.getElementById("tareaTitulo").value;
      const tareaFecha = document.getElementById("tareaFecha").value;
      const tareaDesc = document.getElementById("tareaDesc").value;

      // Crear el objeto tarea con los datos del formulario
      const tarea = {
        NombreTarea: tareaTitulo,
        Descripcion: tareaDesc,
        FechaLimite: tareaFecha,
        DocumentoRuta: "",
      };

      // Obtener el token del localStorage
      const token = localStorage.getItem("token");
      // Realizar el fetch
      fetch(`https://localhost:44370/api/curso/creartarea/${idCurso}`, {
        method: "POST",
        headers: {
          Authorization: "Bearer " + token,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(tarea),
      })
        .then((response) => {
          if (response.ok) {
            handleMostrarFormulario();
            obtenerInformacionCurso();
            document
              .getElementById("successMessage3")
              .classList.remove("d-none");
            // Ocultar el mensaje después de 3 segundos
            setTimeout(() => {
              document
                .getElementById("successMessage3")
                .classList.add("d-none");
            }, 3000);
          } else {
            throw new Error("Error al crear la tarea");
          }
        })
        .catch((error) => {
          console.error("Error:", error);
        });
    }
  });
});
