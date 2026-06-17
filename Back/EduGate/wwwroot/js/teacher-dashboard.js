/* Page script: pages/teacher//Teacher/Dashboard */
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
  const isTeacher = true;
  let currentView = getParam('view') || 'overview';
  let lastGeneratedStudents = [];
  const teacherPackages = [
    // Backend required: available packages with { id, name, seats, price, billing, note, features }.
  ];

  document.body.classList.add('student-shell');

  if (isTeacher && currentView === 'analytics') {
    currentView = 'overview';
    window.history.replaceState({}, '', '/Teacher/Dashboard');
  }

  const teacherNav = [
    { icon: '&#127968;', label: 'Dashboard', page: '/Teacher/Dashboard', view: 'overview' },
    { icon: '&#128218;', label: 'Courses', page: '/Teacher/Courses' },
    { icon: '&#128221;', label: 'Exams', page: '/Teacher/Exams' },
    { icon: '&#128101;', label: 'Students', page: '#', view: 'students' },
    { icon: '&#11014;', label: 'Upgrade', page: '#', view: 'upgrade' },
    { icon: '&#9881;', label: 'Settings', page: '#', view: 'settings' },
  ];

  const studentNav = [
        { icon: '&#127968;', label: 'Dashboard', page: '/Teacher/Dashboard', view: 'overview' },
        { icon: '&#128218;', label: 'My Courses', page: '/Teacher/Courses' },
        { icon: '&#128221;', label: 'My Exams', page: '/Teacher/Exams' },
        { icon: '&#128197;', label: 'Schedule', page: '#', view: 'schedule' },
        { icon: '&#9881;', label: 'Settings', page: '#', view: 'settings' },
      ];

  const navItems = isTeacher ? teacherNav : studentNav;

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

  document.getElementById('sidebar-nav').innerHTML =
    '<div class="sidebar__nav-section">Workspace</div>' +
    navItems.map((item) => `
      <a href="${item.page}" class="nav-item" data-page="${item.page}" ${item.view ? `data-view="${item.view}"` : ''}>
        <span class="nav-icon">${item.icon}</span>
        <span>${item.label}</span>
        ${item.badge ? `<span class="nav-badge">${item.badge}</span>` : ''}
      </a>
    `).join('');

  document.querySelectorAll('#sidebar-nav .nav-item[data-view]').forEach((item) => {
    item.addEventListener('click', (event) => {
      event.preventDefault();
      if (currentView === item.dataset.view) {
        document.getElementById('sidebar').classList.remove('open');
        document.getElementById('sidebar-overlay').style.display = 'none';
        return;
      }
      currentView = item.dataset.view;
      window.history.replaceState({}, '', currentView === 'overview' ? '/Teacher/Dashboard' : `/Teacher/Dashboard?view=${currentView}`);
      loadCurrentView();
      syncActiveNav();
      document.getElementById('sidebar').classList.remove('open');
      document.getElementById('sidebar-overlay').style.display = 'none';
    });
  });

  document.querySelectorAll('#sidebar-nav .nav-item:not([data-view])').forEach((item) => {
    item.addEventListener('click', (event) => {
      const href = item.getAttribute('href');
      if (!href || href === '#') return;
      event.preventDefault();
      document.getElementById('sidebar').classList.remove('open');
      document.getElementById('sidebar-overlay').style.display = 'none';
      showDashboardViewLoader();
      setTimeout(() => {
        window.location.href = href;
      }, VIEW_TRANSITION_DELAY);
    });
  });

  function syncActiveNav() {
    document.querySelectorAll('#sidebar-nav .nav-item').forEach((item) => item.classList.remove('active'));
    const activeView = currentView === 'generate-students' ? 'students' : currentView;
    const active = document.querySelector(`#sidebar-nav .nav-item[data-view="${activeView}"]`)
      || document.querySelector('#sidebar-nav .nav-item[data-page="/Teacher/Dashboard"]');
    if (active) active.classList.add('active');
  }

  function setTitles(title, crumb) {
    document.getElementById('page-title').textContent = title;
    setBreadcrumbTrail('page-breadcrumb', [
      { label: 'Home', href: '/Teacher/Dashboard' },
      { label: crumb },
    ]);
  }

  let viewLoadToken = 0;
  const VIEW_TRANSITION_DELAY = 180;

  function waitForViewTransition() {
    return new Promise((resolve) => setTimeout(resolve, VIEW_TRANSITION_DELAY));
  }

  function showDashboardViewLoader() {
    const content = document.getElementById('dashboard-content');
    if (!content) return;
    content.classList.remove('dashboard-view-ready');
    content.innerHTML = `
      <div class="dashboard-view-loader">
        <div class="dashboard-view-loader__spinner"></div>
      </div>
    `;
  }

  async function renderCurrentDashboardView() {
    if (isTeacher) {
      if (currentView === 'students') return renderTeacherStudents();
      if (currentView === 'generate-students') return renderTeacherGenerateStudents();
      if (currentView === 'upgrade') return renderTeacherUpgrade();
      if (currentView === 'settings') return renderTeacherSettings();
      return renderTeacherOverview();
    }
    if (currentView === 'schedule') return renderStudentSchedule();
    if (currentView === 'settings') return renderStudentSettings();
    return renderStudentOverview();
  }

  async function loadCurrentView() {
    const token = ++viewLoadToken;
    showDashboardViewLoader();
    await waitForViewTransition();
    if (token !== viewLoadToken) return;
    await renderCurrentDashboardView();
    if (token !== viewLoadToken) return;
    document.getElementById('dashboard-content')?.classList.add('dashboard-view-ready');
  }

  function infoCards(items) {
    return `<div class="cards-grid">${items.map((item) => `
      <div class="info-card">
        <h3>${item.title}</h3>
        <p style="font-size:14px;color:var(--gray-600);line-height:1.8">${item.text}</p>
      </div>
    `).join('')}</div>`;
  }

  function getTeacherPackageProfile() {
    const profile = getUser() || {};
    const fallbackPackage = {
      // Backend required: current teacher package with { id, name, seats, price, billing, note, features }.
      id: '',
      name: '',
      seats: 0,
      price: '',
      billing: '',
      note: '',
      features: [],
    };
    return teacherPackages.find((pkg) => pkg.id === profile.packageId) || profile.currentPackage || fallbackPackage;
  }

  function scheduleEventCard(event) {
    return `
      <div class="info-card" style="padding:0;overflow:hidden">
        <div style="display:flex;align-items:stretch">
          <div style="width:88px;background:linear-gradient(180deg,var(--navy),var(--navy-light));color:var(--white);display:flex;flex-direction:column;align-items:center;justify-content:center;padding:18px 12px">
            <div style="font-size:11px;letter-spacing:0.08em;text-transform:uppercase;color:rgba(255,255,255,0.55)">${event.month}</div>
            <div style="font-family:var(--font-head);font-size:30px;line-height:1;color:var(--amber)">${event.day}</div>
          </div>
          <div style="flex:1;padding:20px 22px">
            <div style="display:flex;align-items:flex-start;justify-content:space-between;gap:16px;margin-bottom:10px">
              <div>
                <div style="font-family:var(--font-head);font-size:20px;color:var(--navy);margin-bottom:4px">${event.title}</div>
                <div style="font-size:13px;color:var(--gray-400)">${event.course}</div>
              </div>
              <span class="badge ${event.badgeClass}">${event.badge}</span>
            </div>
            <div style="display:flex;flex-wrap:wrap;gap:10px 18px;font-size:13px;color:var(--gray-600);margin-bottom:14px">
              <span>${event.time}</span>
              <span>${event.type}</span>
              <span>${event.note}</span>
            </div>
            <div style="padding:12px 14px;background:var(--gray-50);border-top:1px solid var(--gray-100);border-radius:12px;font-size:13px;color:var(--gray-600)">
              ${event.description}
            </div>
          </div>
        </div>
      </div>
    `;
  }

  function updateStoredUserProfile(fields) {
    const current = getUser();
    if (!current) return;
    window.__EDU_CURRENT_USER__ = { ...current, ...fields };
    populateSidebarUser();
    return window.__EDU_CURRENT_USER__;
  }

  async function renderTeacherOverview() {
    setTitles('Teacher Dashboard', 'Teaching Control Center');
    const [stats, courses] = await Promise.all([
      api.get('/teacher/stats'),
      api.get('/teacher/courses?limit=6'),
    ]);

    document.getElementById('dashboard-content').innerHTML = `
      <div style="background:var(--navy);border-radius:var(--radius);padding:28px 32px;margin-bottom:28px;display:flex;align-items:center;justify-content:space-between;gap:20px;position:relative;overflow:hidden">
        <div style="position:absolute;inset:0;background:radial-gradient(ellipse at 80% 50%,rgba(245,158,11,0.15),transparent 60%)"></div>
        <div style="position:relative;z-index:1;max-width:620px">
          <div style="font-size:13px;color:rgba(255,255,255,0.5);margin-bottom:6px">Package driven teaching workspace</div>
          <div style="font-family:var(--font-head);font-size:28px;color:var(--white)">Manage your students, lessons, assignments, and exams from one place.</div>
          <div style="font-size:14px;color:rgba(255,255,255,0.58);margin-top:10px">Create accounts within your package limits, upload recorded lectures, and keep your classes organized online.</div>
        </div>
        <button class="btn-amber" style="position:relative;z-index:1;min-width:180px" onclick="openModal('create-course-modal')">+ Create Course</button>
      </div>

      <div class="stats-row">
        <div class="stat-card">
          <div class="stat-card__icon">&#128101;</div>
          <div class="stat-card__num">${stats.totalStudents}</div>
          <div class="stat-card__label">Student Accounts In Use</div>
          <div class="stat-card__change up">Seats currently active</div>
        </div>
        <div class="stat-card">
          <div class="stat-card__icon">&#128218;</div>
          <div class="stat-card__num">${stats.activeCourses}</div>
          <div class="stat-card__label">Published Courses</div>
          <div class="stat-card__change up">Ready for your students</div>
        </div>
        <div class="stat-card">
          <div class="stat-card__icon">&#127909;</div>
          <div class="stat-card__num">${stats.totalLessons}</div>
          <div class="stat-card__label">Lessons & Uploads</div>
          <div class="stat-card__change up">Across all course modules</div>
        </div>
        <div class="stat-card">
          <div class="stat-card__icon">&#128221;</div>
          <div class="stat-card__num">${stats.totalExams}</div>
          <div class="stat-card__label">Exams</div>
          <div class="stat-card__change up">Published assessments</div>
        </div>
      </div>

      <div class="section-header">
        <div>
          <div class="section-title">Your Teaching Catalog</div>
          <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Courses students can access through their generated accounts</div>
        </div>
        <a href="/Teacher/Courses" class="section-link">Manage all courses</a>
      </div>
      <div class="cards-grid">${courses.map(renderCourseCard).join('')}</div>

      <div class="section-header" style="margin-top:40px">
        <div>
          <div class="section-title">Teacher Checklist</div>
          <div style="font-size:13px;color:var(--gray-400);margin-top:6px">A clean workflow for launching your next class</div>
        </div>
      </div>
      ${infoCards([
        { title: '1. Generate Student Accounts', text: 'Create student logins based on your package capacity and send the credentials directly to your class.' },
        { title: '2. Upload Lessons', text: 'Add lecture videos, recorded sessions, and structured course lessons so students can learn anytime.' },
        { title: '3. Publish Exams & Assignments', text: 'Build assessments, assign deadlines, and review results from the same teaching dashboard.' },
      ])}
    `;
  }

  async function renderTeacherStudents() {
    setTitles('Students', 'Student Accounts & Capacity');
    const students = await api.get('/teacher/students?limit=6');
    const activePackage = getTeacherPackageProfile();
    document.getElementById('dashboard-content').innerHTML = `
      <div class="section-header">
        <div>
          <div class="section-title">Student Account Management</div>
          <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Track the students using your package seats and their learning activity.</div>
        </div>
        <button class="btn-amber" onclick="openTeacherGenerateAccounts()">Generate Accounts</button>
      </div>
      <div class="stats-row">
        <div class="stat-card"><div class="stat-card__icon">&#128101;</div><div class="stat-card__num">${students.length}</div><div class="stat-card__label">Visible Students</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#128274;</div><div class="stat-card__num">${activePackage.seats}</div><div class="stat-card__label">Package Capacity</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#9989;</div><div class="stat-card__num">${students.filter((s) => s.status === 'active').length}</div><div class="stat-card__label">Active Students</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#128200;</div><div class="stat-card__num">${Math.round(students.reduce((sum, s) => sum + s.progress, 0) / students.length)}%</div><div class="stat-card__label">Average Progress</div></div>
      </div>
      <div class="table-wrap">
        <table class="data-table">
          <thead>
            <tr><th>Student</th><th>Course</th><th>Progress</th><th>Status</th></tr>
          </thead>
          <tbody>
            ${students.map((s) => `
              <tr>
                <td><strong>${s.name}</strong><div style="font-size:12px;color:var(--gray-400)">${s.email}</div></td>
                <td>${s.course}</td>
                <td>${s.progress}%</td>
                <td><span class="badge ${s.status === 'active' ? 'badge-green' : 'badge-gray'}">${s.status}</span></td>
              </tr>
            `).join('')}
          </tbody>
        </table>
      </div>
    `;
  }

  async function renderTeacherGenerateStudents() {
    setTitles('Generate Accounts', 'Create Student Login Batch');
    const [students, courses] = await Promise.all([
      api.get('/teacher/students'),
      api.get('/teacher/courses?limit=20'),
    ]);
    const activePackage = getTeacherPackageProfile();
    const seatsUsed = students.length;
    const remainingSeats = Math.max(activePackage.seats - seatsUsed, 0);
    const generatedPreview = lastGeneratedStudents.length
      ? `
        <div class="info-card" style="margin-top:24px">
          <div class="section-header" style="margin-bottom:18px">
            <div>
              <div class="section-title">Latest Generated Batch</div>
              <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Share these credentials with your students directly.</div>
            </div>
            <button type="button" class="btn-secondary" style="width:auto;padding-inline:18px" onclick="copyGeneratedCredentials()">Copy Credentials</button>
          </div>
          <div class="table-wrap">
            <table class="data-table">
              <thead>
                <tr><th>Student</th><th>Username</th><th>Password</th><th>Course</th></tr>
              </thead>
              <tbody>
                ${lastGeneratedStudents.map((student) => `
                  <tr>
                    <td><strong>${student.name}</strong><div style="font-size:12px;color:var(--gray-400)">${student.email}</div></td>
                    <td>${student.username}</td>
                    <td>${student.password}</td>
                    <td>${student.course}</td>
                  </tr>
                `).join('')}
              </tbody>
            </table>
          </div>
        </div>
      `
      : '';

    document.getElementById('dashboard-content').innerHTML = `
      <div class="section-header" style="margin-bottom:20px">
        <div>
          <div class="section-title">Student Account Generator</div>
          <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Create a batch of ready-to-share accounts within your package seat limits.</div>
        </div>
        <button type="button" class="btn-secondary" style="width:auto;padding-inline:18px" onclick="backToTeacherStudents()">Back to Students</button>
      </div>

      <div class="stats-row">
        <div class="stat-card"><div class="stat-card__icon">&#128274;</div><div class="stat-card__num">${activePackage.seats}</div><div class="stat-card__label">Package Capacity</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#128101;</div><div class="stat-card__num">${seatsUsed}</div><div class="stat-card__label">Accounts In Use</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#9203;</div><div class="stat-card__num">${remainingSeats}</div><div class="stat-card__label">Seats Remaining</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#128218;</div><div class="stat-card__num">${courses.length}</div><div class="stat-card__label">Courses Available</div></div>
      </div>

      <div style="display:grid;grid-template-columns:minmax(0,1.2fr) minmax(280px,0.8fr);gap:24px;align-items:start">
        <div class="info-card">
          <div class="section-header" style="margin-bottom:24px">
            <div>
              <div class="section-title">Create New Batch</div>
              <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Set the number of accounts, course assignment, and login pattern for this batch.</div>
            </div>
            <span class="badge badge-blue">${remainingSeats} seats left</span>
          </div>

          <form id="generate-students-form" action="/Teacher/Students/Generate" method="post" novalidate>
            <div class="generate-batch-layout">
              <div class="generate-batch-top">
                <div class="form-group">
                  <label>Assign Courses</label>
                  <div class="course-assign-box">
                    <div class="course-dropdown" id="course-dropdown">
                      <button type="button" class="course-dropdown__trigger" id="course-dropdown-trigger" aria-expanded="false">
                        <span id="course-dropdown-label">Choose one or more courses</span>
                        <span class="course-dropdown__arrow">&#9662;</span>
                      </button>
                      <div class="course-dropdown__panel" id="course-dropdown-panel" hidden>
                        <div class="course-dropdown__list" id="course-multi-select">
                          ${courses.map((course) => `
                            <button type="button" class="course-dropdown__option" data-course-option="${course.id}">
                              <input type="checkbox" class="student-course-option" value="${course.id}" />
                              <span>${course.title}</span>
                            </button>
                          `).join('')}
                        </div>
                      </div>
                    </div>
                    <div class="course-dropdown__chips" id="course-selected-chips"></div>
                    <div class="course-dropdown__hint" id="course-selection-hint">Selected courses will be attached to every generated account in this batch.</div>
                    <div class="field-error" id="student-batch-course-error"></div>
                  </div>
                </div>

                <div class="form-group generate-batch-count">
                  <label for="student-batch-count">Number of Accounts</label>
                  <div class="count-stepper">
                    <input type="number" id="student-batch-count" min="1" max="${Math.max(remainingSeats, 1)}" value="${remainingSeats > 0 ? Math.min(remainingSeats, 10) : 0}" />
                    <div class="count-stepper__buttons">
                      <button type="button" class="count-stepper__btn" id="student-batch-count-up" aria-label="Increase number of accounts">&#9650;</button>
                      <button type="button" class="count-stepper__btn" id="student-batch-count-down" aria-label="Decrease number of accounts">&#9660;</button>
                    </div>
                  </div>
                  <div class="field-error"></div>
                </div>
              </div>

              <div class="form-group generate-batch-toggle">
                <label>Select All Courses</label>
                <label class="all-courses-toggle">
                  <input type="checkbox" id="assign-all-courses" name="AssignAllCourses" value="true" />
                  <span class="all-courses-toggle__body">
                    <span class="all-courses-toggle__content">
                      <strong>Select all courses</strong>
                      <span>Generated accounts will get access to every available course.</span>
                    </span>
                    <span class="all-courses-toggle__control" aria-hidden="true"></span>
                  </span>
                </label>
              </div>

            <div class="form-row generate-form-row generate-form-row--details">
              <div class="form-group">
                <label for="student-batch-prefix">Student Name Prefix</label>
                <input type="text" id="student-batch-prefix" name="Prefix" value="Student" placeholder="e.g. Batch A" />
                <div class="field-error"></div>
              </div>
              <div class="form-group">
                <label for="student-password-mode">Password Mode</label>
                <select id="student-password-mode">
                  <option value="auto">Auto-generate password</option>
                  <option value="fixed">Use one fixed password</option>
                </select>
                <div class="field-error"></div>
              </div>
            </div>
            </div>

            <div class="form-group" id="fixed-password-group" style="display:none">
              <label for="student-fixed-password">Fixed Password</label>
              <input type="text" id="student-fixed-password" name="FixedPassword" placeholder="At least 6 characters" />
              <div class="field-error"></div>
            </div>

            <div style="display:flex;justify-content:flex-end;gap:12px">
              <button type="button" class="btn-secondary" id="generate-students-reset">Reset</button>
              <button type="submit" class="btn-amber" id="generate-students-submit" style="width:auto;padding-inline:26px" ${remainingSeats === 0 ? 'disabled' : ''}>Generate Accounts</button>
            </div>
          </form>
        </div>

        <div style="display:grid;gap:16px">
          <div class="info-card">
            <h3>How It Works</h3>
            <p style="font-size:14px;color:var(--gray-600);line-height:1.8;margin-bottom:14px">Every batch creates ready student credentials tied to the selected courses, so you can share them with your class immediately.</p>
            <div style="display:grid;gap:10px">
              <div class="info-row"><span class="info-row-label">Current Package</span><span class="info-row-value">${activePackage.name}</span></div>
              <div class="info-row"><span class="info-row-label">Seat Limit</span><span class="info-row-value">${activePackage.seats}</span></div>
              <div class="info-row"><span class="info-row-label">Available Now</span><span class="info-row-value">${remainingSeats}</span></div>
            </div>
          </div>

          <div class="info-card">
            <h3>Batch Tips</h3>
            <div style="display:grid;gap:12px">
              <div style="font-size:13px;color:var(--gray-600);line-height:1.7">Use a clear prefix like <strong>Grade 10</strong> or <strong>Physics A</strong> so the generated usernames stay organized.</div>
              <div style="font-size:13px;color:var(--gray-600);line-height:1.7">Pick a fixed password only when you want to hand out one simple code to the whole batch.</div>
              <div style="font-size:13px;color:var(--gray-600);line-height:1.7">After generation, copy the credentials table and send it directly to students.</div>
            </div>
          </div>
        </div>
      </div>
      ${generatedPreview}
    `;

    const form = document.getElementById('generate-students-form');
    const resetBtn = document.getElementById('generate-students-reset');
    const submitBtn = document.getElementById('generate-students-submit');
    const passwordMode = document.getElementById('student-password-mode');
    const fixedPasswordGroup = document.getElementById('fixed-password-group');
    const countInput = document.getElementById('student-batch-count');
    const countUpBtn = document.getElementById('student-batch-count-up');
    const countDownBtn = document.getElementById('student-batch-count-down');
    const assignAllCourses = document.getElementById('assign-all-courses');
    const courseOptions = Array.from(document.querySelectorAll('.student-course-option'));
    const courseOptionButtons = Array.from(document.querySelectorAll('[data-course-option]'));
    const courseErrorEl = document.getElementById('student-batch-course-error');
    const courseDropdown = document.getElementById('course-dropdown');
    const courseDropdownTrigger = document.getElementById('course-dropdown-trigger');
    const courseDropdownPanel = document.getElementById('course-dropdown-panel');
    const courseDropdownLabel = document.getElementById('course-dropdown-label');
    const courseSelectedChips = document.getElementById('course-selected-chips');
    const courseSelectionHint = document.getElementById('course-selection-hint');

    const syncPasswordMode = () => {
      fixedPasswordGroup.style.display = passwordMode.value === 'fixed' ? 'block' : 'none';
    };
    const closeCourseDropdown = () => {
      courseDropdown.classList.remove('open');
      courseDropdownTrigger.setAttribute('aria-expanded', 'false');
      courseDropdownPanel.hidden = true;
    };
    const renderCourseSelectionSummary = () => {
      if (assignAllCourses.checked) {
        courseDropdownLabel.textContent = 'All available courses';
        courseSelectedChips.innerHTML = '<span class="course-chip course-chip--all">All courses assigned</span>';
        courseSelectionHint.textContent = 'Every generated account will be attached to all current courses.';
        return;
      }

      const selectedOptions = courseOptions.filter((option) => option.checked);
      courseOptionButtons.forEach((button) => {
        const option = button.querySelector('.student-course-option');
        button.classList.toggle('selected', !!option?.checked);
      });
      if (!selectedOptions.length) {
        courseDropdownLabel.textContent = 'Choose one or more courses';
        courseSelectedChips.innerHTML = '';
        courseSelectionHint.textContent = 'Selected courses will be attached to every generated account in this batch.';
        return;
      }

      courseDropdownLabel.textContent = `${selectedOptions.length} course${selectedOptions.length > 1 ? 's' : ''} selected`;
      courseSelectedChips.innerHTML = `<span class="course-chip">${selectedOptions.length} course${selectedOptions.length > 1 ? 's' : ''} assigned</span>`;
      courseSelectionHint.textContent = 'All selected courses will be available to each generated student account.';
    };
    const syncCourseSelectionMode = () => {
      const disabled = assignAllCourses.checked;
      courseOptions.forEach((option) => {
        option.disabled = disabled;
        if (disabled) option.checked = false;
      });
      courseDropdown.classList.toggle('disabled', disabled);
      courseDropdownTrigger.disabled = disabled;
      courseErrorEl.textContent = '';
      courseErrorEl.classList.remove('visible');
      closeCourseDropdown();
      renderCourseSelectionSummary();
    };
    syncPasswordMode();
    syncCourseSelectionMode();
    passwordMode.addEventListener('change', syncPasswordMode);
    assignAllCourses.addEventListener('change', syncCourseSelectionMode);
    countUpBtn.addEventListener('click', () => {
      countInput.stepUp();
      countInput.dispatchEvent(new Event('input', { bubbles: true }));
    });
    countDownBtn.addEventListener('click', () => {
      countInput.stepDown();
      countInput.dispatchEvent(new Event('input', { bubbles: true }));
    });
    courseDropdownTrigger.addEventListener('click', () => {
      if (assignAllCourses.checked) return;
      const isOpen = courseDropdown.classList.toggle('open');
      courseDropdownTrigger.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
      courseDropdownPanel.hidden = !isOpen;
    });
    courseOptionButtons.forEach((button) => {
      button.addEventListener('click', () => {
        if (assignAllCourses.checked) return;
        const option = button.querySelector('.student-course-option');
        option.checked = !option.checked;
        renderCourseSelectionSummary();
        courseErrorEl.textContent = '';
        courseErrorEl.classList.remove('visible');
      });
    });
    document.addEventListener('click', (event) => {
      if (!courseDropdown.contains(event.target)) closeCourseDropdown();
    });
    renderCourseSelectionSummary();

    form.addEventListener('submit', (event) => {
      const prefixInput = document.getElementById('student-batch-prefix');
      const fixedPasswordInput = document.getElementById('student-fixed-password');
      const selectedCourseIds = courseOptions.filter((option) => option.checked).map((option) => Number(option.value));

      const prefixError = validateField(prefixInput, 'required');
      const countValue = Number(countInput.value || 0);
      let countError = null;
      let fixedPasswordError = null;
      let courseError = null;
      if (!countValue || countValue < 1) countError = 'Enter a valid number of accounts.';
      else if (countValue > remainingSeats) countError = 'This exceeds your remaining package seats.';
      if (!assignAllCourses.checked && !selectedCourseIds.length) {
        courseError = 'Choose at least one course or enable assign to all courses.';
      }
      if (passwordMode.value === 'fixed' && fixedPasswordInput.value.trim().length < 6) {
        fixedPasswordError = 'Password must be at least 6 characters.';
      }

      setFieldError(prefixInput, prefixError);
      setFieldError(countInput, countError);
      setFieldError(fixedPasswordInput, fixedPasswordError);
      courseErrorEl.textContent = courseError || '';
      courseErrorEl.classList.toggle('visible', !!courseError);
      if (courseError || prefixError || countError || fixedPasswordError) {
        event.preventDefault();
        return;
      }

      submitBtn.disabled = true;
      submitBtn.innerHTML = '<span class="spinner" style="border-top-color:var(--navy)"></span>Generating...';
    });

    resetBtn.addEventListener('click', () => {
      renderTeacherGenerateStudents();
    });
  }

  function renderTeacherSettings() {
    setTitles('Settings', 'Teacher Profile Settings');
    const profile = getUser() || {};
    const activePackage = getTeacherPackageProfile();
    document.getElementById('dashboard-content').innerHTML = `
      <div style="display:grid;grid-template-columns:minmax(0,1.25fr) minmax(280px,0.75fr);gap:24px;align-items:start" id="teacher-settings-grid">
        <div class="info-card">
          <div class="section-header" style="margin-bottom:24px">
            <div>
              <div class="section-title">Edit Teacher Profile</div>
              <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Update the account details your students and workspace will use.</div>
            </div>
            <span class="badge badge-amber">Teacher Account</span>
          </div>

          <form id="teacher-settings-form" action="/Teacher/Settings" method="post" novalidate>
            <div class="form-row">
              <div class="form-group">
                <label for="teacher-name">Full Name</label>
                <input type="text" id="teacher-name" name="Name" value="${profile.name || ''}" placeholder="Your full name" />
                <div class="field-error"></div>
              </div>
              <div class="form-group">
                <label for="teacher-email">Email Address</label>
                <input type="email" id="teacher-email" name="Email" value="${profile.email || ''}" placeholder="teacher@email.com" />
                <div class="field-error"></div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="teacher-phone">Phone Number</label>
                <input type="text" id="teacher-phone" name="Phone" value="${profile.phone || ''}" placeholder="For student or parent contact" />
                <div class="field-error"></div>
              </div>
              <div class="form-group">
                <label for="teacher-subject">Teaching Specialty</label>
                <input type="text" id="teacher-subject" name="Subject" value="${profile.subject || ''}" placeholder="e.g. Mathematics" />
                <div class="field-error"></div>
              </div>
            </div>

            <div class="form-group">
              <label for="teacher-bio">About You</label>
              <textarea id="teacher-bio" name="Bio" rows="4" placeholder="A short teacher bio for your workspace">${profile.bio || ''}</textarea>
              <div class="field-error"></div>
            </div>

            <div class="section-header" style="margin:26px 0 14px">
              <div>
                <div class="section-title" style="font-size:18px">Change Password</div>
                <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Leave these fields empty if you want to keep your current password.</div>
              </div>
            </div>

            <div style="display:grid;gap:14px;margin-bottom:22px">
              <div class="form-row">
                <div class="form-group">
                  <label for="teacher-current-password">Current Password</label>
                  <input type="password" id="teacher-current-password" name="CurrentPassword" placeholder="Enter current password" autocomplete="current-password" />
                  <div class="field-error"></div>
                </div>
                <div class="form-group">
                  <label for="teacher-new-password">New Password</label>
                  <input type="password" id="teacher-new-password" name="NewPassword" placeholder="At least 6 characters" autocomplete="new-password" />
                  <div class="field-error"></div>
                </div>
              </div>
              <div class="form-group">
                <label for="teacher-confirm-password">Confirm New Password</label>
                <input type="password" id="teacher-confirm-password" name="ConfirmPassword" placeholder="Repeat the new password" autocomplete="new-password" />
                <div class="field-error"></div>
              </div>
            </div>

            <div style="display:flex;justify-content:flex-end;gap:12px">
              <button type="button" class="btn-secondary" id="teacher-settings-reset">Reset</button>
              <button type="submit" class="btn-primary" id="teacher-settings-save" style="width:auto;padding-inline:26px">Save Changes</button>
            </div>
          </form>
        </div>

        <div style="display:grid;gap:16px">
          <div class="info-card">
            <h3>Profile Summary</h3>
            <div style="display:flex;align-items:center;gap:14px;margin-bottom:16px">
              <div style="width:54px;height:54px;border-radius:50%;background:var(--navy);color:var(--amber);display:flex;align-items:center;justify-content:center;font-weight:700;font-size:18px" id="teacher-profile-initials">${(profile.name || 'T').split(' ').map((word) => word[0]).join('').slice(0,2).toUpperCase()}</div>
              <div>
                <div style="font-weight:700;color:var(--navy);font-size:16px" id="teacher-profile-name">${profile.name || 'Teacher'}</div>
                <div style="font-size:13px;color:var(--gray-400)" id="teacher-profile-email">${profile.email || ''}</div>
              </div>
            </div>
            <div class="info-row"><span class="info-row-label">Role</span><span class="info-row-value">Teacher</span></div>
            <div class="info-row"><span class="info-row-label">Specialty</span><span class="info-row-value" id="teacher-profile-subject">${profile.subject || 'Not set'}</span></div>
            <div class="info-row"><span class="info-row-label">Phone</span><span class="info-row-value" id="teacher-profile-phone">${profile.phone || 'Not set'}</span></div>
          </div>

          <div class="info-card">
            <h3>Current Package</h3>
            <div style="display:flex;align-items:flex-end;justify-content:space-between;gap:12px;margin-bottom:10px">
              <div>
                <div style="font-family:var(--font-head);font-size:22px;color:var(--navy)">${activePackage.name}</div>
                <div style="font-size:13px;color:var(--gray-400)">${activePackage.seats} student seats</div>
              </div>
              <span class="badge badge-blue">${activePackage.price}${activePackage.billing}</span>
            </div>
            <p style="font-size:14px;color:var(--gray-600);line-height:1.8;margin:0">${activePackage.note}</p>
          </div>
        </div>
      </div>
    `;

    const form = document.getElementById('teacher-settings-form');
    const saveBtn = document.getElementById('teacher-settings-save');
    const resetBtn = document.getElementById('teacher-settings-reset');

    form.addEventListener('submit', (event) => {
      const nameIn = document.getElementById('teacher-name');
      const emailIn = document.getElementById('teacher-email');
      const currentPasswordIn = document.getElementById('teacher-current-password');
      const newPasswordIn = document.getElementById('teacher-new-password');
      const confirmPasswordIn = document.getElementById('teacher-confirm-password');
      const wantsPasswordChange = Boolean(
        currentPasswordIn.value.trim() ||
        newPasswordIn.value.trim() ||
        confirmPasswordIn.value.trim()
      );
      const nameErr = validateField(nameIn, 'required');
      const emailErr = validateField(emailIn, 'required') || validateField(emailIn, 'email');
      const currentPasswordErr = wantsPasswordChange ? validateField(currentPasswordIn, 'required') : null;
      const newPasswordErr = wantsPasswordChange ? (validateField(newPasswordIn, 'required') || validateField(newPasswordIn, 'min:6')) : null;
      const confirmPasswordErr = wantsPasswordChange ? (validateField(confirmPasswordIn, 'required') || validateField(confirmPasswordIn, 'match:teacher-new-password')) : null;

      setFieldError(nameIn, nameErr);
      setFieldError(emailIn, emailErr);
      setFieldError(currentPasswordIn, currentPasswordErr);
      setFieldError(newPasswordIn, newPasswordErr);
      setFieldError(confirmPasswordIn, confirmPasswordErr);
      if (nameErr || emailErr || currentPasswordErr || newPasswordErr || confirmPasswordErr) {
        event.preventDefault();
        return;
      }

      saveBtn.disabled = true;
      saveBtn.textContent = 'Saving...';
    });

    resetBtn.addEventListener('click', () => {
      renderTeacherSettings();
    });
  }

  function renderTeacherUpgrade() {
    setTitles('Upgrade', 'Packages & Capacity');
    const activePackage = getTeacherPackageProfile();
    const studentsInUse = getTeacherStudentAccounts(getDemoDb(), getUser() || {}).length;
    document.getElementById('dashboard-content').innerHTML = `
      <div style="background:var(--white);border:1px solid var(--gray-200);box-shadow:var(--shadow);border-radius:var(--radius);padding:24px;margin-bottom:24px">
        <div class="section-header" style="margin-bottom:18px">
          <div>
            <div class="section-title">Your Current Package</div>
            <div style="font-size:13px;color:var(--gray-400);margin-top:6px">See your current student capacity and compare it with the available upgrades.</div>
          </div>
          <span class="badge badge-amber">Active Now</span>
        </div>

        <div class="stats-row">
          <div class="stat-card"><div class="stat-card__icon">&#128274;</div><div class="stat-card__num">${activePackage.seats}</div><div class="stat-card__label">Student Capacity</div></div>
          <div class="stat-card"><div class="stat-card__icon">&#128101;</div><div class="stat-card__num">${studentsInUse}</div><div class="stat-card__label">Accounts In Use</div></div>
          <div class="stat-card"><div class="stat-card__icon">&#9203;</div><div class="stat-card__num">${Math.max(activePackage.seats - studentsInUse, 0)}</div><div class="stat-card__label">Seats Remaining</div></div>
          <div class="stat-card"><div class="stat-card__icon">&#11088;</div><div class="stat-card__num">${activePackage.price}</div><div class="stat-card__label">Current Price${activePackage.billing}</div></div>
        </div>
      </div>

      <div class="section-header">
        <div>
          <div class="section-title">Available Packages</div>
          <div style="font-size:13px;color:var(--gray-400);margin-top:6px">Choose the package that matches your student volume.</div>
        </div>
      </div>

      <div class="cards-grid">
        ${teacherPackages.length ? teacherPackages.map((pkg) => {
          const isCurrent = pkg.id === activePackage.id;
          return `
            <div class="info-card" style="border:${isCurrent ? '2px solid var(--amber)' : '1px solid var(--gray-200)'};position:relative">
              <div style="display:flex;align-items:flex-start;justify-content:space-between;gap:14px;margin-bottom:14px">
                <div>
                  <h3 style="margin-bottom:6px">${pkg.name}</h3>
                  <div style="font-size:13px;color:var(--gray-400)">${pkg.seats} student seats</div>
                </div>
                <span class="badge ${isCurrent ? 'badge-amber' : 'badge-blue'}">${isCurrent ? 'Current' : 'Available'}</span>
              </div>
              <div style="font-family:var(--font-head);font-size:32px;color:var(--navy);margin-bottom:2px">${pkg.price}</div>
              <div style="font-size:13px;color:var(--gray-400);margin-bottom:14px">${pkg.billing}</div>
              <p style="font-size:14px;color:var(--gray-600);line-height:1.8;margin-bottom:16px">${pkg.note}</p>
              <div style="display:grid;gap:10px;margin-bottom:18px">
                ${pkg.features.map((feature) => `<div style="font-size:13px;color:var(--gray-700);display:flex;align-items:flex-start;gap:8px"><span style="color:var(--amber)">&#10003;</span><span>${feature}</span></div>`).join('')}
              </div>
              <button type="button" class="${isCurrent ? 'btn-secondary' : 'btn-amber'}" style="width:auto;padding-inline:22px" ${isCurrent ? 'disabled' : ''} onclick="upgradeTeacherPackage('${pkg.id}')">${isCurrent ? 'Current Package' : 'Upgrade Package'}</button>
            </div>
          `;
        }).join('') : `
          <div class="info-card">
            <h3>Packages are not loaded</h3>
            <p style="font-size:14px;color:var(--gray-600);line-height:1.8">Backend required: return available packages with id, name, seats, price, billing, note, and features.</p>
          </div>
        `}
      </div>
    `;
  }

  async function renderStudentOverview() {
    setTitles('Student Dashboard', 'My Learning Space');
    const [stats, enrolled, upcomingExams] = await Promise.all([
      api.get('/student/stats'),
      api.get('/student/courses?limit=4'),
      api.get('/student/exams/upcoming'),
    ]);
    document.getElementById('dashboard-content').innerHTML = `
      <div style="background:var(--navy);border-radius:var(--radius);padding:28px 32px;margin-bottom:28px;display:flex;align-items:center;justify-content:space-between;gap:20px;position:relative;overflow:hidden">
        <div style="position:absolute;inset:0;background:radial-gradient(ellipse at 80% 50%,rgba(245,158,11,0.15),transparent 60%)"></div>
        <div style="position:relative;z-index:1">
          <div style="font-size:13px;color:rgba(255,255,255,0.5);margin-bottom:6px">Your teacher has shared this workspace with you</div>
          <div style="font-family:var(--font-head);font-size:26px;color:var(--white)">Watch lessons, solve assignments, and take your exams in one place.</div>
          <div style="font-size:14px;color:rgba(255,255,255,0.58);margin-top:8px">Everything below belongs to your teacher's classroom and is ready for you to continue.</div>
        </div>
        <div style="position:relative;z-index:1;text-align:center;background:rgba(255,255,255,0.07);border:1px solid rgba(255,255,255,0.12);border-radius:var(--radius);padding:16px 24px">
          <div style="font-family:var(--font-head);font-size:32px;color:var(--amber)">${stats.avgScore}%</div>
          <div style="font-size:12px;color:rgba(255,255,255,0.45);text-transform:uppercase;letter-spacing:0.08em">Average Score</div>
        </div>
      </div>

      <div class="stats-row">
        <div class="stat-card"><div class="stat-card__icon">&#9989;</div><div class="stat-card__num">${stats.completedCourses}</div><div class="stat-card__label">Completed Courses</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#128214;</div><div class="stat-card__num">${stats.inProgressCourses}</div><div class="stat-card__label">Courses In Progress</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#128221;</div><div class="stat-card__num">${stats.totalExams}</div><div class="stat-card__label">Exams Tracked</div></div>
        <div class="stat-card"><div class="stat-card__icon">&#127942;</div><div class="stat-card__num">${stats.avgScore}%</div><div class="stat-card__label">Average Score</div></div>
      </div>

      <div class="section-header">
        <div><div class="section-title">Continue Watching</div><div style="font-size:13px;color:var(--gray-400);margin-top:6px">Courses and recordings shared by your teacher</div></div>
        <a href="/Teacher/Courses" class="section-link">Open all courses</a>
      </div>
      <div class="cards-grid">${enrolled.map(renderCourseCard).join('')}</div>

      <div class="section-header" style="margin-top:40px">
        <div><div class="section-title">Upcoming Exams</div><div style="font-size:13px;color:var(--gray-400);margin-top:6px">Stay ready for the next exam window</div></div>
        <a href="/Teacher/Exams" class="section-link">See exam page</a>
      </div>
      <div class="exams-grid" style="grid-template-columns:repeat(auto-fill,minmax(260px,1fr))">${upcomingExams.map(renderExamCard).join('')}</div>
    `;
  }

  async function renderStudentSchedule() {
    setTitles('Schedule', 'Upcoming Learning Schedule');
    const exams = await api.get('/student/exams/upcoming');
    const today = new Date();
    const courseMap = await api.get('/student/courses?limit=4');
    const learningEvents = [
      ...exams.map((exam) => {
        const date = exam.dueDate ? new Date(exam.dueDate) : today;
        return {
          month: date.toLocaleDateString(undefined, { month: 'short' }).toUpperCase(),
          day: date.getDate(),
          title: exam.title,
          course: exam.courseName,
          time: `Exam window Â· ${date.toLocaleDateString()}`,
          type: 'Online exam',
          note: `${exam.duration} min`,
          description: 'Be ready before the timer starts and review the related lessons before opening the exam.',
          badge: exam.status === 'pending' ? 'Upcoming' : exam.status,
          badgeClass: 'badge-amber',
        };
      }),
      ...courseMap.slice(0, 2).map((course, index) => ({
        month: today.toLocaleDateString(undefined, { month: 'short' }).toUpperCase(),
        day: today.getDate() + index + 1,
        title: `${course.title} study block`,
        course: course.title,
        time: 'Suggested session Â· 7:00 PM',
        type: 'Lesson review',
        note: `${course.progress || 0}% complete`,
        description: 'A suggested study session generated from your current progress so you can keep moving forward.',
        badge: 'Recommended',
        badgeClass: 'badge-blue',
      })),
    ].sort((a, b) => a.day - b.day);

    document.getElementById('dashboard-content').innerHTML = `
      <div style="background:var(--white);border:1px solid var(--gray-200);box-shadow:var(--shadow);border-radius:var(--radius);padding:24px 24px 10px;margin-bottom:24px">
        <div class="section-header" style="margin-bottom:18px">
          <div>
            <div class="section-title">This Week</div>
            <div style="font-size:13px;color:var(--gray-400);margin-top:6px">A cleaner view for exams, study blocks, and what needs your attention next.</div>
          </div>
          <span class="badge badge-blue">${learningEvents.length} items</span>
        </div>
        <div style="display:flex;flex-direction:column;gap:16px">
          ${learningEvents.map(scheduleEventCard).join('')}
        </div>
      </div>
      <div class="stats-row">
        <div class="stat-card">
          <div class="stat-card__icon">&#128197;</div>
          <div class="stat-card__num">${exams.length}</div>
          <div class="stat-card__label">Upcoming Exams</div>
        </div>
        <div class="stat-card">
          <div class="stat-card__icon">&#9200;</div>
          <div class="stat-card__num">2</div>
          <div class="stat-card__label">Suggested Study Blocks</div>
        </div>
        <div class="stat-card">
          <div class="stat-card__icon">&#9989;</div>
          <div class="stat-card__num">${courseMap.filter((course) => (course.progress || 0) > 0).length}</div>
          <div class="stat-card__label">Active Courses</div>
        </div>
      </div>
    `;
  }

  function renderStudentSettings() {
    setTitles('Settings', 'Student Account Settings');
    const profile = getUser() || {};
    document.getElementById('dashboard-content').innerHTML = `
      <div class="student-settings-shell">
        <div class="student-settings-hero">
          <div>
            <div class="student-settings-hero__eyebrow">Student Settings</div>
            <div class="student-settings-hero__title">Profile & Security</div>
            <div class="student-settings-hero__text">Keep your basic details updated and manage your password from one clean place.</div>
          </div>
          <span class="student-settings-hero__badge">Student Account</span>
        </div>

        <div class="student-settings-layout" id="student-settings-grid">
          <div class="student-settings-card">
            <div class="student-settings-section-head">
              <div>
                <div class="section-title">Edit Profile</div>
                <div class="student-settings-section-subtitle">Your teacher controls the email used for your account, so only your personal details can be edited here.</div>
              </div>
            </div>

            <form id="student-basic-form" action="/Student/Settings" method="post" novalidate>
              <div class="student-settings-panel">
                <div class="student-settings-panel__title">Basic Information</div>
                <div class="form-row">
                  <div class="form-group">
                    <label for="student-name">Full Name</label>
                    <input type="text" id="student-name" name="Name" value="${profile.name || ''}" placeholder="Your full name" />
                    <div class="field-error"></div>
                  </div>
                  <div class="form-group">
                    <label for="student-email">Email Address</label>
                    <input type="email" id="student-email" value="${profile.email || ''}" placeholder="your@email.com" readonly class="student-settings-readonly" />
                    <div class="student-settings-hint">This email is assigned by your teacher and cannot be changed here.</div>
                  </div>
                </div>

                <div class="form-group" style="margin-bottom:0">
                  <label for="student-phone">Phone Number</label>
                  <input type="text" id="student-phone" name="Phone" value="${profile.phone || ''}" placeholder="Optional contact number" />
                  <div class="field-error"></div>
                </div>
                <div class="student-settings-actions">
                  <button type="button" class="btn-secondary" id="student-basic-reset">Reset</button>
                  <button type="submit" class="btn-primary" id="student-basic-save" style="width:auto;padding-inline:26px">Save Changes</button>
                </div>
              </div>
            </form>

            <form id="student-password-form" action="/Student/ChangePassword" method="post" novalidate>
              <div class="student-settings-panel">
                <div class="student-settings-panel__title">Change Password</div>
                <div class="student-settings-panel__subtitle">Leave these fields empty if you do not want to update your password right now.</div>
                <div class="form-row">
                  <div class="form-group">
                    <label for="student-current-password">Current Password</label>
                    <input type="password" id="student-current-password" name="CurrentPassword" placeholder="Enter current password" autocomplete="current-password" />
                    <div class="field-error"></div>
                  </div>
                  <div class="form-group">
                    <label for="student-new-password">New Password</label>
                    <input type="password" id="student-new-password" name="NewPassword" placeholder="At least 6 characters" autocomplete="new-password" />
                    <div class="field-error"></div>
                  </div>
                </div>

                <div class="form-group" style="margin-bottom:0">
                  <label for="student-confirm-password">Confirm New Password</label>
                  <input type="password" id="student-confirm-password" name="ConfirmPassword" placeholder="Repeat the new password" autocomplete="new-password" />
                  <div class="field-error"></div>
                </div>
                <div class="student-settings-actions">
                  <button type="button" class="btn-secondary" id="student-password-reset">Reset</button>
                  <button type="submit" class="btn-primary" id="student-password-save" style="width:auto;padding-inline:26px">Save Password</button>
                </div>
              </div>
            </form>
          </div>

          <div class="student-settings-side">
            <div class="student-settings-card">
              <h3>Profile Summary</h3>
              <div class="student-settings-summary">
                <div class="student-settings-summary__avatar" id="student-profile-initials">${(profile.name || 'S').split(' ').map((word) => word[0]).join('').slice(0,2).toUpperCase()}</div>
                <div>
                  <div class="student-settings-summary__name" id="student-profile-name">${profile.name || 'Student'}</div>
                  <div class="student-settings-summary__email" id="student-profile-email">${profile.email || ''}</div>
                </div>
              </div>
              <div class="info-row"><span class="info-row-label">Role</span><span class="info-row-value">Student</span></div>
              <div class="info-row"><span class="info-row-label">Email</span><span class="info-row-value">${profile.email || 'Not set'}</span></div>
              <div class="info-row"><span class="info-row-label">Phone</span><span class="info-row-value" id="student-profile-phone">${profile.phone || 'Not set'}</span></div>
            </div>

            <div class="student-settings-card">
              <h3>Account Help</h3>
              <p style="font-size:14px;color:var(--gray-600);line-height:1.8;margin-bottom:12px">Use this area to keep your student profile accurate. Your teacher may use these details to organize groups and communication.</p>
            </div>

          </div>
        </div>
      </div>
    `;

    const basicForm = document.getElementById('student-basic-form');
    const basicSaveBtn = document.getElementById('student-basic-save');
    const basicResetBtn = document.getElementById('student-basic-reset');
    const passwordForm = document.getElementById('student-password-form');
    const passwordSaveBtn = document.getElementById('student-password-save');
    const passwordResetBtn = document.getElementById('student-password-reset');

    passwordForm.addEventListener('submit', (event) => {
      const currentPasswordIn = document.getElementById('student-current-password');
      const newPasswordIn = document.getElementById('student-new-password');
      const confirmPasswordIn = document.getElementById('student-confirm-password');
      const currentPasswordErr = validateField(currentPasswordIn, 'required');
      const newPasswordErr = validateField(newPasswordIn, 'required') || validateField(newPasswordIn, 'min:6');
      const confirmPasswordErr = validateField(confirmPasswordIn, 'required') || validateField(confirmPasswordIn, 'match:student-new-password');

      setFieldError(currentPasswordIn, currentPasswordErr);
      setFieldError(newPasswordIn, newPasswordErr);
      setFieldError(confirmPasswordIn, confirmPasswordErr);
      if (currentPasswordErr || newPasswordErr || confirmPasswordErr) {
        event.preventDefault();
        return;
      }

      passwordSaveBtn.disabled = true;
      passwordSaveBtn.textContent = 'Updating...';
    });

    passwordForm.addEventListener('submit', (event) => {
      event.preventDefault();
      const currentPasswordIn = document.getElementById('student-current-password');
      const newPasswordIn = document.getElementById('student-new-password');
      const confirmPasswordIn = document.getElementById('student-confirm-password');
      const db = getDemoDb();
      const dbUser = db.users.find((entry) => entry.id === profile.id);
      const currentPasswordErr = validateField(currentPasswordIn, 'required') || (dbUser?.password !== currentPasswordIn.value.trim() ? 'Current password is incorrect.' : null);
      const newPasswordErr = validateField(newPasswordIn, 'required') || validateField(newPasswordIn, 'min:6');
      const confirmPasswordErr = validateField(confirmPasswordIn, 'required') || validateField(confirmPasswordIn, 'match:student-new-password');

      setFieldError(currentPasswordIn, currentPasswordErr);
      setFieldError(newPasswordIn, newPasswordErr);
      setFieldError(confirmPasswordIn, confirmPasswordErr);
      if (currentPasswordErr || newPasswordErr || confirmPasswordErr) return;

      const confirmed = window.confirm('Are you sure you want to save these changes?');
      if (!confirmed) return;

      passwordSaveBtn.disabled = true;
      passwordSaveBtn.textContent = 'Saving...';

      updateStoredUserProfile({ password: newPasswordIn.value.trim() });

      showToast('Password updated successfully.', 'success');
      passwordSaveBtn.disabled = false;
      passwordSaveBtn.textContent = 'Save Password';
      currentPasswordIn.value = '';
      newPasswordIn.value = '';
      confirmPasswordIn.value = '';
    });

    basicResetBtn.addEventListener('click', () => {
      renderStudentSettings();
    });

    passwordResetBtn.addEventListener('click', () => {
      const currentPasswordIn = document.getElementById('student-current-password');
      const newPasswordIn = document.getElementById('student-new-password');
      const confirmPasswordIn = document.getElementById('student-confirm-password');
      currentPasswordIn.value = '';
      newPasswordIn.value = '';
      confirmPasswordIn.value = '';
      setFieldError(currentPasswordIn, null);
      setFieldError(newPasswordIn, null);
      setFieldError(confirmPasswordIn, null);
    });
  }

  document.getElementById('create-course-form')?.addEventListener('submit', (event) => {
    const titleIn = document.getElementById('course-title');
    const error = validateField(titleIn, 'required');
    setFieldError(titleIn, error);
    if (error) {
      event.preventDefault();
      return;
    }

    const btn = document.getElementById('create-course-btn');
    if (btn) {
      btn.disabled = true;
      btn.innerHTML = '<span class="spinner" style="border-top-color:var(--navy)"></span>Creating...';
    }
  });

  window.upgradeTeacherPackage = (packageId) => {
    submitMvcForm('/Teacher/Packages/Upgrade', { PackageId: packageId });
  };

  window.openTeacherGenerateAccounts = () => {
    currentView = 'generate-students';
    window.history.replaceState({}, '', '/Teacher/Dashboard?view=generate-students');
    syncActiveNav();
    loadCurrentView();
  };

  window.backToTeacherStudents = () => {
    currentView = 'students';
    window.history.replaceState({}, '', '/Teacher/Dashboard?view=students');
    syncActiveNav();
    loadCurrentView();
  };

  window.copyGeneratedCredentials = async () => {
    if (!lastGeneratedStudents.length) return;
    const text = lastGeneratedStudents
      .map((student) => `${student.name} | ${student.username} | ${student.password} | ${student.course}`)
      .join('\n');

    try {
      await navigator.clipboard.writeText(text);
      showToast('Credentials copied to clipboard.', 'success');
    } catch {
      showToast('Could not copy automatically. Please copy from the table.', 'error');
    }
  };

  syncActiveNav();
  loadCurrentView();





















