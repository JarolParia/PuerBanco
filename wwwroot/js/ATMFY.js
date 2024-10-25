document.addEventListener('DOMContentLoaded', function () {
    // Verifica si el botón de transferencia existe y, si es así, añade el event listener
    const transferButton = document.getElementById('Transfer-Btn');
    if (transferButton) {
        transferButton.addEventListener('click', function () {
            window.location.href = window.redirectToTransfer;
        });
    }

    // Agrega listeners para los demás botones
    document.getElementById('BotonSalir').addEventListener('click', function () {
        window.location.href = window.redirectToMenu;
    });

    document.getElementById('withdrawal-Btn').addEventListener('click', function () {
        window.location.href = window.redirectToWithdrawal;
    });

    document.getElementById('Balance-Btn').addEventListener('click', function () {
        window.location.href = window.redirectToBalance;
    });
    document.getElementById('Deposit-Btn').addEventListener('click', function () {
        window.location.href = window.redirectToDeposit;
    });
    document.getElementById('Movements-Btn').addEventListener('click', function () {
        window.location.href = window.redirectToMovements;
    });

    const interestButton = document.getElementById('Interest-Btn');
    if (interestButton) {
        interestButton.addEventListener('click', function () {
            window.location.href = window.redirectToInterest;
        });
    }

});
