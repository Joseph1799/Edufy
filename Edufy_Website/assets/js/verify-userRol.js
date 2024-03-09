document.addEventListener("DOMContentLoaded", function () {
  obtenerTipoUsuario();
});

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
  const script = document.createElement("script");
  if (rol === "Estudiante") {
    script.src = "assets/js/estudiante-profile.js";
  } else if (rol === "Profesor") {
    script.src = "assets/js/profesor-profile.js";
  } else {
    console.error("Tipo de usuario no válido");
    return;
  }
  script.onload = function () {
    console.log("Script cargado correctamente");
  };
  document.head.appendChild(script);
}
