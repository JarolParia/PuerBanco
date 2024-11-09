document.addEventListener("DOMContentLoaded", function () {

    // Capturamos el evento de clic del botón "Ingresar"
        document.getElementById('JoinAccountButton').addEventListener('click', function(event) {
            // Prevenir el comportamiento por defecto (enviar formulario)
            event.preventDefault();

        // Obtener el número de cuenta seleccionado
        var selectedAccount = document.querySelector('input[name="cuenta"]:checked');

        if (selectedAccount) {
            // Guardamos el número de cuenta seleccionado en la sesión
            document.getElementById('accountNumberInput').value = selectedAccount.value;
        // Luego enviamos el formulario
        document.getElementById('loginForm').submit();
        } 
    });

    document.getElementById("ReturnButton").addEventListener("click", function () {
        window.location.href = window.redirectToMenu;
    });
});
