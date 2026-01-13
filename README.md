# AvaloniaCatalogoWinForms

## Qué es
Aplicación de escritorio en C# orientada a gestionar/visualizar un catálogo (productos u otros ítems), construida para practicar UI desktop y comparar enfoques (Avalonia vs WinForms) si el proyecto incluye ambas versiones.

> Si el repo contiene dos implementaciones (Avalonia + WinForms), indícalo claramente. Si es solo Avalonia, elimina la parte WinForms.

## Stack
- C# / .NET: net8.0
- UI: Avalonia UI (11.2.3)
- IDE: Visual Studio 2022
- Arquitectura: MVVM
- Persistencia: JSON

## Features
- Listado de ítems del catálogo con vista detallada
- Búsqueda/filtrado por campos (nombre, categoría, etc.)
- CRUD básico: crear, editar y eliminar
- Navegación entre pantallas/vistas

## Capturas/GIF
<img width="777" height="500" alt="image" src="https://github.com/user-attachments/assets/fad85933-4ebf-4594-8d26-6e2ebc63aa0d" />
<img width="800" height="420" alt="image" src="https://github.com/user-attachments/assets/237d7c1b-5304-4135-8386-d198752f14cf" />
<img width="800" height="449" alt="image" src="https://github.com/user-attachments/assets/a8d14dce-bd93-4554-8677-f12c719a4e34" />

## Cómo ejecutar
1. Clona el repositorio
2. Abre la solución `.sln` en Visual Studio
3. Restaura dependencias (NuGet Restore)
4. Ejecuta (F5)

## Qué aprendí
- Montar UI desktop en .NET con un framework moderno (Avalonia) y/o WinForms
- Patrón MVVM (si aplica): separación UI / lógica / modelos
- Importancia de la higiene del repo: `.vs`, `bin/`, `obj/` y archivos `.user` no deben subirse
- Estructurar un proyecto para que sea entendible por terceros (README + capturas + run steps)
