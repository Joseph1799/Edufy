CREATE TABLE Tarea (
    IdTarea INT PRIMARY KEY,
    NombreTarea VARCHAR(255) NOT NULL,
    Descripcion VARCHAR(MAX),
    FechaLimite DATE,
    DocumentoRuta VARCHAR(255),
    IdCurso INT NOT NULL,
    IdProfesor INT NOT NULL,
    FOREIGN KEY (IdCurso) REFERENCES Curso(IdCurso)
    CONSTRAINT FK_Tarea_Profesor FOREIGN KEY (IdProfesor) REFERENCES Profesor(ID);
);