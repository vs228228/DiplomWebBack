import re

import unicodedata

from src.infrastructure.nlp.ner_skill_extractor import SkillNerWrapper

from src.core.domain.skill import Skill
from src.core.domain.skill_profile import SkillProfile
from src.infrastructure.nlp.lang_detector import LanguageDetector
from src.infrastructure.nlp.skill_dictionary import SkillDictionary


class SkillExtractor:

    def __init__(self):
        self.lang_detector = LanguageDetector()
        self.skillner = SkillNerWrapper()
        self.skill_dict = SkillDictionary()

    def extract(self, text: str, tables: list) -> SkillProfile:
        lang = self.lang_detector.detect(text)

        if lang == "ru":
            ml_skills = self._extract_ru(text)
        else:
            ml_skills = self._extract_en(text)

        skill_tables = [t for t in tables if self._is_skill_table(t)]
        other_tables_text = self._tables_to_text([t for t in tables if not self._is_skill_table(t)])

        # 4. Извлекаем навыки из таблиц
        table_skills = self._extract_skill_tables(skill_tables)

        # 5. Обрабатываем остальной текст из не-таблиц как обычный текст
        if other_tables_text:
            if lang == "ru":
                ml_skills += self._extract_ru(other_tables_text)
            else:
                ml_skills += self._extract_en(other_tables_text)

        # 6. Объединяем все навыки
        merged = self._merge(ml_skills, table_skills)

        return SkillProfile(skills=merged)

    def _extract_en(self, text):
        found = self.skillner.extract(text)
        return [
            Skill(name=s, category="ml", source_section="text")
            for s in found
        ]

    def _extract_ru(self, text):

        text = self._normalize(text)

        dict_skills = self.skill_dict.find_skills_ru(text)

        context_skills = self.skill_dict.extract_context_skills(text)

        fuzzy_skills = self.skill_dict.find_skills_fuzzy(text)

        descriptive_skills = self.skill_dict.extract_descriptive_skills(text)

        all_skills = set(dict_skills + context_skills + fuzzy_skills + descriptive_skills)

        return [
            Skill(
                name=s,
                category="dict",
                source_section="text"
            )
            for s in all_skills
        ]

    def _tables_to_text(self, tables: list) -> str:
        parts = []

        for table in tables:
            for row in table:
                for cell in row:
                    if cell and str(cell).strip():
                        parts.append(str(cell))

        return "\n".join(parts)

    def _extract_skillner(self, text):

        found = self.skillner.extract(text)

        skills = []

        for s in found:
            skills.append(
                Skill(
                    name=s,
                    category="ml",
                    source_section="text"
                )
            )

        return skills

    def _extract_skill_tables(self, tables):
        skills = []

        for table in tables:
            if not self._is_skill_table(table):
                continue

            headers = [str(h).lower() if h else "" for h in table[0]]

            skill_idx = self._find_column(headers, ["skill", "навык", "technology", "stack"])
            level_idx = self._find_column(headers, ["level", "уровень"])
            years_idx = self._find_column(headers, ["years", "experience", "опыт"])

            if skill_idx is None:
                continue

            for row in table[1:]:
                # исключаем пустые строки
                if not any(str(c).strip() for c in row):
                    continue

                # исключаем строки, где все ячейки одинаковые
                stripped_row = [str(c).strip() for c in row if c is not None]
                if len(set(stripped_row)) <= 1:
                    continue

                # берем название навыка
                if skill_idx >= len(row):
                    continue
                skill_name = str(row[skill_idx]).strip()
                if not skill_name:
                    continue

                # уровень и опыт
                level = row[level_idx].strip() if level_idx is not None and level_idx < len(row) else None
                years = self._parse_years(row[years_idx]) if years_idx is not None and years_idx < len(row) else None

                skills.append(
                    Skill(
                        name=skill_name,
                        level=level,
                        years=years,
                        category="table",
                        source_section="table"
                    )
                )
        return skills

    def _is_skill_table(self, table):
        if not table or not table[0]:
            return False

        headers = [str(c).lower() for c in table[0] if c]
        keywords = {
            "skill", "skills",
            "technology", "technologies",
            "stack",
            "навык", "навыки",
            "технологии",
            "опыт", "уровень",
            "years", "experience"
        }

        score = sum(1 for h in headers if any(k in h for k in keywords))
        return score > 0

    def _find_column(self, headers, variants):

        for i, h in enumerate(headers):
            for v in variants:
                if v in h:
                    return i
        return None

    def _parse_years(self, text):
        if not text:
            return None

        import re
        m = re.search(r"\d+(\.\d+)?", text)
        return float(m.group()) if m else None

    def _merge(self, ml, table):

        merged = {}

        # сначала ML/dict
        for s in ml:
            merged[s.name.lower()] = s

        # потом таблицы (приоритет)
        for s in table:
            key = s.name.lower()

            if key in merged:
                existing = merged[key]

                merged[key] = Skill(
                    name=s.name,
                    category="table",
                    level=s.level or existing.level,
                    years=s.years or existing.years,
                    source_section="table"
                )
            else:
                merged[key] = s

        return list(merged.values())

    def _normalize(self, text: str) -> str:
        if not text:
            return ""

        text = unicodedata.normalize("NFC", text)

        replacements = {
            "–": "-",
            "—": "-",
            "“": '"',
            "”": '"',
            "’": "'",
        }

        for k, v in replacements.items():
            text = text.replace(k, v)

        text = re.sub(r"[^\w\s\-\+\#\.\,\/]", " ", text)

        text = re.sub(r"\s+", " ", text).strip()

        return text