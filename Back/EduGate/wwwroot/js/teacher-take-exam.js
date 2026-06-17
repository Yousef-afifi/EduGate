/* Page script: pages/teacher/take-exam.html */
/* ============================================================
   EDU PLATFORM - page script utilities
   Shared utilities, auth helpers, backend data placeholders, UI helpers
   ============================================================ */

/* ---------- Config ---------- */
const MVC_ROUTES = {
  login: '/Auth/Login',
  logout: '/Auth/Logout',
  teacherDashboard: '/Teacher/Dashboard',
  studentDashboard: '/Student/Dashboard',
};
const ICONS = {
  logo: '&#127891;',
  logout: '&#128682;',
  dashboard: '&#127968;',
  courses: '&#128218;',
  exams: '&#128221;',
  students: '&#128101;',
  analytics: '&#128202;',
  achievements: '&#127942;',
  schedule: '&#128197;',
  settings: '&#9881;',
  search: '&#128269;',
  notification: '&#128276;',
  teacher: '&#128105;&#8205;&#127979;',
  student: '&#127891;',
  video: '&#127909;',
  article: '&#128196;',
  quiz: '&#10067;',
  lock: '&#128274;',
  check: '&#10003;',
  fire: '&#128293;',
  note: '&#128204;',
  party: '&#127881;',
  wave: '&#128075;',
  empty: '&#128300;',
};

/* ---------- Backend data placeholders ---------- */
function createDefaultDemoDb() {
  return {
    lastCourseId: 0,
    lastExamId: 0,
    lastStudentAccountId: 0,

    // Backend required: authenticated users with { id, name, email, role, packageId?, phone?, subject? }.
    users: [],

    // Backend required: generated student accounts with { id, teacherId, name, email, username, courseId, course, progress, status }.
    // Passwords must be created/validated by the backend and should not be seeded in frontend files.
    studentAccounts: [],

    // Backend required: course list with { id, title, category, emoji?, lessonsCount, duration, instructorName, instructorId, level, progress, isNew, studentsCount, description }.
    courses: [],

    // Backend required: lessons grouped by course id. Example shape: { [courseId]: [{ id, title, type, duration, completed, locked, active? }] }.
    lessonsByCourse: {},

    // Backend required: lesson details keyed by courseId-lessonId with { title, type, duration, videoUrl, description, notes }.
    lessonContent: {},

    // Backend required: exam summaries with { id, title, courseId, courseName, duration, questionsCount, dueDate, status, score, avgScore }.
    exams: [],

    // Backend required: exam details keyed by exam id with { id, title, courseName, duration, instructions, questions: [{ id, text, options, correct }] }.
    examQuestions: {},
  };
}

function getDemoDb() {
  const pageData = window.__EDU_PAGE_DATA__ || {};
  return {
    ...createDefaultDemoDb(),
    ...pageData,
  };
}

function saveDemoDb() {
  // MVC required: persist changes in Controllers/Services, not in browser storage.
}function clone(data) {
  return JSON.parse(JSON.stringify(data));
}

function buildFallbackExamQuestions(examSummary) {
  return {
    id: examSummary.id,
    title: examSummary.title || '',
    courseName: examSummary.courseName || '',
    duration: Number(examSummary.duration) || 0,
    instructions: examSummary.instructions || '',
    // Backend required: return the real exam questions and options for this exam id.
    questions: [],
  };
}

/* ---------- Auth helpers ---------- */
function inferPageRole() {
  const path = window.location.pathname.toLowerCase();
  if (path.includes('/teacher/')) return 'teacher';
  if (path.includes('/student/')) return 'student';
  return '';
}

function getUser() {
  return window.__EDU_CURRENT_USER__ || { id: null, name: '', email: '', role: inferPageRole() };
}

function getToken() {
  return null;
}

function saveSession() {
  // MVC required: AuthController owns cookie/session creation.
}

function logout() {
  window.location.href = MVC_ROUTES.logout;
}

function requireAuth() {
  // MVC required: protect pages with [Authorize] or controller/session checks.
  return true;
}

function redirectIfLoggedIn() {
  // MVC required: redirect authenticated users from the controller if needed.
  return false;
}

function redirectToDashboard() {
  const user = getUser();
  window.location.href = user?.role === 'teacher' ? MVC_ROUTES.teacherDashboard : MVC_ROUTES.studentDashboard;
}

function submitMvcForm(action, fields = {}) {
  const form = document.createElement('form');
  form.method = 'post';
  form.action = action;
  Object.entries(fields).forEach(([name, value]) => {
    if (Array.isArray(value)) {
      value.forEach((entry) => appendHiddenField(form, name, entry));
      return;
    }
    appendHiddenField(form, name, value);
  });
  document.body.appendChild(form);
  form.submit();
}

function appendHiddenField(form, name, value) {
  const input = document.createElement('input');
  input.type = 'hidden';
  input.name = name;
  input.value = value == null ? '' : String(value);
  form.appendChild(input);
}

/* ---------- MVC page data adapter ---------- */
async function apiRequest(path, method = 'GET', body = null) {
  return handleMvcPageDataRequest(path, method, body);
}
const api = {
  get: (path) => apiRequest(path, 'GET'),
};

