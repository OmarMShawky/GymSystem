

(function () {
    'use strict';

    var STORAGE_KEY = 'pf-theme';
    var root = document.documentElement;

    function current() {
        return root.getAttribute('data-bs-theme') === 'dark' ? 'dark' : 'light';
    }

    function apply(theme) {
        root.setAttribute('data-bs-theme', theme);
        try { localStorage.setItem(STORAGE_KEY, theme); } catch (e) { }

        document.querySelectorAll('[data-theme-toggle]').forEach(function (btn) {
            btn.setAttribute('aria-pressed', theme === 'dark' ? 'true' : 'false');
            btn.setAttribute('title', theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
        });
    }

    document.addEventListener('click', function (e) {
        var btn = e.target.closest('[data-theme-toggle]');
        if (!btn) return;

        e.preventDefault();
        apply(current() === 'dark' ? 'light' : 'dark');
    });

    document.addEventListener('DOMContentLoaded', function () { apply(current()); });
})();
