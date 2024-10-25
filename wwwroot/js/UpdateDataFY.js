function showInput(option) {
    document.getElementById('nombreInput').classList.add('hidden');
    document.getElementById('apellidoInput').classList.add('hidden');
    document.getElementById('emailInput').classList.add('hidden');
    document.getElementById('contrasenaInput').classList.add('hidden');
    document.getElementById(option + 'Input').classList.remove('hidden');
}

document.getElementById('btnout').addEventListener('click', function () {
    window.location.href = window.redirectToMenu; // Usa la variable global
});