document.querySelectorAll('.filter-select').forEach(el => {
    el.addEventListener('change', () => el.closest('form').submit());
});

document.querySelectorAll('.bar-fill').forEach(bar => {
    const target = bar.style.width;
    bar.style.width = '0';
    requestAnimationFrame(() => {
        bar.style.transition = 'width .6s cubic-bezier(.4,0,.2,1)';
        bar.style.width = target;
    });
});