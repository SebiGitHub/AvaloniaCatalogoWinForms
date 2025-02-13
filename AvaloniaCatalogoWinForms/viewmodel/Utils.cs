using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel
{
    // Clase estática que contiene métodos de utilidad para manejar archivos JSON y listas de barajas
    public static class Utils
    {
        // Ruta del archivo JSON donde se almacenan los datos de las barajas
        internal const string FilePath = "C:/Users/Sebas/RiderProjects/AvaloniaApplication1/AvaloniaCatalogoWinForms/catalogo.json";

        // Método para cargar la lista de barajas desde un archivo JSON
        public static List<Baraja> CargarDesdeFichero()
        {
            // Inicializar una lista vacía de barajas
            var listaBarajas = new List<Baraja>();

            // Verificar si el archivo existe
            if (File.Exists(FilePath))
            {
                try
                {
                    // Leer todo el contenido del archivo JSON
                    var jsonContent = File.ReadAllText(FilePath);

                    // Filtrar caracteres de control no deseados (excepto saltos de línea y retornos de carro)
                    jsonContent = new string(jsonContent.Where(c => !char.IsControl(c) || c == '\r' || c == '\n').ToArray());

                    // Verificar si el contenido del archivo está vacío o solo contiene caracteres inválidos
                    if (string.IsNullOrWhiteSpace(jsonContent))
                    {
                        Console.WriteLine("El archivo JSON está vacío o solo tenía caracteres inválidos.");
                        return listaBarajas; // Retornar la lista vacía
                    }

                    // Deserializar el contenido JSON en una lista de objetos Baraja
                    listaBarajas = JsonSerializer.Deserialize<List<Baraja>>(jsonContent) ?? new List<Baraja>();
                }
                catch (Exception ex)
                {
                    // Manejar errores durante la carga del archivo
                    Console.WriteLine($"Error al cargar datos: {ex.Message}");
                }
            }

            // Retornar la lista de barajas (puede estar vacía si hubo errores o el archivo no existe)
            return listaBarajas;
        }

        // Método para serializar una lista de barajas a formato JSON
        public static string SerializarListaMagica(List<Baraja> listaBarajas)
        {
            // Serializar la lista de barajas a una cadena JSON
            return JsonSerializer.Serialize(listaBarajas);
        }
    }
}