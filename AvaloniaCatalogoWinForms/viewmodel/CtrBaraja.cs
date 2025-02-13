using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel
{
    public class CtrBaraja
    {
        // Ruta donde se almacenarán las imágenes
        private const string PATHIMAGENES = "IMG";

        // Instancia única del controlador (patrón Singleton)
        private static CtrBaraja instancia;

        // Contador para asignar IDs únicos a las imágenes
        public static int contadorId;

        // Lista que almacena las barajas (objetos de tipo Baraja)
        private readonly List<Baraja> lista_magica;

        // Constructor privado para evitar instanciación directa (patrón Singleton)
        private CtrBaraja()
        {
            try
            {
                // Cargar la lista de barajas desde un fichero
                lista_magica = Utils.CargarDesdeFichero();

                // Crear el directorio de imágenes si no existe
                if (!Directory.Exists(PATHIMAGENES))
                    Directory.CreateDirectory(PATHIMAGENES);

                // Inicializar el contador de IDs con el máximo ID encontrado en la lista
                if (lista_magica.Any())
                    contadorId = lista_magica.Max(a => a.ImagenId);
            }
            catch (Exception ex)
            {
                // Manejar errores durante la inicialización
                Console.WriteLine($"Error al inicializar CtrBaraja: {ex.Message}");
            }
        }

        // Método estático para obtener la instancia única del controlador (Singleton)
        public static CtrBaraja getControlador() => instancia ??= new CtrBaraja();

        // Método para obtener la lista de barajas
        public List<Baraja> ObtenerListaMagica() => lista_magica ?? new List<Baraja>();

        // Método para agregar una nueva baraja a la lista
        public void agregarArticuloMagico(Baraja baraja)
        {
            try
            {
                lista_magica.Add(baraja);
            }
            catch (Exception ex)
            {
                // Manejar errores al agregar una baraja
                Console.WriteLine($"Error al agregar artículo: {ex.Message}");
            }
        }

        // Método para eliminar una baraja de la lista en una posición específica
        public void eliminarArticuloMagico(List<Baraja> lista_magica_original_eliminar, int posicionEliminar)
        {
            try
            {
                // Verificar que la posición sea válida
                if (posicionEliminar >= 0 && posicionEliminar < lista_magica_original_eliminar.Count)
                {
                    lista_magica_original_eliminar.RemoveAt(posicionEliminar);
                    Console.WriteLine("Producto eliminado exitosamente");
                }
                else
                {
                    Console.WriteLine("La posición especificada no es válida");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores al eliminar una baraja
                Console.WriteLine($"Error al eliminar artículo: {ex.Message}");
            }
        }

        // Método para guardar la lista de barajas en un fichero
        public void GuardarListaEnFichero()
        {
            try
            {
                // Si el fichero ya existe, renombrarlo con la fecha y hora actual
                if (File.Exists(Utils.FilePath))
                {
                    var dt = DateTime.Now;
                    File.Move(Utils.FilePath, $"catalogo_{dt:dd-MM-yyyy}_{dt.Ticks}_.dat");
                }

                // Guardar la lista serializada en el fichero
                File.WriteAllText(Utils.FilePath, Utils.SerializarListaMagica(lista_magica));
                Console.WriteLine($"Guardados {lista_magica.Count} registros.");
            }
            catch (Exception ex)
            {
                // Manejar errores al guardar la lista
                Console.WriteLine($"Error al guardar lista: {ex.Message}");
            }
        }

        // Método para guardar una imagen en el directorio de imágenes
        public void guardarImagen(string pathOrigen, int contador)
        {
            try
            {
                // Generar el nombre de la imagen y su ruta de destino
                var nombre = $"{contador}.jpg";
                var pathDestino = Path.Combine(PATHIMAGENES, nombre);

                // Copiar la imagen al directorio de imágenes
                File.Copy(pathOrigen, pathDestino, true);
            }
            catch (Exception ex)
            {
                // Manejar errores al guardar la imagen
                Console.WriteLine($"Error al guardar imagen: {ex.Message}");
            }
        }
    }
}