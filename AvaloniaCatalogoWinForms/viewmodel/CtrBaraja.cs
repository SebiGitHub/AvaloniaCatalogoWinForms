using System;
using System.Collections.Generic;
using System.IO;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel
{
    public class CtrBaraja
    {
        private static CtrBaraja instancia;
        private List<Baraja> lista_magica;
        public static int contadorId;
        private const string PATHIMAGENES = "IMG";

        private CtrBaraja()
        {
            try
            {
                lista_magica = Utils.CargarDesdeFichero();

                if (!Directory.Exists(PATHIMAGENES))
                {
                    Directory.CreateDirectory(PATHIMAGENES);
                }

                foreach (var a in lista_magica)
                {
                    if (contadorId < a.ImagenId)
                    {
                        contadorId = a.ImagenId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar CtrBaraja: {ex.Message}");
            }
        }

        public static CtrBaraja getControlador()
        {
            return instancia ??= new CtrBaraja();
        }

        public List<Baraja> ObtenerListaMagica()
        {
            return lista_magica ?? new List<Baraja>();
        }

        public void agregarArticuloMagico(Baraja baraja)
        {
            try
            {
                lista_magica.Add(baraja);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar artículo: {ex.Message}");
            }
        }

        public void eliminarArticuloMagico(List<Baraja> lista_magica_original_eliminar, int posicionEliminar)
        {
            try
            {
                if (posicionEliminar >= 0 && posicionEliminar < lista_magica_original_eliminar.Count)
                {
                    lista_magica_original_eliminar.RemoveAt(posicionEliminar);
                    Console.WriteLine("\nProducto eliminado exitosamente");
                }
                else
                {
                    Console.WriteLine("La posición especificada no es válida");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar artículo: {ex.Message}");
            }
        }

        public void GuardarListaEnFichero()
        {
            try
            {
                if (File.Exists(Utils.FilePath))
                {
                    DateTime dt = DateTime.Now;
                    File.Move(Utils.FilePath, $"catalogo_{dt.Day}-{dt.Month}-{dt.Year}_{dt.Ticks}_.dat");
                }

                int numGuardados = 0;

                using (FileStream fs = new FileStream(Utils.FilePath, FileMode.OpenOrCreate))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    foreach (var magia in lista_magica)
                    {
                        Utils.GuardarComun(magia, writer);
                        numGuardados++;
                    }
                }

                Console.WriteLine($"Guardados {numGuardados} registros.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar lista: {ex.Message}");
            }
        }

        public void guardarImagen(string pathOrigen, int contador)
        {
            try
            {
                string nombre = $"{contador}.jpg";
                string pathDestino = Path.Combine(PATHIMAGENES, nombre);
                File.Copy(pathOrigen, pathDestino, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar imagen: {ex.Message}");
            }
        }
    }
}