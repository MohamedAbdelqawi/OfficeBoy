var upToBtn = document.getElementById('upToBtn');

window.addEventListener('scroll', function () {
    if (window.scrollY > 100) {
        upToBtn.style.display = 'block';
    } else {
        upToBtn.style.display = 'none';
    }
});

upToBtn.addEventListener('click', function () {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});
