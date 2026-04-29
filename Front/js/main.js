/* ============================================================
   EDU PLATFORM - main.js
   Shared utilities, auth helpers, local demo data, UI helpers
   ============================================================ */

/* ---------- Config ---------- */
const API_BASE = '/api';
const STORAGE_KEYS = {
  user: 'edu_user',
  token: 'edu_token',
  db: 'edu_demo_db',
  cart: 'edu_book_cart',
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
  star: '&#11088;',
  fire: '&#128293;',
  note: '&#128204;',
  party: '&#127881;',
  wave: '&#128075;',
  empty: '&#128300;',
};

/* ---------- Demo database ---------- */
function createDefaultDemoDb() {
  const courses = [
    {
      id: 1,
      title: 'Python Fundamentals',
      category: 'Programming',
      emoji: ICONS.courses,
      lessonsCount: 12,
      duration: '6 weeks',
      rating: '4.9',
      instructorName: 'Ms. Sarah Ahmed',
      instructorId: 101,
      level: 'Beginner',
      progress: 75,
      isNew: true,
      studentsCount: 1240,
      description: 'Learn Python from scratch with practical exercises and real projects.',
    },
    {
      id: 2,
      title: 'Data Structures & Algorithms',
      category: 'Programming',
      emoji: '&#127794;',
      lessonsCount: 18,
      duration: '8 weeks',
      rating: '4.7',
      instructorName: 'Dr. Mona Adel',
      instructorId: 102,
      level: 'Intermediate',
      progress: 20,
      isNew: false,
      studentsCount: 860,
      description: 'Build strong problem-solving skills with data structures, recursion, and algorithm analysis.',
    },
    {
      id: 3,
      title: 'UI/UX Design Fundamentals',
      category: 'Design',
      emoji: '&#127912;',
      lessonsCount: 10,
      duration: '4 weeks',
      rating: '4.8',
      instructorName: 'Mr. Karim Samir',
      instructorId: 103,
      level: 'Beginner',
      progress: 40,
      isNew: false,
      studentsCount: 540,
      description: 'Understand design thinking, wireframes, typography, and user flows.',
    },
    {
      id: 4,
      title: 'Statistics & Probability',
      category: 'Mathematics',
      emoji: '&#128202;',
      lessonsCount: 14,
      duration: '5 weeks',
      rating: '4.6',
      instructorName: 'Dr. Lina Hassan',
      instructorId: 104,
      level: 'Intermediate',
      progress: 0,
      isNew: false,
      studentsCount: 420,
      description: 'Master descriptive statistics, distributions, and probability models.',
    },
  ];

  const lessonsByCourse = {
    1: [
      { id: 1, title: 'Introduction to Python', type: 'video', duration: '12 min', completed: true, locked: false },
      { id: 2, title: 'Variables & Data Types', type: 'video', duration: '18 min', completed: true, locked: false },
      { id: 3, title: 'Operators & Expressions', type: 'video', duration: '14 min', completed: true, locked: false },
      { id: 4, title: 'Control Flow: if/else', type: 'video', duration: '20 min', completed: false, locked: false, active: true },
      { id: 5, title: 'Loops: for & while', type: 'video', duration: '22 min', completed: false, locked: false },
      { id: 6, title: 'Quiz: Basics Check', type: 'quiz', duration: '10 min', completed: false, locked: false },
      { id: 7, title: 'Functions & Scope', type: 'video', duration: '25 min', completed: false, locked: true },
      { id: 8, title: 'Lists, Tuples & Dicts', type: 'video', duration: '28 min', completed: false, locked: true },
      { id: 9, title: 'File I/O', type: 'article', duration: '15 min', completed: false, locked: true },
      { id: 10, title: 'Object-Oriented Programming', type: 'video', duration: '35 min', completed: false, locked: true },
      { id: 11, title: 'Modules & Packages', type: 'article', duration: '12 min', completed: false, locked: true },
      { id: 12, title: 'Final Project', type: 'quiz', duration: '45 min', completed: false, locked: true },
    ],
    2: [
      { id: 1, title: 'Arrays & Linked Lists', type: 'video', duration: '16 min', completed: false, locked: false, active: true },
      { id: 2, title: 'Stacks & Queues', type: 'video', duration: '18 min', completed: false, locked: false },
      { id: 3, title: 'Trees & Graphs', type: 'video', duration: '24 min', completed: false, locked: false },
      { id: 4, title: 'Recursion', type: 'article', duration: '13 min', completed: false, locked: false },
    ],
    3: [
      { id: 1, title: 'What Makes Great UX?', type: 'video', duration: '11 min', completed: true, locked: false },
      { id: 2, title: 'Wireframes', type: 'video', duration: '17 min', completed: false, locked: false, active: true },
      { id: 3, title: 'Typography Basics', type: 'article', duration: '9 min', completed: false, locked: false },
    ],
    4: [
      { id: 1, title: 'Descriptive Statistics', type: 'video', duration: '20 min', completed: false, locked: false, active: true },
      { id: 2, title: 'Probability Rules', type: 'video', duration: '18 min', completed: false, locked: false },
    ],
  };

  const lessonContent = {
    '1-4': {
      title: 'Control Flow: if/else',
      type: 'video',
      duration: '20 min',
      videoUrl: '',
      description: 'Understand how Python makes decisions using if, elif, and else statements.',
      notes: 'Focus on boolean expressions, logical operators, and nested conditions.',
    },
  };

  const exams = [
    { id: 1, title: 'Python Basics Quiz', courseId: 1, courseName: 'Python Fundamentals', duration: 30, questionsCount: 20, dueDate: '2026-04-20', status: 'completed', score: 85, avgScore: 78 },
    { id: 2, title: 'Python Midterm', courseId: 1, courseName: 'Python Fundamentals', duration: 30, questionsCount: 8, dueDate: '2026-04-21', status: 'pending', score: null, avgScore: null },
    { id: 3, title: 'Design Principles Quiz', courseId: 3, courseName: 'UI/UX Design Fundamentals', duration: 20, questionsCount: 15, dueDate: '2026-04-24', status: 'pending', score: null, avgScore: null },
    { id: 4, title: 'Statistics Midterm', courseId: 4, courseName: 'Statistics & Probability', duration: 45, questionsCount: 25, dueDate: '2026-04-28', status: 'draft', score: null, avgScore: null },
  ];

  const examQuestions = {
    2: {
      id: 2,
      title: 'Python Midterm',
      courseName: 'Python Fundamentals',
      duration: 30,
      instructions: 'Read each question carefully. Each correct answer adds to your final score.',
      questions: [
        { id: 1, text: 'Which syntax assigns a value in Python?', options: ['int x = 5', 'x = 5', 'var x = 5', 'declare x = 5'], correct: 1 },
        { id: 2, text: 'What type is returned by 3.14?', options: ["<class 'int'>", "<class 'str'>", "<class 'float'>", "<class 'double'>"], correct: 2 },
        { id: 3, text: 'Which keyword defines a function?', options: ['function', 'def', 'func', 'define'], correct: 1 },
        { id: 4, text: 'What does len("Hello") return?', options: ['4', '5', '6', 'None'], correct: 1 },
        { id: 5, text: 'Which one is mutable?', options: ['tuple', 'string', 'list', 'int'], correct: 2 },
        { id: 6, text: 'Which is a valid for-loop?', options: ['for i in range(10):', 'for (i=0; i<10; i++)', 'for i = 0 to 10:', 'foreach i in range(10)'], correct: 0 },
        { id: 7, text: 'Which operator means floor division?', options: ['/', '//', '%', '**'], correct: 1 },
        { id: 8, text: 'How do you add to the end of a list?', options: ['list.add()', 'list.push()', 'list.append()', 'list.insert()'], correct: 2 },
      ],
    },
  };

  const users = [
    { id: 101, name: 'Ms. Sarah Ahmed', email: 'teacher@demo.com', role: 'teacher', password: 'demo123', packageId: 'growth-600', phone: '', subject: 'Programming' },
    { id: 201, name: 'Ali Hassan', email: 'student@demo.com', role: 'student', password: 'demo123' },
  ];

  const studentAccounts = [
    { id: 1, teacherId: 101, name: 'Ali Hassan', email: 'ali@demo.com', username: 'ali.hassan', password: 'demo123', courseId: 1, course: 'Python Fundamentals', progress: 75, status: 'active' },
    { id: 2, teacherId: 101, name: 'Nour Salem', email: 'nour@demo.com', username: 'nour.salem', password: 'demo123', courseId: 3, course: 'UI/UX Design Fundamentals', progress: 40, status: 'active' },
    { id: 3, teacherId: 101, name: 'Omar Khaled', email: 'omar@demo.com', username: 'omar.khaled', password: 'demo123', courseId: 2, course: 'Data Structures & Algorithms', progress: 90, status: 'active' },
    { id: 4, teacherId: 101, name: 'Layla Ibrahim', email: 'layla@demo.com', username: 'layla.ibrahim', password: 'demo123', courseId: 4, course: 'Statistics & Probability', progress: 20, status: 'inactive' },
    { id: 5, teacherId: 101, name: 'Yusuf Mansour', email: 'yusuf@demo.com', username: 'yusuf.mansour', password: 'demo123', courseId: 1, course: 'Python Fundamentals', progress: 55, status: 'active' },
  ];

  return {
    lastCourseId: 4,
    lastExamId: 4,
    lastStudentAccountId: 5,
    users,
    studentAccounts,
    courses,
    lessonsByCourse,
    lessonContent,
    exams,
    examQuestions,
  };
}

