const urlParams = new URLSearchParams(window.location.search);
const idCurso = urlParams.get("idCurso");
const idTarea = urlParams.get("idTarea");
const idEstudiante = urlParams.get("idEstudiante");

// Variables para descargar archivo
let a;
let url;

const token = localStorage.getItem("token"); // Obtiene el token del localStorage

fetch(
    `https://localhost:44370/api/curso/cursodetalle?idCurso=${idCurso}`,
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



fetch(`https://localhost:44370/api/curso/estudiante/tarea-entregada-de-curso?idTarea=${idTarea}&idEstudiante=${idEstudiante}&idCurso=${idCurso}`,)
    .then((response) => {
        if (response.ok) {
            return response.json();
        } else {
            throw new Error("Error al obtener tarea de estudiante");
        }
    })
    .then((tareaEstudiante) => {
        var docuEntregadoRuta = tareaEstudiante.DocuEntregadoRuta;

        // Descargar archivo
        fetch(`/obtener-archivo/${docuEntregadoRuta}`)
            .then(response => {
                if (response.ok) {
                    return response.blob();
                } else {
                    throw new Error("Error al obtener archivo");
                }
            })
            .then(blob => {
                url = URL.createObjectURL(blob);
                a = document.createElement("a");
                a.href = url;
                a.download = docuEntregadoRuta;
                document.body.appendChild(a);

                // Mostrar el nombre del archivo en la página y la fecha de entrega de la tarea
                document.getElementById("nombre-archivo").textContent = docuEntregadoRuta;
                // Convertir la fecha límite a un formato legible
                const endDate = new Date(tareaEstudiante.FechaEntrega);
                endDate.setDate(endDate.getDate() + 1);
                const formattedEndDate = endDate.toLocaleDateString("es-ES", {
                    weekday: "long",
                    year: "numeric",
                    month: "long",
                    day: "numeric",
                });
                document.getElementById("fecha-entregada").textContent = formattedEndDate;
            })
            .catch(error => {
                console.error("Error al obtener archivo:", error);
            });
    })
    .catch((error) => {
        console.error("Error al obtener tarea de estudiante:", error);
    });

document.getElementById("descargarArchivo").addEventListener("click", function () {
    a.click();
    window.URL.revokeObjectURL(url);
});

document.getElementById("enviar-tarea").addEventListener("click", function () {
    const puntaje = document.getElementById("txtPuntaje").value;
    fetch(`https://localhost:44370/api/tarea/revisar-tarea/${idEstudiante}/${puntaje}/${idCurso}`, {
        method: "PUT",
        headers: {
            Authorization: "Bearer " + token,
        },
    })
        .then((response) => {
            if (response.ok) {
                Swal.fire({
                    title: "Tarea revisada!",
                    text: "El puntaje ha sido enviado al estudiante exitosamente",
                    icon: "success"
                });
                document.getElementById("txtPuntaje").value = "";
            } else {
                throw new Error("Error al revisar tarea");
            }

        })
        .catch((error) => {
            console.error("Error al revisar tarea:", error);
        });
});