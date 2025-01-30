using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
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
        private string rutaImagenOriginal;
        private int aux;
        
        
        [ObservableProperty] private bool _btnDeshabilitar;
        [ObservableProperty] private String _btnAceptar;
        [ObservableProperty] private String _btnCancelar;
        
        
        [ObservableProperty] private String _dato;
        [ObservableProperty] private String _imageDisplay;

        public Mainviewmodel()
        {
            //noEditable();                           // Deshabilita la edición de los campos inicialmente
            controlador = CtrBaraja.getControlador(); // Obtiene la instancia del controlador
            listaArticulos = controlador.ObtenerListaMagica(); // Carga la lista de artículos mágicos
            MostrarArticulo();                      // Muestra el primer artículo de la lista
            
        }
        
        //[ObservableProperty] public string _nombre;
        /*
         *     [RelayCommand]
                private void CambiarPersona()
                {
                    Nombre = _listaNombres[r.Next(_listaNombres.Length)];
                    Edad = r.Next(18, 75);
                }
         */
        
        //Habilitar y deshabilitar los botones además de modificar el texto
        [RelayCommand]
        private void EstadoAnadir()
        {
            if (!enModoEdicion)
            {
                BtnDeshabilitar = false;
                BtnAceptar = "Aceptar";
                BtnCancelar = "Cancelar";
            }
            else
            {
                BtnDeshabilitar = true;
                BtnAceptar = "Anterior";
                BtnCancelar = "Siguiente";
            }

        }
        
        

        
        
        
        
        public void limpiarCampos()
        {
            // Limpiar los campos si la lista está vacía
            Dato = "";
            ImageDisplay = Image.FromFile("incognita.jpg");
        }

        // Muestra el artículo actual en los campos de texto y la imagen correspondiente
        private void MostrarArticulo()
        {
            listaArticulos = controlador.ObtenerListaMagica(); // Actualiza la lista de artículos

            // Configura la imagen predeterminada si no se encuentra ninguna
            ImageDisplay.Image = Image.FromFile("incognita.jpg");
            ImageDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

            if (listaArticulos != null && listaArticulos.Count > 0)
            {
                Baraja articulo = listaArticulos[posicionActual];
                txtNombre.Text = articulo.Nombre;
                txtCategoria.Text = articulo.Categoria;
                txtDificultad.Text = articulo.Dificultad.ToString();
                txtPrecio.Text = articulo.Precio.ToString("F2");     // Formato de precio con 2 decimales
                txtDesc.Text = articulo.Desc;

                // Verifica si existe una imagen asociada al artículo
                string imagePath = $"IMG/{articulo.ImagenId}.jpg";
                if (File.Exists(imagePath))
                {
                    ImageDisplay.Image = Image.FromFile(imagePath);
                }
            }
            else
            {
                // Limpia los campos si la lista está vacía
                limpiarCampos();
            }
        }

        // Evento del botón "Siguiente" para avanzar al siguiente artículo
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (posicionActual < listaArticulos.Count - 1)
            {
                posicionActual++;
                MostrarArticulo();
            }
        }

        // Evento del botón "Anterior" para retroceder al artículo anterior
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (posicionActual > 0)
            {
                posicionActual--;
                MostrarArticulo();
            }
        }

        // Evento del botón "Eliminar" para eliminar el artículo actual
        private void btnEliminar_Click(object sender, EventArgs e)
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
        
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!enModoEdicion)
            {
                btnImagen.Enabled = true;
                articuloOriginal = listaArticulos.Count > 0 ? listaArticulos[posicionActual] : null;

                limpiarCampos(); // Prepara el formulario para una nueva entrada
                SetTextBoxReadOnly(true); // Habilita la edición

                btnAgregar.Text = "Guardar";
                enModoEdicion = true;
                btnSiguiente.Enabled = btnAnterior.Enabled = false; // Deshabilita la navegación
            }
            else
                {
                    //Imagen si es null setea la imagen como 0, sino toma el orden que le toca.
                    if (pictureBoxPD == null)
                    {
                        aux = 0;
                    }
                    else
                    {
                        aux = viewmodel.CtrBaraja.contadorId;
                    }
                    
                    Baraja CtrBaraja = new Baraja
                    {
                        Nombre = txtNombre.Text,
                        Categoria = txtCategoria.Text,
                        Precio = double.TryParse(txtPrecio.Text, out double precio) ? precio : 0,
                        Dificultad = txtDificultad.Text.ToLower() == "si",
                        Desc = txtDesc.Text,
                        ImagenId = aux 
                    };
                    CtrBaraja.getControlador().agregarArticuloMagico(nuevaBaraja);
                    

                    MessageBox.Show("Artículo mágico agregado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Después de guardar
                    SetTextBoxReadOnly(false);

                    // Habilitar botones Siguiente y Anterior
                    btnSiguiente.Enabled = true;
                    btnAnterior.Enabled = true;

                    btnAgregar.Text = "Agregar"; // Cambiar el texto del botón de nuevo
                    enModoEdicion = false; // Desactivar el modo edición
                    controlador.GuardarListaEnFichero();
                } 
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Restaurar los valores originales si existe un artículo guardado
            if (articuloOriginal != null)
            {
                // Restaurar los valores del artículo original
                listaArticulos[posicionActual] = articuloOriginal;  // Restauramos el artículo a su estado original
                MostrarArticulo();  // Mostrar la información restaurada

                // Salir del modo edición
                SetTextBoxReadOnly(false);
                enModoEdicion = false;
            }
            // Habilitar botones Siguiente y Anterior
            btnSiguiente.Enabled = true;
            btnAnterior.Enabled = true;
        }

        private void btnImagenPD_Click(object sender, EventArgs e)
        {
            // Crear un nuevo OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configuración del diálogo de selección de archivo
            openFileDialog.Title = "Seleccionar una imagen"; // Título del diálogo
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;"; // Filtro para solo mostrar imágenes
            openFileDialog.InitialDirectory = "C:/Users/Sebas/Desktop"; // Directorio inicial que se abrirá al abrir el diálogo (puedes cambiarlo según tu preferencia)

            // Mostrar el diálogo y comprobar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Almacenar la ruta del archivo seleccionado
                rutaImagenOriginal = openFileDialog.FileName;

                // Aquí puedes almacenar la ruta de la imagen en una variable, como por ejemplo un campo de la clase 'Magia'
                pictureBox.Image = Image.FromFile(rutaImagenOriginal);

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                controlador.guardarImagen(rutaImagenOriginal, ++CtrBaraja.contadorId);
            }
        }
}