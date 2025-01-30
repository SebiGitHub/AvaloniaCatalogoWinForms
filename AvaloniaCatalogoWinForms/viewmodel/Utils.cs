using System;
using System.Collections.Generic;
using System.IO;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel;

public static class Utils
{
    private const int BYTES_BARAJA = 107;

    // Cambiado el FilePath
    internal const string FilePath = "C:/Users/Sebas/source/repos/WinFormsMagia/catalogo.dat";

    // Método para eliminar un artículo mágico del fichero
    public static void EliminarArticuloEnFichero(Baraja articuloEliminar, List<Baraja> lista_magica)
    {
        List<Baraja> listaMagica = CargarDesdeFichero();

        // Eliminamos el artículo basado en el nombre
        listaMagica.RemoveAll(magia => magia.Nombre == articuloEliminar.Nombre);

        Console.WriteLine("Artículo eliminado del fichero.");
    }

    // Método para actualizar un artículo mágico en el fichero
    public static void ActualizarArticuloEnFichero(Baraja articuloActualizado, List<Baraja> lista_magica)
    {
        List<Baraja> listaMagica = CargarDesdeFichero();

        // Buscamos y reemplazamos el artículo por el actualizado
        for (int i = 0; i < listaMagica.Count; i++)
        {
            if (listaMagica[i].Nombre == articuloActualizado.Nombre)
            {
                listaMagica[i] = articuloActualizado;
                break;
            }
        }
        Console.WriteLine("Artículo actualizado en el fichero.");
    }
    

    // Método para guardar datos comunes de cualquier artículo
    public static void GuardarComun(Baraja baraja, BinaryWriter writer)
    {
        writer.Write(baraja.Nombre.CompletarHasta(Baraja.TAM_MAX));
        writer.Write(baraja.Categoria.CompletarHasta(Baraja.TAM_MAX));
        writer.Write(baraja.Dificultad);
        writer.Write(baraja.Precio);
        writer.Write(baraja.Desc.CompletarHasta(Baraja.TAM_MAX));
        writer.Write(baraja.ImagenId);
    }

    // Método para cargar los artículos mágicos desde el archivo .dat
    public static List<Baraja> CargarDesdeFichero()
    {
        List<Baraja> listaMagica = new List<Baraja>();

        if (File.Exists(FilePath))
        {
            try
            {
                using (var fileStr = new FileStream(FilePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fileStr))
                    {
                        while (fileStr.Position < fileStr.Length - sizeof(Char))
                        {
                            if (reader.SePuedenLeer(BYTES_BARAJA - sizeof(Char)))
                            {
                                string nombre = reader.ReadString().Trim();
                                string categoria = reader.ReadString().Trim();
                                bool dificultad = reader.ReadBoolean();
                                double precio = reader.ReadDouble();
                                string descripcion = reader.ReadString().Trim();
                                int imagen = reader.ReadInt16();

                                Baraja baraja = new Baraja(nombre, categoria, dificultad, precio, descripcion, imagen);
                                listaMagica.Add(baraja);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Error durante la carga del catálogo. Se ha leído {listaMagica.Count} registros.\nDetalles: {ex.ToString()}");
            }
        }
        return listaMagica;
    }

    // Método auxiliar para escribir un artículo en el fichero
    public static void EscribirArticulo(BinaryWriter writer, Baraja baraja)
    {
        writer.Write(baraja.Nombre.CompletarHasta(Baraja.TAM_MAX));
        writer.Write(baraja.Categoria.CompletarHasta(Baraja.TAM_MAX));
        writer.Write(baraja.Dificultad);
        writer.Write(baraja.Precio);
        writer.Write(baraja.Desc.CompletarHasta(Baraja.TAM_MAX));
        writer.Write(baraja.ImagenId);
    }

    // Método auxiliar para leer un artículo del fichero
    public static Baraja LeerArticulo(BinaryReader reader, List<Baraja> lista_magica)
    {
        try
        {
            return new Baraja
            {
                Nombre = reader.ReadString(),
                Categoria = reader.ReadString(),
                Precio = reader.ReadDouble(),
                Dificultad = reader.ReadBoolean(),
                Desc = reader.ReadString(),
                ImagenId = reader.ReadInt16()
            };
        }
        catch (EndOfStreamException)
        {
            return null;  // Fin del archivo
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer artículo: {ex.Message}");
            return null;
        }
    }

    // Métodos de extensión
    private static bool SePuedenLeer(this BinaryReader br, int numBytes)
    {
        bool sePuede = false;
        if (br != null)
        {
            sePuede = br.BaseStream.Length - br.BaseStream.Position >= numBytes;
        }
        return sePuede;
    }

    private static string CompletarHasta(this string str, int tamanio)
    {
        return str.PadRight(tamanio, ' ');
    }
}