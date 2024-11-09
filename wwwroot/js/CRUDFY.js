document.addEventListener('DOMContentLoaded', () => {
    // Corregir el selector para el botón de historial
    const historyButton = document.querySelector('#filtro-datos button');
    const modalHistory = document.querySelector('#modal-History');
    const modalBody = document.querySelector('.modalH-body');
    const closeModalButton = document.querySelector('.close-btn');

    if (historyButton && modalHistory && closeModalButton) {
        // Prevenir el comportamiento por defecto del formulario
        document.querySelector('#filtro-datos').addEventListener('submit', (e) => {
            e.preventDefault();
            modalHistory.style.display = 'flex';
            loadChangesHistory();
        });

        closeModalButton.addEventListener('click', () => {
            closeModal();
        });

        // Cerrar modal al hacer clic fuera
        window.addEventListener('click', (e) => {
            if (e.target === modalHistory) {
                closeModal();
            }
        });
    }
});

function closeModal() {
    const modalHistory = document.querySelector('#modal-History');
    modalHistory.style.display = 'none';
}

async function loadChangesHistory() {
    const modalBody = document.querySelector('.modalH-body');
    try {
        const response = await fetch('/CRUD/GetChangesHistory');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        if (data.error) {
            modalBody.innerHTML = `<p class="error">${data.error}</p>`;
            return;
        }

        const table = `
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Usuario</th>
                        <th>Fecha</th>
                        <th>Tipo de Cambio</th>
                        <th>Tabla Afectada</th>
                        <th>ID Registro</th>
                        <th>Descripción</th>
                    </tr>
                </thead>
                <tbody>
                    ${data.map(change => `
                        <tr>
                            <td>${change.changesId}</td>
                            <td>${change.userName}</td>
                            <td>${change.changeDate}</td>
                            <td>${change.changeType}</td>
                            <td>${change.tableAffected}</td>
                            <td>${change.recordId}</td>
                            <td>${change.description}</td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;
        modalBody.innerHTML = table;
    } catch (error) {
        console.error('Error:', error);
        modalBody.innerHTML = '<p class="error">Error al cargar el historial de cambios</p>';
    }
}