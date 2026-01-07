// Sistema de Atalhos de Teclado
window.hotelaria = window.hotelaria || {};

window.hotelaria.atalhos = {
    init: function(dotNetHelper) {
        this.dotNetHelper = dotNetHelper;
        this.setupKeyboardShortcuts();
    },

    setupKeyboardShortcuts: function() {
        document.addEventListener('keydown', (e) => {
            // F1 - Ajuda contextual
            if (e.key === 'F1') {
                e.preventDefault();
                this.triggerAjuda();
                return;
            }

            // Atalhos com Ctrl
            if (e.ctrlKey) {
                switch(e.key.toLowerCase()) {
                    case 'h':
                        e.preventDefault();
                        window.location.href = '/';
                        break;
                    case 'r':
                        e.preventDefault();
                        window.location.href = '/reservas';
                        break;
                    case 'q':
                        e.preventDefault();
                        window.location.href = '/quartos';
                        break;
                    case 'd':
                        e.preventDefault();
                        window.location.href = '/disponibilidade';
                        break;
                    case 'f':
                        e.preventDefault();
                        window.location.href = '/financeiro';
                        break;
                }
            }

            // ESC - Fechar modais
            if (e.key === 'Escape') {
                const closeButtons = document.querySelectorAll('.btn-close, .modal-overlay');
                if (closeButtons.length > 0) {
                    closeButtons[0].click();
                }
            }

            // F3 - Focar campo de busca
            if (e.key === 'F3') {
                e.preventDefault();
                const searchInput = document.querySelector('input[type="text"][placeholder*="Buscar"], input[type="search"]');
                if (searchInput) {
                    searchInput.focus();
                }
            }

            // F5 - Atualizar (deixar comportamento padrão)
        });
    },

    triggerAjuda: function() {
        const ajudaButton = document.querySelector('.btn-ajuda');
        if (ajudaButton) {
            ajudaButton.click();
        }
    }
};

// Inicializar quando o DOM estiver pronto
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.hotelaria.atalhos.init();
    });
} else {
    window.hotelaria.atalhos.init();
}
