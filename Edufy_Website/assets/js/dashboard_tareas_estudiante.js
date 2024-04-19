/*
 * Obtener datos del estudiante
 */
var rol = "";
obtenerTipoUsuario();
function obtenerTipoUsuario() {
  const token = localStorage.getItem("token");
  if (token) {
    try {
      const decodedToken = parseJwt(token);
      rol = decodedToken.Rol;
      cargarScript(rol);
    } catch (error) {
      console.error("Error al decodificar el token:", error);
    }
  } else {
    console.error("Token no encontrado en el localStorage");
  }
}

function parseJwt(token) {
  const base64Url = token.split(".")[1];
  const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  const jsonPayload = decodeURIComponent(
    atob(base64)
      .split("")
      .map(function (c) {
        return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
      })
      .join("")
  );

  const { unique_name, CorreoElectronico, Rol } = JSON.parse(jsonPayload);
  return { id: unique_name, CorreoElectronico, Rol };
}

function cargarScript(rol) {
  if (rol === "Estudiante") {
    document.addEventListener("DOMContentLoaded", function () {
      const token = localStorage.getItem("token"); // Obtiene el token del localStorage

      let allData = []; // Variable para almacenar todas las tareas originales
      let filteredData = []; // Variable para almacenar las tareas filtradas

      fetch("https://localhost:44370/api/curso/estudiante/tareas", {
        method: "GET",
        headers: {
          Authorization: "Bearer " + token,
        },
      })
        .then((response) => response.json())
        .then((data) => {
          allData = data; // Almacena todas las tareas originales

          const cursosRow = document.getElementById("activityItems");
          let currentStartDate = null;
          let currentEndDate = null;

          renderTareas(allData); // Renderiza todas las tareas al cargar la página

          const searchForm = document.querySelector(".search-form");
          searchForm.addEventListener("submit", function (event) {
            event.preventDefault(); // Evita el envío del formulario

            const query = this.querySelector(
              'input[name="query"]'
            ).value.toLowerCase();

            // Filtra las tareas por el nombre del curso
            filteredData = allData.filter((tarea) =>
              tarea.NombreCurso.toLowerCase().includes(query)
            );

            renderTareas(filteredData);
          });

          // Escucha el evento 'input' para detectar cambios en el campo de búsqueda
          searchForm
            .querySelector('input[name="query"]')
            .addEventListener("input", function () {
              const query = this.value.toLowerCase();

              if (query === "") {
                filteredData = allData; // Restablece las tareas filtradas a todas las tareas originales
                renderTareas(filteredData);
              }
            });

          function renderTareas(tareas) {
            cursosRow.innerHTML = ""; // Limpia el contenido actual
            let currentStartDate = null;
            let currentEndDate = null;

            if (tareas.length > 0) {
              tareas.forEach((tarea) => {
                // Convertir la fecha de inicio a un formato legible
                const startDate = new Date(tarea.FechaInicio);
                const formattedStartDate = startDate.toLocaleDateString(
                  "es-ES",
                  {
                    month: "long",
                    day: "numeric",
                  }
                );

                // Convertir la fecha límite a un formato legible
                const endDate = new Date(tarea.FechaLimite);
                const formattedEndDate = endDate.toLocaleDateString("es-ES", {
                  weekday: "long",
                  year: "numeric",
                  month: "long",
                  day: "numeric",
                });

                // Convertir la fecha límite a un formato legible
                const endDate2 = new Date(tarea.FechaLimite);
                const formattedEndDate2 = endDate.toLocaleDateString("es-ES", {
                  weekday: "long",
                  day: "numeric",
                });

                // Si la fecha de inicio es diferente a la fecha actual, mostrar una nueva fecha en cards-dates
                if (formattedEndDate !== currentEndDate) {
                  cursosRow.innerHTML += `
              <div class="cards-dates">
                <h7>${formattedEndDate}</h7>
              </div>`;
                  currentEndDate = formattedEndDate;
                }

                // Mostrar la tarea
                cursosRow.innerHTML += `
            <div class="activity-item">
              <div class="activite-label">${formattedStartDate}</div>
              <div class="activity-content">
                <div class="item-content">
                  <div class="icon">
                    <i class="bi bi-clipboard"></i>
                  </div>
                  <div class="title-description ms-2">
                  <a href="#" onclick="redireccionarUsuario(${tarea.IdTarea}, ${tarea.IdCurso})">
                      <h6>${tarea.NombreCurso}</h6>
                    </a>
                    <p>${tarea.Descripcion}</p>
                    <small>Due date: ${formattedEndDate2}</small>
                  </div>
                </div>
              </div>
            </div>`;
              });
            } else {
              cursosRow.innerHTML = "<p>No hay tareas disponibles.</p>";
            }
          }
        })
        .catch((error) => {
          console.error("Error al obtener las tareas:", error);
          document.getElementById("activityItems").innerHTML =
            "<p>Ocurrió un error al obtener las tareas.</p>";
        });
    });
  }
}

function redireccionarUsuario(idTarea, idCurso) {
  if (rol === "Estudiante") {
    pagina = `entregar-tarea.html?idTarea=${idTarea}&idCurso=${idCurso}`;
  } else {
    pagina = `tareas-entregadas.html?idTarea=${idTarea}&idCurso=${idCurso}`;
  }
  window.location.href = pagina;
}