function buildStudentStats(db) {
  const completedCourses = db.courses.filter((course) => (course.progress || 0) >= 100).length;
  const inProgressCourses = db.courses.filter((course) => (course.progress || 0) > 0 && (course.progress || 0) < 100).length;
  const completedExams = db.exams.filter((exam) => exam.status === 'completed' && exam.score != null);
  const avgScore = completedExams.length
    ? Math.round(completedExams.reduce((sum, exam) => sum + exam.score, 0) / completedExams.length)
    : 0;

  return {
    completedCourses,
    inProgressCourses,
    totalExams: db.exams.length,
    avgScore,
  };
}

function buildTeacherStats(db, user) {
  const teacherCourses = db.courses.filter((course) => course.instructorId === user.id || user.role === 'teacher');
  const teacherStudents = getTeacherStudentAccounts(db, user);
  const totalLessons = teacherCourses.reduce((sum, course) => sum + (db.lessonsByCourse[course.id] || []).length, 0);

  return {
    totalStudents: teacherStudents.length,
    activeCourses: teacherCourses.length,
    totalLessons,
    totalExams: db.exams.length,
  };
}

function getCourseById(db, courseId) {
  return db.courses.find((course) => String(course.id) === String(courseId));
}

function getLessons(db, courseId) {
  return clone(db.lessonsByCourse[courseId] || []);
}

function getTeacherStudentAccounts(db, user) {
  return clone((db.studentAccounts || []).filter((account) => account.teacherId === user.id));
}

