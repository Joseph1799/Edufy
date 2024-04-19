const urlParams = new URLSearchParams(window.location.search);
const idCurso = urlParams.get("idCurso");
const idTarea = urlParams.get("idTarea");

// Obtiene los detalles de la tarea
fetch(
    `https://localhost:44370/api/tarea/obtenerTareaPorId?idTarea=${idTarea}`,
    {
        method: "GET",
        headers: {},
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

// Obtiene los estudiantes que han entregado la tarea y los que no para mostrarlos en la página respectivamente
document.querySelector(".estudiantes-entregados").innerHTML = "";
document.querySelector(".estudiantes-no-entregados").innerHTML = "";
fetch(
    `https://localhost:44370/api/estudiante/estudiantes-con-tareas-entregadas/${idTarea}`,
    {
        method: "GET",
        headers: {},
    }
)
    .then((response) => response.json())
    .then((estudiantes) => {
        const estudiantesEntregadosDiv = document.querySelector(
            ".estudiantes-entregados"
        );
        const estudiantesNoEntregadosDiv = document.querySelector(
            ".estudiantes-no-entregados"
        );

        // Agregar título para estudiantes no entregados fuera del bucle
        const tituloNoEntregados = document.createElement("h5");
        tituloNoEntregados.className = "card-title";
        tituloNoEntregados.textContent = "Estudiantes con Tareas Pendientes";
        estudiantesNoEntregadosDiv.appendChild(tituloNoEntregados);

        estudiantes.forEach((estudiante) => {
            const postItem = document.createElement("div");
            postItem.className =
                "panel panel-" + (estudiante.Entregada === 1 ? "success" : "danger");
            postItem.innerHTML = `
            <div class="panel-heading">
                <a href="#" onclick="redireccionarUsuario(${estudiante.Id}, ${idCurso})">
                    <h3 class="panel-title">${estudiante.Nombre} ${estudiante.Apellido
                }</h3>
                </a>
            </div>
            <div class="panel-body">${estudiante.Entregada === 1 ? "Tarea Entregada" : "Tarea Pendiente"
                }</div>`;

            if (estudiante.Entregada === 1) {
                estudiantesEntregadosDiv.appendChild(postItem);
            } else {
                estudiantesNoEntregadosDiv.appendChild(postItem);
            }
        });
    })

    .catch((error) => {
        console.error("Error al obtener las tareas del curso:", error);
    });

function redireccionarUsuario(idEstudiante, idCurso) {
    pagina = `revisar-tarea.html?idEstudiante=${idEstudiante}&idCurso=${idCurso}&idTarea=${idTarea}`;
    window.location.href = pagina;
}
