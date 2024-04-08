/*
 * Carga los cursos del profesor
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
  if (rol === "Profesor") {
    document.addEventListener("DOMContentLoaded", function () {
      const token = localStorage.getItem("token"); // Obtiene el token del localStorage

      fetch("https://localhost:44370/api/curso/profesor/cursos", {
        method: "GET",
        headers: {
          Authorization: "Bearer " + token,
        },
      })
        .then((response) => response.json())
        .then((data) => {
          const cursosRow = document.getElementById("cursosRow");
          if (data.length > 0) {
            data.forEach((curso) => {
              const col = document.createElement("div");
              col.classList.add("col-md-3");
              col.innerHTML = `
              <div class="curso-card">
                <img src="assets/img/card.jpg" class="card-img card-img-top" alt="...">
                <div class="card-body">
                  <a href="#" onclick="redireccionarUsuario(${curso.IdCurso})">
                    <h5 class="cardtitle" title="${curso.NombreCurso}">${curso.NombreCurso}</h5>
                  </a>     
                  <p class="card-text" title="${curso.Descripcion}"><strong>Código:</strong> ${curso.CodigoCurso} <br> 
                      <strong>Descripción:</strong> ${curso.Descripcion}</p>
                </div>
              </div>
              `;
              cursosRow.appendChild(col);
            });
          } else {
            cursosRow.innerHTML = "<p>No hay cursos disponibles.</p>";
          }
        })
        .catch((error) => {
          console.error("Error al obtener los cursos:", error);
          document.getElementById("cursosSection").innerHTML =
            "<p>Ocurrió un error al obtener los cursos.</p>";
        });
    });
  }
}
function redireccionarUsuario(idCurso) {
  if (rol === "Estudiante") {
    pagina = `curso-seleccionado-estudiante.html?id=${idCurso}`;
    window.location.href = pagina;
  } else if (rol === "Profesor") {
    pagina = `curso-seleccionado-profesor.html?id=${idCurso}`;
    window.location.href = pagina;
  }
}