function handleMvcPageDataRequest(path, method, body) {
  const db = getDemoDb();
  const user = getUser();

  if (!user) throw new Error('Unauthorized');

  if (path === '/teacher/stats' && method === 'GET') return buildTeacherStats(db, user);
  if (path === '/student/stats' && method === 'GET') return buildStudentStats(db);
  if ((path === '/teacher/courses' || path === '/student/courses') && method === 'GET') {
    return clone(db.courses);
  }
  if (path.startsWith('/teacher/courses?') && method === 'GET') return clone(db.courses.slice(0, 6));
  if (path.startsWith('/student/courses?') && method === 'GET') return clone(db.courses.slice(0, 4));

  if (path === '/courses' && method === 'POST') {
    const course = {
      id: ++db.lastCourseId,
      title: body.title,
      category: body.category || 'General',
      emoji: ICONS.courses,
      lessonsCount: 0,
      duration: body.duration || 'Self paced',
      instructorName: user.name,
      instructorId: user.id,
      level: body.level || 'All levels',
      progress: 0,
      isNew: true,
      studentsCount: 0,
      description: body.description || 'New course description will appear here.',
    };
    db.courses.unshift(course);
    db.lessonsByCourse[course.id] = [];
    saveDemoDb(db);
    return clone(course);
  }

  const courseMatch = path.match(/^\/courses\/(\d+)$/);
  if (courseMatch && method === 'GET') {
    const course = getCourseById(db, courseMatch[1]);
    if (!course) throw new Error('Course not found');
    return clone(course);
  }

  const lessonsMatch = path.match(/^\/courses\/(\d+)\/lessons$/);
  if (lessonsMatch && method === 'GET') {
    return getLessons(db, lessonsMatch[1]);
  }
  if (lessonsMatch && method === 'POST') {
    const courseId = lessonsMatch[1];
    const lessons = db.lessonsByCourse[courseId] || [];
    const lesson = {
      id: lessons.length ? Math.max(...lessons.map((item) => item.id)) + 1 : 1,
      title: body.title,
      type: body.type || 'video',
      duration: body.duration || '',
      completed: false,
      locked: false,
    };
    lessons.push(lesson);
    db.lessonsByCourse[courseId] = lessons;
    const course = getCourseById(db, courseId);
    if (course) course.lessonsCount = lessons.length;
    if (body.url || body.description) {
      db.lessonContent[`${courseId}-${lesson.id}`] = {
        title: body.title,
        type: body.type || 'video',
        duration: body.duration || '',
        videoUrl: body.url || '',
        description: body.description || '',
        notes: body.description || '',
      };
    }
    saveDemoDb(db);
    return clone(lesson);
  }

  const lessonDetailsMatch = path.match(/^\/courses\/(\d+)\/lessons\/(\d+)$/);
  if (lessonDetailsMatch && method === 'GET') {
    const [, courseId, lessonId] = lessonDetailsMatch;
    const lesson = (db.lessonsByCourse[courseId] || []).find((item) => String(item.id) === lessonId);
    if (!lesson) throw new Error('Lesson not found');
    return {
      ...clone(lesson),
      ...(db.lessonContent[`${courseId}-${lessonId}`] || {}),
    };
  }

  const lessonCompleteMatch = path.match(/^\/courses\/(\d+)\/lessons\/(\d+)\/complete$/);
  if (lessonCompleteMatch && method === 'POST') {
    const [, courseId, lessonId] = lessonCompleteMatch;
    const lessons = db.lessonsByCourse[courseId] || [];
    lessons.forEach((lesson) => {
      if (String(lesson.id) === lessonId) lesson.completed = true;
      if (lesson.locked && lesson.id === Number(lessonId) + 1) lesson.locked = false;
    });
    const course = getCourseById(db, courseId);
    if (course) {
      const completed = lessons.filter((lesson) => lesson.completed).length;
      course.progress = Math.round((completed / Math.max(lessons.length, 1)) * 100);
    }
    saveDemoDb(db);
    return { success: true };
  }

  if (path.startsWith('/teacher/students') && method === 'GET') {
    const limitMatch = path.match(/limit=(\d+)/);
    const limit = limitMatch ? Number(limitMatch[1]) : null;
    const accounts = getTeacherStudentAccounts(db, user);
    return clone(limit ? accounts.slice(0, limit) : accounts);
  }

  if (path === '/teacher/students/generate' && method === 'POST') {
    // Backend required: teacher profile should include packageSeats/currentPackage.seats for account capacity checks.
    const count = Number(body.count || 0);
    const prefix = (body.prefix || 'Student').trim();
    const fixedPassword = (body.fixedPassword || '').trim();
    const selectedCourseIds = Array.isArray(body.courseIds) ? body.courseIds.map(Number).filter(Boolean) : [];
    if (!count || count < 1) throw new Error('Choose a valid number of accounts to generate.');
    if (!body.assignAllCourses && !selectedCourseIds.length) {
      throw new Error('Choose at least one course for these student accounts.');
    }
    if (body.passwordMode === 'fixed' && fixedPassword.length < 6) {
      throw new Error('Fixed password must be at least 6 characters.');
    }

    const teacherAccounts = getTeacherStudentAccounts(db, user);
    const packageSeats = Number(user.packageSeats || user.currentPackage?.seats || 0);
    if (teacherAccounts.length + count > packageSeats) {
      throw new Error('This batch exceeds the remaining seats in your current package.');
    }

    const selectedCourses = body.assignAllCourses
      ? db.courses.filter((course) => course.instructorId === user.id || user.role === 'teacher')
      : selectedCourseIds.map((courseId) => getCourseById(db, courseId)).filter(Boolean);
    if (!selectedCourses.length) throw new Error('Selected courses were not found.');

    const slugify = (value) => value.toLowerCase().replace(/[^a-z0-9]+/g, '.').replace(/(^\.|\.$)/g, '');
    const generated = [];
    for (let index = 0; index < count; index += 1) {
      const sequence = teacherAccounts.length + index + 1;
      const name = `${prefix} ${sequence}`;
      const username = `${slugify(prefix) || 'student'}.${String(sequence).padStart(3, '0')}`;
      // Backend required: return the generated student email for this account.
      const email = '';
      const password = body.passwordMode === 'fixed'
        ? fixedPassword
        // Backend required: return a generated password when passwordMode is not fixed.
        : '';

      const account = {
        id: ++db.lastStudentAccountId,
        teacherId: user.id,
        name,
        email,
        username,
        password,
        courseId: selectedCourses[0].id,
        courseIds: selectedCourses.map((course) => course.id),
        courses: selectedCourses.map((course) => course.title),
        course: selectedCourses.map((course) => course.title).join(', '),
        progress: 0,
        status: 'inactive',
      };
      db.studentAccounts.push(account);
      db.users.push({
        id: Date.now() + index,
        name,
        email,
        role: 'student',
        password,
      });
      generated.push(account);
    }

    selectedCourses.forEach((course) => {
      const courseRef = db.courses.find((entry) => entry.id === course.id);
      if (courseRef) {
        courseRef.studentsCount = (courseRef.studentsCount || 0) + generated.length;
      }
    });

    saveDemoDb(db);
    return {
      success: true,
      generated: clone(generated),
      totalStudents: db.studentAccounts.filter((account) => account.teacherId === user.id).length,
      remainingSeats: packageSeats - db.studentAccounts.filter((account) => account.teacherId === user.id).length,
    };
  }

  if (path === '/teacher/exams' && method === 'GET') return clone(db.exams);
  if (path === '/student/exams' && method === 'GET') return clone(db.exams.filter((exam) => exam.status !== 'draft'));
  if (path === '/student/exams/upcoming' && method === 'GET') {
    return clone(db.exams.filter((exam) => exam.status === 'pending'));
  }

  if (path === '/exams' && method === 'POST') {
    const course = getCourseById(db, body.courseId);
    const exam = {
      id: ++db.lastExamId,
      title: body.title,
      courseId: Number(body.courseId),
      courseName: course ? course.title : '',
      duration: body.duration || 0,
      questionsCount: body.questionsCount || 0,
      dueDate: body.dueDate || '',
      status: 'draft',
      score: null,
      avgScore: null,
      instructions: body.instructions || '',
    };
    db.exams.unshift(exam);
    db.examQuestions[exam.id] = {
      id: exam.id,
      title: exam.title,
      courseName: exam.courseName,
      duration: exam.duration,
      instructions: exam.instructions || '',
      // Backend required: persist and return real questions for the created exam.
      questions: [],
    };
    saveDemoDb(db);
    return clone(exam);
  }

  const examMatch = path.match(/^\/exams\/(\d+)$/);
  if (examMatch && method === 'GET') {
    const examId = examMatch[1];
    const examSummary = db.exams.find((exam) => String(exam.id) === examId);
    const examDetails = db.examQuestions[examId];
    if (examDetails?.questions?.length) {
      return clone({
        ...examDetails,
        ...(examSummary || {}),
        questions: examDetails.questions,
      });
    }

    if (!examSummary) throw new Error('Exam not found');

    const fallbackExam = buildFallbackExamQuestions(examSummary);
    db.examQuestions[examId] = fallbackExam;
    saveDemoDb(db);
    return clone(fallbackExam);
  }

  const submitMatch = path.match(/^\/exams\/(\d+)\/submit$/);
  if (submitMatch && method === 'POST') {
    const examId = submitMatch[1];
    const exam = db.examQuestions[examId];
    if (!exam) throw new Error('Exam not found');
    let correct = 0;
    const answers = exam.questions.map((question, index) => {
      const selectedOption = body.answers[index]?.selectedOption ?? null;
      const isCorrect = selectedOption === question.correct;
      if (isCorrect) correct += 1;
      return {
        questionId: question.id,
        correct: isCorrect,
        correctOption: question.correct,
        selectedOption,
      };
    });
    const score = Math.round((correct / exam.questions.length) * 100);
    const examSummary = db.exams.find((item) => String(item.id) === examId);
    const resultSummary = {
      score,
      correct,
      total: exam.questions.length,
      passed: score >= 70,
      answers,
      submittedAt: new Date().toISOString(),
    };
    if (examSummary) {
      examSummary.status = 'completed';
      examSummary.score = score;
      examSummary.lastResult = resultSummary;
    }
    saveDemoDb(db);
    return resultSummary;
  }

  throw new Error('MVC page data is missing for this view.');
}