function getDemoDb() {
  try {
    const saved = JSON.parse(localStorage.getItem(STORAGE_KEYS.db));
    if (saved && typeof saved === 'object') return saved;
  } catch {}
  const fresh = createDefaultDemoDb();
  saveDemoDb(fresh);
  return fresh;
}

function saveDemoDb(db) {
  localStorage.setItem(STORAGE_KEYS.db, JSON.stringify(db));
}

function clone(data) {
  return JSON.parse(JSON.stringify(data));
}

/* ---------- Auth helpers ---------- */
function getUser() {
  try { return JSON.parse(localStorage.getItem(STORAGE_KEYS.user)); }
  catch { return null; }
}

function getToken() {
  return localStorage.getItem(STORAGE_KEYS.token);
}

function saveSession(user, token) {
  localStorage.setItem(STORAGE_KEYS.user, JSON.stringify(user));
  localStorage.setItem(STORAGE_KEYS.token, token);
}

function logout() {
  localStorage.removeItem(STORAGE_KEYS.user);
  localStorage.removeItem(STORAGE_KEYS.token);
  window.location.href = 'login.html';
}

function requireAuth() {
  if (!getToken()) {
    window.location.href = 'login.html';
    return false;
  }
  return true;
}

function redirectIfLoggedIn() {
  if (getToken()) redirectToDashboard();
}

