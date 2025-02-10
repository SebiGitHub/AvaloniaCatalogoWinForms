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

        public static void EliminarArticuloEnFichero(Baraja articuloEliminar, List<Baraja> lista_magica)
        {
            try
            {
                List<Baraja> listaMagica = CargarDesdeFichero();
                listaMagica.RemoveAll(magia => magia.Nombre == articuloEliminar.Nombre);
                
                // Guardar la lista actualizada en el archivo JSON
                File.WriteAllText(FilePath, SerializarListaMagica(listaMagica));

                Console.WriteLine("Artículo eliminado del fichero.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar artículo: {ex.Message}");
            }
        }

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

        public static void GuardarComun(Baraja baraja, BinaryWriter writer)
        {
            try
            {
                writer.Write(baraja.Nombre.CompletarHasta(Baraja.TAM_MAX));
                writer.Write(baraja.Categoria.CompletarHasta(Baraja.TAM_MAX));
                writer.Write(baraja.Dificultad);
                writer.Write(baraja.Precio);
                writer.Write(baraja.Desc.CompletarHasta(Baraja.TAM_MAX));
                writer.Write(baraja.ImagenId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar datos comunes: {ex.Message}");
            }
        }

        // Método para serializar la lista de Barajas a JSON
        public static string SerializarListaMagica(List<Baraja> listaBarajas)
        {
            return JsonSerializer.Serialize(listaBarajas);
        }

        private static string CompletarHasta(this string str, int tamanio)
        {
            return str.PadRight(tamanio, ' ');
        }
    }
}