/* ---------- UI helpers ---------- */
(function initToasts() {
  if (!document.body) return;
  const container = document.createElement('div');
  container.className = 'toast-container';
  document.body.appendChild(container);
  window._toastContainer = container;
})();

function showToast(message, type = 'info', duration = 3500) {
  const toast = document.createElement('div');
  toast.className = `toast ${type}`;
  const icons = {
    info: '&#128172;',
    success: '&#9989;',
    error: '&#10060;',
  };
  toast.innerHTML = `<span>${icons[type] || icons.info}</span><span>${message}</span>`;
  window._toastContainer.appendChild(toast);
  setTimeout(() => {
    toast.style.animation = 'toastOut 0.3s ease forwards';
    setTimeout(() => toast.remove(), 300);
  }, duration);
}

function populateSidebarUser() {
  const user = getUser();
  if (!user) return;

  const nameEl = document.getElementById('sidebar-user-name');
  const roleEl = document.getElementById('sidebar-user-role');
  const avatarEl = document.getElementById('sidebar-avatar');
  const topNameEl = document.getElementById('topbar-user-name');

  const initials = (user.name || user.email || 'U')
    .split(' ')
    .map((word) => word[0])
    .join('')
    .toUpperCase()
    .slice(0, 2);

  if (nameEl) nameEl.textContent = user.name || user.email;
  if (roleEl) roleEl.textContent = user.role || 'Student';
  if (avatarEl) avatarEl.textContent = initials;
  if (topNameEl) topNameEl.textContent = user.name || user.email;
}

function setBreadcrumbTrail(targetId, items) {
  const el = document.getElementById(targetId);
  if (!el || !Array.isArray(items)) return;
  el.innerHTML = items.map((item, index) => {
    const isLast = index === items.length - 1;
    const content = item.href && !isLast
      ? `<a href="${item.href}">${item.label}</a>`
      : `<span>${item.label}</span>`;
    return `${index > 0 ? ' <span>&rsaquo;</span> ' : ''}${content}`;
  }).join('');
}

function setActiveNav() {
  const page = window.location.pathname.split('/').pop();
  document.querySelectorAll('.nav-item').forEach((item) => {
    if (item.dataset.page === page) item.classList.add('active');
  });
}

function initMobileSidebar() {
  const toggle = document.getElementById('menu-toggle');
  const sidebar = document.getElementById('sidebar');
  const overlay = document.getElementById('sidebar-overlay');
  if (!toggle || !sidebar) return;

  toggle.addEventListener('click', () => {
    sidebar.classList.toggle('open');
    if (overlay) overlay.style.display = sidebar.classList.contains('open') ? 'block' : 'none';
  });

  if (overlay) {
    overlay.addEventListener('click', () => {
      sidebar.classList.remove('open');
      overlay.style.display = 'none';
    });
  }
}

function initSidebarScrollClose() {
  const sidebar = document.getElementById('sidebar');
  const overlay = document.getElementById('sidebar-overlay');
  if (!sidebar) return;

  const closeSidebar = () => {
    sidebar.classList.remove('open');
    if (overlay) overlay.style.display = 'none';
  };

  window.addEventListener('wheel', (event) => {
    if (!sidebar.classList.contains('open')) return;
    closeSidebar();
    event.preventDefault();
    window.scrollBy({ top: event.deltaY, left: event.deltaX, behavior: 'auto' });
  }, { passive: false, capture: true });

  let touchY = null;
  window.addEventListener('touchstart', (event) => {
    if (sidebar.classList.contains('open')) touchY = event.touches[0]?.clientY ?? null;
  }, { passive: true, capture: true });

  window.addEventListener('touchmove', (event) => {
    if (!sidebar.classList.contains('open') || touchY == null) return;
    const nextY = event.touches[0]?.clientY ?? touchY;
    closeSidebar();
    event.preventDefault();
    window.scrollBy({ top: touchY - nextY, behavior: 'auto' });
    touchY = null;
  }, { passive: false, capture: true });
}

