document.getElementById('Botonsalir').addEventListener('click', function () {
    window.location.href = window.redirectToMenu;
});

document.getElementById('v-1').addEventListener('click', function () {
    document.getElementById('montoInput').value = '10000';  // Asigna el valor al input
    document.getElementById('montoInput').placeholder = '$ 10.000';  // Cambia el placeholder
});

document.getElementById('v-2').addEventListener('click', function () {
    document.getElementById('montoInput').value = '20000';  // Asigna el valor al input
    document.getElementById('montoInput').placeholder = '$ 20.000';  // Cambia el placeholder
});

document.getElementById('v-3').addEventListener('click', function () {
    document.getElementById('montoInput').value = '50000';  // Asigna el valor al input
    document.getElementById('montoInput').placeholder = '$ 50.000';  // Cambia el placeholder
});

document.getElementById('v-4').addEventListener('click', function () {
    document.getElementById('montoInput').value = '100000';
    document.getElementById('montoInput').placeholder = '$ 100.000';
});