function redirectToDashboard() {
  const user = getUser();
  if (!user) {
    logout();
    return;
  }
  window.location.href = 'dashboard.html';
}

function getBookCart() {
  try {
    const saved = JSON.parse(localStorage.getItem(STORAGE_KEYS.cart));
    return Array.isArray(saved) ? saved : [];
  } catch {
    return [];
  }
}

function saveBookCart(items) {
  localStorage.setItem(STORAGE_KEYS.cart, JSON.stringify(items));
}

function addBookToCart(book) {
  const items = getBookCart();
  const existing = items.find((item) => item.id === book.id);
  if (existing) {
    existing.quantity = (existing.quantity || 1) + 1;
    existing.lastUpdated = new Date().toISOString();
  } else {
    items.push({
      ...book,
      quantity: 1,
      orderedAt: new Date().toISOString(),
      status: book.status || 'Preparing',
    });
  }
  saveBookCart(items);
  return items;
}

function removeBookFromCart(bookId) {
  const items = getBookCart().filter((item) => item.id !== bookId);
  saveBookCart(items);
  return items;
}

/* ---------- API layer with demo fallback ---------- */
async function apiRequest(path, method = 'GET', body = null) {
  const headers = { 'Content-Type': 'application/json' };
  const token = getToken();
  if (token) headers.Authorization = `Bearer ${token}`;

  try {
    const response = await fetch(`${API_BASE}${path}`, {
      method,
      headers,
      body: body ? JSON.stringify(body) : null,
    });

    if (response.status === 401) {
      logout();
      return;
    }

    const data = await response.json().catch(() => ({}));
    if (!response.ok) {
      throw new Error(data.message || data.title || `Error ${response.status}`);
    }
    return data;
  } catch (error) {
    return handleDemoRequest(path, method, body, error);
  }
}

