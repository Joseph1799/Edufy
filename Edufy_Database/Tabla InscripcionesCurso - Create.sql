CREATE TABLE InscripcionesCurso (
    IdInscripcion INT PRIMARY KEY,
    IdEstudiante INT NOT NULL,
    IdCurso INT NOT NULL,
    FechaInscripcion DATE,
    FOREIGN KEY (IdEstudiante) REFERENCES Estudiante(ID),
    FOREIGN KEY (IdCurso) REFERENCES Curso(IdCurso)
);