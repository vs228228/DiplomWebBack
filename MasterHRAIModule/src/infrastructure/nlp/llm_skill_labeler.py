import json
import os
import time
from typing import Any

from dotenv import load_dotenv
from google import genai
from google.genai import types


class LLMSkillLabeler:
    def __init__(self, model_name: str = "gemini-3.1-flash-lite-preview", max_retries: int = 3):
        load_dotenv()
        self.client = genai.Client(api_key=os.getenv("GOOGLE_API_KEY"))
        self.model_name = model_name
        self.max_retries = max_retries

    def extract(self, text: str) -> list[Any] | tuple[Any, Any] | tuple[list[Any], list[Any]]:
        if not text or len(text.strip()) < 10:
            return []

        prompt = self._build_prompt(text)

        for attempt in range(self.max_retries):
            try:
                response = self.client.models.generate_content(
                    model=self.model_name,
                    contents=prompt,
                    config=types.GenerateContentConfig(
                        system_instruction="Ты система извлечения hard skills. Возвращай только JSON массив строк.",
                        response_mime_type="application/json",
                        temperature=0.0
                    )
                )

                time.sleep(5)

                return self._parse_response(response.text)

            except Exception as e:
                err_str = str(e)
                if "429" in err_str:
                    print(f"Лимит достигнут (429). Попытка {attempt + 1}/{self.max_retries}. Спим 65 сек...")
                    time.sleep(65)
                elif "404" in err_str:
                    print(f"Ошибка 404: Модель {self.model_name} не найдена. Проверьте имя модели.")
                    return []
                else:
                    print(f"[LLM Error]: {err_str}")
                    time.sleep(5)

        return []

    def _build_prompt(self, text: str) -> str:
        return f"""
    Извлеки навыки из текста и раздели их на 2 категории:

    1. hard_skills:
    - технологии, языки, инструменты
    - примеры: C#, .NET, Docker, PostgreSQL

    2. applied_skills:
    - прикладные инженерные навыки
    - избегать "абстрактных" представителей как "разработка масштабируемых/надежных решений", "проведение код ревью"
    - примеры:
      - проектирование архитектуры бд
      - анализ требований
      - разработка api
      - оптимизация запросов

    ❗ Правила:
    - НЕ добавляй soft skills
    - НЕ добавляй абстрактные фразы
    - максимум 1-4 слова
    - Если резюме написано на английском языке, то и навыки ищи и выводи в английском формате

    Верни строго JSON:

    {{
      "hard_skills": [...],
      "applied_skills": [...]
    }}

    Текст:
    {text[:8000]}
    """

    def _parse_response(self, content: str):
        try:
            data = json.loads(content)
            print(data)
            hard = data.get("hard_skills", [])
            applied = data.get("applied_skills", [])

            return hard, applied
        except:
            return [], []