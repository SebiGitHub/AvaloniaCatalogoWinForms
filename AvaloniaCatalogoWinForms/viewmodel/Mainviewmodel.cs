using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApplication1.model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1.viewmodel;

public partial class Mainviewmodel : ObservableObject
{
    private CtrBaraja controlador;              // Instancia del controlador principal
    private List<Baraja> listaArticulos;        // Lista de artículos mágicos
    private int posicionActual = 0;            // Índice del artículo actual en la lista
    private bool enModoEdicion = false;
    private Baraja articuloOriginal;  // Para guardar el artículo antes de editarlo
    private int aux;

    //Botones
    [ObservableProperty] private bool _btnHabilitar;
    [ObservableProperty] private string _btnBorrar;
    [ObservableProperty] private bool _btnCargarImagen;
    [ObservableProperty] private string _btnAnterior;
    [ObservableProperty] private string _btnSiguiente;

    //TextBox
    [ObservableProperty] private string _txBNombre;
    [ObservableProperty] private string _txBCategoria;
    [ObservableProperty] private string _txBDificultad;
    [ObservableProperty] private string _txBPrecio;
    [ObservableProperty] private string _txBDesc;
    [ObservableProperty] private bool _habilitar;

    //Imagen
    [ObservableProperty] private Bitmap _imagen;

    public Mainviewmodel()
    {
        controlador = CtrBaraja.getControlador(); // Obtiene la instancia del controlador
        listaArticulos = controlador.ObtenerListaMagica(); // Carga la lista de artículos mágicos
        Imagen = new Bitmap("/assets/img/incognita.png");
    }

    [RelayCommand]
    private void btnSiguiente_Click()
    {
        
    }
    
    [RelayCommand]
    private void btnAnterior_Click()
    {
        
    }
    
    [RelayCommand]
    private void btnAnadir_Click()
    {
        
    }
    
    [RelayCommand]
    private void btnBorrar_Click()
    {
        
    }
    
