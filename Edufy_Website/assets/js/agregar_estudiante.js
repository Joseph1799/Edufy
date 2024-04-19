document.addEventListener("DOMContentLoaded", function () {
  const form = document.querySelector(".needs-validation");

  form.addEventListener("submit", function (event) {
    event.preventDefault();
    let cedula = document.getElementById("yourId").value;
    let errorMessage = document.getElementById("cedula-error-message");
    let formIsValid = true;

    // Verifica si algún campo está vacío
    form.querySelectorAll("input, select").forEach((input) => {
      if (!input.value.trim()) {
        input.classList.add("is-invalid");
        formIsValid = false;
      } else {
        input.classList.remove("is-invalid");
      }
    });

    // Verifica la cédula y muestra el mensaje de error si es necesario
    if (cedula.trim() === "") {
      errorMessage.textContent = "Por favor, ingrese una cédula.";
      errorMessage.style.display = "block";
      formIsValid = false;
    } else if (isNaN(parseInt(cedula))) {
      errorMessage.textContent = "La cédula debe ser un número.";
      errorMessage.style.display = "block";
      formIsValid = false;
    } else {
      errorMessage.style.display = "none";
    }

    if (!formIsValid) {
      event.stopPropagation();
      form.classList.add("was-validated");
      return;
    }

    const formData = new FormData(form);
    const estudiante = {
      Id: formData.get("Id"),
      Nombre: formData.get("name"),
      Apellido: formData.get("lastName"),
      CorreoElectronico: formData.get("email"),
      Contrasenia: formData.get("password"),
      FechaNacimiento: formData.get("BirthDate"),
      Genero: formData.get("profile-gender"),
      Carrera: formData.get("carrera"),
      AnioIngreso: new Date().getFullYear(),
      EstadoCuenta: 1,
      FotoPerfil: "",
      Rol: "Estudiante",
    };

    fetch("https://localhost:44370/api/estudiante/registrar", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(estudiante),
    })
      .then((response) => {
        if (response.ok) {
          document.getElementById("successMessage").classList.remove("d-none");
          // Redirigir a otro HTML después de 2 segundos
          setTimeout(function () {
            history.back();
          }, 2000);
        } else {
          throw new Error("Error al registrar estudiante");
        }
      })
      .catch((error) => {
        console.error("Error:", error);
        // Aquí puedes mostrar un mensaje de error al usuario
      });

    form.classList.add("was-validated");
  });
});
