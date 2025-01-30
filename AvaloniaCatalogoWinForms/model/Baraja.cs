namespace AvaloniaApplication1.model;

public class Baraja
{
    public const int TAM_MAX = 30;
    string? nombre;
    string? categoria;
    bool dificultad;
    double precio;
    string? desc;
    public int ImagenId { get; set; }
    
    // Constructor que asegura que el tipo sea BARAJA
    public Baraja(string? nombre, string? categoria, bool dificultad, double precio, string? desc, int imagen)
    {
        this.nombre = nombre;
        this.categoria = categoria;
        this.dificultad = dificultad;
        this.precio = precio;
        this.desc = desc;
        this.ImagenId = ImagenId;
    }

    public Baraja()
    {
    }

    // Método get() y set() para Nombre
    public string? Nombre
    {
        get { return nombre; }
        set
        {
            if (value == null)  // Validar si el valor recibido es null
            {
                throw new nombreInvalidoException("Nombre inválido, el nombre no puede ser null");
            }
            else if (value.Length > 25)  // Validar que la longitud no exceda los 25 caracteres
            {
                throw new nombreInvalidoException("Nombre demasiado largo, debe ser inferior a 25 caracteres");
            }
            else
            {
                nombre = value;  // Asignar el valor si es válido
            }
        }
    }


    // Método get() y set() para Categoria
    public string? Categoria
    {
        get { return categoria; }
        set
        {
            if (value == null)  // Validar si el valor recibido es null
            {
                throw new categoriaInvalidaException("Categoria inválida, la categoría no puede ser null");
            }
            else if (value.Length > TAM_MAX)  // Validar longitud
            {
                throw new categoriaInvalidaException("Categoria demasiado larga, debe ser inferior a 25 caracteres");
            }
            else
            {
                categoria = value;  // Asignar el valor si es válido
            }
        }
    }

    // Método get() y set() para Dificultad
    public bool Dificultad
    {
        get { return dificultad; }
        set
        {
            dificultad = value;  // Asignar el valor recibido (true o false)
        }
    }

    // Método get() y set() para Precio
    public double Precio
    {
        get { return precio; }
        set
        {
            if (value <= 0)  // Validar que el precio sea mayor que 0
            {
                throw new precioInvalidoException("Precio inválido, debe ser superior a 0");
            }
            else
            {
                precio = value;  // Asignar el valor si es válido
            }
        }
    }

    // Método get() y set() para Desc
    public string? Desc
    {
        get { return desc; }
        set
        {
            if (value == null)  // Validar si el valor recibido es null
            {
                throw new descInvalidaException("Descripción inválida, la descripción no puede ser null");
            }
            else if (value.Length > 100)  // Validar longitud
            {
                throw new descInvalidaException("Descripción inválida, debe ser inferior a 100 caracteres");
            }
            else
            {
                desc = value;  // Asignar el valor si es válido
            }
        }
    }

    public string ToString()
    {
        return $"\nNombre: {Nombre}" +
               $"\tCategoria: {Categoria}" +
               $"\tDificultad: {(Dificultad ? "Difícil" : "Fácil")}" +
               $"\tPrecio: {Precio} euros" +
               $"\tDescripcion: {Desc}" +
               "\n------------------------------------------------------------------";
    }
    
}