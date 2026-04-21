import re

import unicodedata

from src.core.domain.skill import Skill
from src.core.domain.skill_profile import SkillProfile
from src.infrastructure.ml.dataset_filter import DatasetFilter
from src.infrastructure.nlp.lang_detector import LanguageDetector
from src.infrastructure.nlp.llm_skill_labeler import LLMSkillLabeler
from src.infrastructure.nlp.ml_skill_extractor import MLSkillExtractor
from src.infrastructure.nlp.ner_skill_extractor import SkillNerWrapper
from src.infrastructure.nlp.skill_dictionary import SkillDictionary


class SkillExtractor:

    def __init__(self):
        self.lang_detector = LanguageDetector()
        self.skillner = SkillNerWrapper()
        self.skill_dict = SkillDictionary()
        self.llm_labeler = LLMSkillLabeler()
        self.dataset_filter = DatasetFilter()
        self.ml_extractor = MLSkillExtractor()

    def extract(self, text: str, tables: list, usellm: bool = True) -> SkillProfile:

        lang = self.lang_detector.detect(text)

        if lang == "ru":
            ml_skills = self._extract_ru(text)
        else:
            ml_skills = self._extract_en(text)

        ml_model_skills = self.ml_extractor.extract(text)

        ml_model_skills = [
            Skill(name=s, category="ml_model", source_section="text")
            for s in ml_model_skills
        ]

        skill_tables = [t for t in tables if self._is_skill_table(t)]
        table_skills = self._extract_skill_tables(skill_tables)

        llm_skills = []
        if usellm:

            llm_hard, llm_applied = self.llm_labeler.extract(text)

            llm_skills = [
                            Skill(name=s, category="llm_hard", source_section="text")
                            for s in llm_hard
                        ] + [
                            Skill(name=s, category="llm_applied", source_section="text")
                            for s in llm_applied
                        ]

        merged = self._merge(
            ml_skills + llm_skills + ml_model_skills,
            table_skills
        )

        filtered = [
            s for s in merged
            if self.dataset_filter.is_valid_skill(s.name)
        ]

        return SkillProfile(skills=filtered)

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
                if not any(str(c).strip() for c in row):
                    continue

                stripped_row = [str(c).strip() for c in row if c is not None]
                if len(set(stripped_row)) <= 1:
                    continue

                if skill_idx >= len(row):
                    continue
                skill_name = str(row[skill_idx]).strip()
                if not skill_name:
                    continue

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

    def _merge(self, skills, table_skills):

        pool = {}

        def add_skill(s: Skill):
            key = self._normalize_name(s.name)

            if not key:
                return

            if key not in pool:
                pool[key] = {
                    "skill": s,
                    "score": 0,
                    "sources": set()
                }

            pool[key]["score"] += SOURCE_PRIORITY.get(s.category, 1)
            pool[key]["sources"].add(s.category)

            if s.category == "table":
                pool[key]["skill"] = s

        for s in skills + table_skills:
            add_skill(s)

        result = []

        for item in pool.values():
            skill = item["skill"]
            score = item["score"]

            if score < 3:
                continue

            result.append(skill)

        return result

    def _normalize_name(self, name: str) -> str:
        if not name or isinstance(name, list):
            return ""

        name = name.lower().strip()

        # 🔥 через aliases
        for skill, data in self.skill_dict.raw.items():
            if name in data["aliases"]:
                return skill

        return name

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

SOURCE_PRIORITY = {
    "llm": 6,
    "table": 5,
    "dict": 4,
    "ml": 2,
    "ml_model": 1,
}