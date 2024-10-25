function toggleMenu() {
    const contenedor = document.querySelector('.contenedor');
    contenedor.classList.toggle('menu-abierto');
}

function mostrarCuentas(button) {
    var tablaCuentas = document.querySelector('.tabla-cuentas');
    tablaCuentas.style.display = (tablaCuentas.style.display === "none" || tablaCuentas.style.display === "") ? "table" : "none";
}