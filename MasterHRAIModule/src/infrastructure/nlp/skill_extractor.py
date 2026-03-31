import re

import unicodedata

from src.infrastructure.nlp.ner_skill_extractor import SkillNerWrapper

from src.core.domain.skill import Skill
from src.core.domain.skill_profile import SkillProfile
from src.infrastructure.nlp.lang_detector import LanguageDetector


class SkillExtractor:

    def __init__(self):
        self.lang_detector = LanguageDetector()
        self.skillner = SkillNerWrapper()

    def extract(self, text: str, tables: list) -> SkillProfile:

        table_text = self._tables_to_text(tables)

        full_text = text + "\n" + table_text

        full_text = self._normalize(full_text)

        ml_skills = self._extract_skillner(full_text)

        table_skills = self._extract_skill_tables(tables)

        merged = self._merge(ml_skills, table_skills)

        return SkillProfile(skills=merged)

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

            if not table or not table[0]:
                continue

            headers = [str(h).lower() if h else "" for h in table[0]]

            skill_idx = self._find_column(headers, ["skill", "навык", "technology"])

            if skill_idx is None:
                continue

            level_idx = self._find_column(headers, ["level", "уровень"])
            years_idx = self._find_column(headers, ["years", "experience", "опыт"])

            for row in table[1:]:

                if not row:
                    continue

                filled_cells = [c for c in row if str(c).strip()]
                if len(filled_cells) == 1:
                    continue

                if skill_idx >= len(row):
                    continue

                skill_name = str(row[skill_idx]).strip()
                if not skill_name:
                    continue

                level = None
                if level_idx is not None and level_idx < len(row):
                    level = row[level_idx]

                years = None
                if years_idx is not None and years_idx < len(row):
                    years = self._parse_years(row[years_idx])

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

        headers = [c.lower() for c in table[0]]

        keywords = {
            "skill", "skills",
            "technology", "technologies",
            "stack",
            "навык", "навыки",
            "технологии",
            "стек"
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

        for s in ml:
            merged[s.name.lower()] = s

        for s in table:
            merged[s.name.lower()] = s

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