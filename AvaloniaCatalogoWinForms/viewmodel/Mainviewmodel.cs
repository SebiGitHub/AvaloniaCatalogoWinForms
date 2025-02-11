using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApplication1.model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1.viewmodel
{
    public partial class Mainviewmodel : ObservableObject
    {
        private CtrBaraja controlador;
        private List<Baraja> listaArticulos;
        private int posicionActual = 0;
        private bool enModoEdicion = false;
        private Baraja articuloOriginal;
        private int aux;

        [ObservableProperty] private bool _btnHabilitar;
        [ObservableProperty] private string _btnBorrar;
        [ObservableProperty] private bool _btnCargarImagen;
        [ObservableProperty] private string _btnAnterior;
        [ObservableProperty] private string _btnSiguiente;

        [ObservableProperty] private string _txBNombre;
        [ObservableProperty] private string _txBCategoria;
        [ObservableProperty] private string _txBDificultad;
        [ObservableProperty] private string _txBPrecio;
        [ObservableProperty] private string _txBDesc;
        [ObservableProperty] private bool _habilitar;

        [ObservableProperty] private Bitmap _imagen;

        public Mainviewmodel()
        {
            try
            {
                EstadoAnadir(enModoEdicion);
                controlador = CtrBaraja.getControlador();
                listaArticulos = controlador.ObtenerListaMagica() ?? new List<Baraja>();
                Imagen = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "incognita.jpg"));
                MostrarArticulo();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar: {ex.Message}");
            }
        }

        [RelayCommand]
        private void EstadoAnadir(bool enModoEdicion)
        {
            try
            {
                if (enModoEdicion)
                {
                    BtnHabilitar = false;
                    BtnCargarImagen = true;
                    BtnBorrar = "LIMPIAR";
                    BtnSiguiente = "GUARDAR";
                    BtnAnterior = "CANCELAR";
                    Habilitar = true;

                    if (string.IsNullOrEmpty(TxBNombre) || string.IsNullOrEmpty(TxBCategoria))
                    {
                        Console.WriteLine($"Nombre: {TxBNombre}, Categoria: {TxBCategoria}"); // Imprimir valores
                        throw new InvalidOperationException("Campos obligatorios no completados.");
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EstadoAnadir: {ex.Message}");
            }
        }

        public void limpiarCampos()
        {
            TxBNombre = "";
            TxBCategoria = "";
            TxBDificultad = "";
            TxBPrecio = "";
            TxBDesc = "";
            Imagen = new Bitmap("incognita.jpg");
        }

        private void MostrarArticulo()
        {
            try
            {
                listaArticulos = controlador.ObtenerListaMagica() ?? new List<Baraja>();
                Imagen = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "incognita.jpg"));

                if (listaArticulos != null && listaArticulos.Count > 0)
                {
                    Baraja articulo = listaArticulos[posicionActual];
                    TxBNombre = articulo.Nombre;
                    TxBCategoria = articulo.Categoria;
                    TxBDificultad = articulo.Dificultad.ToString();
                    TxBPrecio = articulo.Precio.ToString("F2");
                    TxBDesc = articulo.Desc;

                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", $"{articulo.ImagenId}.jpg");
                    Console.WriteLine($"Buscando imagen en: {imagePath}");

                    if (File.Exists(imagePath))
                    {
                        using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                        {
                            Imagen = new Bitmap(stream);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"La imagen {imagePath} no existe.");
                    }
                }
                else
                {
                    limpiarCampos();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en MostrarArticulo: {ex.Message}");
            }
        }

        [RelayCommand]
        private void btnSiguiente_Click()
        {
            try
            {
                if (enModoEdicion)
                {
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
                else if (posicionActual < listaArticulos.Count - 1)
                {
                    posicionActual++;
                    MostrarArticulo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en btnSiguiente_Click: {ex.Message}");
            }
        }

        [RelayCommand]
        private void btnAnterior_Click()
        {
            try
            {
                if (enModoEdicion)
                {
                    enModoEdicion = false;
                    EstadoAnadir(enModoEdicion);
                    MostrarArticulo();
                }
                else if (posicionActual > 0)
                {
                    posicionActual--;
                    MostrarArticulo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en btnAnterior_Click: {ex.Message}");
            }
        }

        [RelayCommand]
        private void btnBorrar_Click()
        {
            try
            {
                if (listaArticulos.Count > 0)
                {
                    controlador.eliminarArticuloMagico(listaArticulos, posicionActual);

                    if (posicionActual >= listaArticulos.Count)
                    {
                        posicionActual = listaArticulos.Count - 1;
                    }

                    controlador.GuardarListaEnFichero();
                    MostrarArticulo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en btnBorrar_Click: {ex.Message}");
            }
        }

        [RelayCommand]
        private void btnAnadir_Click()
        {
            try
            {
                enModoEdicion = true;
                articuloOriginal = listaArticulos.Count > 0 ? listaArticulos[posicionActual] : null;
                limpiarCampos();
                EstadoAnadir(enModoEdicion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en btnAnadir_Click: {ex.Message}");
            }
        }

        [RelayCommand]
        private async void btnCargarImagen_Click(Window ventanaPadre)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Seleccionar una imagen";
                openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Imágenes JPG", Extensions = { "jpg" } });
                openFileDialog.InitialFileName = "C:/Users/Sebas/Desktop";
                openFileDialog.AllowMultiple = false;

                var result = await openFileDialog.ShowAsync(ventanaPadre);

                if (result != null)
                {
                    string rutaFoto = result[0];
                    Imagen = new Bitmap(rutaFoto);
                    controlador.guardarImagen(rutaFoto, ++CtrBaraja.contadorId);
                    aux = CtrBaraja.contadorId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en btnCargarImagen_Click: {ex.Message}");
            }
        }
    }
}