function showSkeletons(containerId, count = 3, type = 'card') {
  const el = document.getElementById(containerId);
  if (!el) return;
  el.innerHTML = Array(count).fill(
    type === 'card'
      ? '<div class="skeleton skeleton-card"></div>'
      : `<div style="padding:16px;background:#fff;border-radius:8px;border:1px solid #e2e8f0">
           <div class="skeleton skeleton-text" style="width:60%"></div>
           <div class="skeleton skeleton-text" style="width:80%"></div>
           <div class="skeleton skeleton-text" style="width:40%"></div>
         </div>`
  ).join('');
}

function showEmpty(containerId, message = 'No data found', icon = ICONS.empty) {
  const el = document.getElementById(containerId);
  if (!el) return;
  el.innerHTML = `
    <div class="empty-state">
      <div class="empty-icon">${icon}</div>
      <h3>Nothing here yet</h3>
      <p>${message}</p>
    </div>`;
}

function validateField(input, rule) {
  const val = input.value.trim();
  if (rule === 'required' && !val) return 'This field is required.';
  if (rule === 'email' && val && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val)) return 'Enter a valid email address.';
  if (rule.startsWith('min:')) {
    const min = parseInt(rule.split(':')[1], 10);
    if (val.length < min) return `Must be at least ${min} characters.`;
  }
  if (rule.startsWith('match:')) {
    const otherId = rule.split(':')[1];
    const other = document.getElementById(otherId);
    if (other && val !== other.value.trim()) return 'Passwords do not match.';
  }
  return null;
}

function setFieldError(input, message) {
  input.classList.toggle('error', !!message);
  const errEl = input.parentElement.querySelector('.field-error');
  if (errEl) {
    errEl.textContent = message || '';
    errEl.classList.toggle('visible', !!message);
  }
}

function showAlert(id, message, type = 'error') {
  const el = document.getElementById(id);
  if (!el) return;
  el.className = `alert alert-${type} visible`;
  const msg = el.querySelector('.alert-msg');
  if (msg) msg.textContent = message;
}

function hideAlert(id) {
  const el = document.getElementById(id);
  if (el) el.classList.remove('visible');
}

function getParam(key) {
  return new URLSearchParams(window.location.search).get(key);
}

function initLogoutBtn() {
  const btn = document.getElementById('logout-btn');
  if (btn) btn.addEventListener('click', logout);
}

function openModal(id) {
  const el = document.getElementById(id);
  if (el) el.classList.add('open');
}

function closeModal(id) {
  const el = document.getElementById(id);
  if (el) el.classList.remove('open');
}

function initModalCloseButtons() {
  document.querySelectorAll('[data-close-modal]').forEach((btn) => {
    btn.addEventListener('click', () => closeModal(btn.dataset.closeModal));
  });

  document.querySelectorAll('.modal-overlay').forEach((overlay) => {
    overlay.addEventListener('click', (event) => {
      if (event.target === overlay) overlay.classList.remove('open');
    });
  });
}

function renderCourseCard(course) {
  const progress = course.progress ?? 0;
  const emoji = course.emoji || ICONS.courses;
  return `
    <div class="course-card" onclick="window.location.href='/Teacher/CourseDetails?id=${course.id}'">
      <div class="course-card__thumb">
        <span style="position:relative;z-index:1">${emoji}</span>
        ${course.isNew ? '<span class="course-card__thumb-label">New</span>' : ''}
      </div>
      <div class="course-card__body">
        <div class="course-card__category">${course.category || 'General'}</div>
        <div class="course-card__title">${course.title}</div>
        <div class="course-card__meta">
          <span>${ICONS.article} ${course.lessonsCount || 0} lessons</span>
          <span>${ICONS.video} ${course.duration || '-'}</span>
        </div>
        <div class="course-card__progress-bar">
          <div class="course-card__progress-fill" style="width:${progress}%"></div>
        </div>
        <div class="course-card__progress-label">
          <span>Progress</span><span>${progress}%</span>
        </div>
      </div>
      <div class="course-card__footer">
        <div class="course-card__author">
          <div class="course-card__avatar-sm">${(course.instructorName || 'T')[0]}</div>
          ${course.instructorName || 'Instructor'}
        </div>
        <span class="badge badge-amber">${course.level || 'All levels'}</span>
      </div>
    </div>`;
}

