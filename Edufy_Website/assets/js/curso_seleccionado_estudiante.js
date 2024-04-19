const urlParams = new URLSearchParams(window.location.search);
const idCurso = urlParams.get("id");

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

fetch(
  `https://localhost:44370/api/curso/estudiante/tareas-de-curso?idCurso=${idCurso}`,
  {
    method: "GET",
    headers: {
      Authorization: "Bearer " + token,
    },
  }
)
  .then((response) => response.json())
  .then((tareas) => {
    const tareasDiv = document.getElementById("tareas-pendientes");
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

fetch(
  `https://localhost:44370/api/curso/estudiante/tarea-revisada-de-curso?idCurso=${idCurso}`,
  {
    method: "GET",
    headers: {
      Authorization: "Bearer " + token,
    },
  }
)
  .then((response) => response.json())
  .then((tareas) => {
    const tareasDiv = document.getElementById("tareas-realizadas");
    tareas.forEach((tarea) => {
      const postItem = document.createElement("div");
      postItem.className = "post-item clearfix";
      postItem.innerHTML = `
              <img src="assets/img/PendingTask.jpg" alt="">
              <div class="post-content">
                  <h4><a href="#">${tarea.NombreTarea}</a></h4>
                  <p>${tarea.Descripcion}</p>
                  <h5>Puntaje Obtenido: <span>${tarea.Puntaje}</span></h5>
              </div>`;
      tareasDiv.appendChild(postItem);
    });
  })
  .catch((error) => {
    console.error("Error al obtener las tareas del curso:", error);
    document.getElementById("cursosSection").innerHTML =
      "<p>Ocurrió un error al obtener las tareas del curso.</p>";
  });