    [RelayCommand]
    private void btnCargarImagen_Click()
    {
        
    }
    
    
    /*
    public Mainviewmodel()
    {
        EstadoAnadir(enModoEdicion); // Deshabilita la edición de los campos inicialmente
        controlador = CtrBaraja.getControlador(); // Obtiene la instancia del controlador
        listaArticulos = controlador.ObtenerListaMagica(); // Carga la lista de artículos mágicos
        MostrarArticulo();                      // Muestra el primer artículo de la lista
        Imagen = new Bitmap("/assets/img/incognita.png");
    }

    //Habilitar y deshabilitar los botones además de modificar el texto
    [RelayCommand]
    private void EstadoAnadir(bool enModoEdicion)
    {
        if (enModoEdicion)
        {
            BtnHabilitar = false;
            BtnCargarImagen = true;
            BtnBorrar = "LIMPIAR";
            BtnSiguiente = "GUARDAR";
            BtnAnterior = "CANCELAR";
            Imagen = new Bitmap("/assets/img/incognita.png");

            Habilitar = true;

            if (Imagen == null)
            {
                aux = 0;
            }
            else
            {
                aux = CtrBaraja.contadorId;
            }

            Baraja nuevaBaraja = new Baraja
            {
                Nombre = TxBNombre,
                Categoria = TxBCategoria,
                Precio = double.TryParse(TxBPrecio, out double precio) ? precio : 0,
                Dificultad = TxBDificultad.ToLower() == "si",
                Desc = TxBDesc,
                ImagenId = aux
            };
            controlador.agregarArticuloMagico(nuevaBaraja);
        }
        else
        {
            BtnHabilitar = true;
            BtnCargarImagen = false;
            BtnBorrar = "BORRAR";
            BtnAnterior = "ANTERIOR";
            BtnSiguiente = "SIGUIENTE";

            Habilitar = false;
        }
    }

    public void limpiarCampos()
    {
        // Limpiar los campos si la lista está vacía
        TxBNombre = "";
        TxBCategoria = "";
        TxBDificultad = "";
        TxBPrecio = "";
        TxBDesc = "";
        Imagen = new Bitmap("/assets/img/incognita.png");
    }

    // Muestra el artículo actual en los campos de texto y la imagen correspondiente
    private void MostrarArticulo()
    {
        listaArticulos = controlador.ObtenerListaMagica(); // Actualiza la lista de artículos

        // Configura la imagen predeterminada si no se encuentra ninguna
        Imagen = new Bitmap("/assets/img/incognita.png");

        if (listaArticulos != null && listaArticulos.Count > 0)
        {
            Baraja articulo = listaArticulos[posicionActual];
            TxBNombre = articulo.Nombre;
            TxBCategoria = articulo.Categoria;
            TxBDificultad = articulo.Dificultad.ToString();
            TxBPrecio = articulo.Precio.ToString("F2");     // Formato de precio con 2 decimales
            TxBDesc = articulo.Desc;

            // Verifica si existe una imagen asociada al artículo
            string imagePath = $"IMG/{articulo.ImagenId}.jpg";
            if (File.Exists(imagePath))
            {
                Imagen = new Bitmap(imagePath);
            }
        }
        else
        {
            // Limpia los campos si la lista está vacía
            limpiarCampos();
        }
    }

    // Evento del botón "Siguiente" para avanzar al siguiente artículo
    [RelayCommand]
    private void btnSiguiente_Click()
    {
        if (enModoEdicion)
        {
            // Guardar el artículo
            Baraja nuevaBaraja = new Baraja
            {
                Nombre = TxBNombre,
                Categoria = TxBCategoria,
                Precio = double.TryParse(TxBPrecio, out double precio) ? precio : 0,
                Dificultad = TxBDificultad.ToLower() == "si",
                Desc = TxBDesc,
                ImagenId = aux
            };
            controlador.agregarArticuloMagico(nuevaBaraja);
            controlador.GuardarListaEnFichero();
            enModoEdicion = false;
            EstadoAnadir(enModoEdicion);
        }
        else
        {
            if (posicionActual < listaArticulos.Count - 1)
            {
                posicionActual++;
                MostrarArticulo();
            }
        }
    }

    // Evento del botón "Anterior" para retroceder al artículo anterior
    [RelayCommand]
    private void btnAnterior_Click()
    {
        if (enModoEdicion)
        {
            // Cancelar la edición
            enModoEdicion = false;
            EstadoAnadir(enModoEdicion);
            MostrarArticulo();
        }
        else
        {
            if (posicionActual > 0)
            {
                posicionActual--;
                MostrarArticulo();
            }
        }
    }

    // Evento del botón "Eliminar" para eliminar el artículo actual
    [RelayCommand]
    private void btnBorrar_Click()
    {
        if (listaArticulos.Count > 0)
        {
            controlador.eliminarArticuloMagico(listaArticulos, posicionActual);

            // Ajusta la posición actual después de eliminar
            if (posicionActual >= listaArticulos.Count)
            {
                posicionActual = listaArticulos.Count - 1;
            }

            controlador.GuardarListaEnFichero(); // Guarda los cambios en el archivo
            MostrarArticulo();                   // Actualiza la visualización
        }
    }

    [RelayCommand]
    private void btnAnadir_Click()
    {
        enModoEdicion = true;
        articuloOriginal = listaArticulos.Count > 0 ? listaArticulos[posicionActual] : null;
        limpiarCampos();
        EstadoAnadir(enModoEdicion);
    }

    [RelayCommand]
    private async void btnCargarImagen_Click(Window ventanaPadre)
    {
        // Crear un nuevo OpenFileDialog
        var openFileDialog = new OpenFileDialog();

        // Configuración del diálogo de selección de archivo
        openFileDialog.Title = "Seleccionar una imagen"; // Título del diálogo
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Imágenes JPG", Extensions = { "jpg" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Imágenes JPEG", Extensions = { "jpeg" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Todos los archivos", Extensions = { "*" } });
        openFileDialog.InitialFileName = "C:/Users/Sebas/Desktop"; // Directorio inicial que se abrirá al abrir el diálogo
        openFileDialog.AllowMultiple = false;

        var result = await openFileDialog.ShowAsync(ventanaPadre);
        if (result != null)
        {
            string rutaFoto = result[0];
            Imagen = new Bitmap(rutaFoto);

            controlador.guardarImagen(rutaFoto, ++CtrBaraja.contadorId);
        }
    }
    */
}