const api = {
  get: (path) => apiRequest(path, 'GET'),
  post: (path, body) => apiRequest(path, 'POST', body),
  put: (path, body) => apiRequest(path, 'PUT', body),
  delete: (path) => apiRequest(path, 'DELETE'),
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
  const ratedCourses = teacherCourses.filter((course) => Number(course.rating));
  const avgRating = ratedCourses.length
    ? (ratedCourses.reduce((sum, course) => sum + Number(course.rating), 0) / ratedCourses.length).toFixed(1)
    : '0.0';

  return {
    totalStudents: teacherStudents.length,
    activeCourses: teacherCourses.length,
    totalLessons,
    avgRating,
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

function handleDemoRequest(path, method, body) {
  const db = getDemoDb();
  const user = getUser();

  if (path === '/auth/login' && method === 'POST') {
    const found = db.users.find((entry) => entry.email === body.email && entry.password === body.password);
    if (!found) throw new Error('Invalid email or password.');
    const sessionUser = { id: found.id, name: found.name, email: found.email, role: found.role };
    return { user: sessionUser, token: `demo-token-${found.role}` };
  }

  if (path === '/auth/register' && method === 'POST') {
    const exists = db.users.some((entry) => entry.email.toLowerCase() === body.email.toLowerCase());
    if (exists) throw new Error('An account with this email already exists.');
    const newUser = {
      id: Date.now(),
      name: `${body.firstName} ${body.lastName}`.trim(),
      email: body.email.trim(),
      role: 'teacher',
      password: body.password,
    };
    db.users.push(newUser);
    saveDemoDb(db);
    return { success: true, user: newUser };
  }

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
      rating: 'New',
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
      duration: body.duration || '10 min',
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
        duration: body.duration || '10 min',
        videoUrl: body.url || '',
        description: body.description || 'Lesson description coming soon.',
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
    const packageSeatsMap = {
      'starter-200': 200,
      'growth-600': 600,
      'scale-1000': 1000,
    };
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
    const packageSeats = packageSeatsMap[user.packageId] || 600;
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
      const email = `${username}@student.edugate.local`;
      const password = body.passwordMode === 'fixed'
        ? fixedPassword
        : `edu${String(Math.floor(1000 + Math.random() * 9000))}`;

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
      courseName: course ? course.title : 'General',
      duration: body.duration || 30,
      questionsCount: body.questionsCount || 20,
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
      instructions: exam.instructions || 'Read each question carefully.',
      questions: [
        { id: 1, text: 'Sample question for preview mode?', options: ['Option A', 'Option B', 'Option C', 'Option D'], correct: 0 },
      ],
    };
    saveDemoDb(db);
    return clone(exam);
  }

  const examMatch = path.match(/^\/exams\/(\d+)$/);
  if (examMatch && method === 'GET') {
    const examId = examMatch[1];
    return clone(db.examQuestions[examId] || db.exams.find((exam) => String(exam.id) === examId));
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
    if (examSummary) {
      examSummary.status = 'completed';
      examSummary.score = score;
    }
    saveDemoDb(db);
    return {
      score,
      correct,
      total: exam.questions.length,
      passed: score >= 70,
      answers,
    };
  }

  throw new Error('Demo handler does not support this request yet.');
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
    <div class="course-card" onclick="window.location.href='course-details.html?id=${course.id}'">
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
          <span>${ICONS.star} ${course.rating || '4.5'}</span>
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
          ? `<button class="btn-amber" onclick="window.location.href='take-exam.html?id=${exam.id}'">Start</button>`
          : '<button class="btn-secondary">Review</button>'}
      </div>
    </div>`;
}

document.addEventListener('DOMContentLoaded', () => {
  document.body.classList.add('page-fade');
  initLogoutBtn();
  initModalCloseButtons();
  initMobileSidebar();
});
