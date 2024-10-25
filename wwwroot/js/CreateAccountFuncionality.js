function selectAccount(accountType) {
    const radios = document.querySelectorAll('input[name="tipo"]');
    radios.forEach((radio) => {
        radio.checked = false;
    });


    const selectedRadio = document.getElementById(accountType);
    if (selectedRadio) {
        selectedRadio.checked = true;
    }
}

document.getElementById("CreateAccount").addEventListener("click", function () {
    window.location.href = "Menu"
})

document.getElementById('btn-outca').addEventListener('click', function () {
    window.location.href = window.redirectToMenu; 
});