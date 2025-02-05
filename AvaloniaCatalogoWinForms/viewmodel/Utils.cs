using System;
using System.Collections.Generic;
using System.IO;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel
{
    public static class Utils
    {
        private const int BYTES_BARAJA = 107;
        private const char MARCA_BARAJA = 'B';
        internal const string FilePath = "C:/Users/Sebas/RiderProjects/AvaloniaApplication1/AvaloniaCatalogoWinForms/catalogo.dat";

        public static void EliminarArticuloEnFichero(Baraja articuloEliminar, List<Baraja> lista_magica)
        {
            try
            {
                List<Baraja> listaMagica = CargarDesdeFichero();
                listaMagica.RemoveAll(magia => magia.Nombre == articuloEliminar.Nombre);
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
                    using (var fileStr = new FileStream(FilePath, FileMode.Open))
                    using (BinaryReader reader = new BinaryReader(fileStr))
                    {
                        while (fileStr.Position < fileStr.Length - sizeof(Char))
                        {
                            char marca = reader.ReadChar();
                            if (marca == MARCA_BARAJA && reader.SePuedenLeer(BYTES_BARAJA - sizeof(Char)))
                            {
                                string nombre = reader.ReadString().Trim();
                                string categoria = reader.ReadString().Trim();
                                bool dificultad = reader.ReadBoolean();
                                double precio = reader.ReadDouble();
                                string descripcion = reader.ReadString().Trim();
                                int imagen = reader.ReadInt16();

                                listaBarajas.Add(new Baraja(nombre, categoria, dificultad, precio, descripcion, imagen));
                            }
                        }
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

        private static bool SePuedenLeer(this BinaryReader br, int numBytes)
        {
            return br?.BaseStream.Length - br.BaseStream.Position >= numBytes;
        }

        private static string CompletarHasta(this string str, int tamanio)
        {
            return str.PadRight(tamanio, ' ');
        }
    }
}