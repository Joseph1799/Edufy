/*
 * Obtener datos del estudiante
 */
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
      var correoElectronico = data.CorreoElectronico;
      var id = data.Id;
      var fechaNacimiento = data.FechaNacimiento;
      var anioIngreso = data.AnioIngreso;
      var genero = data.Genero;

      if (fotoPerfil !== "") {
        document.getElementById("profile-img").src = fotoPerfil;
      } else {
        document.getElementById("profile-img").src =
          "assets/img/blank-profile.png";
      }

      if (fotoPerfil !== "") {
        document.getElementById("profile-img2").src = fotoPerfil;
      } else {
        document.getElementById("profile-img2").src =
          "assets/img/blank-profile.png";
      }

      if (fotoPerfil !== "") {
        document.getElementById("profile-img-big").src = fotoPerfil;
      } else {
        document.getElementById("profile-img-big").src =
          "assets/img/blank-profile.png";
      }

      document.getElementById("profile-name").innerText =
        nombre + " " + apellido;
      document.getElementById("profile-name2").innerText =
        nombre + " " + apellido;
      document.getElementById("profile-name3").innerText =
        nombre + " " + apellido;
      document.getElementById("profile-name4").innerText =
        nombre + " " + apellido;
      document.getElementById("fullName").value = nombre + " " + apellido;
      document.getElementById("profile-career").innerText = carrera;
      document.getElementById("profile-career2").innerText = carrera;
      document.getElementById("profile-email").innerText = correoElectronico;
      document.getElementById("profile-email2").value = correoElectronico;
      document.getElementById("profile-id").innerText = id;
      document.getElementById("profile-id2").value = id;
      document.getElementById("profile-birth-date").innerText = fechaNacimiento;
      document.getElementById("profile-birth-date2").value = fechaNacimiento;
      document.getElementById("profile-issued").innerText = anioIngreso;
      document.getElementById("profile-issued2").value = anioIngreso;
      document.getElementById("profile-gender").innerText = genero;
      document.getElementById("profile-gender2").value = genero;
    })
    .catch((error) => console.error("Error:", error));
});

// Función para enviar la solicitud PUT al endpoint de modificación de estudiante
function actualizarDatosEstudiante(datos) {
  // Obtener el token del localStorage
  const token = localStorage.getItem("token");

  console.log("Datos a enviar:", datos);

  // Enviar la solicitud PUT al endpoint
  fetch("https://localhost:44370/api/estudiante/modificar", {
    method: "PUT",
    headers: {
      Authorization: "Bearer " + token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(datos),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Error en la respuesta de la red");
      }
      return response.json(); // Parsear la respuesta JSON
    })
    .then((data) => {
      console.log("Datos actualizados exitosamente:", data);
      // Puedes agregar aquí lógica adicional para actualizar la UI o mostrar un mensaje de éxito
    })
    .catch((error) => console.error("Error:", error));
}

// Agregar eventListener al formulario de edición
document.addEventListener("DOMContentLoaded", function () {
  document.querySelector("form").addEventListener("submit", function (event) {
    event.preventDefault(); // Prevenir el envío del formulario

    // Obtener el valor completo del nombre
    const fullName = document.getElementById("fullName").value;
    // Dividir el nombre completo en partes separadas por espacios en blanco
    const parts = fullName.split(" ");

    // El primer elemento es el nombre
    const nombre = parts[0];

    // Los elementos restantes son los apellidos
    const apellido = parts.slice(1).join(" ");
    // Obtener los valores de los demás campos del formulario
    const correoElectronico = document.getElementById("profile-email2").value;
    const id = document.getElementById("profile-id2").value;
    const fechaNacimiento = document.getElementById(
      "profile-birth-date2"
    ).value;
    const anioIngreso = document.getElementById("profile-issued2").value;
    const genero = document.getElementById("profile-gender2").value;

    // Verificar si los campos requeridos están vacíos
    if (
      !nombre ||
      !apellido ||
      !correoElectronico ||
      !fechaNacimiento ||
      !anioIngreso ||
      !genero
    ) {
      alert("Por favor, completa todos los campos");
      return; // Detener la ejecución si algún campo está vacío
    }

    // Llamar a la función para actualizar los datos del estudiante
    actualizarDatosEstudiante({
      Id: id,
      Nombre: nombre,
      Apellido: apellido,
      CorreoElectronico: correoElectronico,
      FechaNacimiento: fechaNacimiento,
      AnioIngreso: anioIngreso,
      Genero: genero,
    });
  });
});

// Función para enviar la solicitud PUT al endpoint de modificación de contraseña de estudiante
function actualizarContraseniaEstudiante(datos) {
  // Obtener el token del localStorage
  const token = localStorage.getItem("token");

  console.log("Datos a enviar:", datos);

  // Enviar la solicitud PUT al endpoint
  fetch("https://localhost:44370/api/estudiante/modificar/contrasenia", {
    method: "PUT",
    headers: {
      Authorization: "Bearer " + token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(datos),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(
          "Error en la respuesta de la red: " + response.statusText
        );
      }
      return response.json(); // Parsear la respuesta JSON
    })
    .then((data) => {
      console.log("Contraseña actualizada exitosamente:", data);
      // Puedes agregar aquí lógica adicional para actualizar la UI o mostrar un mensaje de éxito
    })
    .catch((error) => console.error("Error:", error));
}

// Agregar eventListener al botón de cambio de contraseña
document
  .getElementById("changePasswordButton")
  .addEventListener("click", function (event) {
    event.preventDefault(); // Prevenir el comportamiento predeterminado del botón

    // Obtener la contraseña actual desde el localStorage
    const token = localStorage.getItem("token");
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
        contrasenia = data.Contrasenia;

        // Obtener los valores de los demás campos del formulario
        const contraseniaActual =
          document.getElementById("currentPassword").value;
        const nuevaContrasenia = document.getElementById("newPassword").value;
        const confirmarContrasenia =
          document.getElementById("renewPassword").value;

        // Verificar si los campos requeridos están vacíos
        if (!contraseniaActual || !nuevaContrasenia || !confirmarContrasenia) {
          alert("Por favor, completa todos los campos");
          return; // Detener la ejecución si algún campo está vacío
        }

        if (contraseniaActual !== contrasenia) {
          alert("La contraseña actual no coincide");
          return; // Detener la ejecución si la contraseña actual no coincide
        }

        if (nuevaContrasenia !== confirmarContrasenia) {
          alert("Las contraseñas no coinciden");
          return; // Detener la ejecución si las contraseñas no coinciden
        }

        // Llamar a la función para actualizar los datos del estudiante
        actualizarContraseniaEstudiante({
          Contrasenia: nuevaContrasenia,
        });
        document.getElementById("currentPassword").value = "";
        document.getElementById("newPassword").value = "";
        document.getElementById("renewPassword").value = "";
        alert("Contraseña actualizada exitosamente");
      })
      .catch((error) => console.error("Error:", error));
  });
