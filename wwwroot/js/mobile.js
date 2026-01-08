// Mobile Menu Controller
window.MobileMenu = {
    isOpen: false,
    
    init: function() {
        this.createMenuToggle();
        this.createOverlay();
        this.attachEventListeners();
        this.handleResize();
    },
    
    createMenuToggle: function() {
        const topRow = document.querySelector('.top-row');
        if (!topRow || window.innerWidth > 768) return;
        
        const menuToggle = document.createElement('button');
        menuToggle.className = 'menu-toggle';
        menuToggle.innerHTML = `
            <div class="hamburger">
                <span></span>
                <span></span>
                <span></span>
            </div>
        `;
        
        topRow.insertBefore(menuToggle, topRow.firstChild);
    },
    
    createOverlay: function() {
        if (document.querySelector('.mobile-overlay')) return;
        
        const overlay = document.createElement('div');
        overlay.className = 'mobile-overlay';
        document.body.appendChild(overlay);
    },
    
    attachEventListeners: function() {
        // Menu toggle click
        document.addEventListener('click', (e) => {
            if (e.target.closest('.menu-toggle')) {
                this.toggle();
            }
        });
        
        // Overlay click
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('mobile-overlay')) {
                this.close();
            }
        });
        
        // Navigation link click
        document.addEventListener('click', (e) => {
            if (e.target.closest('.sidebar .nav-link')) {
                this.close();
            }
        });
        
        // Escape key
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.close();
            }
        });
        
        // Resize
        window.addEventListener('resize', () => {
            this.handleResize();
        });
    },
    
    toggle: function() {
        if (this.isOpen) {
            this.close();
        } else {
            this.open();
        }
    },
    
    open: function() {
        const sidebar = document.querySelector('.sidebar');
        const overlay = document.querySelector('.mobile-overlay');
        const menuToggle = document.querySelector('.menu-toggle');
        
        if (sidebar) sidebar.classList.add('open');
        if (overlay) overlay.classList.add('active');
        if (menuToggle) menuToggle.classList.add('open');
        
        document.body.style.overflow = 'hidden';
        this.isOpen = true;
    },
    
    close: function() {
        const sidebar = document.querySelector('.sidebar');
        const overlay = document.querySelector('.mobile-overlay');
        const menuToggle = document.querySelector('.menu-toggle');
        
        if (sidebar) sidebar.classList.remove('open');
        if (overlay) overlay.classList.remove('active');
        if (menuToggle) menuToggle.classList.remove('open');
        
        document.body.style.overflow = '';
        this.isOpen = false;
    },
    
    handleResize: function() {
        if (window.innerWidth > 768) {
            this.close();
            const menuToggle = document.querySelector('.menu-toggle');
            if (menuToggle) menuToggle.remove();
        } else {
            this.createMenuToggle();
        }
    }
};

// Touch Gestures Support
window.TouchGestures = {
    startX: 0,
    startY: 0,
    threshold: 50,
    
    init: function() {
        if (!('ontouchstart' in window)) return;
        
        document.addEventListener('touchstart', (e) => {
            this.startX = e.touches[0].clientX;
            this.startY = e.touches[0].clientY;
        }, { passive: true });
        
        document.addEventListener('touchend', (e) => {
            const endX = e.changedTouches[0].clientX;
            const endY = e.changedTouches[0].clientY;
            const diffX = endX - this.startX;
            const diffY = Math.abs(endY - this.startY);
            
            // Swipe right to open menu
            if (diffX > this.threshold && diffY < this.threshold && this.startX < 50) {
                window.MobileMenu.open();
            }
            
            // Swipe left to close menu
            if (diffX < -this.threshold && diffY < this.threshold && window.MobileMenu.isOpen) {
                window.MobileMenu.close();
            }
        }, { passive: true });
    }
};

// Scroll Position Persistence
window.ScrollPersistence = {
    save: function(key) {
        const scrollPos = window.scrollY || document.documentElement.scrollTop;
        sessionStorage.setItem(`scroll_${key}`, scrollPos);
    },
    
    restore: function(key) {
        const scrollPos = sessionStorage.getItem(`scroll_${key}`);
        if (scrollPos) {
            window.scrollTo(0, parseInt(scrollPos));
            sessionStorage.removeItem(`scroll_${key}`);
        }
    }
};

// Viewport Height Fix (iOS Safari)
window.ViewportFix = {
    init: function() {
        this.setHeight();
        window.addEventListener('resize', () => this.setHeight());
        window.addEventListener('orientationchange', () => {
            setTimeout(() => this.setHeight(), 200);
        });
    },
    
    setHeight: function() {
        const vh = window.innerHeight * 0.01;
        document.documentElement.style.setProperty('--vh', `${vh}px`);
    }
};

// Pull to Refresh Disable
window.DisablePullToRefresh = {
    init: function() {
        let lastY = 0;
        
        document.addEventListener('touchstart', (e) => {
            lastY = e.touches[0].clientY;
        }, { passive: false });
        
        document.addEventListener('touchmove', (e) => {
            const currentY = e.touches[0].clientY;
            const isAtTop = window.scrollY === 0;
            const isPulling = currentY > lastY;
            
            if (isAtTop && isPulling) {
                e.preventDefault();
            }
        }, { passive: false });
    }
};

// Auto Init on Load
document.addEventListener('DOMContentLoaded', () => {
    window.MobileMenu.init();
    window.TouchGestures.init();
    window.ViewportFix.init();
    window.DisablePullToRefresh.init();
});

// Blazor Reconnection
if (window.Blazor) {
    Blazor.start().then(() => {
        // Reinit after Blazor loads
        setTimeout(() => {
            window.MobileMenu.init();
        }, 100);
    });
}
