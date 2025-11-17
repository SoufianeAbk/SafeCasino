// SafeCasino JavaScript

// DOM Ready
document.addEventListener('DOMContentLoaded', function() {
    initializeApp();
});

// Initialize all components
function initializeApp() {
    initTooltips();
    initGameFilters();
    initLazyLoading();
    initGameCards();
    initLanguageSwitch();
    initSmoothScroll();
    initSearchDebounce();
}

// Initialize Bootstrap tooltips
function initTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Game filter handling
function initGameFilters() {
    const filterForm = document.querySelector('.filter-form');
    if (filterForm) {
        // Auto-submit on select change
        const selects = filterForm.querySelectorAll('select');
        selects.forEach(select => {
            select.addEventListener('change', function() {
                // Add loading indicator
                showLoadingOverlay();
                filterForm.submit();
            });
        });

        // Clear filters button
        const clearButton = document.querySelector('.clear-filters');
        if (clearButton) {
            clearButton.addEventListener('click', function(e) {
                e.preventDefault();
                // Reset all form fields
                filterForm.reset();
                // Submit to reload without filters
                window.location.href = window.location.pathname;
            });
        }
    }
}

// Lazy loading for game images
function initLazyLoading() {
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    img.classList.add('loaded');
                    observer.unobserve(img);
                }
            });
        });

        document.querySelectorAll('img[data-src]').forEach(img => {
            imageObserver.observe(img);
        });
    }
}

// Game card interactions
function initGameCards() {
    const gameCards = document.querySelectorAll('.game-card');
    
    gameCards.forEach(card => {
        // Add hover effect with touch support
        card.addEventListener('mouseenter', function() {
            this.classList.add('hovering');
        });
        
        card.addEventListener('mouseleave', function() {
            this.classList.remove('hovering');
        });
        
        // Touch support for mobile
        card.addEventListener('touchstart', function() {
            this.classList.add('hovering');
        });
        
        card.addEventListener('touchend', function() {
            setTimeout(() => {
                this.classList.remove('hovering');
            }, 500);
        });

        // Quick play buttons
        const playButton = card.querySelector('.quick-play');
        if (playButton) {
            playButton.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                const gameId = this.dataset.gameId;
                quickPlay(gameId);
            });
        }
    });
}

// Language switch handler
function initLanguageSwitch() {
    const langSwitcher = document.querySelector('.language-switcher');
    if (langSwitcher) {
        // Store language preference immediately
        langSwitcher.addEventListener('change', function() {
            const selectedLang = this.value;
            localStorage.setItem('preferredLanguage', selectedLang);
        });
    }
}

// Smooth scroll for anchor links
function initSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Search with debounce
function initSearchDebounce() {
    const searchInput = document.querySelector('input[name="SearchTerm"]');
    if (searchInput) {
        let debounceTimer;
        
        searchInput.addEventListener('input', function() {
            clearTimeout(debounceTimer);
            const searchValue = this.value;
            
            // Show searching indicator
            this.classList.add('searching');
            
            debounceTimer = setTimeout(() => {
                if (searchValue.length >= 3 || searchValue.length === 0) {
                    // Auto-submit search after delay
                    const form = this.closest('form');
                    if (form) {
                        showLoadingOverlay();
                        form.submit();
                    }
                }
                this.classList.remove('searching');
            }, 500);
        });
    }
}

// Quick play function
function quickPlay(gameId) {
    // Show loading
    showLoadingOverlay();
    
    // Create form and submit
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Games/Play';
    
    const idInput = document.createElement('input');
    idInput.type = 'hidden';
    idInput.name = 'id';
    idInput.value = gameId;
    
    const demoInput = document.createElement('input');
    demoInput.type = 'hidden';
    demoInput.name = 'demo';
    demoInput.value = 'true';
    
    form.appendChild(idInput);
    form.appendChild(demoInput);
    document.body.appendChild(form);
    form.submit();
}

// Loading overlay
function showLoadingOverlay() {
    const overlay = document.createElement('div');
    overlay.className = 'loading-overlay';
    overlay.innerHTML = `
        <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    `;
    document.body.appendChild(overlay);
}

// Hide loading overlay
function hideLoadingOverlay() {
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) {
        overlay.remove();
    }
}

// Format currency
function formatCurrency(amount, currency = 'EUR') {
    return new Intl.NumberFormat('nl-NL', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

// Countdown timer for promotions
function initCountdown(endDate, elementId) {
    const countdownElement = document.getElementById(elementId);
    if (!countdownElement) return;
    
    const timer = setInterval(() => {
        const now = new Date().getTime();
        const distance = new Date(endDate).getTime() - now;
        
        if (distance < 0) {
            clearInterval(timer);
            countdownElement.innerHTML = "EXPIRED";
            return;
        }
        
        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);
        
        countdownElement.innerHTML = `${days}d ${hours}h ${minutes}m ${seconds}s`;
    }, 1000);
}

// Copy to clipboard
function copyToClipboard(text) {
    if (navigator.clipboard) {
        navigator.clipboard.writeText(text).then(() => {
            showNotification('Copied to clipboard!', 'success');
        }).catch(err => {
            console.error('Failed to copy: ', err);
        });
    } else {
        // Fallback for older browsers
        const textarea = document.createElement('textarea');
        textarea.value = text;
        document.body.appendChild(textarea);
        textarea.select();
        document.execCommand('copy');
        document.body.removeChild(textarea);
        showNotification('Copied to clipboard!', 'success');
    }
}

// Show notification
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} notification fade-in`;
    notification.innerHTML = message;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// Game statistics tracker
class GameStats {
    constructor() {
        this.stats = JSON.parse(localStorage.getItem('gameStats')) || {};
    }
    
    trackPlay(gameId) {
        if (!this.stats[gameId]) {
            this.stats[gameId] = {
                plays: 0,
                lastPlayed: null
            };
        }
        
        this.stats[gameId].plays++;
        this.stats[gameId].lastPlayed = new Date().toISOString();
        this.save();
    }
    
    getMostPlayed() {
        return Object.entries(this.stats)
            .sort((a, b) => b[1].plays - a[1].plays)
            .slice(0, 5);
    }
    
    save() {
        localStorage.setItem('gameStats', JSON.stringify(this.stats));
    }
}

// Initialize game stats
const gameStats = new GameStats();

// Export functions for external use
window.SafeCasino = {
    quickPlay,
    showLoadingOverlay,
    hideLoadingOverlay,
    formatCurrency,
    copyToClipboard,
    showNotification,
    gameStats
};

// Custom styles for loading overlay
const style = document.createElement('style');
style.textContent = `
    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 9999;
    }
    
    .notification {
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 10000;
        min-width: 250px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    }
    
    .searching::after {
        content: '...';
        animation: dots 1s steps(4, end) infinite;
    }
    
    @keyframes dots {
        0%, 20% {
            content: '.';
        }
        40% {
            content: '..';
        }
        60%, 100% {
            content: '...';
        }
    }
`;
document.head.appendChild(style);