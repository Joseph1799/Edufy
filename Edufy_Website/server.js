const express = require("express");
const fileUpload = require("express-fileupload");
const path = require("path");
const app = express();

// Middleware para manejar la carga de archivos
app.use(fileUpload());

// Servir archivos estáticos desde la carpeta 'public'
app.use(express.static(__dirname));

// Ruta de inicio
app.get("/", (req, res) => {
  res.sendFile(__dirname + "/index.html");
});

app.post("/guardar-archivo", (req, res) => {
  const file = req.files.archivo;
  const rutaArchivo = path.join(__dirname, "Documentos_db", file.name);
  const rutaArchivoGuardado = path.join(file.name);

  file.mv(rutaArchivo, (err) => {
    if (err) {
      console.error("Error al guardar el archivo:", err);
      res.status(500).json({ message: "Error al guardar el archivo" });
    } else {
      console.log("El archivo se ha guardado correctamente en:", rutaArchivo);
      res.json({
        message: "Archivo guardado correctamente",
        ruta: rutaArchivoGuardado,
      });
    }
  });
});

app.get("/obtener-archivo/:nombreArchivo", (req, res) => {
  const nombreArchivo = req.params.nombreArchivo;
  const rutaArchivo = path.join(__dirname, "Documentos_db", nombreArchivo);

  res.sendFile(rutaArchivo, (err) => {
    if (err) {
      console.error("Error al enviar el archivo:", err);
      res.status(500).json({ message: "Error al obtener el archivo" });
    } else {
      console.log("El archivo se ha enviado correctamente desde:", rutaArchivo);
    }
  });
});


const server = app.listen(3000, () => {
  const host = server.address().address;
  const port = server.address().port;
  console.log(`Servidor iniciado en http://${host}:${port}`);
});
