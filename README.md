# Catálogo de barajas — Avalonia

Proyecto académico de escritorio en C#/.NET 8 con Avalonia 11.2.3, CommunityToolkit.Mvvm y persistencia JSON.

## Implementado
- Vista de detalle y navegación entre registros.
- Alta y eliminación de barajas con imagen.
- Modelo con validaciones, propiedades observables y comandos MVVM.
- Lectura y escritura del catálogo en JSON.

## Ejecutar
Instala el SDK .NET 8 y ejecuta desde la raíz:

```sh
dotnet restore
dotnet build
dotnet run --project AvaloniaCatalogoWinForms
```

## Alcance y límites
El código publicado contiene la versión Avalonia. No contiene una segunda aplicación WinForms ni implementación de edición, búsqueda, filtros u ordenación. La persistencia utiliza rutas locales; es una aplicación académica y requiere validación funcional en una ejecución limpia.

## Mantenimiento
Corregidos el parámetro de imagen del constructor, los límites contradictorios de anchura, la creación prematura de registros al entrar en alta y la normalización del índice después de vaciar el catálogo. Estas correcciones requieren comprobación visual en .NET.

## Próximas mejoras
Edición y búsqueda, mensajes de error visibles, persistencia más robusta y pruebas de las operaciones del catálogo.

## Capturas del proyecto original

<img width="777" height="500" alt="image" src="https://github.com/user-attachments/assets/fad85933-4ebf-4594-8d26-6e2ebc63aa0d" />

<img width="800" height="420" alt="image" src="https://github.com/user-attachments/assets/237d7c1b-5304-4135-8386-d198752f14cf" />

<img width="800" height="449" alt="image" src="https://github.com/user-attachments/assets/a8d14dce-bd93-4554-8677-f12c719a4e34" />

Estas capturas son históricas; no verifican los cambios de mantenimiento.
