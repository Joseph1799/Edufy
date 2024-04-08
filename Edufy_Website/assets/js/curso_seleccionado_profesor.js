function obtenerInformacionCurso() {
  const urlParams = new URLSearchParams(window.location.search);
  const idCurso = urlParams.get("id");

  const token = localStorage.getItem("token"); // Obtiene el token del localStorage

  fetch(`https://localhost:44370/api/curso/cursodetalle?idCurso=${idCurso}`, {
    method: "GET",
    headers: {
      Authorization: "Bearer " + token,
    },
  })
    .then((response) => response.json())
    .then((data) => {
      document.getElementById("Nombre-Curso").textContent = data.NombreCurso;
      document.getElementById("Nombre-Profesor").textContent =
        data.NombreProfesor + " " + data.ApellidoProfesor;
      document.getElementById("Correo-Profesor").textContent =
        data.CorreoElectronicoProfesor;
      document.getElementById("Descripcion-Curso").textContent =
        data.DescripcionCurso;
    })
    .catch((error) => {
      console.error("Error al obtener la información del curso:", error);
      document.getElementById("cursosSection").innerHTML =
        "<p>Ocurrió un error al obtener la información del curso.</p>";
    });

  // Función para mostrar las tareas del curso
  mostrarTareasCurso();
  function mostrarTareasCurso() {
    document.querySelector(".tareas").innerHTML = "";
    fetch(`https://localhost:44370/api/curso/tareascurso?idCurso=${idCurso}`, {
      method: "GET",
      headers: {},
    })
      .then((response) => response.json())
      .then((tareas) => {
        const tareasDiv = document.querySelector(".tareas");
        tareas.forEach((tarea) => {
          const postItem = document.createElement("div");
          postItem.className = "post-item clearfix";
          postItem.innerHTML = `
          <img src="assets/img/PendingTask.jpg" alt="">
          <div class="post-content">
              <h4><a href="#">${tarea.NombreTarea}</a></h4>
              <p>${tarea.Descripcion}</p>
          </div>`;
          tareasDiv.appendChild(postItem);
        });
      })
      .catch((error) => {
        console.error("Error al obtener las tareas del curso:", error);
        document.getElementById("cursosSection").innerHTML =
          "<p>Ocurrió un error al obtener las tareas del curso.</p>";
      });
  }

  // Función para manejar el evento de click en el botón "Remover"
  function handleRemoverButtonClick(id) {
    fetch(
      `https://localhost:44370/api/curso/remover-estudiante?idEstudiante=${id}&idCurso=${idCurso}`,
      {
        method: "DELETE",
        headers: {},
      }
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error("No se pudo eliminar al estudiante del curso.");
        }
        // Ocultar el mensaje después de 3 segundos
        document.getElementById("successMessage2").classList.remove("d-none");
        setTimeout(() => {
          document.getElementById("successMessage2").classList.add("d-none");
        }, 3000);
        poblarListaEstudiantes();
      })
      .catch((error) => {
        console.error("Error al eliminar al estudiante del curso:", error);
      });
  }

  // Función para poblar la lista de estudiantes inscritos en el curso
  poblarListaEstudiantes();
  function poblarListaEstudiantes() {
    // Limpiar la tabla antes de poblarla
    document.querySelector("tbody").innerHTML = "";

    fetch(
      `https://localhost:44370/api/curso/estudiantes-de-curso?idCurso=${idCurso}`,
      {
        method: "GET",
        headers: {},
      }
    )
      .then((response) => response.json())
      .then((estudiantes) => {
        if (estudiantes.length > 0) {
          estudiantes.forEach((estudiante) => {
            // Crear una nueva fila para cada estudiante
            const newRow = document.createElement("tr");

            // Crear celdas para el nombre y correo electrónico del estudiante
            const nombreCell = document.createElement("td");
            nombreCell.textContent = estudiante.Nombre;

            const correoCell = document.createElement("td");
            correoCell.textContent = estudiante.CorreoElectronico;

            // Crear celda para el botón de remover
            const removerCell = document.createElement("td");
            const removerButton = document.createElement("button");
            removerButton.classList.add("btn", "btn-outline-danger", "btn-sm");
            removerButton.textContent = "Remover";
            // Agregar el evento click al botón y llamar a la función handleRemoverButtonClick con el ID del estudiante como argumento
            removerButton.addEventListener("click", () => {
              handleRemoverButtonClick(estudiante.Id);
            });
            removerCell.appendChild(removerButton);

            // Agregar las celdas a la fila
            newRow.appendChild(nombreCell);
            newRow.appendChild(correoCell);
            newRow.appendChild(removerCell);

            // Agregar la fila a la tabla
            document.querySelector("tbody").appendChild(newRow);
          });
        }
      });
  }

  fetch("https://localhost:44370/api/estudiante/todos-los-estudiantes", {
    method: "GET",
    headers: {},
  })
    .then((response) => response.json())
    .then((estudiantes) => {
      const listGroup = document.querySelector(".list-group");

      estudiantes.forEach((estudiante) => {
        const button = document.createElement("button");
        button.type = "button";
        button.classList.add("list-group-item", "list-group-item-action");
        button.textContent = `${estudiante.Id} - ${estudiante.Nombre} ${estudiante.Apellido}`;

        // Agregar evento click al botón para inscribir al estudiante en el curso
        button.addEventListener("click", () => {
          fetch(
            `https://localhost:44370/api/curso/inscribir-estudiante?idEstudiante=${estudiante.Id}&idCurso=${idCurso}`,
            {
              method: "POST",
              headers: {},
            }
          )
            .then((response) => response.json())
            .then((estudiante) => {
              handleOcultarEstudiantes();
              document
                .getElementById("successMessage")
                .classList.remove("d-none");
              // Ocultar el mensaje después de 3 segundos
              setTimeout(() => {
                document
                  .getElementById("successMessage")
                  .classList.add("d-none");
              }, 3000);
              poblarListaEstudiantes();
            });
        });

        listGroup.appendChild(button);
      });
    });
}

// Llamar a la función para obtener la información del curso cuando se cargue la página
document.addEventListener("DOMContentLoaded", obtenerInformacionCurso);
