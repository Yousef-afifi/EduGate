/* MVC-ready UI behavior only.
   Rendering of courses, lessons, exams, questions, students, packages,
   teacher data, and dashboard statistics belongs in Razor views. */
(function () {
    function qs(selector, scope) {
        return (scope || document).querySelector(selector);
    }

    function qsa(selector, scope) {
        return Array.prototype.slice.call((scope || document).querySelectorAll(selector));
    }

    window.openModal = function (id) {
        const modal = document.getElementById(id);
        if (modal) modal.classList.add('open');
    };

    window.closeModal = function (id) {
        const modal = document.getElementById(id);
        if (modal) modal.classList.remove('open');
    };

    function setFieldError(input, message) {
        if (!input || !input.parentElement) return;

        const error = input.parentElement.querySelector('.field-error');
        if (!error) return;

        error.textContent = message || '';
        input.classList.toggle('input-error', Boolean(message));
    }

    function validateField(input, rule) {
        if (!input) return null;

        const value = input.value.trim();

        if (rule === 'required' && !value) return 'This field is required.';
        if (rule === 'email' && value && !/^\S+@\S+\.\S+$/.test(value)) return 'Enter a valid email address.';

        if (rule && rule.indexOf('min:') === 0 && value.length < Number(rule.split(':')[1])) {
            return 'Enter at least ' + rule.split(':')[1] + ' characters.';
        }

        if (rule && rule.indexOf('match:') === 0) {
            const other = document.getElementById(rule.split(':')[1]);
            if (other && value !== other.value.trim()) return 'Values do not match.';
        }

        return null;
    }

    function validateRequiredForm(form, buttonSelector, loadingText) {
        if (!form) return;

        form.addEventListener('submit', function (event) {
            let hasError = false;

            qsa('[data-validate]', form).forEach(function (input) {
                const message = input.dataset.validate
                    .split('|')
                    .map(function (rule) {
                        return validateField(input, rule);
                    })
                    .find(Boolean);

                setFieldError(input, message);
                if (message) hasError = true;
            });

            if (hasError) {
                event.preventDefault();
                return;
            }

            const button = buttonSelector ? qs(buttonSelector, form) || document.querySelector(buttonSelector) : null;
            if (button) {
                button.disabled = true;
                button.textContent = loadingText || 'Saving...';
            }
        });
    }

    function initAuthForms() {
        validateRequiredForm(document.getElementById('login-form'), '#login-btn', 'Signing in...');
        validateRequiredForm(document.getElementById('register-form'), '#reg-btn', 'Creating account...');

        const password = document.getElementById('reg-password');
        const bar = document.getElementById('strength-bar');
        const label = document.getElementById('strength-label');

        if (!password || !bar || !label) return;

        password.addEventListener('input', function () {
            const score = Math.min(
                100,
                password.value.length * 12 + (/[A-Z]/.test(password.value) ? 15 : 0) + (/\d/.test(password.value) ? 15 : 0)
            );

            bar.style.width = score + '%';
            bar.style.background = score > 70 ? '#16a34a' : score > 40 ? '#f59e0b' : '#ef4444';
            label.textContent = password.value
                ? score > 70
                    ? 'Strong password'
                    : score > 40
                        ? 'Medium password'
                        : 'Weak password'
                : '';
        });
    }

    function initUserChrome() {
        const name = document.body.dataset.userName || '';
        const role = document.body.dataset.userRole || '';
        const nameEl = document.getElementById('sidebar-user-name');
        const roleEl = document.getElementById('sidebar-user-role');
        const topNameEl = document.getElementById('topbar-user-name');
        const avatar = document.getElementById('sidebar-avatar');

        if (nameEl && !nameEl.textContent.trim()) nameEl.textContent = name || 'User';
        if (roleEl && !roleEl.textContent.trim()) roleEl.textContent = role || '';
        if (topNameEl && !topNameEl.textContent.trim()) topNameEl.textContent = name;
        if (avatar) avatar.textContent = (name || role || 'U').trim().charAt(0).toUpperCase();
    }

    function initSidebar() {
        const sidebar = document.getElementById('sidebar');
        const overlay = document.getElementById('sidebar-overlay');

        function closeSidebar() {
            if (sidebar) sidebar.classList.remove('open');
            if (overlay) overlay.style.display = 'none';
        }

        function toggleSidebar() {
            if (!sidebar || !overlay) return;

            const open = sidebar.classList.toggle('open');
            overlay.style.display = open ? 'block' : 'none';
        }

        qsa('#menu-toggle, #sidebar-peek-toggle').forEach(function (button) {
            button.addEventListener('click', toggleSidebar);
        });

        if (overlay) overlay.addEventListener('click', closeSidebar);

        const logout = document.getElementById('logout-btn');
        if (logout) {
            logout.addEventListener('click', function () {
                window.location.href = '/Auth/Logout';
            });
        }
    }

    function initNav() {
        const current = window.location.pathname.toLowerCase();

        qsa('#sidebar-nav .nav-item').forEach(function (item) {
            const href = (item.getAttribute('href') || '').split('?')[0].toLowerCase();
            if (href && current.indexOf(href) === 0) item.classList.add('active');

            item.addEventListener('click', function () {
                const sidebar = document.getElementById('sidebar');
                const overlay = document.getElementById('sidebar-overlay');

                if (sidebar) sidebar.classList.remove('open');
                if (overlay) overlay.style.display = 'none';
            });
        });
    }

    function initModals() {
        qsa('[data-close-modal]').forEach(function (button) {
            button.addEventListener('click', function () {
                window.closeModal(button.dataset.closeModal);
            });
        });

        qsa('.modal-overlay').forEach(function (overlay) {
            overlay.addEventListener('click', function (event) {
                if (event.target === overlay) overlay.classList.remove('open');
            });
        });
    }

    function initStaticFilters() {
        const courseSearch = document.getElementById('search-courses');
        const courseCards = qsa('#courses-grid .course-card');
        let courseLimit = 6;

        function applyCourseFilters() {
            const query = courseSearch ? courseSearch.value.trim().toLowerCase() : '';
            const active = qs('.filter-pill.active[data-filter]');
            const category = active ? active.dataset.filter : 'all';
            let matches = 0;

            courseCards.forEach(function (card) {
                const matched =
                    (!query || card.textContent.toLowerCase().indexOf(query) >= 0) &&
                    (category === 'all' || card.dataset.category === category);

                matches += matched ? 1 : 0;
                card.style.display = matched && matches <= courseLimit ? '' : 'none';
            });

            const more = document.getElementById('load-more-wrap');
            if (more) more.style.display = matches > courseLimit ? 'block' : 'none';
        }

        qsa('.filter-pill[data-filter]').forEach(function (pill) {
            pill.addEventListener('click', function () {
                qsa('.filter-pill[data-filter]').forEach(function (item) {
                    item.classList.remove('active');
                });

                pill.classList.add('active');
                courseLimit = 6;
                applyCourseFilters();
            });
        });

        if (courseSearch) courseSearch.addEventListener('input', applyCourseFilters);

        const loadMore = document.getElementById('load-more-btn');
        if (loadMore) {
            loadMore.addEventListener('click', function () {
                courseLimit += 6;
                applyCourseFilters();
            });
        }

        if (courseCards.length) applyCourseFilters();

        const examSearch = document.getElementById('search-exams');
        const examCards = qsa('#exams-grid .exam-card');

        function applyExamFilters() {
            const query = examSearch ? examSearch.value.trim().toLowerCase() : '';
            const active = qs('.filter-pill.active[data-status]');
            const status = active ? active.dataset.status : 'all';

            examCards.forEach(function (card) {
                const visible =
                    (!query || card.textContent.toLowerCase().indexOf(query) >= 0) &&
                    (status === 'all' || card.dataset.status === status);

                card.style.display = visible ? '' : 'none';
            });
        }

        qsa('.filter-pill[data-status]').forEach(function (pill) {
            pill.addEventListener('click', function () {
                qsa('.filter-pill[data-status]').forEach(function (item) {
                    item.classList.remove('active');
                });

                pill.classList.add('active');
                applyExamFilters();
            });
        });

        if (examSearch) examSearch.addEventListener('input', applyExamFilters);
        if (examCards.length) applyExamFilters();
    }

    function initExamTaking() {
        const form = document.getElementById('exam-form');
        if (!form) return;

        const submitButton = document.getElementById('submit-exam-btn');
        const confirmButton = document.getElementById('confirm-submit-btn');

        if (submitButton) {
            submitButton.addEventListener('click', function () {
                window.openModal('submit-confirm-modal');
            });
        }

        if (confirmButton) {
            confirmButton.addEventListener('click', function () {
                form.submit();
            });
        }

        const total = qsa('.question-card', form).length;

        function updateProgress() {
            const answered = qsa('.question-card', form).filter(function (card) {
                return Boolean(qs('input[type="radio"]:checked', card));
            }).length;

            const label = document.getElementById('answered-count');
            if (label) label.textContent = answered + '/' + total;
        }

        qsa('input[type="radio"]', form).forEach(function (input) {
            input.addEventListener('change', updateProgress);
        });

        updateProgress();
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.body.classList.add('page-fade');

        initAuthForms();
        initUserChrome();
        initSidebar();
        initNav();
        initModals();
        initStaticFilters();
        initExamTaking();

        validateRequiredForm(document.getElementById('create-course-form'), '#create-course-btn', 'Creating...');
        validateRequiredForm(document.getElementById('add-lesson-form'), '#add-lesson-btn', 'Adding...');
        validateRequiredForm(document.getElementById('create-exam-form'), '#create-exam-btn', 'Creating...');
        validateRequiredForm(document.getElementById('student-basic-form'), '#student-basic-save', 'Saving...');
        validateRequiredForm(document.getElementById('student-password-form'), '#student-password-save', 'Saving...');
        validateRequiredForm(document.getElementById('teacher-settings-form'), '#teacher-settings-save', 'Saving...');
    });
})();
