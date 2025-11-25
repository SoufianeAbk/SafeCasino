/**
 * SafeCasino Roulette Game Engine
 * Advanced interactive roulette with realistic animations
 */

class RouletteGame {
    constructor() {
        // Roulette numbers in order as they appear on the wheel
        this.NUMBERS = [0, 26, 3, 35, 12, 28, 7, 29, 18, 22, 9, 31, 14, 20, 1, 33, 16, 24, 5, 10, 23, 8, 30, 11, 36, 13, 27, 6, 34, 17, 25, 2, 21, 4, 19, 15, 32];

        // Number colors
        this.RED_NUMBERS = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];
        this.BLACK_NUMBERS = [2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35];

        // Game state
        this.state = {
            isSpinning: false,
            balance: 1234.56,
            currentBet: 1.00,
            betType: 'number',
            selectedNumber: 0,
            lastWinningNumber: null,
            totalWinnings: 0,
            totalLosses: 0,
            spinCount: 0
        };

        // DOM elements
        this.elements = {
            wheel: null,
            ball: null,
            resultNumber: null,
            playBtn: null,
            betAmount: null,
            betType: null,
            selectedNumber: null,
            balance: null,
            balanceDisplay: null,
            spinStatus: null,
            spinRotation: null,
            gameResultModal: null,
            resultIcon: null,
            resultTitle: null,
            resultAmount: null,
            resultDetails: null
        };

