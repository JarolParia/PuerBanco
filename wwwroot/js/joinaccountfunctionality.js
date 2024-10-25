
document.addEventListener("DOMContentLoaded", function () { // Espera a que el DOM esté completamente cargado
    document.getElementById("JoinAccountButton").addEventListener("click", function () {
        const radios = document.querySelectorAll('input[name="cuenta"]');
        let selectedAccountType = null;

        radios.forEach((radio) => {
            if (radio.checked) {
                selectedAccountType = radio.value;
            }
        });

        if (selectedAccountType) {
            window.location.href = `/ATM/ATM?accountType=${selectedAccountType}`;
        } else {
            alert("Por favor, seleccione una cuenta antes de continuar.");
        }
    });

    document.getElementById("ReturnButton").addEventListener("click", function () {
        window.location.href = window.redirectToMenu;
    });
});

