using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AvaloniaApplication1.model;

namespace AvaloniaApplication1.viewmodel;

public class CtrBaraja
{
    private static CtrBaraja instancia;
    private List<Baraja> lista_magica;
    public static int contadorId;
    private const string PATHIMAGENES = "IMG";

    private CtrBaraja()
    {
        lista_magica = Utils.CargarDesdeFichero(); // Cargar artículos desde el fichero al inicializar

        if (!Directory.Exists(PATHIMAGENES))
        {
            Directory.CreateDirectory(PATHIMAGENES);
        }

        foreach(Baraja a  in lista_magica)
        {
            if(contadorId < a.ImagenId)
            {
                contadorId = a.ImagenId;
            }
        }
    }

    public static CtrBaraja getControlador()
    {
        if (instancia == null)
        {
            instancia = new CtrBaraja();
        }
        return instancia;
    }

    public List<Baraja> ObtenerListaMagica()
    {
        return lista_magica;
    }


    public void agregarArticuloMagico(Baraja baraja)
    {
        lista_magica.Add(baraja);
    }
    


    public void eliminarArticuloMagico(List<Baraja> lista_magica_original_eliminar, int posicionEliminar)
    {
        // Verifica si la posición es válida
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

    public Baraja actualizarListaArticuloMagico(List<Baraja> lista_magica_original_actualizar, string opcionActualizar)
    {
        if (!opcionActualizar.Equals("si"))
        {
            Console.WriteLine("No se ha seleccionado ninguna actualización.");
        }

        if (lista_magica_original_actualizar.Count == 0)
        {
            Console.WriteLine("No hay artículos en la lista para actualizar.");
        }

        // Mostrar los artículos y permitir seleccionar cuál actualizar
        Console.WriteLine("Seleccione el artículo que desea actualizar:");
        for (int i = 0; i < lista_magica_original_actualizar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lista_magica_original_actualizar[i].Nombre}");
        }

        int seleccion = int.Parse(Console.ReadLine());

        return lista_magica_original_actualizar[seleccion - 1];
    }
    public void actualizarArticuloMagico(List<Baraja> lista_magica_original_actualizar, string opcionActualizar)
    {
        Baraja articulo = actualizarListaArticuloMagico(lista_magica_original_actualizar, opcionActualizar);

        // Nuevos datos
        Console.WriteLine("\n¿Qué atributo deseas actualizar?");
        Console.WriteLine("1. Nombre");
        Console.WriteLine("2. Categoría");
        Console.WriteLine("3. Precio");
        Console.WriteLine("4. Dificultad");
        Console.WriteLine("5. Descripción");

        Console.WriteLine("Introduzca alguna opción");
        int opcion;
        if (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 5)
        {
            Console.WriteLine("Opción inválida.");
            return;
        }

        // Actualizar el atributo seleccionado
        switch (opcion)
        {
            case 1:
                Console.WriteLine("Introduce el nuevo nombre:");
                articulo.Nombre = Console.ReadLine();
                break;
            case 2:
                Console.WriteLine("Introduce la nueva categoría:");
                articulo.Categoria = Console.ReadLine();
                break;
            case 3:
                Console.WriteLine("Introduce el nuevo precio:");
                if (double.TryParse(Console.ReadLine(), out double nuevoPrecio))
                {
                    articulo.Precio = nuevoPrecio;
                }
                else
                {
                    Console.WriteLine("Precio inválido.");
                }
                break;
            case 4:
                Console.WriteLine("Introduce la nueva dificultad (true para difícil, false para fácil):");
                if (bool.TryParse(Console.ReadLine(), out bool nuevaDificultad))
                {
                    articulo.Dificultad = nuevaDificultad;
                }
                else
                {
                    Console.WriteLine("Dificultad inválida.");
                }
                break;
            case 5:
                Console.WriteLine("Introduce la nueva descripción:");
                articulo.Desc = Console.ReadLine();
                break;
        }

        Console.WriteLine("Artículo actualizado correctamente.");
    }

    public void listarArticuloMagico()
    {
        if (lista_magica.Count == 0)
        {
            Console.WriteLine("No hay artículos para mostrar.");
        }
        else
        {
            foreach (var articulo in lista_magica)
            {
                Console.WriteLine($"Nombre: {articulo.Nombre}, Categoría: {articulo.Categoria}, Precio: {articulo.Precio}, Dificultad: {(articulo.Dificultad ? "Difícil" : "Fácil")}, Descripción: {articulo.Desc}");
            }
        }
    }