        this.init();
    }

    /**
     * Initialize the game
     */
    init() {
        this.cacheElements();
        this.setupEventListeners();
        this.generateWheel();
        this.updateDisplay();
    }

    /**
     * Cache DOM elements
     */
    cacheElements() {
        this.elements.wheel = document.getElementById('rouletteWheel');
        this.elements.ball = document.getElementById('rouletteBall');
        this.elements.resultNumber = document.getElementById('resultNumber');
        this.elements.playBtn = document.getElementById('playBtn');
        this.elements.betAmount = document.getElementById('betAmount');
        this.elements.betType = document.getElementById('betType');
        this.elements.selectedNumber = document.getElementById('selectedNumber');
        this.elements.balance = document.getElementById('balance');
        this.elements.balanceDisplay = document.getElementById('balanceDisplay');
        this.elements.spinStatus = document.getElementById('spinStatus');
        this.elements.spinRotation = document.getElementById('spinRotation');
        this.elements.gameResultModal = document.getElementById('gameResultModal');
        this.elements.resultIcon = document.getElementById('resultIcon');
        this.elements.resultTitle = document.getElementById('resultTitle');
        this.elements.resultAmount = document.getElementById('resultAmount');
        this.elements.resultDetails = document.getElementById('resultDetails');
    }

    /**
     * Setup event listeners
     */
    setupEventListeners() {
        // Bet amount input
        if (this.elements.betAmount) {
            this.elements.betAmount.addEventListener('change', () => {
                this.state.currentBet = parseFloat(this.elements.betAmount.value);
                this.updateDisplay();
            });
        }

        // Bet type selector
        if (this.elements.betType) {
            this.elements.betType.addEventListener('change', () => {
                this.state.betType = this.elements.betType.value;
                if (this.state.betType === 'number') {
                    this.elements.selectedNumber.style.display = 'block';
                } else {
                    this.elements.selectedNumber.style.display = 'none';
                }
            });
        }

        // Selected number input
        if (this.elements.selectedNumber) {
            this.elements.selectedNumber.addEventListener('change', () => {
                this.state.selectedNumber = parseInt(this.elements.selectedNumber.value);
            });
        }

        // Close result modal on Escape
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.elements.gameResultModal) {
                this.closeResult();
            }
        });
    }

    /**
     * Generate the roulette wheel with SVG
     */
    generateWheel() {
        if (!this.elements.wheel) return;

        const wheelSegments = this.elements.wheel.querySelector('#wheelSegments');
        if (!wheelSegments) return;

        wheelSegments.innerHTML = '';

        const segmentAngle = 360 / this.NUMBERS.length;

        this.NUMBERS.forEach((number, index) => {
            const angle = (index * segmentAngle) - 90;
            const nextAngle = ((index + 1) * segmentAngle) - 90;

            // Determine segment color
            let color;
            if (number === 0) {
                color = '#1a7f3e'; // Green for 0
            } else if (this.RED_NUMBERS.includes(number)) {
                color = '#e63946'; // Red
            } else {
                color = '#1a1a1a'; // Black
            }

            // Create path for segment
            const radius = 200;
            const x1 = 250 + radius * Math.cos((angle * Math.PI) / 180);
            const y1 = 250 + radius * Math.sin((angle * Math.PI) / 180);
            const x2 = 250 + radius * Math.cos((nextAngle * Math.PI) / 180);
            const y2 = 250 + radius * Math.sin((nextAngle * Math.PI) / 180);

            const path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            path.setAttribute('d', `M 250 250 L ${x1} ${y1} A 200 200 0 0 1 ${x2} ${y2} Z`);
            path.setAttribute('fill', color);
            path.setAttribute('stroke', '#f59e0b');
            path.setAttribute('stroke-width', '2');
            path.setAttribute('class', 'wheel-segment');
            wheelSegments.appendChild(path);

            // Create number text
            const midAngle = (angle + nextAngle) / 2;
            const textRadius = 150;
            const textX = 250 + textRadius * Math.cos((midAngle * Math.PI) / 180);
            const textY = 250 + textRadius * Math.sin((midAngle * Math.PI) / 180);

            const text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
            text.setAttribute('x', textX);
            text.setAttribute('y', textY);
            text.setAttribute('text-anchor', 'middle');
            text.setAttribute('dominant-baseline', 'middle');
            text.setAttribute('font-size', '16');
            text.setAttribute('font-weight', 'bold');
            text.setAttribute('fill', '#ffffff');
            text.setAttribute('class', 'wheel-number');
            text.textContent = number;
            wheelSegments.appendChild(text);
        });
    }

    /**
     * Set bet amount
     */
    setBet(amount) {
        this.state.currentBet = amount;
        if (this.elements.betAmount) {
            this.elements.betAmount.value = amount.toFixed(2);
        }
        this.updateDisplay();
    }

    /**
     * Start the roulette spin
     */
    startSpin() {
        if (this.state.isSpinning) return;

        const betAmount = parseFloat(this.elements.betAmount.value);

        // Validate bet
        if (isNaN(betAmount) || betAmount <= 0) {
            this.showMessage('Voer een geldig inzetbedrag in!', 'danger');
            return;
        }

        if (betAmount > this.state.balance) {
            this.showMessage('Onvoldoende saldo!', 'danger');
            return;
        }

        if (this.state.betType === 'number' && (this.state.selectedNumber < 0 || this.state.selectedNumber > 36)) {
            this.showMessage('Selecteer een geldig getal (0-36)!', 'danger');
            return;
        }

        // Start spin
        this.state.isSpinning = true;
        this.state.currentBet = betAmount;
        this.state.balance -= betAmount;
        this.state.spinCount++;

        // Update UI
        this.elements.playBtn.disabled = true;
        this.elements.playBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Draait...';

        // Random spin parameters
        const spinDuration = 2000 + Math.random() * 3000;
        const totalRotation = 360 * (3 + Math.random() * 5);

        this.performSpin(spinDuration, totalRotation);
    }

    /**
     * Perform the spin animation
     */
    performSpin(duration, totalRotation) {
        const startTime = Date.now();
        const statusEl = this.elements.spinStatus;
        const rotationEl = this.elements.spinRotation;

        if (this.elements.wheel) {
            this.elements.wheel.classList.add('spinning');
        }

        if (statusEl) statusEl.textContent = 'Draaiing...';

        const animate = () => {
            const elapsed = Date.now() - startTime;
            const progress = elapsed / duration;

            if (progress < 1) {
                // Easing function for smooth deceleration
                const easeOutCubic = 1 - Math.pow(1 - progress, 3);
                const currentRotation = totalRotation * easeOutCubic;

                if (this.elements.wheel) {
                    this.elements.wheel.style.transform = `rotate(${currentRotation}deg)`;
                }

                if (rotationEl) {
                    rotationEl.textContent = `${Math.round(currentRotation)}°`;
                }

                requestAnimationFrame(animate);
            } else {
                // Spin complete
                if (this.elements.wheel) {
                    this.elements.wheel.style.transform = `rotate(${totalRotation}deg)`;
                    this.elements.wheel.classList.remove('spinning');
                }

                if (rotationEl) {
                    rotationEl.textContent = `${Math.round(totalRotation)}°`;
                }

                // Determine winning number
                this.onSpinComplete(totalRotation % 360);
            }
        };

        animate();
    }

    /**
     * Handle spin completion
     */
    onSpinComplete(finalRotation) {
        // Calculate winning number
        const segmentAngle = 360 / this.NUMBERS.length;
        const normalizedRotation = (360 - finalRotation + 90) % 360;
        const winningIndex = Math.floor(normalizedRotation / segmentAngle) % this.NUMBERS.length;
        const winningNumber = this.NUMBERS[winningIndex];

        this.state.lastWinningNumber = winningNumber;

        // Update result display
        if (this.elements.resultNumber) {
            this.elements.resultNumber.textContent = winningNumber.toString().padStart(2, '0');
        }

        // Check if player won
        const won = this.checkWin(winningNumber);

        // Calculate winnings
        const winnings = won ? this.calculateWinnings() : 0;

        // Update balance
        if (won) {
            this.state.balance += winnings;
            this.state.totalWinnings += winnings;
        } else {
            this.state.totalLosses += this.state.currentBet;
        }

        // Update display
        this.updateDisplay();

        // Show result
        this.showResult(winningNumber, won, winnings);

        // Reset button
        this.elements.playBtn.disabled = false;
        this.elements.playBtn.innerHTML = '<i class="fas fa-play"></i> Start Spin';

        this.state.isSpinning = false;
    }

    /**
     * Check if the player won
     */
    checkWin(winningNumber) {
        const betType = this.state.betType;

        switch (betType) {
            case 'number':
                return winningNumber === this.state.selectedNumber;
            case 'red':
                return this.RED_NUMBERS.includes(winningNumber);
            case 'black':
                return this.BLACK_NUMBERS.includes(winningNumber);
            case 'even':
                return winningNumber !== 0 && winningNumber % 2 === 0;
            case 'odd':
                return winningNumber !== 0 && winningNumber % 2 === 1;
            case 'high':
                return winningNumber >= 19 && winningNumber <= 36;
            case 'low':
                return winningNumber >= 1 && winningNumber <= 18;
            default:
                return false;
        }
    }

    /**
     * Calculate winnings based on bet type
     */
    calculateWinnings() {
        const multipliers = {
            'number': 36,
            'red': 2,
            'black': 2,
            'even': 2,
            'odd': 2,
            'high': 2,
            'low': 2
        };

        const multiplier = multipliers[this.state.betType] || 0;
        return this.state.currentBet * multiplier;
    }

    /**
     * Show result modal
     */
    showResult(winningNumber, won, winnings) {
        if (!this.elements.gameResultModal) return;

        if (won) {
            this.elements.resultIcon.className = 'fas fa-star';
            this.elements.resultIcon.style.color = 'var(--success-color)';
            this.elements.resultTitle.textContent = 'GEWONNEN!';
            this.elements.resultTitle.style.color = 'var(--success-color)';
            this.elements.resultAmount.textContent = winnings.toFixed(2);
            this.elements.resultAmount.style.color = 'var(--success-color)';
        } else {
            this.elements.resultIcon.className = 'fas fa-times-circle';
            this.elements.resultIcon.style.color = 'var(--danger-color)';
            this.elements.resultTitle.textContent = 'VERLOREN';
            this.elements.resultTitle.style.color = 'var(--danger-color)';
            this.elements.resultAmount.textContent = '0.00';
            this.elements.resultAmount.style.color = 'var(--danger-color)';
        }

        // Build details
        let details = `<strong>Nummer:</strong> ${winningNumber.toString().padStart(2, '0')}<br>`;
        details += `<strong>Je inzet:</strong> €${this.state.currentBet.toFixed(2)}<br>`;

        if (won) {
            const multiplier = this.calculateWinnings() / this.state.currentBet;
            details += `<strong>Vermenigvuldiger:</strong> ${multiplier}x`;
        } else {
            details += `<strong>Beter geluk volgende keer!</strong>`;
        }

        this.elements.resultDetails.innerHTML = details;

        // Show modal
        this.elements.gameResultModal.classList.add('show');
    }

    /**
     * Close result modal
     */
    closeResult() {
        if (this.elements.gameResultModal) {
            this.elements.gameResultModal.classList.remove('show');
        }
    }

    /**
     * Update display values
     */
    updateDisplay() {
        if (this.elements.balance) {
            const formatted = this.state.balance.toLocaleString('nl-NL', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
            this.elements.balance.textContent = formatted;
        }

        if (this.elements.balanceDisplay) {
            const formatted = this.state.balance.toLocaleString('nl-NL', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
            this.elements.balanceDisplay.textContent = formatted;
        }
    }

    /**
     * Show temporary message
     */
    showMessage(message, type = 'info') {
        const alert = document.createElement('div');
        alert.className = `alert alert-${type} position-fixed`;
        alert.style.top = '20px';
        alert.style.right = '20px';
        alert.style.zIndex = '9999';
        alert.style.minWidth = '300px';
        alert.innerHTML = `
            <i class="fas fa-${type === 'danger' ? 'exclamation-circle' : 'info-circle'}"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        document.body.appendChild(alert);

        setTimeout(() => {
            alert.remove();
        }, 3000);
    }

    /**
     * Get game statistics
     */
    getStats() {
        return {
            balance: this.state.balance,
            totalSpins: this.state.spinCount,
            totalWinnings: this.state.totalWinnings,
            totalLosses: this.state.totalLosses,
            winRate: this.state.spinCount > 0
                ? ((this.state.totalWinnings / (this.state.totalWinnings + this.state.totalLosses)) * 100).toFixed(1)
                : 0
        };
    }
}

// Global game instance
let rouletteGame;

// Initialize when document is ready
document.addEventListener('DOMContentLoaded', function () {
    rouletteGame = new RouletteGame();

    // Make functions available globally for HTML onclick handlers
    window.setBet = (amount) => rouletteGame.setBet(amount);
    window.startRoulette = () => rouletteGame.startSpin();
    window.closeResult = () => rouletteGame.closeResult();
});

// Export for external use
window.RouletteGame = RouletteGame;