CREATE TABLE TareaEstudiante (
    IdTareaEstudiante INT PRIMARY KEY,
    IdTarea INT NOT NULL,
    IdEstudiante INT NOT NULL,
    FechaEntrega DATE,
    DocuEntregadoRuta VARCHAR(255), 
    FOREIGN KEY (IdTarea) REFERENCES Tarea(IdTarea),
    FOREIGN KEY (IdEstudiante) REFERENCES Estudiante(ID)
);