function renderExamCard(exam) {
  const statusClass = {
    pending: 'badge-amber',
    completed: 'badge-green',
    missed: 'badge-red',
    active: 'badge-green',
    draft: 'badge-gray',
    closed: 'badge-blue',
  }[exam.status] || 'badge-gray';

  return `
    <div class="exam-card">
      <div class="exam-card__icon">${ICONS.exams}</div>
      <div class="exam-card__title">${exam.title}</div>
      <div class="exam-card__course">${exam.courseName || 'General'}</div>
      <div class="exam-card__details">
        <div class="exam-detail-row">${ICONS.video} Duration: <strong>${exam.duration || 'N/A'} min</strong></div>
        <div class="exam-detail-row">${ICONS.quiz} Questions: <strong>${exam.questionsCount || 0}</strong></div>
        <div class="exam-detail-row">${ICONS.schedule} Due: <strong>${exam.dueDate ? new Date(exam.dueDate).toLocaleDateString() : 'Open'}</strong></div>
        ${exam.score != null ? `<div class="exam-detail-row">${ICONS.achievements} Score: <strong>${exam.score}%</strong></div>` : ''}
      </div>
      <div class="exam-card__footer">
        <span class="badge ${statusClass}">${exam.status || 'pending'}</span>
        ${exam.status !== 'completed'
          ? `<button class="btn-amber" onclick="window.location.href='/Teacher/TakeExam?id=${exam.id}'">Start</button>`
          : `<button class="btn-secondary" onclick="window.location.href='/Teacher/TakeExam?id=${exam.id}&review=true'">Review</button>`}
      </div>
    </div>`;
}

document.addEventListener('DOMContentLoaded', () => {
  document.body.classList.add('page-fade');
  initLogoutBtn();
  initModalCloseButtons();
  initMobileSidebar();
  initSidebarScrollClose();
});


requireAuth();
const __pageUser = getUser();
// Backend required: enforce role authorization server-side. Do not cross-redirect from stale preview state.

