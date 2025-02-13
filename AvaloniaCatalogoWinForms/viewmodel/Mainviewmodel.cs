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
    // ViewModel principal que gestiona la lógica de la interfaz de usuario
    public partial class Mainviewmodel : ObservableObject
    {
        // Controlador de barajas (patrón Singleton)
        private CtrBaraja controlador;

        // Lista de artículos (barajas)
        private List<Baraja> listaArticulos;

        // Posición actual en la lista de artículos
        private int posicionActual = 0;

        // Indica si la interfaz está en modo edición
        private bool enModoEdicion = false;

        // Almacena el artículo original antes de la edición
        private Baraja articuloOriginal;

        // Variable auxiliar para manejar el ID de la imagen
        private int aux;

        // Propiedades observables para enlazar con la interfaz de usuario
        [ObservableProperty] private bool _btnHabilitar;
        [ObservableProperty] private string _btnBorrar;
        [ObservableProperty] private bool _btnCargarImagen;
        [ObservableProperty] private string _btnAnterior;
        [ObservableProperty] private string _btnSiguiente;

        // Campos de texto para enlazar con los controles de la interfaz
        [ObservableProperty] private string _txBNombre;
        [ObservableProperty] private string _txBCategoria;
        [ObservableProperty] private string _txBDificultad;
        [ObservableProperty] private string _txBPrecio;
        [ObservableProperty] private string _txBDesc;

        // Propiedades para habilitar/deshabilitar controles y mostrar la imagen
        [ObservableProperty] private bool _habilitar;
        [ObservableProperty] private Bitmap _imagen;

        // Constructor del ViewModel
        public Mainviewmodel()
        {
            try
            {
                // Obtener la instancia del controlador de barajas
                controlador = CtrBaraja.getControlador();

                // Cargar la lista de artículos desde el controlador
                listaArticulos = controlador.ObtenerListaMagica();

                // Cargar una imagen predeterminada
                Imagen = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "incognita.jpg"));

                // Configurar el estado inicial de los botones
                EstadoAnadir(enModoEdicion);

                // Mostrar el primer artículo de la lista
                MostrarArticulo();
            }
            catch (Exception ex)
            {
                // Manejar errores durante la inicialización
                Console.WriteLine($"Error al inicializar: {ex.Message}");
            }
        }

        // Método para cambiar el estado de los botones según el modo (edición o visualización)
        [RelayCommand]
        private void EstadoAnadir(bool enModoEdicion)
        {
            try
            {
                if (enModoEdicion)
                {
                    // Configurar botones para el modo edición
                    BtnHabilitar = false;
                    BtnCargarImagen = true;
                    BtnBorrar = "LIMPIAR";
                    BtnSiguiente = "GUARDAR";
                    BtnAnterior = "CANCELAR";
                    Habilitar = true;

                    // Crear una nueva baraja con los datos actuales y agregarla al controlador
                    var nuevaBaraja = CrearBarajaDesdeCampos();
                    controlador.agregarArticuloMagico(nuevaBaraja);
                }
                else
                {
                    // Configurar botones para el modo visualización
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
                // Manejar errores durante el cambio de estado
                Console.WriteLine($"Error en EstadoAnadir: {ex.Message}");
            }
        }

        // Método para crear un objeto Baraja a partir de los campos actuales
        private Baraja CrearBarajaDesdeCampos()
        {
            return new Baraja
            {
                Nombre = TxBNombre,
                Categoria = TxBCategoria,
                Precio = double.TryParse(TxBPrecio, out double precio) ? precio : 0,
                Dificultad = TxBDificultad.ToLower() == "si",
                Desc = TxBDesc,
                ImagenId = aux
            };
        }

        // Método para limpiar los campos de la interfaz
        public void limpiarCampos()
        {
            TxBNombre = "";
            TxBCategoria = "";
            TxBDificultad = "";
            TxBPrecio = "";
            TxBDesc = "";
            Imagen = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "incognita.jpg"));
        }

        // Método para mostrar el artículo actual en la interfaz
        private void MostrarArticulo()
        {
            try
            {
                // Actualizar la lista de artículos por si hubo cambios externos
                listaArticulos = controlador.ObtenerListaMagica();

                // Cargar una imagen predeterminada
                Imagen = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "incognita.jpg"));

                // Verificar si hay artículos en la lista
                if (listaArticulos != null && listaArticulos.Count > 0)
                {
                    // Obtener el artículo actual
                    var articulo = listaArticulos[posicionActual];

                    // Actualizar los campos de la interfaz con los datos del artículo
                    TxBNombre = articulo.Nombre;
                    TxBCategoria = articulo.Categoria;
                    TxBDificultad = articulo.Dificultad.ToString();
                    TxBPrecio = articulo.Precio.ToString("F2");
                    TxBDesc = articulo.Desc;

                    // Construir la ruta de la imagen asociada al artículo
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", $"{articulo.ImagenId}.jpg");

                    // Cargar la imagen si existe
                    if (File.Exists(imagePath))
                    {
                        using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                        Imagen = new Bitmap(stream);
                    }
                    else
                    {
                        Console.WriteLine($"La imagen {imagePath} no existe.");
                    }
                }
                else
                {
                    // Limpiar los campos si no hay artículos
                    limpiarCampos();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores durante la visualización del artículo
                Console.WriteLine($"Error en MostrarArticulo: {ex.Message}");
            }
        }

        // Comando para cerrar la ventana
        [RelayCommand]
        private void btnSalir_Click(Window ventana)
        {
            ventana.Close();
        }

        // Comando para manejar el botón "Siguiente"
        [RelayCommand]
        private void btnSiguiente_Click()
        {
            try
            {
                if (enModoEdicion)
                {
                    // Guardar el nuevo artículo en modo edición
                    var nuevaBaraja = CrearBarajaDesdeCampos();
                    controlador.agregarArticuloMagico(nuevaBaraja);
                    controlador.GuardarListaEnFichero();
                    enModoEdicion = false;
                    EstadoAnadir(enModoEdicion);
                }
                else if (posicionActual < listaArticulos.Count - 1)
                {
                    // Avanzar al siguiente artículo
                    posicionActual++;
                    MostrarArticulo();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores durante la acción
                Console.WriteLine($"Error en btnSiguiente_Click: {ex.Message}");
            }
        }

        // Comando para manejar el botón "Anterior"
        [RelayCommand]
        private void btnAnterior_Click()
        {
            try
            {
                if (enModoEdicion)
                {
                    // Cancelar la edición y volver al modo visualización
                    enModoEdicion = false;
                    EstadoAnadir(enModoEdicion);
                    MostrarArticulo();
                }
                else if (posicionActual > 0)
                {
                    // Retroceder al artículo anterior
                    posicionActual--;
                    MostrarArticulo();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores durante la acción
                Console.WriteLine($"Error en btnAnterior_Click: {ex.Message}");
            }
        }

        // Comando para manejar el botón "Borrar"
        [RelayCommand]
        private void btnBorrar_Click()
        {
            try
            {
                if (listaArticulos.Count > 0)
                {
                    // Eliminar el artículo actual
                    controlador.eliminarArticuloMagico(listaArticulos, posicionActual);

                    // Ajustar la posición actual si es necesario
                    if (posicionActual >= listaArticulos.Count)
                        posicionActual = listaArticulos.Count - 1;

                    // Guardar los cambios en el fichero
                    controlador.GuardarListaEnFichero();

                    // Mostrar el artículo actualizado
                    MostrarArticulo();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores durante la acción
                Console.WriteLine($"Error en btnBorrar_Click: {ex.Message}");
            }
        }

        // Comando para manejar el botón "Añadir"
        [RelayCommand]
        private void btnAnadir_Click()
        {
            try
            {
                // Entrar en modo edición
                enModoEdicion = true;

                // Guardar el artículo original si existe
                articuloOriginal = listaArticulos.Count > 0 ? listaArticulos[posicionActual] : null;

                // Limpiar los campos para añadir un nuevo artículo
                limpiarCampos();

                // Cambiar el estado de los botones
                EstadoAnadir(enModoEdicion);
            }
            catch (Exception ex)
            {
                // Manejar errores durante la acción
                Console.WriteLine($"Error en btnAnadir_Click: {ex.Message}");
            }
        }

        // Comando para manejar el botón "Cargar Imagen"
        [RelayCommand]
        private async void btnCargarImagen_Click(Window ventanaPadre)
        {
            try
            {
                // Configurar el diálogo para seleccionar una imagen
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Seleccionar una imagen",
                    InitialFileName = "C:/Users/Sebas/Desktop",
                    AllowMultiple = false
                };
                openFileDialog.Filters.Add(new FileDialogFilter { Name = "Imágenes JPG", Extensions = { "jpg" } });

                // Mostrar el diálogo y obtener la ruta de la imagen seleccionada
                var result = await openFileDialog.ShowAsync(ventanaPadre);
                if (result != null && result.Length > 0)
                {
                    string rutaFoto = result[0];

                    // Cargar la imagen seleccionada en la interfaz
                    Imagen = new Bitmap(rutaFoto);

                    // Guardar la imagen en el directorio correspondiente
                    controlador.guardarImagen(rutaFoto, ++CtrBaraja.contadorId);

                    // Actualizar el ID de la imagen
                    aux = CtrBaraja.contadorId;
                }
            }
            catch (Exception ex)
            {
                // Manejar errores durante la acción
                Console.WriteLine($"Error en btnCargarImagen_Click: {ex.Message}");
            }
        }
    }
}