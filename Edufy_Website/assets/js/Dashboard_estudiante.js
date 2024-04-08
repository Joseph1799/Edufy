/*
 * Obtener datos del estudiante
 */
obtenerTipoUsuario();
function obtenerTipoUsuario() {
  const token = localStorage.getItem("token");
  if (token) {
    try {
      const decodedToken = parseJwt(token);
      const rol = decodedToken.Rol;
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
    // ------------EJECUTA TODO EL CÓDIGO DE ESTE ARCHIVO SI EL ROL ES ESTUDIANTE-----------
    document.addEventListener("DOMContentLoaded", function () {
      const token = localStorage.getItem("token"); // Obtiene el token del localStorage

      fetch("https://localhost:44370/api/estudiante/datos", {
        method: "GET",
        headers: {
          Authorization: "Bearer " + token,
        },
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error("Error en la respuesta de la red");
          }
          return response.json(); // Parsea la respuesta JSON
        })
        .then((data) => {
          var fotoPerfil = data.FotoPerfil;
          var nombre = data.Nombre;
          var apellido = data.Apellido;
          var carrera = data.Carrera;

          if (fotoPerfil !== "") {
            document.getElementById("profile-img").src = fotoPerfil;
          } else {
            document.getElementById("profile-img").src =
              "assets/img/blank-profile.png";
          }

          document.getElementById("profile-name").innerText =
            nombre + " " + apellido;
          document.getElementById("profile-name2").innerText =
            nombre + " " + apellido;
          document.getElementById("profile-career").innerText = carrera;
        })
        .catch((error) => console.error("Error:", error));
    });
  }
}
