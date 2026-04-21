import json
import random
from faker import Faker

fake = Faker("ru_RU")

HARD_SKILLS = [
    "C#", "C++", "Java", "Python", "JavaScript", "TypeScript",
    ".NET", ".NET Core", "ASP.NET", "ASP.NET Core",
    "Spring", "Spring Boot", "Django", "Flask", "FastAPI",
    "Node.js", "Express.js",
    "React", "Angular", "Vue.js",
    "HTML", "CSS", "SCSS", "SASS",
    "Docker", "Docker Compose", "Kubernetes",
    "PostgreSQL", "MySQL", "MSSQL", "MongoDB", "Redis",
    "Oracle DB", "SQLite",
    "RabbitMQ", "Kafka",
    "gRPC", "REST", "GraphQL",
    "Git", "GitHub", "GitLab", "Bitbucket",
    "Linux", "Windows", "Bash", "PowerShell",
    "Nginx", "Apache",
    "AWS", "Amazon S3", "EC2", "Lambda",
    "Azure", "Google Cloud",
    "Terraform", "Ansible",
    "CI/CD", "Jenkins", "GitHub Actions",
    "Prometheus", "Grafana", "ELK Stack",
    "Serilog", "NLog",
    "xUnit", "NUnit", "Moq", "TestContainers",
    "Selenium", "Playwright",
    "AutoMapper", "Mapster",
    "Entity Framework", "Entity Framework Core",
    "Dapper",
    "Ocelot", "YARP",
    "SignalR",
    "Hangfire",
    "IdentityServer", "OAuth2", "JWT",
    "Microservices", "Monolith", "Clean Architecture",
    "CQRS", "MediatR",
    "WebSockets",
    "Blazor",
    "Pandas", "NumPy", "Scikit-learn", "TensorFlow", "PyTorch",
    "OpenCV", "NLTK", "SpaCy",
    "Unity", "Unreal Engine",
    "Figma", "Adobe XD",
    "Webpack", "Vite",
    "RxJS",
    "Three.js",
    "WebRTC",
    "Electron",
    "Tauri"
]

APPLIED_SKILLS = [
    "разработка API", "проектирование архитектуры", "написание юнит тестов",
    "оптимизация запросов", "проведение код ревью", "анализ требований",
    "рефакторинг кодовой базы", "разработка фронтенда", "разработка бэкенда",
    "проектирование баз данных", "проектирование веб-систем",
    "разработка микросервисов", "интеграция сторонних сервисов",
    "написание интеграционных тестов", "написание e2e тестов",
    "настройка CI/CD", "контейнеризация приложений",
    "развертывание приложений", "мониторинг систем",
    "логирование приложений", "обработка ошибок",
    "разработка REST API", "разработка GraphQL API",
    "разработка gRPC сервисов", "управление состоянием",
    "оптимизация производительности", "масштабирование системы",
    "разработка UI/UX", "адаптивная верстка",
    "кроссбраузерная верстка", "дебаггинг",
    "профилирование приложений", "работа с очередями сообщений",
    "работа с кэшем", "разработка многопоточных приложений",
    "обработка данных", "машинное обучение",
    "обработка изображений", "обработка текста",
    "разработка мобильных приложений",
    "разработка десктопных приложений",
    "интеграция API", "написание документации",
    "поддержка легаси кода", "миграция данных",
    "проектирование схем БД", "нормализация БД",
    "работа с ORM", "ручное написание SQL",
    "разработка бизнес-логики", "реализация авторизации",
    "реализация аутентификации", "работа с токенами",
    "обеспечение безопасности", "пентест",
    "разработка highload систем", "оптимизация latency",
    "работа с event-driven архитектурой",
    "разработка realtime приложений",
    "работа с websocket", "разработка SPA",
    "разработка SSR приложений",
    "интеграция платежных систем",
    "разработка админ панелей",
    "настройка серверов", "администрирование Linux",
    "CI/CD pipelines", "blue-green deployment",
    "canary deployment"
]

SKILL_SYNONYMS = {
    ".NET": ["dotnet", ".net core", "asp.net core"],
    "JavaScript": ["js", "javascript"],
    "TypeScript": ["ts"],
    "PostgreSQL": ["postgres", "postgresql"],
    "Docker": ["docker container"],
    "Kubernetes": ["k8s"],
    "Git": ["git vcs"],
}

SKILL_PATTERNS = [
    "Опыт работы с {skill}",
    "Использовал {skill}",
    "Работал с {skill}",
    "Применял {skill}",
    "Использую {skill} ежедневно",
    "{skill} на уровне {level}",
    "Глубокие знания {skill}",
    "Коммерческий опыт с {skill}",
    "Разрабатывал решения на {skill}",
    "Имел дело с {skill}",
    "Участвовал в проектах с {skill}",
    "Использование {skill}",
    "Практический опыт с {skill}",
    "Знание {skill}",
    "Работал с технологией {skill}",
]

EXPERIENCE_PATTERNS = [
    "{skill} — опыт {years} лет",
    "{skill}: {years} года опыта",
    "Опыт работы с {skill} {years} лет",
    "{skill} ({years} yrs)",
    "{skill} - {years} years",
]

LEVELS = ["базовый", "средний", "продвинутый"]

PROJECTS = [
    "веб-приложение", "микросервис", "REST API",
    "платформу аналитики", "CRM систему",
    "систему обработки данных", "онлайн-сервис",
    "маркетплейс", "финтех платформу"
]

PROJECT_PATTERNS = [
    "Разрабатывал {project} используя {skill}",
    "Создал {project} с применением {skill}",
    "Работал над {project}, где использовался {skill}",
]

