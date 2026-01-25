// ============================================
// GLOBAL IMAGE ERROR HANDLER
// Automatically handles ALL broken images
// ============================================

document.addEventListener('DOMContentLoaded', function () {
    // Attach error handler to ALL images on page load
    document.querySelectorAll('img').forEach(img => {
        img.addEventListener('error', function () {
            handleImageError(this);
        });
    });

    // Handle dynamically loaded images (AJAX)
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            mutation.addedNodes.forEach(function (node) {
                if (node.tagName === 'IMG') {
                    node.addEventListener('error', function () {
                        handleImageError(this);
                    });
                } else if (node.querySelectorAll) {
                    node.querySelectorAll('img').forEach(img => {
                        img.addEventListener('error', function () {
                            handleImageError(this);
                        });
                    });
                }
            });
        });
    });

    observer.observe(document.body, { childList: true, subtree: true });
});

/**
 * Handles image loading errors with SVG placeholder
 */
function handleImageError(img) {
    // Only run once per image
    if (img.dataset.errorHandled) return;
    img.dataset.errorHandled = 'true';

    // Casino-themed SVG placeholder
    const placeholderSvg =
        'data:image/svg+xml;charset=UTF-8,' +
        encodeURIComponent(`
            <svg width="400" height="300" xmlns="http://www.w3.org/2000/svg">
                <defs>
                    <linearGradient id="bg" x1="0%" y1="0%" x2="100%" y2="100%">
                        <stop offset="0%" style="stop-color:#1a1a2e;stop-opacity:1" />
                        <stop offset="100%" style="stop-color:#16213e;stop-opacity:1" />
                    </linearGradient>
                </defs>
                <rect width="400" height="300" fill="url(#bg)"/>
                <g transform="translate(200, 120)">
                    <rect x="-30" y="-30" width="60" height="60" rx="8" fill="#2d2d44" stroke="#4a4a5a" stroke-width="2"/>
                    <circle cx="-15" cy="-15" r="4" fill="#7c7c99"/>
                    <circle cx="15" cy="-15" r="4" fill="#7c7c99"/>
                    <circle cx="0" cy="0" r="4" fill="#7c7c99"/>
                    <circle cx="-15" cy="15" r="4" fill="#7c7c99"/>
                    <circle cx="15" cy="15" r="4" fill="#7c7c99"/>
                </g>
                <text x="50%" y="220" font-family="Arial, sans-serif" font-size="16" fill="#999" text-anchor="middle">
                    No Image Available
                </text>
            </svg>
        `);

    img.src = placeholderSvg;
    img.alt = 'No image available';
    img.classList.add('placeholder-image');
}