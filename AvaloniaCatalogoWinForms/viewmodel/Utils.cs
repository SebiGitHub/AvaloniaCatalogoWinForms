using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel
{
    public static class Utils
    {
        internal const string FilePath = "C:/Users/Sebas/RiderProjects/AvaloniaApplication1/AvaloniaCatalogoWinForms/catalogo.json";

        public static List<Baraja> CargarDesdeFichero()
        {
            List<Baraja> listaBarajas = new List<Baraja>();

            if (File.Exists(FilePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(FilePath);

                    // Elimina caracteres de control no imprimibles, pero preserva otros válidos
                    jsonContent = new string(jsonContent.Where(c => !char.IsControl(c) || c == '\r' || c == '\n').ToArray());

                    if (string.IsNullOrWhiteSpace(jsonContent))
                    {
                        Console.WriteLine("El archivo JSON está vacío o solo tenía caracteres inválidos.");
                        return new List<Baraja>();
                    }

                    try
                    {
                        listaBarajas = JsonSerializer.Deserialize<List<Baraja>>(jsonContent) ?? new List<Baraja>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error de formato en JSON después de limpieza: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar datos: {ex.Message}");
                }
            }

            return listaBarajas;
        }
        
        
        // Método para serializar la lista de Barajas a JSON
        public static string SerializarListaMagica(List<Baraja> listaBarajas)
        {
            return JsonSerializer.Serialize(listaBarajas);
        }
    }
}
