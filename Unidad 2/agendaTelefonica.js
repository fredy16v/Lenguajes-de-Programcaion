const fs = require('fs');
const readline = require('readline');

function leerArchivoCSV(rutaArchivo) {
    fs.readFile(rutaArchivo, 'utf8', (error, contenido) => {
        if (error) {
            console.error('Error al leer el archivo:', error.message);
            return;
        }

        solicitarCampoBusqueda(contenido);
    });
}

function solicitarCampoBusqueda(contenido) {
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout
    });

    rl.question('¿En qué campo desea buscar? (nombre, apellido, teléfono, correo o dirección): ', (campo) => {
        rl.close();
        solicitarTextoBusqueda(contenido, campo.toLowerCase());
    });
}

function solicitarTextoBusqueda(contenido, campo) {
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout
    });

    rl.question(`Ingrese el texto que desea buscar en el campo '${campo}': `, (textoBusqueda) => {
        rl.close();
        buscarTextoEnArchivo(contenido, campo, textoBusqueda);
    });
}

function buscarTextoEnArchivo(contenido, campo, textoBusqueda) {
    const lineas = contenido.split('\n');
    let resultadosEncontrados = [];
    let regex;

    switch (campo) {
        case 'nombre':
            regex = new RegExp(`\\b${textoBusqueda}\\b`, 'i'); // /\b\w+/ig
            break;
        case 'apellido':
            regex = new RegExp(`\\b${textoBusqueda}\\b`, 'i');
            break;
        case 'telefono':
            regex = new RegExp(`\\b${textoBusqueda}\\b`); // /\b\d{10}\b/g
            break;
        case 'correo':
            regex = new RegExp(`\\b${textoBusqueda}\\b`, 'i');
            break;
        case 'direccion':
            regex = new RegExp(`\\b${textoBusqueda}\\b`, 'i');
            break;
        default:
            console.log('Opción no válida.');
            return;
    }

    lineas.forEach((linea) => {
        const campos = linea.split(',');
        campos.forEach((campo) => {
            if (campo.trim().match(regex)) {
                resultadosEncontrados.push(campo.trim());
            }
        });
    });

    if (resultadosEncontrados.length > 0) {
        console.log(`Resultados encontrados en el campo '${campo}':`);
        resultadosEncontrados.forEach((resultado) => {
            console.log(resultado);
        });
    } else {
        console.log(`No se encontraron resultados para la búsqueda en el campo '${campo}'.`);
    }
}

const rutaArchivoCSV = 'MOCK_DATA.csv';
leerArchivoCSV(rutaArchivoCSV);

//MOCK_DATA.csv


