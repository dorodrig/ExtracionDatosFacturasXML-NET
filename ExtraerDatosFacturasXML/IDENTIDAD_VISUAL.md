# ?? GUÍA DE IDENTIDAD VISUAL - FASECOLDA

## ?? Resumen

Se ha implementado una hoja de estilos corporativa completa basada en la identidad visual de **Fasecolda** (Federación de Aseguradores de Colombia).

---

## ?? Paleta de Colores Corporativos

### Colores Primarios

| Color | Hex | Uso | Muestra |
|-------|-----|-----|---------|
| **Navy Fasecolda** | `#1B3B6F` | Headers, títulos principales | ?? |
| **Azul Fasecolda** | `#0066CC` | Links, botones primarios | ?? |
| **Verde Fasecolda** | `#7ACC00` | Acentos, CTAs, highlights | ?? |

### Colores Secundarios

| Color | Hex | Uso |
|-------|-----|-----|
| Navy Oscuro | `#162E55` | Hover de navy |
| Navy Claro | `#2D4A7C` | Variaciones |
| Azul Hover | `#0056B3` | Hover de links |
| Verde Hover | `#6AB700` | Hover de botones |

### Colores Neutros

| Color | Hex | Uso |
|-------|-----|-----|
| Blanco | `#FFFFFF` | Fondos, tarjetas |
| Gris 50 | `#F9FAFB` | Fondos secundarios |
| Gris 700 | `#374151` | Texto principal |
| Gris 600 | `#4B5563` | Texto secundario |

### Colores de Estado

| Color | Hex | Uso |
|-------|-----|-----|
| Success | `#10B981` | Mensajes de éxito |
| Warning | `#F59E0B` | Advertencias |
| Error | `#EF4444` | Errores |
| Info | `#3B82F6` | Información |

---

## ?? Variables CSS Principales

```css
/* Colores Primarios */
--fasecolda-navy: #1B3B6F;
--fasecolda-blue: #0066CC;
--fasecolda-green: #7ACC00;

/* Gradientes */
--gradient-primary: linear-gradient(135deg, #1B3B6F 0%, #0066CC 100%);
--gradient-accent: linear-gradient(135deg, #7ACC00 0%, #9DE01A 100%);

/* Sombras */
--shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
--shadow-xl: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
--shadow-2xl: 0 25px 50px -12px rgba(0, 0, 0, 0.25);

/* Tipografía */
--font-family-base: 'Segoe UI', sans-serif;

/* Espaciados */
--spacing-md: 1rem;
--spacing-lg: 1.5rem;
--spacing-xl: 2rem;

/* Bordes */
--border-radius-md: 0.5rem;
--border-radius-lg: 0.75rem;
--border-radius-xl: 1rem;
```

---

## ?? Componentes Implementados

### 1. **Layout Principal**
- Fondo con gradiente corporativo Navy ? Azul
- Efecto de animación sutil (pulse)
- Centrado vertical y horizontal

### 2. **Tarjetas (Cards)**
- Fondo blanco con sombras profundas
- Bordes redondeados modernos
- Animación de entrada (fadeInUp)
- Border inferior verde en headers

### 3. **Formularios**
- Input de archivo personalizado con drag & drop visual
- Border dashed azul corporativo
- Hover effect con cambio a fondo azul
- Feedback visual al seleccionar archivos

### 4. **Botones**

#### Botón Primario (.btn-primary)
```css
background: linear-gradient(135deg, #1B3B6F 0%, #0066CC 100%);
```
- Gradiente navy ? azul
- Efecto hover con elevación
- Usado para acciones principales

#### Botón Acento (.btn-accent)
```css
background: linear-gradient(135deg, #7ACC00 0%, #9DE01A 100%);
```
- Gradiente verde corporativo
- Usado para acciones secundarias destacadas

#### Botón Secundario (.btn-secondary)
```css
background: #6B7280;
```
- Gris neutro
- Usado para acciones terciarias

### 5. **Alertas y Mensajes**
- 4 tipos: success, error, warning, info
- Border lateral colorido
- Fondos sutiles con transparencia
- Iconos de emoji para accesibilidad

### 6. **Página de Éxito**
- Icono animado con bounceIn
- Lista de facturas con hover effect
- Border lateral verde corporativo
- Botones con gradientes

### 7. **Página de Error**
- Icono con animación shake
- Colores de error corporativos
- Diseño empático y profesional

---

## ?? Responsive Design

### Breakpoints

```css
/* Tablets y dispositivos medianos */
@media (max-width: 768px) {
    /* Padding reducido */
    /* Logo más pequeño (200px) */
    /* Botones en columna */
}

/* Móviles */
@media (max-width: 480px) {
    /* Títulos más pequeños */
    /* Iconos reducidos */
    /* Espaciados compactos */
}
```

---

## ? Animaciones Implementadas