NOISE_BLOCKS = [
    "Люблю путешествовать",
    "Увлекаюсь спортом",
    "Читаю книги",
    "Смотрю фильмы",
    "Командный игрок",
    "Быстро обучаюсь",
    "Ответственный",
    "Коммуникабельный",
    "Люблю кофе",
    "Играю в игры",
    "Интересуюсь технологиями",
    "Хожу в зал",
    "Люблю музыку",
    "Изучаю языки",
    "Люблю готовить",
    "Занимаюсь бегом",
    "Путешествую по миру",
    "Играю на гитаре",
    "Смотрю сериалы",
    "Участвую в митапах",
    "Посещаю конференции",
    "Пишу статьи",
    "Веду блог",
    "Увлекаюсь фотографией",
    "Люблю природу",
    "Занимаюсь йогой",
    "Люблю животных",
    "Хожу в походы",
    "Интересуюсь стартапами",
    "Слушаю подкасты",
    "Изучаю психологию",
    "Работаю в команде",
    "Люблю эксперименты",
    "Увлекаюсь историей",
    "Смотрю YouTube",
    "Люблю автомобили",
    "Интересуюсь финансами",
    "Занимаюсь саморазвитием",
    "Читаю технические статьи",
    "Пишу код в свободное время"
] * 3

FAKE_SKILLS = [
    "system design patterns",
    "clean architecture principles deep dive",
    "scalable system thinking",
    "async event processing flow",
    "high performance backend mindset",
    "distributed computing basics",
    "frontend backend communication flow",
    "data driven development approach",
]

CONTEXT_PATTERNS = [
    "Опыт работы с {skill} в коммерческих проектах",
    "Разрабатывал сервисы на основе {skill}",
    "Использовал {skill} для построения backend систем",
    "Проектировал архитектуру с применением {skill}",
    "Настраивал и поддерживал инфраструктуру на {skill}",
    "Писал production код с использованием {skill}",
    "Интегрировал {skill} в существующие системы",
    "Оптимизировал приложения с помощью {skill}",
    "Разрабатывал микросервисы на {skill}",
]

def apply_synonyms(skill):
    if skill in SKILL_SYNONYMS and random.random() < 0.3:
        return random.choice(SKILL_SYNONYMS[skill])
    return skill

def generate_experience():
    return round(random.uniform(0.5, 6), 1)


def maybe_corrupt(skill: str):
    variants = [
        skill,
        skill.lower(),
        skill.upper(),
        skill.replace(".", " . "),
        skill.replace("C#", "C Sharp"),
        skill.replace("JavaScript", "JS"),
        f"{skill} dev",
    ]
    return random.choice(variants)


SECTION_HEADERS = {
    "skills": ["Ключевые навыки:", "Технологический стек:", "Skills:", "Технологии:"],
    "experience": ["Опыт работы:", "Professional Experience:", "Работал в проектах:"],
    "about": ["О себе:", "Summary:", "Дополнительная информация:"]
}

DELIMITERS = [", ", " | ", " • ", " / ", "; "]


def generate_skills_block(hard_count=10, applied_count=5):
    hard = random.sample(HARD_SKILLS, k=hard_count)
    applied = random.sample(APPLIED_SKILLS, k=applied_count)

    delim = random.choice(DELIMITERS)
    header = random.choice(SECTION_HEADERS["skills"])

    skills_list = [maybe_corrupt(s) for s in hard + applied]
    block_text = f"{header}\n{delim.join(skills_list)}"

    skill_meta = []
    for s in hard: skill_meta.append({"name": s.lower(), "category": "HARD_SKILL"})
    for s in applied: skill_meta.append({"name": s.lower(), "category": "APPLIED_SKILL"})

    return block_text, skill_meta


def generate_job_period():
    job_hard = random.sample(HARD_SKILLS, k=random.randint(3, 6))
    job_applied = random.sample(APPLIED_SKILLS, k=random.randint(2, 4))

    company = fake.company()
    position = fake.job()
    years = generate_experience()

    # Формируем описание проекта
    desc_patterns = [
        "Разработка {project}. Стек: {stack}.",
        "Участие в проекте {project}. Использовали {stack}.",
        "Проектирование и поддержка {project}. Технологии: {stack}.",
    ]

    stack_text = ", ".join([maybe_corrupt(s) for s in job_hard])
    description = random.choice(desc_patterns).format(
        project=random.choice(PROJECTS),
        stack=stack_text
    )

    job_text = f"{company}\n{position} ({years} лет)\n{description}"

    meta = []
    for s in job_hard: meta.append({"name": s.lower(), "category": "HARD_SKILL", "years": years})
    for s in job_applied: meta.append({"name": s.lower(), "category": "APPLIED_SKILL", "years": years})

    return job_text, meta


def generate_resume():
    full_meta = []
    sections = []

    sections.append(f"{fake.name()}\n{fake.city()}\n{fake.email()}")

    if random.random() > 0.5:
        txt, meta = generate_skills_block()
        sections.append(txt)
        full_meta.extend(meta)

    sections.append(random.choice(SECTION_HEADERS["experience"]))
    for _ in range(random.randint(2, 3)):
        txt, meta = generate_job_period()
        sections.append(txt)
        full_meta.extend(meta)

    if random.random() > 0.3:
        header = random.choice(SECTION_HEADERS["about"])
        noise = "\n".join(random.sample(NOISE_BLOCKS, k=random.randint(2, 5)))
        sections.append(f"{header}\n{noise}")

    return {
        "text": "\n\n".join(sections),
        "skills": full_meta
    }


def generate_dataset(path="dataset.jsonl", size=10000):
    with open(path, "w", encoding="utf-8") as f:
        for _ in range(size):
            f.write(json.dumps(generate_resume(), ensure_ascii=False) + "\n")

    print(f"Generated {size} samples")


if __name__ == "__main__":
    generate_dataset()