    public List<Baraja> buscarLista(Dictionary<string, string> miFiltroEliminar)
    {
        List<Baraja> lista_magica_ya_filtrada = buscarPorAtributos(miFiltroEliminar);

        return lista_magica_ya_filtrada;
    }

    private List<Baraja> buscarPorAtributos(Dictionary<string, string> miFiltro)
    {
        List<Baraja> lista_magica_a_filtrar = new List<Baraja>(lista_magica);

        if (miFiltro.ContainsKey("Nombre"))
        {
            string valor = miFiltro["Nombre"];

            for (int i = lista_magica_a_filtrar.Count - 1; i >= 0; i--)
            {
                if (lista_magica_a_filtrar[i].Nombre != valor)
                    lista_magica_a_filtrar.RemoveAt(i);

            }
        }

        if (miFiltro.ContainsKey("Categoria"))
        {
            string valor = miFiltro["Categoria"];

            for (int i = lista_magica_a_filtrar.Count - 1; i >= 0; i--)
            {
                if (lista_magica_a_filtrar[i].Categoria != valor)
                    lista_magica_a_filtrar.RemoveAt(i);
            }
        }

        if (miFiltro.ContainsKey("Dificultad"))
        {
            string valor = miFiltro["Dificultad"].ToLower();
            bool valorBooleano = valor == "si" ? true : false;

            for (int i = lista_magica_a_filtrar.Count - 1; i >= 0; i--)
            {
                if (lista_magica_a_filtrar[i].Dificultad != valorBooleano)
                    lista_magica_a_filtrar.RemoveAt(i);
            }
        }

        if (miFiltro.ContainsKey("Precio"))
        {
            string valor = miFiltro["Precio"];

            for (int i = lista_magica_a_filtrar.Count - 1; i >= 0; i--)
            {
                if (lista_magica_a_filtrar[i].Precio != double.Parse(valor))
                    lista_magica_a_filtrar.RemoveAt(i);
            }
        }

        if (miFiltro.ContainsKey("Descripcion"))
        {
            string valor = miFiltro["Descripcion"];

            for (int i = lista_magica_a_filtrar.Count - 1; i >= 0; i--)
            {
                if (lista_magica_a_filtrar[i].Desc != valor)
                    lista_magica_a_filtrar.RemoveAt(i);
            }
        }
        return lista_magica_a_filtrar;
    }


    // Método para guardar toda la lista de artículos mágicos en el archivo .dat
    public void GuardarListaEnFichero()
    {
        if (File.Exists(Utils.FilePath))
        {
            DateTime dt = DateTime.Now;
            File.Move(Utils.FilePath, $"catalogo_{dt.Day}-{dt.Month}-{dt.Year}_{dt.Ticks}_.dat");
        }

        int numGuardados = 0;

        try
        {
            using (FileStream fs = new FileStream(Utils.FilePath, FileMode.OpenOrCreate))
            {
                Console.WriteLine("Abriendo archivo para guardar");

                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    foreach (var magia in lista_magica)
                    {
                        Utils.GuardarComun(magia, writer);
                        numGuardados++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Error durantre el guardado del catálogo. Se han escrito {numGuardados} registros.\nDetalles: {ex.ToString()}");
        }
    }


    //Form2:
    public void modificarArticuloMagico(Baraja articuloModificado)
    {
        // Buscar el artículo en la lista utilizando el nombre o algún otro identificador único
        var articuloExistente = lista_magica.FirstOrDefault(a => a.Nombre == articuloModificado.Nombre);

        if (articuloExistente != null)
        {
            articuloExistente.Nombre = articuloModificado.Nombre;
            articuloExistente.Categoria = articuloModificado.Categoria;
            articuloExistente.Precio = articuloModificado.Precio;
            articuloExistente.Dificultad = articuloModificado.Dificultad;
            articuloExistente.Desc = articuloModificado.Desc;
            
        }
        else
        {
            Console.WriteLine("No se encontró el artículo para modificar.");
        }
    }

    public void guardarImagen(string pathOrigen, int contador)
    {
        string nombre = $"{contador}.jpg";

        string pathDestino = Path.Combine(PATHIMAGENES, nombre);
        File.Copy(pathOrigen, pathDestino, true);
    }
}