### 1. **fadeInUp** (Entrada de tarjetas)
```css
@keyframes fadeInUp {
    from {
        opacity: 0;
        transform: translateY(30px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}
```

### 2. **bounceIn** (Icono de éxito)
```css
@keyframes bounceIn {
    0% { transform: scale(0); }
    50% { transform: scale(1.1); }
    100% { transform: scale(1); }
}
```

### 3. **shake** (Icono de error)
```css
@keyframes shake {
    0%, 100% { transform: translateX(0); }
    25% { transform: translateX(-10px); }
    75% { transform: translateX(10px); }
}
```

### 4. **pulse** (Fondo)
```css
@keyframes pulse {
    0%, 100% { transform: scale(1); opacity: 0.3; }
    50% { transform: scale(1.1); opacity: 0.5; }
}
```

### 5. **spin** (Spinner de carga)
```css
@keyframes spin {
    to { transform: rotate(360deg); }
}
```

---

## ?? Clases Utilitarias

```css
/* Texto */
.text-center { text-align: center; }
.text-muted { color: #6B7280; font-size: 0.875rem; }

/* Espaciados */
.mt-1, .mt-2, .mt-3, .mt-4, .mt-5
.mb-1, .mb-2, .mb-3, .mb-4, .mb-5

/* Botones */
.btn-full { width: 100%; }
.btn-lg { padding: 1.5rem 2rem; font-size: 1.125rem; }
```

---

## ?? Estructura de Archivos

```
ExtraerDatosFacturasXML/
??? wwwroot/
    ??? css/
        ??? fasecolda-styles.css  ? Hoja de estilos corporativa
```

---

## ?? Cómo Usar

### En Razor Pages

```html
<!DOCTYPE html>
<html lang="es">
<head>
    <link rel="stylesheet" href="~/css/fasecolda-styles.css" />
</head>
<body>
    <div class="page-wrapper">
        <div class="card">
            <!-- Contenido -->
        </div>
    </div>
</body>
</html>
```

### Ejemplos de Componentes

#### Botón Primario
```html
<button class="btn btn-primary btn-full btn-lg">
    <span>??</span>
    <span>Procesar Facturas</span>
</button>
```

#### Alerta de Éxito
```html
<div class="alert alert-success">
    ? Factura procesada correctamente
</div>
```

#### Lista de Facturas
```html
<div class="invoice-list">
    <h3 class="invoice-list-title">
        <span>??</span>
        <span>Facturas procesadas:</span>
    </h3>
    <ul>
        <li>FACTURA-001</li>
        <li>FACTURA-002</li>
    </ul>
</div>
```

---

## ?? Comparación Antes/Después

### Antes (Inline Styles)
- Estilos repetidos en cada página
- Sin paleta de colores consistente
- Difícil de mantener
- No responsive optimizado

### Después (Hoja de Estilos Corporativa)
- ? Estilos centralizados y reutilizables
- ? Paleta corporativa de Fasecolda
- ? Fácil mantenimiento
- ? Responsive design completo
- ? Animaciones profesionales
- ? Variables CSS para customización
- ? Accesibilidad mejorada

---

## ?? Características Destacadas

1. **Gradientes Corporativos**: Navy ? Azul (primario), Verde (acento)
2. **Sombras Profundas**: Look moderno y profesional
3. **Animaciones Sutiles**: Mejoran UX sin distraer
4. **Responsive**: Adaptable a móviles, tablets y desktop
5. **Accesibilidad**: Focus visible, colores con contraste adecuado
6. **Performance**: Animaciones con GPU acceleration
7. **Mantenibilidad**: Variables CSS para cambios rápidos

---

## ?? Personalización Futura

### Cambiar Colores Primarios

Editar las variables en `:root`:

```css
:root {
    --fasecolda-navy: #TU_COLOR;
    --fasecolda-blue: #TU_COLOR;
    --fasecolda-green: #TU_COLOR;
}
```

### Agregar Nuevos Componentes

Seguir la estructura de nomenclatura:

```css
/* ====================================================================
   XX. NOMBRE DEL COMPONENTE
   ==================================================================== */
.mi-componente {
    /* Estilos */
}
```

---

## ?? Referencias

- **Sitio web oficial**: https://www.fasecolda.com
- **Logo**: https://www.fasecolda.com/wp-content/themes/fasecolda/assets/img/logo-fasecolda.svg
- **Colores extraídos**: Del header y elementos principales del sitio

---

## ? Checklist de Implementación

- [x] Paleta de colores corporativos definida
- [x] Variables CSS creadas
- [x] Componentes de formulario estilizados
- [x] Botones con gradientes corporativos
- [x] Páginas Index, Success, Error actualizadas
- [x] Animaciones implementadas
- [x] Responsive design aplicado
- [x] Accesibilidad verificada
- [x] Documentación completa

---

**Fecha de implementación**: 2024  
**Versión**: 1.0.0  
**Archivo**: `wwwroot/css/fasecolda-styles.css`
