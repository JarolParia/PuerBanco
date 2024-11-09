function selectAccount(accountType) {
    const radios = document.querySelectorAll('input[name="typeId"]');
    radios.forEach((radio) => {
        radio.checked = false;
    });


    const selectedRadio = document.getElementById(accountType);
    if (selectedRadio) {
        selectedRadio.checked = true;
    }
}


document.getElementById('btn-outca').addEventListener('click', function () {
    window.location.href = window.redirectToMenu; 
});