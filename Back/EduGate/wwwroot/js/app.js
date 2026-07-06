// MySchool LMS - Main JavaScript

// ===== Toast Notifications =====
function showToast(message, type = 'success') {
  let container = document.querySelector('.toast-container');
  if (!container) {
    container = document.createElement('div');
    container.className = 'toast-container';
    document.body.appendChild(container);
  }

  const toast = document.createElement('div');
  toast.className = `toast ${type}`;

  const icons = {
    success: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#10B981" stroke-width="2"><path d="M5 13l4 4L19 7"/></svg>',
    error: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#E84C4C" stroke-width="2"><circle cx="12" cy="12" r="10"/><path d="M15 9l-6 6M9 9l6 6"/></svg>',
    info: '<svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#3B82F6" stroke-width="2"><circle cx="12" cy="12" r="10"/><path d="M12 16v-4M12 8h.01"/></svg>'
  };

  toast.innerHTML = `${icons[type]} <span>${message}</span>`;
  container.appendChild(toast);

  setTimeout(() => {
    toast.style.opacity = '0';
    toast.style.transform = 'translateX(100%)';
    setTimeout(() => toast.remove(), 300);
  }, 3000);
}

// ===== FAQ Accordion =====
function initFaq() {
  document.querySelectorAll('.faq-question').forEach(btn => {
    btn.addEventListener('click', () => {
      const answer = btn.nextElementSibling;
      const arrow = btn.querySelector('svg');
      const isOpen = answer.classList.contains('open');

      // Close all
      document.querySelectorAll('.faq-answer').forEach(a => a.classList.remove('open'));
      document.querySelectorAll('.faq-question svg').forEach(s => s.style.transform = 'rotate(0deg)');

      if (!isOpen) {
        answer.classList.add('open');
        if (arrow) arrow.style.transform = 'rotate(180deg)';
      }
    });
  });
}

// ===== Modal =====
function openModal(id) {
  const modal = document.getElementById(id);
  if (modal) modal.classList.add('active');
}

function closeModal(id) {
  const modal = document.getElementById(id);
  if (modal) modal.classList.remove('active');
}

// Close modal on overlay click
document.addEventListener('click', (e) => {
  if (e.target.classList.contains('modal-overlay')) {
    e.target.classList.remove('active');
  }
});

// ===== Form Validation =====
function validateForm(form) {
  const inputs = form.querySelectorAll('input[required], select[required], textarea[required]');
  let valid = true;

  inputs.forEach(input => {
    if (!input.value.trim()) {
      valid = false;
      input.style.borderColor = 'var(--primary)';
    } else {
      input.style.borderColor = 'var(--border)';
    }
  });

  return valid;
}

// ===== Chart Animation =====
function animateCharts() {
  document.querySelectorAll('.chart-bar').forEach(bar => {
    const height = bar.getAttribute('data-height');
    if (height) {
      setTimeout(() => {
        bar.style.height = height;
      }, 100);
    }
  });
}

// ===== Counter Animation =====
function animateCounters() {
  document.querySelectorAll('[data-counter]').forEach(el => {
    const target = parseInt(el.getAttribute('data-counter'));
    const duration = 1000;
    const step = target / (duration / 16);
    let current = 0;

    const timer = setInterval(() => {
      current += step;
      if (current >= target) {
        current = target;
        clearInterval(timer);
      }
      el.textContent = Math.floor(current);
    }, 16);
  });
}

// ===== Sidebar Toggle (mobile) =====
function toggleSidebar() {
  document.querySelector('.sidebar').classList.toggle('open');
}

// ===== Copy to Clipboard =====
function copyToClipboard(text) {
  navigator.clipboard.writeText(text).then(() => {
    showToast('Copied to clipboard!', 'success');
  }).catch(() => {
    // Fallback
    const textarea = document.createElement('textarea');
    textarea.value = text;
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand('copy');
    document.body.removeChild(textarea);
    showToast('Copied to clipboard!', 'success');
  });
}

// ===== Generate Password =====
function generatePassword(length = 10) {
  const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
  let password = '';
  for (let i = 0; i < length; i++) {
    password += chars.charAt(Math.floor(Math.random() * chars.length));
  }
  return password;
}

// ===== Quiz Builder =====
function addQuestion(containerId) {

    const container = document.getElementById(containerId);

    const qIndex = container.children.length;

    const questionHTML = `
    <div class="card" style="margin-bottom:16px;">

        <div class="form-group">
            <label class="form-label">
                Question ${qIndex + 1}
            </label>

            <input
                type="text"
                class="form-input"
                name="Questions[${qIndex}].Text">
        </div>


        <div class="form-group">

            <label class="form-label">
                Mark
            </label>

            <input
                type="number"
                class="form-input"
                name="Questions[${qIndex}].Mark">

        </div>


        <div class="form-group">

            <label class="form-label">

                Options

            </label>

            ${createChoice(qIndex, 0, 'A')}

            ${createChoice(qIndex, 1, 'B')}

            ${createChoice(qIndex, 2, 'C')}

            ${createChoice(qIndex, 3, 'D')}

        </div>


        <div class="form-group">

            <label class="form-label">

                Correct Answer

            </label>

            <select
                class="form-select"
                onchange="setCorrectAnswer(this,${qIndex})">

                <option value="0">A</option>

                <option value="1">B</option>

                <option value="2">C</option>

                <option value="3">D</option>

            </select>

        </div>

    </div>
    `;

    container.insertAdjacentHTML(
        'beforeend',
        questionHTML
    );

    setCorrectAnswer(
        container.lastElementChild.querySelector("select"),
        qIndex
    );
}

function createChoice(q, c, letter) {

    return `
        <div class="quiz-option">

            <div class="quiz-option-letter">
                ${letter}
            </div>

            <input
                type="text"
                class="quiz-option-input"
                name="Questions[${q}].Choices[${c}].Text">

            <input
                type="hidden"
                name="Questions[${q}].Choices[${c}].IsCorrect"
                value="false">

        </div>
    `;
}

function setCorrectAnswer(select, q) {

    let value = parseInt(select.value);

    for (let i = 0; i < 4; i++) {

        document.querySelector(

            `input[name="Questions[${q}].Choices[${i}].IsCorrect"]`

        ).value = "false";

    }

    document.querySelector(

        `input[name="Questions[${q}].Choices[${value}].IsCorrect"]`

    ).value = "true";

}

// ===== Smooth Scroll =====
function smoothScroll(target) {
  const el = document.querySelector(target);
  if (el) el.scrollIntoView({ behavior: 'smooth' });
}

// ===== Exam Filters =====
function initExamFilters() {
  const filters = document.querySelector('[data-exam-filters]');
  if (!filters) return;

  const tabs = filters.querySelectorAll('[data-filter]');
  const exams = document.querySelectorAll('.exam-card[data-status]');

  tabs.forEach(tab => {
    tab.addEventListener('click', () => {
      const filter = tab.getAttribute('data-filter');

      tabs.forEach(item => item.classList.remove('active'));
      tab.classList.add('active');

      exams.forEach(exam => {
        const status = exam.getAttribute('data-status');
        exam.classList.toggle('hidden', filter !== 'all' && status !== filter);
      });
    });
  });
}

// ===== Initialize on DOM Ready =====
document.addEventListener('DOMContentLoaded', () => {
  initFaq();
  initExamFilters();
  animateCounters();
  animateCharts();
});
