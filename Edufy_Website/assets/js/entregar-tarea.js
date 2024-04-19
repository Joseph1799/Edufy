const urlParams = new URLSearchParams(window.location.search);
const idCurso = urlParams.get("idCurso");
const idTarea = urlParams.get("idTarea");

const token = localStorage.getItem("token"); // Obtiene el token del localStorage

fetch(
  `https://localhost:44370/api/curso/estudiante/curso-detalle?idCurso=${idCurso}`,
  {
    method: "GET",
    headers: {
      Authorization: "Bearer " + token,
    },
  }
)
  .then((response) => response.json())
  .then((curso) => {
    document.getElementById("Nombre-Curso").textContent = curso.NombreCurso;
    document.getElementById("Nombre-Profesor").textContent =
      curso.NombreProfesor + " " + curso.ApellidoProfesor;
    document.getElementById("Correo-Profesor").textContent =
      curso.CorreoElectronicoProfesor;
  })
  .catch((error) => {
    console.error("Error al obtener la información del curso:", error);
    document.getElementById("cursosSection").innerHTML =
      "<p>Ocurrió un error al obtener la información del curso.</p>";
  });

function entregarTarea(docuEntregadoRuta) {
  fetch(
    `https://localhost:44370/api/tarea/entregar-tarea/${docuEntregadoRuta}/${idTarea}`,
    {
      method: "PUT",
      headers: {
        Authorization: "Bearer " + token,
      },
    }
  )
    .then((response) => {
      if (response.ok) {
        console.log("Tarea entregada correctamente");
        let timerInterval;
        Swal.fire({
          title: "Tarea entregada con exito!",
          html: "Redirigiendo página <b></b> milliseconds.",
          timer: 2000,
          timerProgressBar: true,
          didOpen: () => {
            Swal.showLoading();
            const timer = Swal.getPopup().querySelector("b");
            timerInterval = setInterval(() => {
              timer.textContent = `${Swal.getTimerLeft()}`;
            }, 100);
          },
          willClose: () => {
            clearInterval(timerInterval);
          },
        }).then((result) => {
          /* Read more about handling dismissals below */
          if (result.dismiss === Swal.DismissReason.timer) {
            window.location.href = "Dashboard.html";
          }
        });
      } else {
        throw new Error("Error al entregar la tarea");
      }
    })
    .then((data) => {
      console.log("Respuesta del servidor:", data);
    })
    .catch((error) => {
      console.error("Error al enviar el archivo:", error);
    });
}

fetch(
  `https://localhost:44370/api/tarea/obtenerTareaPorId?idTarea=${idTarea}`,
  {
    method: "GET",
    headers: {
      Authorization: "Bearer " + token,
    },
  }
)
  .then((response) => response.json())
  .then((tarea) => {
    document.getElementById("titulo-tarea").textContent = tarea.NombreTarea;
    document.getElementById("indicaciones-tarea").textContent =
      tarea.Descripcion;
    // Convertir la fecha límite a un formato legible
    const endDate = new Date(tarea.FechaLimite);
    const formattedEndDate = endDate.toLocaleDateString("es-ES", {
      weekday: "long",
      year: "numeric",
      month: "long",
      day: "numeric",
    });
    document.getElementById("fecha-entrega").textContent = formattedEndDate;
  });

var boton = document.getElementById("entregarTarea");
boton.addEventListener("click", function () {
  var input = document.getElementById("formFile");
  var file = input.files[0];

  var formData = new FormData();
  formData.append("archivo", file);

  fetch("/guardar-archivo", {
    method: "POST",
    body: formData,
  })
    .then((response) => {
      if (response.ok) {
        return response.json();
      } else {
        throw new Error("Error al enviar archivo");
      }
    })
    .then((data) => {
      var docuEntregadoRuta = data.ruta;
      entregarTarea(docuEntregadoRuta);
    })
    .catch((error) => {
      console.error("Error al enviar archivo:", error);
    });
});
