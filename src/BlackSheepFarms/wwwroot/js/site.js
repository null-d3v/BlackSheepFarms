document.addEventListener('DOMContentLoaded', function() {
    const splash = document.getElementById('splash');
    
    window.addEventListener('scroll', function() {
        const scrollPosition = window.scrollY;
        const windowHeight = window.innerHeight;
        
        // Calculate opacity based on scroll position
        const opacity = 1 - (scrollPosition / windowHeight);
        
        // Apply parallax effect
        if (opacity >= 0) {
            splash.style.opacity = opacity;
            splash.style.transform = `translateY(${scrollPosition * 0.5}px)`;
        }
        
        // Hide splash completely when scrolled past
        if (opacity < 0) {
            splash.style.visibility = 'hidden';
        } else {
            splash.style.visibility = 'visible';
        }
    });
});
