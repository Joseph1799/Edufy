CREATE TABLE ProfesorCurso (
    Id INT PRIMARY KEY,
    IdProfesor INT NOT NULL,
    IdCurso INT NOT NULL,
    FechaCreacion DATE,
    FOREIGN KEY (IdProfesor) REFERENCES Profesor(ID),
    FOREIGN KEY (IdCurso) REFERENCES Curso(IdCurso)
);