populateSidebarUser();

  const user = getUser();
  document.body.classList.add('student-shell');
  const examId = getParam('id') || '2';
  const reviewMode = getParam('review') === 'true';
  const navItems = [
    { icon: '&#127968;', label: 'Dashboard', page: '/Teacher/Dashboard' },
    { icon: '&#128218;', label: 'Courses', page: '/Teacher/Courses' },
    { icon: '&#128221;', label: 'Exams', page: '/Teacher/Exams' },
    { icon: '&#128101;', label: 'Students', page: '/Teacher/Dashboard?view=students' },
    { icon: '&#11014;', label: 'Upgrade', page: '/Teacher/Dashboard?view=upgrade' },
    { icon: '&#9881;', label: 'Settings', page: '/Teacher/Dashboard?view=settings' },
  ];

  document.getElementById('sidebar-nav').innerHTML =
    '<div class="sidebar__nav-section">Workspace</div>' +
    navItems.map((item) => `
      <a href="${item.page}" class="nav-item" data-page="${item.page}">
        <span class="nav-icon">${item.icon}</span><span>${item.label}</span>
      </a>
    `).join('');

  document.querySelectorAll('#sidebar-nav .nav-item').forEach((item) => {
    if (item.getAttribute('href') === '/Teacher/Exams') item.classList.add('active');
  });

  const showNavTransition = () => {
    const pageBody = document.querySelector('.page-body');
    if (!pageBody) return;
    pageBody.innerHTML = `
      <div style="min-height:60vh;display:flex;align-items:center;justify-content:center">
        <div style="width:40px;height:40px;border:3px solid var(--gray-200);border-top-color:var(--amber);border-radius:50%;animation:spin 0.7s linear infinite"></div>
      </div>
    `;
  };

  document.querySelectorAll('#sidebar-nav .nav-item').forEach((item) => {
    item.addEventListener('click', (event) => {
      const href = item.getAttribute('href');
      if (!href || href === '#') return;
      event.preventDefault();
      document.getElementById('sidebar').classList.remove('open');
      document.getElementById('sidebar-overlay').style.display = 'none';
      showNavTransition();
      setTimeout(() => {
        window.location.href = href;
      }, 180);
    });
  });

  document.getElementById('sidebar-peek-toggle')?.addEventListener('click', () => {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebar-overlay');
    const open = sidebar.classList.toggle('open');
    overlay.style.display = open ? 'block' : 'none';
  });
  document.getElementById('sidebar-overlay')?.addEventListener('click', () => {
    document.getElementById('sidebar').classList.remove('open');
    document.getElementById('sidebar-overlay').style.display = 'none';
  });

  let exam = null;
  let answers = {};
  let currentQ = 0;
  let timerSecs = 0;
  let timerInterval = null;
  let submitted = false;

  async function loadExam() {
    try {
      exam = await api.get(`/exams/${examId}`);
      if (!exam || !Array.isArray(exam.questions) || !exam.questions.length) {
        throw new Error('Exam questions are not available right now.');
      }

      if (reviewMode) {
        if (!exam.lastResult) {
          throw new Error('No saved review is available for this exam yet.');
        }

        document.getElementById('exam-title-bar').textContent = `${exam.title || 'Exam'} Review`;
        setBreadcrumbTrail('page-breadcrumb', [
          { label: 'Home', href: '/Teacher/Dashboard' },
          { label: 'Exams', href: '/Teacher/Exams' },
          { label: 'Review' },
        ]);
        document.title = `${exam.title || 'Exam'} Review - EduPlatform`;
        renderResults(exam.lastResult, true);
        return;
      }

      timerSecs = (Number(exam.duration) || 30) * 60;
      document.getElementById('exam-title-bar').textContent = exam.title || 'Take Exam';
      setBreadcrumbTrail('page-breadcrumb', [
        { label: 'Home', href: '/Teacher/Dashboard' },
        { label: 'Exams', href: '/Teacher/Exams' },
        { label: exam.title || 'Take Exam' },
      ]);
      document.title = `${exam.title || 'Take Exam'} - EduPlatform`;
      renderExamStart();
    } catch (error) {
      renderLoadError(error);
    }
  }

  function renderLoadError(error) {
    document.getElementById('exam-title-bar').textContent = 'Exam Unavailable';
    setBreadcrumbTrail('page-breadcrumb', [
      { label: 'Home', href: '/Teacher/Dashboard' },
      { label: 'Exams', href: '/Teacher/Exams' },
      { label: 'Unavailable' },
    ]);
    document.getElementById('exam-content').innerHTML = `
      <div class="exam-layout">
        <div style="background:var(--white);border-radius:var(--radius);padding:40px;text-align:center;box-shadow:var(--shadow-lg);border:1px solid var(--gray-200);max-width:560px;margin:0 auto">
          <div style="font-size:52px;margin-bottom:18px">&#9888;</div>
          <h2 style="font-family:var(--font-head);font-size:26px;color:var(--navy);margin-bottom:10px">This exam could not be opened</h2>
          <p style="font-size:14px;color:var(--gray-600);line-height:1.7;margin-bottom:24px">${error?.message || 'Something went wrong while loading the exam.'}</p>
          <div style="display:flex;gap:12px;justify-content:center;flex-wrap:wrap">
            <button class="btn-secondary" onclick="window.location.href='/Teacher/Exams'">Back to Exams</button>
            <button class="btn-primary" onclick="loadExam()">Try Again</button>
          </div>
        </div>
      </div>`;
    showToast(error?.message || 'Could not load the exam.', 'error');
  }

  function renderExamStart() {
    document.getElementById('exam-content').innerHTML = `
      <div class="exam-layout">
        <div style="background:var(--white);border-radius:var(--radius);padding:48px;text-align:center;box-shadow:var(--shadow-lg);border:1px solid var(--gray-200);max-width:560px;margin:0 auto">
          <div style="font-size:56px;margin-bottom:20px">&#128221;</div>
          <h2 style="font-family:var(--font-head);font-size:26px;color:var(--navy);margin-bottom:8px">${exam.title}</h2>
          <p style="font-size:13.5px;color:var(--amber-dark);font-weight:500;margin-bottom:20px">${exam.courseName}</p>
          <div style="display:grid;grid-template-columns:repeat(3,1fr);gap:16px;margin-bottom:24px">
            <div style="padding:16px;background:var(--gray-50);border-radius:var(--radius-sm);border:1px solid var(--gray-200)">
              <div style="font-size:22px;font-family:var(--font-head);color:var(--navy)">${exam.questions.length}</div>
              <div style="font-size:12px;color:var(--gray-400);margin-top:4px">Questions</div>
            </div>
            <div style="padding:16px;background:var(--gray-50);border-radius:var(--radius-sm);border:1px solid var(--gray-200)">
              <div style="font-size:22px;font-family:var(--font-head);color:var(--navy)">${exam.duration}</div>
              <div style="font-size:12px;color:var(--gray-400);margin-top:4px">Minutes</div>
            </div>
            <div style="padding:16px;background:var(--gray-50);border-radius:var(--radius-sm);border:1px solid var(--gray-200)">
              <div style="font-size:22px;font-family:var(--font-head);color:var(--navy)">70%</div>
              <div style="font-size:12px;color:var(--gray-400);margin-top:4px">Pass Mark</div>
            </div>
          </div>
          ${exam.instructions ? `
            <div style="background:var(--amber-glow);border:1px solid rgba(245,158,11,0.2);border-radius:var(--radius-sm);padding:16px 20px;text-align:left;margin-bottom:28px">
              <div style="font-size:12px;font-weight:700;color:var(--amber-dark);text-transform:uppercase;letter-spacing:0.06em;margin-bottom:8px">Instructions</div>
              <p style="font-size:13.5px;color:var(--gray-800);line-height:1.7">${exam.instructions}</p>
            </div>` : ''}
          <button class="btn-primary" id="start-exam-btn" style="padding:14px 48px;font-size:16px;width:auto;display:inline-block">Start Exam</button>
        </div>
      </div>`;

    document.getElementById('start-exam-btn').addEventListener('click', startExam);
  }

  function startExam() {
    startTimer();
    renderQuestion(0);
  }

  function startTimer() {
    timerInterval = setInterval(() => {
      timerSecs--;
      updateTimerDisplay();
      if (timerSecs <= 0) {
        clearInterval(timerInterval);
        submitExam(true);
      }
    }, 1000);
  }

  function updateTimerDisplay() {
    const el = document.getElementById('timer-display');
    if (!el) return;
    const m = Math.floor(timerSecs / 60).toString().padStart(2, '0');
    const s = (timerSecs % 60).toString().padStart(2, '0');
    el.textContent = `${m}:${s}`;
    el.style.color = timerSecs < 120 ? '#ef4444' : 'var(--amber)';
  }

  function renderQuestion(index) {
    currentQ = index;
    const q = exam.questions[index];
    const answered = Object.keys(answers).length;
    const progressPct = Math.round((answered / exam.questions.length) * 100);

    document.getElementById('exam-content').innerHTML = `
      <div class="exam-layout">
        <div class="exam-header-card">
          <div class="exam-header-card__info">
            <div class="exam-header-card__title">${exam.title}</div>
            <div class="exam-header-card__sub">Question ${index + 1} of ${exam.questions.length} Â· ${answered} answered</div>
          </div>
          <div class="exam-timer">
            <div class="exam-timer__time" id="timer-display">--:--</div>
            <div class="exam-timer__label">Time Left</div>
          </div>
        </div>
        <div class="exam-progress-bar"><div class="exam-progress-fill" style="width:${progressPct}%"></div></div>
        <div class="question-card">
          <div class="question-num">Question ${index + 1} of ${exam.questions.length}</div>
          <div class="question-text">${q.text}</div>
          <div class="options-list">
            ${q.options.map((opt, i) => `
              <label class="option-label">
                <input type="radio" name="question-${q.id}" value="${i}" ${answers[index] === i ? 'checked' : ''} onchange="selectAnswer(${index}, ${i})" />
                <div class="option-radio"></div>
                <span class="option-text">${opt}</span>
              </label>
            `).join('')}
          </div>
        </div>
        <div class="exam-nav">
          <button class="btn-secondary" ${index === 0 ? 'disabled style="opacity:0.4"' : ''} onclick="renderQuestion(${index - 1})">Previous</button>
          <div class="question-dots">
            ${exam.questions.map((_, i) => `<div class="q-dot ${i === index ? 'active' : ''} ${answers[i] !== undefined ? 'answered' : ''}" onclick="renderQuestion(${i})"></div>`).join('')}
          </div>
          ${index < exam.questions.length - 1
            ? `<button class="btn-primary" onclick="renderQuestion(${index + 1})">Next</button>`
            : `<button class="btn-amber" onclick="confirmSubmit()">Submit Exam</button>`}
        </div>
      </div>`;
    updateTimerDisplay();
  }

  function selectAnswer(qIndex, optIndex) {
    answers[qIndex] = optIndex;
  }
  window.selectAnswer = selectAnswer;
  window.renderQuestion = renderQuestion;

  function confirmSubmit() {
    const unanswered = exam.questions.length - Object.keys(answers).length;
    document.getElementById('submit-confirm-msg').textContent = unanswered > 0
      ? `You still have ${unanswered} unanswered question${unanswered > 1 ? 's' : ''}.`
      : 'You answered all questions. Ready to submit?';
    openModal('submit-confirm-modal');
    document.getElementById('confirm-submit-btn').onclick = () => {
      closeModal('submit-confirm-modal');
      submitExam(false);
    };
  }

  function submitExam() {
    if (submitted) return;
    submitted = true;
    clearInterval(timerInterval);
    const fields = { ExamId: examId };
    exam.questions.forEach((question, index) => {
      fields[`Answers[${index}].QuestionId`] = question.id;
      fields[`Answers[${index}].SelectedOption`] = answers[index] ?? '';
    });
    submitMvcForm('/Teacher/Exams/Submit', fields);
  }

  function renderResults(result, isReview = false) {
    document.getElementById('exam-content').innerHTML = `
      <div class="exam-layout">
        <div class="result-card">
          <div class="result-score-circle" style="--pct:${result.score * 3.6}deg">
            <div class="result-score-num">${result.score}%</div>
          </div>
          <div class="result-title">${isReview ? 'Exam Review' : (result.score >= 70 ? 'Congratulations!' : 'Keep Going')}</div>
          <div class="result-sub">${isReview
            ? `Here is your latest saved result${result.submittedAt ? ` from ${new Date(result.submittedAt).toLocaleString()}` : ''}.`
            : (result.score >= 70 ? 'You passed the exam.' : 'Review the answers and try to improve next time.')}</div>
          <div style="display:grid;grid-template-columns:repeat(3,1fr);gap:12px;margin-bottom:28px">
            <div style="padding:14px;background:var(--gray-50);border-radius:var(--radius-sm)"><div style="font-family:var(--font-head);font-size:22px;color:var(--green)">${result.correct}</div><div style="font-size:12px;color:var(--gray-400)">Correct</div></div>
            <div style="padding:14px;background:var(--gray-50);border-radius:var(--radius-sm)"><div style="font-family:var(--font-head);font-size:22px;color:var(--red)">${result.total - result.correct}</div><div style="font-size:12px;color:var(--gray-400)">Incorrect</div></div>
            <div style="padding:14px;background:var(--gray-50);border-radius:var(--radius-sm)"><div style="font-family:var(--font-head);font-size:22px;color:var(--navy)">${result.total}</div><div style="font-size:12px;color:var(--gray-400)">Total</div></div>
          </div>
          <div style="display:flex;gap:12px;justify-content:center">
            <button class="btn-secondary" onclick="window.location.href='/Teacher/Exams'" style="padding:12px 24px">Back to Exams</button>
            <button class="btn-primary" onclick="window.location.href='/Teacher/Dashboard'" style="padding:12px 24px">Go to Dashboard</button>
          </div>
        </div>
      </div>`;
  }

  loadExam();
















