import json
import re

from rapidfuzz import process


class SkillDictionary:

    def __init__(self, path="src/models/dicts/skills_dict.json"):
        with open(path, encoding="utf-8") as f:
            self.raw = json.load(f)

        self.alias_to_skill = self._build_index()

        from src.infrastructure.nlp.ru_text_processor import RussianTextProcessor
        self.ru_processor = RussianTextProcessor()

    def find_skills_ru(self, text: str):

        tokens = self.ru_processor.process(text)

        found = set()

        for token in tokens:
            if token in self.alias_to_skill:
                found.add(self.alias_to_skill[token])

        return list(found)

    def _build_index(self):
        index = {}

        for skill_name, data in self.raw.items():
            for alias in data["aliases"]:
                index[alias.lower()] = skill_name

        return index

    def extract_context_skills(self, text: str):

        patterns = [
            r"опыт работы с ([\w\+#\.]+)",
            r"работал с ([\w\+#\.]+)",
            r"использовал ([\w\+#\.]+)",
            r"разрабатывал на ([\w\+#\.]+)",
            r"знание ([\w\+#\.]+)",
            r"специализируюсь на ([\w\+#\.]+)"
        ]

        found = set()

        for pattern in patterns:
            matches = re.findall(pattern, text.lower())

            for m in matches:
                if m in self.alias_to_skill:
                    found.add(self.alias_to_skill[m])

        return list(found)

    def find_skills_fuzzy(self, text: str):
        tokens = re.findall(r"[a-zA-Z0-9\+#\.]+", text.lower())
        found = set()

        for token in tokens:
            match, score, _ = process.extractOne(
                token,
                self.alias_to_skill.keys()
            )

            if score > 90:
                found.add(self.alias_to_skill[match])

        return list(found)

    def extract_descriptive_skills(self, text: str) -> list[str]:
        if not text:
            return []

        patterns = [
            r"(?i)специализируюсь на\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)опыт[ае]?\s+(?:работы\s+)?с\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)опыт[ае]?\s+в\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)знани[яе]\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)уме[ею]\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)владею\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)анализ[ае]?\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)проектировани[еи]?\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)разработк[ие]\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)тестировани[еи]\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)внедрени[еи]\s+([^\.]+?)(?=\s*(?:\.|$))",
            r"(?i)оптимизаци[ие]\s+([^\.]+?)(?=\s*(?:\.|$))",
        ]

        found = set()
        for pattern in patterns:
            for match in re.finditer(pattern, text):
                clause = match.group(1).strip()
                if not clause or len(clause) < 5:
                    continue

                clause = re.sub(r"\s+и\s+", ", ", clause, flags=re.IGNORECASE)
                items = [item.strip() for item in re.split(r'\s*,\s*', clause) if item.strip()]

                for item in items:
                    item_lower = item.lower()
                    if len(item) > 5 and item_lower not in self.alias_to_skill:
                        found.add(item)

        return list(found)