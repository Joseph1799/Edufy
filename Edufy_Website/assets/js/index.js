/**
 * Verify Login Form
 */
document
  .getElementById("loginForm")
  .addEventListener("submit", function (event) {
    event.preventDefault(); // Evita que el formulario se envíe de forma convencional
    const formData = new FormData(this); // Obtiene los datos del formulario
    const email = formData.get("CorreoElectronico");
    const password = formData.get("Contrasenia");
    const rol = formData.get("rol");

    // Verifica si los campos están vacíos y muestra los mensajes de error
    if (!email) {
      document.getElementById("email").classList.add("is-invalid");
    } else {
      document.getElementById("email").classList.remove("is-invalid");
    }
    if (!password) {
      document.getElementById("password").classList.add("is-invalid");
      document.getElementById("password-error").style.display = "block"; // Muestra el mensaje de "Por favor, introduce tu contraseña."
    } else {
      document.getElementById("password").classList.remove("is-invalid");
      document.getElementById("password-error").style.display = "none"; // Oculta el mensaje de "Por favor, introduce tu contraseña."
    }
    if (!rol) {
      document.getElementById("rol").classList.add("is-invalid");
    } else {
      document.getElementById("rol").classList.remove("is-invalid");
    }

    if (!email || !password) {
      return; // Detiene el envío del formulario si hay campos vacíos
    }

    fetch(
      "https://localhost:44370/api/" +
        (rol.toLowerCase() === "estudiante" ? "estudiante" : "profesor") +
        "/login",
      {
        // Realiza la solicitud POST a la API
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          CorreoElectronico: email,
          Contrasenia: password,
          Rol: rol,
        }), // Convierte los datos en formato JSON y los envía en el cuerpo de la solicitud
      }
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error("Error en la respuesta de la red");
        }
        return response.json(); // Parsea la respuesta JSON
      })
      .then((data) => {
        // Almacena el token en el localStorage
        localStorage.setItem("token", data.Token);

        // Redirige al usuario a la página de inicio después del inicio de sesión exitoso
        window.location.href = "dashboard.html";
      })
      .catch((error) => {
        console.error("Hubo un problema con la operación fetch:", error);
        document.getElementById("password").classList.add("is-invalid");
        document.querySelector(".incorrecto").style.display = "block"; // Muestra el mensaje de "Correo o contraseña incorrectos"
      });
  });
